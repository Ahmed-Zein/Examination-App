import {Component, OnDestroy, OnInit} from '@angular/core';
import {ExamResult} from '../../../core/models/examresults.model';
import {ExamResultsService} from '../../../core/services/examresults.service';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';
import {HttpErrorResponse} from '@angular/common/http';
import {ExamResultsTableComponent} from '../../../components/exam-results-table/exam-results-table.component';
import {NgbPagination} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {Pagination} from '../../../core/models/pagination';
import {NotificationService} from '../../../core/services/notification.service';
import {Subject, takeUntil} from 'rxjs';

@Component({
  selector: 'app-admin-exam-results-page',
  standalone: true,
  imports: [
    LoadingSpinnerComponent,
    ExamResultsTableComponent,
    NgbPagination,
    FormsModule,
  ],
  templateUrl: './admin-exam-results-page.component.html',
  styleUrl: './admin-exam-results-page.component.css'
})
export class AdminExamResultsPageComponent implements OnInit, OnDestroy {
  isLoading = true;
  examResults: ExamResult[] = [];
  pagination!: Pagination<ExamResult[]>;
  page = 1;
  pageSize = 10;
  private signalAction!: (data: string, level: number) => void;
  private destroy$ = new Subject<void>();
  private orderBy?: string;
  private ascending: boolean = false;

  constructor(private examResultsService: ExamResultsService, private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.load();
    this.signalAction = (data: string, level: number) => {
      this.load();
    }
    this.notificationService.OnReceiveNotification(this.signalAction);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.notificationService.DestroyOnReceiveNotification(this.signalAction);
  }

  load() {
    this.examResultsService.GetExamResults(this.page, this.pageSize, this.orderBy, this.ascending)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response: any) => {
          this.pagination = response.data;
          this.examResults = response.data!.data;
          this.isLoading = false;

        },
        error: (error: HttpErrorResponse) => {
          this.isLoading = false;
        }
      })
  }

  changeOrder(sorting: { orderBy: string; asc: boolean }) {
    this.orderBy = sorting.orderBy;
    this.ascending = sorting.asc
    this.load()
  }

  paginationHandler(page: number) {
    this.page = page;
    this.load();
  }
}
