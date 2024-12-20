import {Component, OnDestroy, OnInit} from "@angular/core";
import {FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {ExamSolution, StudentExam} from "../../../core/models/exam.model";
import {ExamService} from "../../../core/services/exam.services";
import {Subscription} from 'rxjs';
import {PageState} from '../../../core/models/page.status';
import {JsonResponse} from '../../../core/models/jsonResponse';
import {PageStateHandlerComponent} from '../../../components/page-state-handler/page-state-handler.component';
import {TimerComponent} from '../../../components/timer/timer.component';


@Component({
  selector: 'app-student-examination-page',
  standalone: true,
  imports: [ReactiveFormsModule, PageStateHandlerComponent, TimerComponent],
  templateUrl: './student-examination-page.component.html',
  styleUrl: './student-examination-page.component.css',
})
export class StudentExaminationPageComponent implements OnInit, OnDestroy {
  pageState = PageState.init;
  studentExam!: StudentExam;
  examinationForm: FormGroup;
  examSubscription!: Subscription;
  subjectId!: string;
  error: JsonResponse<any> | undefined;
  protected readonly PageState = PageState;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private examService: ExamService,
    private activatedRoute: ActivatedRoute) {
    this.examinationForm = this.fb.group({
      answers: this.fb.array([], Validators.required),
    });
  }

  get answersArray(): FormArray {
    return this.examinationForm.get('answers') as FormArray;
  }

  ngOnDestroy(): void {
    this.examSubscription.unsubscribe();
  }

  examTimeInSeconds() {
    // "00:00:00"
    const [hours, minutes, _] = this.studentExam.duration.split(':').map(Number);
    return hours * 60 * 60 + minutes * 60;
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.subjectId = params['subjectId'];
      this.load()
    })
  }

  onExamFinished(): void {
    if (!this.examinationForm.valid) {
      this.examinationForm.markAllAsTouched();
    }
    this.submitSolution();
  }

  submitSolution(): void {
    const examSolution: ExamSolution = {
      examResultId: this.studentExam.examResultId,
      solutions: this.examinationForm.value.answers
    };
    examSolution.solutions = examSolution.solutions.filter(solution => solution.answerId != null) ?? [];
    this.examService.SendStudentSolutions(this.subjectId, this.studentExam.id.toString(), examSolution)
      .subscribe({
        next: (result) => {
          const _ = this.router.navigate(['student/dashboard']);
        },
        error: (err) => {
          console.error(err);
        }
      })
  }

  load() {
    this.pageState = PageState.init;
    this.examSubscription = this.fetchExam();
  }

  protected fetchExam(): Subscription {
    return this.examService.StartStudentExam(this.subjectId).subscribe({
      next: (examData) => {
        this.setupExamForm(examData);
        this.pageState = PageState.Loaded;
      },
      error: (error) => {
        this.error = error.error;
        this.pageState = PageState.Error;
        console.log(error.error);
      }, complete: () => {
      }
    });
  }

  private setupExamForm(examData: StudentExam): void {
    this.studentExam = examData;
    this.studentExam.questions.forEach((question) => {
      const questionFormGroup = this.fb.group({
        questionId: [question.id, Validators.required],
        answerId: [null, Validators.required],
      });
      this.answersArray.push(questionFormGroup);
    });
  }
}
