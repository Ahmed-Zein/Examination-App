import { Component, Input } from '@angular/core';
import { AdminQuestion } from '../../../core/models/question.model';
import { NgbAccordionBody, NgbAccordionButton, NgbAccordionCollapse, NgbAccordionDirective, NgbAccordionHeader, NgbAccordionItem } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-question-list',
  standalone: true,
  imports: [
    NgbAccordionBody,
    NgbAccordionButton,
    NgbAccordionCollapse,
    NgbAccordionDirective,
    NgbAccordionHeader,
    NgbAccordionItem
  ],
  templateUrl: './admin-question-list.component.html',
  styleUrl: './admin-question-list.component.css',
})
export class AdminQuestionListComponent {
  @Input() questions: AdminQuestion[] = [];
}
