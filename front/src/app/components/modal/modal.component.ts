import { Component, useContext, ChangeEvent, FormEvent } from '@angular/core';
import { ModalContext } from '@/contexts/ModalContext';
import { CustomerContext } from '@/contexts/CustomerContext';
import { instance } from '@/services/axios';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  showModal: boolean = false;
  modalAction: string = '';
  customer: any = {};

  constructor(
    private modalContext: ModalContext,
    private customerContext: CustomerContext
  ) {}

  async getAddress(zipCode: string) {
    try {
      const response = await instance.get(`/customer/address/${zipCode}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });

      this.customer = {
        ...this.customer,
        zipCode,
        address: response.data
      };
    } catch (error: any) {
      alert("Erro ao buscar endereço " + error.message);
      console.error("Erro ao buscar endereço:", error);
    }
  }

  async logout() {
    try {
      const response = await instance.get(`/customer/filter/`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      });
      this.customerContext.setCustomers(response.data);
      localStorage.removeItem('token');
      this.customer = { ...this.customer, email: '' };
      this.showModal = false;
    } catch (error: any) {
      alert("Erro ao fazer logout " + error.message);
      console.error("Erro ao fazer logout:", error);
    }
  }

  onChange(event: ChangeEvent) {
    let { name, value }: any = event.target;

    if (name === 'active') {
      value = value === 'true' ? true : false;
    }

    this.customer = {
      ...this.customer,
      [name]: value
    };

    if (name === 'zipCode' && value.length === 8) {
      this.getAddress(value);
    }
  }

  async onSubmit(event: FormEvent) {
    event.preventDefault();

    try {
      if (this.modalAction === 'create') {
        await instance.post('/customer', this.customer, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`
          }
        });
        this.modalAction = 'update';
      } else {
        await instance.put('/customer', this.customer, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`
          }
        });
      }
      alert("Dados salvos com sucesso!");
    } catch (error: any) {
      alert("Erro ao enviar formulário " + error.message)
      console.error("Erro ao enviar formulário:", error);
    }
  }
}
