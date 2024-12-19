import {HttpErrorResponse} from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import {Student} from '../../../core/models/student.model';
import {StudentsService} from '../../../core/services/students.service';
import {Router} from '@angular/router';
import {NgbPagination} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {Pagination} from '../../../core/models/pagination';
import {LoadingSpinnerComponent} from '../../../components/shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-admin-students-page',
  standalone: true,
  imports: [
    NgbPagination,
    ReactiveFormsModule,
    FormsModule,
    LoadingSpinnerComponent
  ],
  templateUrl: './admin-students-page.component.html',
  styleUrl: './admin-students-page.component.css'
})
export class AdminStudentsPageComponent implements OnInit {
  page = 1;
  pageSize = 10;
  isLoading = true;
  studentsList: Student[] = [];
  pagination!: Pagination<Student[]>;

  constructor(private studentsService: StudentsService, private router: Router) {
  }

  ngOnInit(): void {
    this.loadStudentsList()
  }

  loadStudentsList(): void {
    this.studentsService.GetAllStudents(this.page, this.pageSize).subscribe({
      next: (res: any) => {
        this.pagination = res;
        this.studentsList = res.data;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        console.error(error);
        this.isLoading = false;
      },
    });
  }

  viewStudentPage(studentId: string) {
    this.router.navigate(['admin/students/' + studentId], {})
  }

  changeStudentLock(student: Student): void {
    console.log('page');
    this.studentsService.ChangeStudentLock(student).subscribe(
      {
        next: (stu) => {
          student.isLocked = stu.isLocked;
        }
      }
    )
  }

  paginationHandler(page: number) {
    this.page = page;
    this.loadStudentsList()
  }
}
