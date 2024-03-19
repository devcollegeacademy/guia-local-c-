import { Component, ElementRef, ViewChild } from '@angular/core';
import { CustomerService } from '../../services/customer.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  providers: [CustomerService],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  @ViewChild('searchInput') searchInput!: ElementRef<HTMLInputElement>;

  constructor(private customerService: CustomerService) { }

  submitForm(event: Event) {
    event.preventDefault();
    (async () => {      
      const response = await fetch(`http://localhost:5074/customer/filter/${this.searchInput.nativeElement.value}`);
      const body = await response.json();
      this.customerService.setCustomers(body)
    })()
  }

}
