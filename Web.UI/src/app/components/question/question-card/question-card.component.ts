import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {NgClass} from '@angular/common';
import {AdminAnswer, AdminQuestion} from '../../../core/models/question.model';


@Component({
  selector: 'app-question-card',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './question-card.component.html',
  styleUrl: './question-card.component.css'
})
export class QuestionCardComponent {
  @Input({required: true}) question!: AdminQuestion;
  @Input() showDeleteBtn = false;
  @Output() deleteQuestion = new EventEmitter<AdminQuestion>();

  iconClassList(ans: AdminAnswer): string {
    return ans.isCorrect
      ? 'bi-check-circle-fill text-success' : 'bi-circle text-muted'
  }

  onDelete(): void {
    this.deleteQuestion.emit(this.question);
  }
}
