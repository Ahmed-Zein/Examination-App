import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {JsonResponse} from '../models/jsonResponse';
import {ExamResult} from '../models/examresults.model';
import {Pagination} from '../models/pagination';

@Injectable({providedIn: 'root'})
export class ExamResultsService {
  constructor(private http: HttpClient) {
  }

  public GetExamResults(page: number, pageSize: number) {
    return this.http.get<JsonResponse<Pagination<ExamResult>>>(`/students/results?Page=${page}&pageSize=${pageSize}`);
  }
}
