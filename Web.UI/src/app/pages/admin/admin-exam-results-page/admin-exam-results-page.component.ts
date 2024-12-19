import {Component, OnDestroy, OnInit} from '@angular/core';
import {ExamResult} from '../../../core/models/examresults.model';
import {ExamResultsService} from '../../../core/services/examresults.service';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {HttpErrorResponse} from '@angular/common/http';
import {ExamResultsTableComponent} from '../../../components/exam-results-table/exam-results-table.component';
import {NgbPagination} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {Pagination} from '../../../core/models/pagination';
import {HubConnection} from '@microsoft/signalr';
import {NotificationService} from '../../../core/services/notification.service';

@Component({
  selector: 'app-admin-exam-results-page',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    ExamResultsTableComponent,
    NgbPagination,
    FormsModule
  ],
  templateUrl: './admin-exam-results-page.component.html',
  styleUrl: './admin-exam-results-page.component.css'
})
export class AdminExamResultsPageComponent implements OnInit, OnDestroy {
  page = 1;
  isLoading = true;
  examResults: ExamResult[] = [];
  pagination!: Pagination<ExamResult[]>;
  pageSize = 10;
  private signalAction!: (data: string, level: number) => void;
  private signalConnection!: HubConnection;

  constructor(private examResultsService: ExamResultsService, private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.loadExamResults();
    this.signalAction = (data: string, level: number) => {
      this.loadExamResults();
    }
    this.signalConnection = this.notificationService.Connection;
    this.signalConnection.on("ReceiveNotification", this.signalAction);
  }

  ngOnDestroy(): void {
    this.signalConnection.off("ReceiveNotification", this.signalAction);
  }

  loadExamResults() {
    this.examResultsService.GetExamResults(this.page, this.pageSize).subscribe({
      next: (response: any) => {
        console.log(response);
        this.pagination = response.data;
        this.examResults = response.data!.data;
        this.isLoading = false;

      },
      error: (error: HttpErrorResponse) => {
        console.error(error);
        this.isLoading = false;
      }
    })
  }

  paginationHandler(page: number) {
    this.page = page;
    this.loadExamResults();
  }
}
