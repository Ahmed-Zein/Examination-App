import {NgClass} from '@angular/common';
import {HttpErrorResponse} from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {AddNewExamFormComponent} from '../../../components/exam/add-new-exam-form/add-new-exam-form.component';
import {AdminExam} from '../../../core/models/exam.model';
import {AdminQuestion} from '../../../core/models/question.model';
import {Subject} from '../../../core/models/subject.model';
import {ExamService} from '../../../core/services/exam.services';
import {SubjectService} from '../../../core/services/subject.service';
import {ExamsGridComponent} from '../../../components/exam/exams-grid/exams-grid.component';
import {QuestionFormComponent} from "../../../components/question/admin-question-form/admin-question-form.component";
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {QuestionCardComponent} from '../../../components/question/question-card/question-card.component';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-admin-subject-details-page',
  standalone: true,
  imports: [NgClass, ExamsGridComponent, AddNewExamFormComponent, QuestionFormComponent, ReactiveFormsModule, FormsModule, QuestionCardComponent, LoadingSpinnerComponent],
  templateUrl: './admin-subject-details-page.component.html',
  styleUrl: './admin-subject-details-page.component.css',
})
export class AdminSubjectDetailsPageComponent implements OnInit {
  isSubjectLoading = true;
  isFirstTab = true;
  subject!: Subject;
  subjectId: string | null = null;
  questions: AdminQuestion[] = [];
  exams: AdminExam[] = [];
  editing = false;

  constructor(
    private subjectService: SubjectService,
    private examService: ExamService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  addExam(exam: AdminExam) {
    this.examService.AddExamToSubject(this.subjectId!, exam).subscribe({
      next: (_) => {
        this.loadSubject();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.subjectId = params.get('subjectId');
    });
    this.loadSubject();
  }

  loadSubject() {
    this.subjectService.GetSubjectById(this.subjectId ?? '0').subscribe({
      next: (data: any) => {
        this.subject = data;
        this.questions = data.questions;
        this.isSubjectLoading = false;
        this.exams = data.exams;
      },
      error: (error: HttpErrorResponse) => console.error(error),
    });
  }

  onQuestionSubmit() {
    this.subjectService.GetQuestionsBySubjectId(this.subjectId ?? '0').subscribe({
      next: (data: any) => {
        this.questions = data;
      },
      error: (error: HttpErrorResponse) => console.error(error),
    });
  }

  changeTab(isFirstTab: boolean) {
    console.log(this.isFirstTab);
    this.isFirstTab = isFirstTab;
  }

  toggleEdit() {
    if (this.editing) {
      this.subjectService.UpdateSubjectName(this.subject.id.toString(), this.subject.name).subscribe({})
    }
    this.editing = !this.editing;
  }

  deleteSubject() {
    this.subjectService.DeleteSubject(this.subject.id.toString()).subscribe({
      next: async () => {
        await this.router.navigate(['/admin/dashboard']);
      }
    })
  }

  deleteQuestion(question: AdminQuestion) {
    this.subjectService.DeleteQuestion(this.subjectId!, question.id.toString()).subscribe({
      next: () => {
        this.loadSubject();
      }
    })
  }
}
