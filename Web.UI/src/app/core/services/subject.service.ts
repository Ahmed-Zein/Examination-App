import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs';
import {JsonResponse} from '../models/jsonResponse';
import {Subject} from '../models/subject.model';
import {AdminQuestion, QuestionDto} from '../models/question.model';

@Injectable({providedIn: 'root'})
export class SubjectService {
  constructor(private http: HttpClient) {
  }

  public GetAllSubjects() {
    return this.http
      .get<JsonResponse<Subject[]>>('/subjects')
      .pipe(map((res) => res.data!));
  }

  public GetSubjectById(subjectId: string) {
    return this.http
      .get<JsonResponse<Subject>>(`/subjects/${subjectId}`)
      .pipe(map((res) => res.data!));
  }

  public GetQuestionsBySubjectId(subjectId: string) {
    return this.http
      .get<JsonResponse<AdminQuestion[]>>(`/subjects/${subjectId}/questions`)
      .pipe(map((res) => res.data!));
  }


  public AddQuestion(subjectId: string, questions: QuestionDto[]) {
    return this.http.post<JsonResponse<any>>(
      `/subjects/${subjectId}/questions`,
      questions
    );
  }

  public UpdateSubjectName(subjectId: string, name: string) {
    return this.http.put<JsonResponse<any>>(
      `/subjects/${subjectId}`,
      {name: name},
    );
  }

  public DeleteSubject(subjectId: string) {
    return this.http.delete<JsonResponse<any>>(
      `/subjects/${subjectId}`,
    );
  }

  public AddSubject(subjectName: string) {
    return this.http.post<JsonResponse<any>>(
      `/subjects/`, [{name: subjectName}],
    );
  }

  public DeleteQuestion(subjectId: string, questionId: string) {
    return this.http.delete(`/subjects/${subjectId}/questions/${questionId}`);
  }
}
