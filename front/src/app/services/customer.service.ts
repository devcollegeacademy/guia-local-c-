import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Customer {
  id: number;
  title: string;
  zipCode: string;
  number: string;
  address: string;
  email: string;
  active: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private _customerSelected: BehaviorSubject<Customer | undefined> = new BehaviorSubject<Customer | undefined>(undefined);
  public customerSelected$: Observable<Customer | undefined> = this._customerSelected.asObservable();

  private _customers: BehaviorSubject<Customer[] | undefined> = new BehaviorSubject<Customer[] | undefined>([]);
  public customers$: Observable<Customer[] | undefined> = this._customers.asObservable();

  private _customersFilter: BehaviorSubject<Customer[] | undefined> = new BehaviorSubject<Customer[] | undefined>([]);
  public customersFilter$: Observable<Customer[] | undefined> = this._customersFilter.asObservable();

  private _inputSearch: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public inputSearch$: Observable<string> = this._inputSearch.asObservable();

  private _selectedCategoryId: BehaviorSubject<number | undefined> = new BehaviorSubject<number | undefined>(undefined);
  public selectedCategoryId$: Observable<number | undefined> = this._selectedCategoryId.asObservable();

  private _customerOption: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public customerOption$: Observable<string> = this._customerOption.asObservable();

  constructor() { }

  setCustomerSelected(customer: Customer | undefined): void {
    this._customerSelected.next(customer);
  }

  setCustomers(customers: Customer[] | undefined): void {
    this._customers.next(customers);
  }

  setCustomersFilter(customersFilter: Customer[] | undefined): void {
    this._customersFilter.next(customersFilter);
  }

  setInputSearch(inputSearch: string): void {
    this._inputSearch.next(inputSearch);
  }

  setSelectedCategoryId(selectedCategoryId: number | undefined): void {
    this._selectedCategoryId.next(selectedCategoryId);
  }

  setCustomerOption(customerOption: string): void {
    this._customerOption.next(customerOption);
  }
}
