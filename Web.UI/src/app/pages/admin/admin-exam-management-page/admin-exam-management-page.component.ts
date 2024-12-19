import {Component, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators,} from "@angular/forms";
import {AdminExam} from '../../../core/models/exam.model';
import {SubjectService} from '../../../core/services/subject.service';
import {ExamService, UpdateExam} from '../../../core/services/exam.services';
import {ActivatedRoute, Router} from '@angular/router';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {AdminQuestion} from '../../../core/models/question.model';
import {HttpErrorResponse} from '@angular/common/http';
import {NgClass} from '@angular/common';
import {Utils} from '../../../core/utils/utils';

@Component({
  selector: 'app-admin-exam-management-page',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingSpinnerComponent, NgClass, FormsModule],
  templateUrl: './admin-exam-management-page.component.html',
  styleUrls: ['./admin-exam-management-page.component.css'],
})
export class AdminExamManagementPageComponent implements OnInit {
  isExamLoading = true;
  isQuestionsLoading = true;

  exam!: AdminExam;
  minutes!: number;
  hours!: number;
  subjectId!: string;
  subjectQuestions: AdminQuestion[] = [];
  formGroup!: FormGroup;
  editing = false;
  modelName!: string;

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

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.route.paramMap.subscribe((params) => {
      this.subjectId = params.get('subjectId')!;
      const examId = params.get('examId')!;

      this.isExamLoading = true;
      this.isQuestionsLoading = true;

      this.loadExam(this.subjectId, examId);
      this.loadAvailableQuestions();
    });
  }

  loadExam(subjectId: string, examId: string): void {
    this.examService.GetExamById(subjectId, examId).subscribe({
      next: (exam) => {
        this.exam = exam;
        this.isExamLoading = false;
        this.initializeFormArray();
        this.hours = parseInt(this.exam.duration.split(':').at(0)!);
        this.minutes = parseInt(this.exam.duration.split(':').at(1)!);
        this.modelName = this.exam.modelName;
      },
      error: (error) => console.error('Error loading exam:', error),
    });
  }

  loadAvailableQuestions(): void {
    this.subjectService.GetQuestionsBySubjectId(this.subjectId).subscribe({
      next: (questions) => {
        this.subjectQuestions = questions;
        this.isQuestionsLoading = false;
        this.initializeFormArray();
      },
      error: (error: HttpErrorResponse) => console.error('Error loading questions:', error),
    });
  }

  onSubmit(): void {
    if (this.formGroup.invalid || this.selectedQuestions.length === 0) {
      this.formGroup.markAllAsTouched();
      return;
    }
    this.examService.UpdateExamQuestions(this.subjectId, this.exam.id.toString(), this.selectedQuestions).subscribe({
      next: () => this.loadData(),
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
          this.loadData();
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
    if (this.isQuestionsLoading || this.isExamLoading) return;

    const questionsArray = this.fb.array(
      this.subjectQuestions.map((question) =>
        this.fb.control(this.exam.questions.some((q) => q.id === question.id))
      )
    );
    this.formGroup.setControl('questions', questionsArray);
  }
}
