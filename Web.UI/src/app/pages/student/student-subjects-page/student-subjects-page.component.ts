import { NgOptimizedImage } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoadingSpinnerComponent } from "../../../components/shared/loading-spinner/loading-spinner.component";
import { Subject } from '../../../core/models/subject.model';
import { SubjectService } from '../../../core/services/subject.service';

@Component({
  selector: 'app-student-subjects-page',
  standalone: true,
  imports: [NgOptimizedImage, LoadingSpinnerComponent],
  templateUrl: './student-subjects-page.component.html',
  styleUrl: './student-subjects-page.component.css',
})
export class StudentSubjectsPageComponent implements OnInit {
  isLoading= true;
  subjects: Subject[] = [];

  constructor(
    private subjectService: SubjectService,
    private router: Router
  ) {}

  ngOnInit() {
    this.subjectService.GetAllSubjects().subscribe({
      next: (data: any) => {
        this.subjects = data;
        this.isLoading=false;
      },
      error: (error: HttpErrorResponse) => console.error(error),
    });
  }

  goToExamPage(index: number) {
    const subject = this.subjects.at(index);
    if (!subject) {
      alert(`Subject not found at index ${index}`);
      return;
    }
    this.router
      .navigate([`student/subjects/${subject.id}/examination`])
      .then(() => {
        console.log('Navigation successful');
      })
      .catch((error) => {
        console.error('Navigation error:', error);
      });
  }
}
