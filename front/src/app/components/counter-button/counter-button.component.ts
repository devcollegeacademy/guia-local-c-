import { Component } from '@angular/core';
import { CounterService } from '../../services/counter.service';

@Component({
  selector: 'app-counter-button',
  standalone: true,
  imports: [],
  templateUrl: './counter-button.component.html',
  styleUrl: './counter-button.component.css'
})

export class CounterButtonComponent {
  constructor(private counterService: CounterService) {}

  increment() {
    this.counterService.increment();    
  }
}
