import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-error-display',
  templateUrl: './error-display.component.html',
  styleUrls: ['./error-display.component.css'],
  standalone: true
})
export class ErrorDisplayComponent {
  @Input({required: true}) title!: string;
  @Input({required: true}) message!: string;
  @Output() retry = new EventEmitter<void>();

  onRetry(): void {
    this.retry.emit();
  }
}
