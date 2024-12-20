import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {JsonResponse} from '../models/jsonResponse';
import {ExamResult} from '../models/examresults.model';
import {Pagination} from '../models/pagination';

@Injectable({providedIn: 'root'})
export class ExamResultsService {
  constructor(private http: HttpClient) {
  }

  public GetExamResults(page: number, pageSize: number, orderBy?: string, ascending = false) {
    let params = new HttpParams();
    params = params.append('page', page);
    params = params.append('pageSize', pageSize);
    if (orderBy) {
      params = params.append('orderBy', orderBy);
      params = params.append('Ascending', ascending);
    }

    return this.http.get<JsonResponse<Pagination<ExamResult>>>(`/students/results`, {params: params});
  }
}
