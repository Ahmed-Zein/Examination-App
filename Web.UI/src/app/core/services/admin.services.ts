import {HttpClient} from '@angular/common/http';
import {JsonResponse} from '../models/jsonResponse';
import {Injectable} from '@angular/core';


export interface DashboardData {
  totalStudents: number
  totalSubjects: number
  totalExamsTaken: number
}

@Injectable({providedIn: 'root'})
export class AdminServices {
  constructor(private http: HttpClient) {
  }

  public GetDashboard() {
    return this.http.get<JsonResponse<DashboardData>>('/admin/dashboard');
  }

}
