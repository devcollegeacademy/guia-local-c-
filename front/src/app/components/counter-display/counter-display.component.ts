import { Component } from '@angular/core';
import { CounterService } from '../../services/counter.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-counter-display',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './counter-display.component.html',
  styleUrl: './counter-display.component.css'
})
export class CounterDisplayComponent {
  count$: Observable<number> | undefined;

  constructor(private counterService: CounterService) { }
  
  ngOnInit() {
    this.count$ = this.counterService.count$;
  }

}
