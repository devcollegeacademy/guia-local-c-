import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CounterService {
  private _count: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  public count$: Observable<number> = this._count.asObservable();

  constructor() { }

  increment(): void {
    const currentCount = this._count.getValue();
    this._count.next(currentCount + 1);
  }
}
