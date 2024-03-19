import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private _showModal: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public showModal$: Observable<boolean> = this._showModal.asObservable();

  private _modalAction: BehaviorSubject<'create' | 'update'> = new BehaviorSubject<'create' | 'update'>('create');
  public modalAction$: Observable<'create' | 'update'> = this._modalAction.asObservable();

  constructor() { }

  setShowModal(show: boolean): void {
    this._showModal.next(show);
  }

  setModalAction(action: 'create' | 'update'): void {
    this._modalAction.next(action);
  }
}
