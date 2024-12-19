import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthService} from '../../../core/services/auth.service';
import {StudentsService} from '../../../core/services/students.service';
import {StudentDashboard} from '../../../core/models/student.dashboard';
import {Subscription} from 'rxjs';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {HttpErrorResponse} from '@angular/common/http';
import {
  DashboardItemCardComponent
} from '../../admin/admin-dashboard-page/dashboard-item-card/dashboard-item-card.component';

enum PageState {
  init,
  Error,
  Loaded
}

@Component({
  selector: 'app-student-dashboard-page',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    DashboardItemCardComponent
  ],
  templateUrl: './student-dashboard-page.component.html',
  styleUrl: './student-dashboard-page.component.css'
})
export class StudentDashboardPageComponent implements OnInit, OnDestroy {
  state: PageState = PageState.init;
  dashboard!: StudentDashboard;
  protected readonly PageState = PageState;
  private subscription!: Subscription;

  constructor(private authService: AuthService, private studentService: StudentsService) {
  }

  ngOnInit() {
    this.loadDate();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private loadDate() {
    this.subscription =
      this.studentService.GetStudentDashboard(this.authService.GetUserId()).subscribe({
        next: (data: StudentDashboard) => {
          this.dashboard = data;
          console.log(data);
          console.log(this.dashboard);
          this.state = PageState.Loaded;
        },
        error: (error: HttpErrorResponse) => {
          this.state = PageState.Error;
        }
      })
  }
}
