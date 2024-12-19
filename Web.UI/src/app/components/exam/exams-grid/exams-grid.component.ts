import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgClass } from '@angular/common';
import { AdminExam } from '../../../core/models/exam.model';
import { DurationFormatPipe } from '../../../core/pipes/DurationFormat.pipe';

@Component({
  selector: 'app-exams-grid',
  standalone: true,
  imports: [NgClass, DurationFormatPipe],
  templateUrl: './exams-grid.component.html',
  styleUrl: './exams-grid.component.css',
})
export class ExamsGridComponent {
  @Input() exams: AdminExam[] = [];

  constructor(private activatedRoute: ActivatedRoute, private router: Router) {}

  onExamClick(id: number) {
    this.router
      .navigate(['exams', id.toString()], { relativeTo: this.activatedRoute })
      .then((r) => console.log(r))
      .catch((err) => console.error(err));
  }
}
