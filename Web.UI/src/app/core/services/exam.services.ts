import {HttpClient} from '@angular/common/http';
import {JsonResponse} from '../models/jsonResponse';
import {map} from 'rxjs';
import {Injectable} from '@angular/core';
import {AdminExam, ExamSolution, StudentExam} from '../models/exam.model';

export interface UpdateExam {
  modelName: string;
  duration: string;
}

@Injectable({providedIn: 'root'})
export class ExamService {

  constructor(private http: HttpClient) {
  }

  public GetExamById(subjectId: string, examId: string) {
    return this.http
      .get<JsonResponse<AdminExam>>(`/subjects/${subjectId}/exams/${examId}`)
      .pipe(map((res) => res.data!));
  }

  public AddExamToSubject(subjectId: string, exam: AdminExam) {
    return this.http.post<JsonResponse<any>>(`/subjects/${subjectId}/exams`, {
      modelName: exam.modelName,
      duration: exam.duration
    });
  }

  public UpdateExamDetails(subjectId: string, examId: string, exam: UpdateExam) {
    return this.http.put(`/subjects/${subjectId}/exams/${examId}`, exam)
  }

  public UpdateExamQuestions(subjectId: string, examId: string, selectedQuestions: number[]) {
    return this.http.put<JsonResponse<any>>(`/subjects/${subjectId}/exams/${examId}/questions`, selectedQuestions);
  }

  public StartStudentExam(subjectId: string) {
    return this.http
      .get<JsonResponse<StudentExam>>(`/subjects/${subjectId}/exams/start`)
      .pipe(map((res) => res.data!));
  }

  public SendStudentSolutions(subjectId: string, examId: string, examSolution: ExamSolution) {
    return this.http.post<any>(`/subjects/${subjectId}/exams/${examId}/evaluation`, examSolution);
  }

  public DeleteExam(subjectId: string, examId: string) {
    return this.http.delete<any>(`/subjects/${subjectId}/exams/${examId}`);
  }
}
