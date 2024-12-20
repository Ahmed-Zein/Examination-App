import {Component, OnDestroy, OnInit} from '@angular/core';
import {AdminServices, DashboardData} from '../../../core/services/admin.services';
import {HttpErrorResponse} from '@angular/common/http';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {DashboardItemCardComponent} from '../../../components/dashboard-item-card/dashboard-item-card.component';
import {Subject, takeUntil} from 'rxjs';

@Component({
  selector: 'app-admin-dashboard-page',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    DashboardItemCardComponent
  ],
  templateUrl: './admin-dashboard-page.component.html',
  styleUrl: './admin-dashboard-page.component.css'
})
export class AdminDashboardPageComponent implements OnInit, OnDestroy {
  isLoading = true;
  dashboardData!: DashboardData;
  destroy$ = new Subject<void>();

  constructor(private adminService: AdminServices) {
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.adminService.GetDashboard()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data: any) => {
          this.dashboardData = data;
          this.isLoading = false;
          console.log(data)
        },
        error: (error: HttpErrorResponse) => console.error(error),
      })
  }
}
