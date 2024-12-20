import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../../core/services/auth.service';
import {StudentsService} from '../../../core/services/students.service';
import {StudentDashboard} from '../../../core/models/student.dashboard';
import {Subject, takeUntil} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {DashboardItemCardComponent} from '../../../components/dashboard-item-card/dashboard-item-card.component';
import {PageState} from '../../../core/models/page.status';
import {PageStateHandlerComponent} from '../../../components/page-state-handler/page-state-handler.component';
import {JsonResponse} from '../../../core/models/jsonResponse';

@Component({
  selector: 'app-student-dashboard-page',
  standalone: true,
  imports: [
    DashboardItemCardComponent,
    PageStateHandlerComponent
  ],
  templateUrl: './student-dashboard-page.component.html',
  styleUrl: './student-dashboard-page.component.css'
})
export class StudentDashboardPageComponent implements OnInit, OnDestroy {
  pageState: PageState = PageState.init;
  dashboard!: StudentDashboard;
  error?: JsonResponse<any>
  protected readonly PageState = PageState;
  private destroy$ = new Subject<void>();

  constructor(private authService: AuthService, private studentService: StudentsService) {
  }

  ngOnInit() {
    this.load();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  load() {
    this.studentService.GetStudentDashboard(this.authService.GetUserId())
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data: StudentDashboard) => {
          this.dashboard = data;
          console.log(data);
          console.log(this.dashboard);
          this.pageState = PageState.Loaded;
        },
        error: (error: HttpErrorResponse) => {
          this.error = error.error;
          this.pageState = PageState.Error;
        }
      })
  }
}
