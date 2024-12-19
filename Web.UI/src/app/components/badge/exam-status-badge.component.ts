import {Component, Input} from '@angular/core';
import {ExamResultStatus} from '../../core/models/examresults.model';

@Component({
  selector: 'app-badge',
  standalone: true,
  imports: [],
  templateUrl: './exam-status-badge.component.html',
  styleUrl: './exam-status-badge.component.css'
})
export class ExamStatusBadgeComponent {
  @Input({required: true}) status!: ExamResultStatus;

  get backgroundColor(): string {
    switch (this.status) {
      case ExamResultStatus.Evaluated:
        return '#e6f4ea'; // Green background
      case ExamResultStatus.UnSubmitted:
        return '#fdecea'; // Red background
      case ExamResultStatus.InEvaluation:
        return '#fff4e5'; // Yellow background
      default:
        return '#ffffff'; // Default background
    }
  }

  get styleColor(): string {
    switch (this.status) {
      case ExamResultStatus.Evaluated:
        return '#4caf50'; // Green text
      case ExamResultStatus.UnSubmitted:
        return '#d9534f'; // Red text
      case ExamResultStatus.InEvaluation:
        return '#ff9800'; // Orange text
      default:
        return '#000000'; // Default text
    }
  }
}
