import {Component, EventEmitter, Input, Output} from '@angular/core';
import {PageState} from '../../core/models/page.status';
import {LoadingSpinnerComponent} from '../shared/loading-spinner/loading-spinner.component';
import {ErrorDisplayComponent} from '../error-display/error-display.component';
import {JsonResponse} from '../../core/models/jsonResponse';

@Component({
  selector: 'app-page-state-handler',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    ErrorDisplayComponent
  ],
  templateUrl: './page-state-handler.component.html',
  styleUrl: './page-state-handler.component.css'
})
export class PageStateHandlerComponent {
  @Input({required: true}) pageState!: PageState;
  @Input({required: true}) error: JsonResponse<any> | undefined;
  @Output() retryAction = new EventEmitter<void>();
  protected readonly PageState = PageState;

  get title() {
    return this.error?.message ?? 'Error';
  }

  get details() {
    if (this.error && this.error?.errors.length > 1) {
      this.error?.errors.pop();
    }
    return this.error?.errors.join(', ') ?? 'Error';
  }

  onRetry() {
    this.retryAction.emit();
  }
}
