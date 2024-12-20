import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../../core/services/auth.service';
import {Student} from '../../../core/models/student.model';
import {StudentsService} from '../../../core/services/students.service';
import {HttpErrorResponse} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {ExamResultsTableComponent} from '../../../components/exam-results-table/exam-results-table.component';
import {AppRoles} from '../../../core/config/configuration';
import {ActivatedRoute} from '@angular/router';
import {NotificationService} from '../../../core/services/notification.service';
import {PageState} from '../../../core/models/page.status';
import {PageStateHandlerComponent} from '../../../components/page-state-handler/page-state-handler.component';
import {JsonResponse} from '../../../core/models/jsonResponse';
import {Subject, takeUntil} from 'rxjs';

@Component({
  selector: 'app-student-profile-page',
  standalone: true,
  imports: [
    FormsModule,
    ExamResultsTableComponent,
    PageStateHandlerComponent
  ],
  templateUrl: './student-profile-page.component.html',
  styleUrl: './student-profile-page.component.css'
})
export class StudentProfilePageComponent implements OnInit, OnDestroy {
  @Input() student!: Student;
  pageState: PageState = PageState.init;
  error?: JsonResponse<any>;
  editing = false;
  studentId!: string;
  protected readonly PageState = PageState;
  private destroy$ = new Subject<void>();

  constructor(private authService: AuthService,
              private notificationService: NotificationService,
              private studentService: StudentsService, private route: ActivatedRoute) {
  }

  get canEdit() {
    return this.authService.UserRole() == AppRoles.Student
  }

  get notificationAction() {
    return (data: string, level: number) => {
      this.loadStudent(this.studentId);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.notificationService.DestroyOnReceiveNotification(this.notificationAction);
  }

  ngOnInit() {
    this.load()
  }

  handleAdminRole() {
    this.route.paramMap.subscribe(paramMap => {
      this.studentId = paramMap.get('studentId') ?? '';
      this.loadStudent(this.studentId)
    })
  }

  handleStudentRole() {
    this.studentId = this.authService.GetUserId();
    this.loadStudent(this.studentId)
  }

  loadStudent(studentId: string) {
    this.studentService.GetStudent(studentId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data: Student) => {
          this.student = data;
          this.pageState = PageState.Loaded;
        },
        error: (error: HttpErrorResponse) => {
          this.pageState = PageState.Error;
          this.error = error.error;
        },
      })
  }

  toggleEdit() {
    this.editing = !this.editing;
    // TODO: impl user first/last name update
  }

  load() {
    this.pageState = PageState.init;
    this.notificationService.OnReceiveNotification(this.notificationAction);
    if (this.authService.UserRole() === AppRoles.Admin) {
      this.handleAdminRole()
    } else {
      this.handleStudentRole()
    }
  }
}
