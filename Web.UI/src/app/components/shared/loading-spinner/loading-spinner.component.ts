import {Component} from '@angular/core';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.css'
})
export class LoadingSpinnerComponent {
}
