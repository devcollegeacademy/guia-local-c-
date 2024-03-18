import { Component, FormEvent, ChangeEvent } from '@angular/core';
import { ModalContext } from '@/contexts/ModalContext';
import { CustomerContext } from '@/contexts/CustomerContext';
import { instance } from '@/services/axios';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  input: string = '';
  customer: any = {};

  constructor(
    private modalContext: ModalContext,
    private customerContext: CustomerContext,
    private router: Router
  ) {}

  async onSubmit(e: FormEvent) {
    e.preventDefault();
    try {
      const response = await instance.get(`/customer/filter/${this.input}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });
      this.customerContext.setCustomers(response.data);
    } catch (error: any) {
      alert("Erro ao filtrar clientes: " + error.message);
      console.error("Erro ao filtrar clientes:", error);
    }
  }

  onChange(e: ChangeEvent<HTMLInputElement>) {
    this.input = e.target.value;
  }

  async ngOnInit() {
    const hashString = window.location.hash;
    const accessTokenRegex = /access_token=([^&]*)/;
    const match = hashString.match(accessTokenRegex);

    if (match) {
      const accessToken = match[1];
      if (accessToken !== '') {
        console.log(accessToken);
        try {
          const response = await instance.get('/customer', {
            headers: {
              Authorization: `Bearer ${accessToken}`
            }
          });

          this.customerContext.setCustomer(response.data);

          localStorage.setItem('token', accessToken);
          this.router.navigate(['/']);
          this.modalContext.setModalAction(response.data?.title ? 'update' : 'create');
          this.modalContext.setShowModal(true);
        } catch (error: any) {
          alert("Erro ao obter dados do usuário: " + error.message);
          console.error("Erro ao obter dados do usuário:", error);
        }
      } else {
        alert('Falha ao tentar entrar com sua conta da Google!');
      }
    }
  }
}
