import {HttpClient} from '@angular/common/http';
import {JsonResponse} from '../models/jsonResponse';
import {map} from 'rxjs';
import {Injectable} from '@angular/core';
import {Student} from '../models/student.model';
import {Pagination} from '../models/pagination';
import {StudentDashboard} from '../models/student.dashboard';
import {StudentNotification} from '../models/notification.student.model';

@Injectable({providedIn: 'root'})
export class StudentsService {
  constructor(private http: HttpClient) {
  }

  public GetStudentDashboard(studentId: string) {
    return this.http
      .get<JsonResponse<StudentDashboard>>(`/students/${studentId}/dashboard`)
      .pipe(map((res) => res.data!));

  }

  public GetAllStudents(page: number, pageSize: number) {
    return this.http.get<JsonResponse<Pagination<Student[]>>>(`/students?page=${page}&pageSize=${pageSize}`).pipe(
      map((res: JsonResponse<Pagination<Student[]>>) => {
        return res.data;
      })
    )
  }

  public GetStudent(studentId: string) {
    return this.http.get<JsonResponse<Student>>(`/students/${studentId}`).pipe(map((res) => res.data!));
  }

  public UpdateStudentLockStatus(student: Student) {
    return this.http.put<JsonResponse<Student>>(`/students/lock/${student.id}`, student).pipe(
      map((res: JsonResponse<Student>) => {
        return res.data!;
      })
    )
  }

  public GetStudentNotifications(studentId: string) {
    return this.http
      .get<JsonResponse<StudentNotification[]>>(`/students/${studentId}/notifications`)
      .pipe(map((res) => res.data!));

  }

  public DeleteStudentNotifications(studentId: string, notificationId: string) {
    return this.http
      .delete<void>(`/students/${studentId}/notifications/${notificationId}`);

  }
}
