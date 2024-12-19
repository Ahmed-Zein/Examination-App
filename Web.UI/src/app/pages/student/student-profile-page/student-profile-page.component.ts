import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../../core/services/auth.service';
import {Student} from '../../../core/models/student.model';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {StudentsService} from '../../../core/services/students.service';
import {HttpErrorResponse} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import {ExamResultsTableComponent} from '../../../components/exam-results-table/exam-results-table.component';
import {AppRoles} from '../../../core/config/configuration';
import {ActivatedRoute} from '@angular/router';
import {NotificationService} from '../../../core/services/notification.service';
import {HubConnection} from '@microsoft/signalr';

@Component({
  selector: 'app-student-profile-page',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    FormsModule,
    ExamResultsTableComponent
  ],
  templateUrl: './student-profile-page.component.html',
  styleUrl: './student-profile-page.component.css'
})
export class StudentProfilePageComponent implements OnInit, OnDestroy {
  @Input() student!: Student;
  isLoading = true;
  editing = false;
  studentId!: string;
  signalConnections!: HubConnection;
  signalAction: any;
  protected readonly AppRoles = AppRoles;

  constructor(private authService: AuthService,
              private notificationService: NotificationService,
              private studentService: StudentsService, private route: ActivatedRoute) {
  }

  get canEdit() {
    return this.authService.UserRole() == AppRoles.Student
  }

  ngOnDestroy(): void {
    if (this.signalConnections) {
      this.signalConnections.off("ReceiveNotification", this.signalAction);
    }
  }

  ngOnInit() {
    if (this.authService.UserRole() === AppRoles.Admin) {
      this.handleAdminRole()

    } else {
      this.handleStudentRole()
      this.signalAction = (data: string, level: number) => {
        this.loadStudent(this.studentId);
      }
      this.signalConnections = this.notificationService.Connection;
      this.signalConnections.on("ReceiveNotification", this.signalAction);
    }
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
    this.studentService.GetStudent(studentId).subscribe({
      next: (data: Student) => {
        this.student = data;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        console.error(error);
      },
    })
  }

  toggleEdit() {
    this.editing = !this.editing;
    // TODO: impl user first/last name update
  }
}
