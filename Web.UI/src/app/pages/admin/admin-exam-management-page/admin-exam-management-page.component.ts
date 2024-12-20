import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators,} from "@angular/forms";
import {AdminExam} from '../../../core/models/exam.model';
import {SubjectService} from '../../../core/services/subject.service';
import {ExamService, UpdateExam} from '../../../core/services/exam.services';
import {ActivatedRoute, Router} from '@angular/router';
import {AdminQuestion} from '../../../core/models/question.model';
import {NgClass} from '@angular/common';
import {Utils} from '../../../core/utils/utils';
import {PageState} from '../../../core/models/page.status';
import {forkJoin, Observable, Subject, switchMap, takeUntil} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {JsonResponse} from '../../../core/models/jsonResponse';
import {PageStateHandlerComponent} from '../../../components/page-state-handler/page-state-handler.component';

@Component({
  selector: 'app-admin-exam-management-page',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass, FormsModule, PageStateHandlerComponent],
  templateUrl: './admin-exam-management-page.component.html',
  styleUrls: ['./admin-exam-management-page.component.css'],
})
export class AdminExamManagementPageComponent implements OnInit, OnDestroy {
  pageState = PageState.init
  exam!: AdminExam;
  minutes!: number;
  hours!: number;
  subjectId!: string;
  subjectQuestions: AdminQuestion[] = [];
  formGroup!: FormGroup;
  editing = false;
  modelName!: string;
  protected error?: JsonResponse<any>;
  protected readonly PageState = PageState;
  private destroy$ = new Subject<void>();

  constructor(
    private subjectService: SubjectService,
    private examService: ExamService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.initializeFormGroup();
  }

  get questionsFormArray(): FormArray {
    return this.formGroup.get('questions') as FormArray;
  }

  get selectedQuestions(): number[] {
    return this.formGroup.value.questions
      .map((checked: boolean, index: number) => (checked ? this.subjectQuestions[index].id : null))
      .filter((id: number | null): id is number => id !== null);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  examDuration(duration: string) {
    const hours = parseInt(duration.split(':').at(0)!);
    const minutes = parseInt(duration.split(':').at(1)!);
    return [hours, minutes];
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.pageState = PageState.init
    this.route.paramMap
      .pipe(
        takeUntil(this.destroy$),
        switchMap((params) => {
          this.subjectId = params.get('subjectId')!;
          const examId = params.get('examId')!;
          return forkJoin({
            exam: this.loadExam(this.subjectId, examId),
            questions: this.loadAvailableQuestions(),
          });
        })
      )
      .subscribe({
        next: (data) => {
          this.pageState = PageState.Loaded;
          console.log("DEBUG POINT 84")
          this.exam = data.exam;
          this.modelName = this.exam.modelName;
          [this.hours, this.minutes] = this.examDuration(data.exam.duration)
          //
          this.subjectQuestions = data.questions;
          this.initializeFormArray();
        },
        error: (error: HttpErrorResponse) => {
          this.error = error.error;
          this.pageState = PageState.Error;
        },
      });

  }

  loadExam(subjectId: string, examId: string) {
    return this.examService.GetExamById(subjectId, examId);
  }

  loadAvailableQuestions(): Observable<AdminQuestion[]> {
    return this.subjectService.GetQuestionsBySubjectId(this.subjectId);
  }

  onSubmit(): void {
    if (this.formGroup.invalid || this.selectedQuestions.length === 0) {
      this.formGroup.markAllAsTouched();
      return;
    }
    this.examService.UpdateExamQuestions(this.subjectId, this.exam.id.toString(), this.selectedQuestions).subscribe({
      next: () => this.load(),
      error: (err) => console.error('Error updating exam:', err),
    });
  }

  toggleEdit() {
    if (this.editing) {
      const updatedExam: UpdateExam = {
        modelName: this.modelName, duration: `${Utils.leftPad(this.hours, 2)}:${Utils.leftPad(this.minutes, 2)}:00`
      };
      console.log(updatedExam);
      this.examService.UpdateExamDetails(this.subjectId, this.exam.id.toString(), updatedExam).subscribe({
        next: (result) => {
          this.load();
        },
        error: (err) => {
          console.log('Error updating exam:', err)
        }
      })
    }
    this.editing = !this.editing;
  }

  deleteExam() {
    this.examService.DeleteExam(this.subjectId, this.exam.id.toString()).subscribe({
      next: (data: any) => {
        this.router.navigate(['admin/subjects/' + this.subjectId]);
      },
      error: (err: any) => {
        console.error('Error deleting exam:', err);
      }
    })
  }

  private initializeFormGroup(): void {
    this.formGroup = this.fb.group({
      questions: this.fb.array([], [Validators.required]),
    });
  }

  private initializeFormArray(): void {
    const questionsArray = this.fb.array(
      this.subjectQuestions.map((question) =>
        this.fb.control(this.exam.questions.some((q) => q.id === question.id))
      )
    );
    this.formGroup.setControl('questions', questionsArray);
  }
}
