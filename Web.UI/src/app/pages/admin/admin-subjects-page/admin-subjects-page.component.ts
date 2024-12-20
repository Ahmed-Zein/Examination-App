import {HttpErrorResponse} from '@angular/common/http';
import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Subject} from '../../../core/models/subject.model';
import {SubjectService} from '../../../core/services/subject.service';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgOptimizedImage} from '@angular/common';
import {Subject as RxSubject, takeUntil} from 'rxjs';

@Component({
  selector: 'app-admin-subjects-page',
  standalone: true,
  imports: [LoadingSpinnerComponent, FormsModule, ReactiveFormsModule, NgOptimizedImage],
  templateUrl: './admin-subjects-page.component.html',
  styleUrl: './admin-subjects-page.component.css',
})
export class AdminSubjectsPageComponent implements OnInit, OnDestroy {
  isLoading = true;
  subjects: Subject[] = [];
  subjectNameForm: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(3)]),
  })
  private destroy$ = new RxSubject<void>();

  constructor(private subjectService: SubjectService, private router: Router) {
  }

  ngOnInit() {
    this.loadSubjects();
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  goToSubject(subjectId: number) {
    this.router.navigate(['admin/subjects', subjectId]).then((r) => true);
  }

  addSubject() {
    if (this.subjectNameForm.invalid) {
      this.subjectNameForm.markAllAsTouched();
      return;
    }
    this.subjectService
      .AddSubject(this.subjectNameForm.value.name)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (_: any) => {
          this.loadSubjects();
        },
        error: (error: HttpErrorResponse) => console.error(error),
        complete: () => {
        }
      })
  }

  private loadSubjects() {
    this.subjectService.GetAllSubjects().subscribe({
      next: (data: Subject[]) => {
        this.subjects = data;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        console.error(error);
      },
    });
  }
}
