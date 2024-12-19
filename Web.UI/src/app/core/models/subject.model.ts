import {AdminQuestion} from './question.model';
import {AdminExam} from './exam.model';

export class Subject {
  id = 0;
  name = '';
  questions: AdminQuestion[] = [];
  exams: AdminExam[] = [];
}
