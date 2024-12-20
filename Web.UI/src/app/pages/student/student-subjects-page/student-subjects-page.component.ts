import {NgOptimizedImage} from '@angular/common';
import {HttpErrorResponse} from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Subject} from '../../../core/models/subject.model';
import {SubjectService} from '../../../core/services/subject.service';
import {PageStateHandlerComponent} from '../../../components/page-state-handler/page-state-handler.component';
import {PageState} from '../../../core/models/page.status';
import {JsonResponse} from '../../../core/models/jsonResponse';

@Component({
  selector: 'app-student-subjects-page',
  standalone: true,
  imports: [NgOptimizedImage, PageStateHandlerComponent],
  templateUrl: './student-subjects-page.component.html',
  styleUrl: './student-subjects-page.component.css',
})
export class StudentSubjectsPageComponent implements OnInit {
  pageState = PageState.init;
  error?: JsonResponse<any>;
  subjects: Subject[] = [];
  protected readonly PageState = PageState;

  constructor(
    private subjectService: SubjectService,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.load();
  }

  load() {
    this.pageState = PageState.init
    this.subjectService.GetAllSubjects().subscribe({
      next: (data: any) => {
        this.subjects = data;
        this.pageState = PageState.Loaded
      },
      error: (error: HttpErrorResponse) => {
        this.pageState = PageState.Error
        this.error = error.error;
      }
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
