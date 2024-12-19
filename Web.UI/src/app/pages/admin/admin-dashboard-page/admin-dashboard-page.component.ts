import {Component, OnInit} from '@angular/core';
import {AdminServices, DashboardData} from '../../../core/services/admin.services';
import {HttpErrorResponse} from '@angular/common/http';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {DashboardItemCardComponent} from './dashboard-item-card/dashboard-item-card.component';

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
export class AdminDashboardPageComponent implements OnInit {
  isLoading = true;

  dashboardData!: DashboardData;

  constructor(private adminService: AdminServices) {
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.adminService.GetDashboard().subscribe({
      next: (data: any) => {
        this.dashboardData = data;
        this.isLoading = false;
        console.log(data)
      },
      error: (error: HttpErrorResponse) => console.error(error),
    })
  }
}
