import {Component, EventEmitter, Input, Output} from '@angular/core';
import {ExamResult} from '../../core/models/examresults.model';
import {DatePipe} from '@angular/common';
import {ExamResultStatusPipePipe} from '../../core/pipes/examResultStatus.pipe';
import {ExamStatusBadgeComponent} from '../badge/exam-status-badge.component';
import {Utils} from '../../core/utils/utils';

@Component({
  selector: 'app-exam-results-table',
  standalone: true,
  imports: [
    DatePipe,
    ExamResultStatusPipePipe,
    ExamStatusBadgeComponent
  ],
  templateUrl: './exam-results-table.component.html',
  styleUrl: './exam-results-table.component.css'
})
export class ExamResultsTableComponent {
  @Input({required: true}) examResults!: ExamResult[];
  @Output() pageEmitter = new EventEmitter<number>();


  protected readonly Utils = Utils;
}