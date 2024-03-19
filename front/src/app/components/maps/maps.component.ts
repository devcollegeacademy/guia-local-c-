import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { CustomerService, Customer } from '../../services/customer.service';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

interface Coords {
  lat: number;
  lng: number;
}

declare var google: any;

@Component({
  selector: 'app-maps',
  standalone: true,
  imports: [],
  providers: [CustomerService],
  templateUrl: './maps.component.html',
  styleUrl: './maps.component.css'
})

export class MapsComponent implements OnInit {

  @ViewChild('map') mapElement!: ElementRef<HTMLDivElement>;
  @ViewChild('routes') routesElement!: ElementRef<HTMLDivElement>;

  public directionsService: any;
  public directionsRenderer: any;
  public currentLocation: Coords | undefined;

  public customers$: Observable<Customer[] | undefined>;


  constructor(private customerService: CustomerService) {
    this.customers$ = this.customerService.customers$;
  }

  ngOnInit() {
    (async () => {
      const response = await fetch('http://localhost:5074/customer/filter');
      const body = await response.json();
      console.log(body);

      this.customerService.setCustomers(body)
    })().then(() => this.initMap())
  }

  async getCoords(address: string): Promise<Coords> {
    try {
      return new Promise((resolve, reject) => {
        const geocoder = new google.maps.Geocoder();
        geocoder.geocode({ address }, (results: any, status: any) => {
          if (status === google.maps.GeocoderStatus.OK) {
            const location = results[0].geometry.location;
            const lat = location.lat();
            const lng = location.lng();
            resolve({ lat, lng });
          } else {
            reject(`Geocodificação falhou para o endereço: ${address}`);
          }
        });
      });
    } catch (error: any) {
      console.error("Erro ao obter coordenadas:", error.message);
      throw error;
    }
  }

  async getCurrentLocation(): Promise<Coords> {
    try {
      if ("geolocation" in navigator) {
        const position: GeolocationPosition = await new Promise((resolve, reject) => {
          navigator.geolocation.getCurrentPosition(resolve, reject);
        });
        const { latitude: lat, longitude: lng } = position.coords;
        return { lat, lng };
      } else {
        console.error("Geolocalização não é suportada neste navegador.");
        throw new Error("Geolocalização não é suportada neste navegador.");
      }
    } catch (error: any) {
      console.error("Erro ao obter geolocalização:", error.message);
      throw error;
    }
  }

  addMarker(mapInstance: any, destination: Coords, title: string, label: string) {
    const marker = new google.maps.Marker({
      position: destination,
      map: mapInstance,
      title: title,
      label: label
    });

    marker.addListener('click', () => {
      this.calculateAndDisplayRoute(this.currentLocation, destination);
    });
  };

  async initMap() {
      try {
        this.directionsRenderer = new google.maps.DirectionsRenderer();

        this.currentLocation = await this.getCurrentLocation();
        const mapInstance = new google.maps.Map(this.mapElement.nativeElement, {
          zoom: 14,
          center: this.currentLocation,
          disableDefaultUI: false,
        });

        this.directionsRenderer.setMap(mapInstance);
        this.directionsRenderer.setPanel(this.routesElement.nativeElement);

        this.addMarker(mapInstance, this.currentLocation, 'Minha Localização', '0')

        this.customers$
          .pipe(
            finalize(() => {
              console.log('All markers added');
            })
          )
          .subscribe(async customers => {
            if (customers && customers.length > 0) {
              for (let i = 0; i < customers.length; i++) {
                const customer = customers[i];
                const coods = await this.getCoords(customer.address);
                this.addMarker(mapInstance, coods, customer.title, i.toString());
              }
            }
          });
      } catch (error: any) {
        console.error("Erro ao inicializar o mapa:", error.message);
        alert("Erro ao inicializar o mapa");
      }
  };

  async calculateAndDisplayRoute(origin: Coords | undefined, destination: Coords) {
    try {
      this.directionsService = new google.maps.DirectionsService();
      if (!this.directionsService || !this.directionsRenderer) return;
      const response = await this.directionsService.route({
        origin: origin,
        destination: destination,
        travelMode: 'DRIVING',
        language: 'pt-BR'
      });
      this.directionsRenderer.setDirections(response);
    } catch (error: any) {
      console.error("Falha na solicitação de direções:", error.message);
      alert("Falha na solicitação de direções");
    }
  };

}
