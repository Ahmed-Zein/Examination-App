import {HttpErrorResponse} from '@angular/common/http';
import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, Validators} from '@angular/forms';
import {QuestionDto} from '../../../core/models/question.model';
import {SubjectService} from '../../../core/services/subject.service';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-admin-question-form',
  templateUrl: './admin-question-form.component.html',
  styleUrl: './admin-question-form.component.css',
  imports: [FormsModule],
  standalone: true,
})
export class QuestionFormComponent implements OnInit {
  @Output() submit = new EventEmitter<void>();
  currentQuestion: QuestionDto = {
    text: '',
    answers: [{text: '', isCorrect: false}],
  };
  formGroup: FormGroup;
  questions: QuestionDto[] = [];
  subjectId!: string;

  constructor(
    private subjectService: SubjectService,
    private fb: FormBuilder,
    private router: ActivatedRoute
  ) {
    this.formGroup = this.fb.group({
      questions: this.fb.array([], Validators.required),
    });
  }

  ngOnInit(): void {
    this.router.paramMap.subscribe((params) => {
      this.subjectId = params.get('subjectId')!;
    });
  }

  addAnswer() {
    this.currentQuestion.answers.push({text: '', isCorrect: false});
  }

  removeAnswer(index: number) {
    this.currentQuestion.answers.splice(index, 1);
  }

  validateCurrentQuestion(): boolean {
    return (this.currentQuestion.text.trim().length > 0 &&
      this.currentQuestion.answers.length > 0 &&
      this.currentQuestion.answers.find(e => e.isCorrect && e.text.trim())) as boolean
  }

  addQuestion() {
    if (this.validateCurrentQuestion()) {
      console.log(this.currentQuestion);
      this.questions.push({...this.currentQuestion});
      this.resetCurrentQuestion();
    } else {
      alert('Please fill in the question and at least one correct answer.');
    }
  }

  resetCurrentQuestion() {
    this.currentQuestion = {
      text: '',
      answers: [{text: '', isCorrect: false}],
    };
  }

  submitForm() {
    console.log('Submitted Questions:', this.questions);
    this.subjectService.AddQuestion(this.subjectId, this.questions).subscribe({
      next: (_) => {
        this.submit.emit();
        this.questions = [];
      },
      error: (error: HttpErrorResponse) => console.error(error),
    });
  }

  popFromPreview(i: number) {
    this.questions = this.questions.filter((_, j) => j != i);
  }
}
