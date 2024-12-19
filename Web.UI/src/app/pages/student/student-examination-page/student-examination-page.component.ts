import {Component, OnInit} from "@angular/core";
import {FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {LoadingSpinnerComponent} from "../../../components/shared/loading-spinner/loading-spinner.component";
import {ExamSolution, StudentExam} from "../../../core/models/exam.model";
import {ExamService} from "../../../core/services/exam.services";
import {TimerComponent} from '../../../components/timer/timer.component';
import {NotificationService} from '../../../core/services/notification.service';


@Component({
  selector: 'app-student-examination-page',
  standalone: true,
  imports: [LoadingSpinnerComponent, ReactiveFormsModule, TimerComponent],
  templateUrl: './student-examination-page.component.html',
  styleUrl: './student-examination-page.component.css',
})
export class StudentExaminationPageComponent implements OnInit {
  isLoading = true;
  studentExam!: StudentExam;
  examinationForm: FormGroup
  subjectId!: string;

  constructor(
    private examService: ExamService,
    private notficationService: NotificationService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.examinationForm = this.fb.group({
      answers: this.fb.array([], Validators.required),
    });
  }

  get answersArray(): FormArray {
    return this.examinationForm.get('answers') as FormArray;
  }

  examTimeInSeconds() {
    // "00:00:00"
    const [hours, minutes, _] = this.studentExam.duration.split(':').map(Number);
    return hours * 60 * 60 + minutes * 60;
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.subjectId = params['subjectId'];
      this.fetchExam(this.subjectId);
    });
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
    console.log(examSolution);
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

  private fetchExam(subjectId: string): void {
    this.examService.StartStudentExam(subjectId).subscribe({
      next: (examData) => {
        this.setupExam(examData);
      },
      error: () => (this.isLoading = false),
    });
  }

  private setupExam(examData: StudentExam): void {
    this.studentExam = examData;
    this.studentExam.questions.forEach((question) => {
      const questionFormGroup = this.fb.group({
        questionId: [question.id, Validators.required],
        answerId: [null, Validators.required],
      });
      this.answersArray.push(questionFormGroup);
    });
    this.isLoading = false;
  }
}
