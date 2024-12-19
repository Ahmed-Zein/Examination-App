import {AdminQuestion, StudentQuestion} from './question.model';

export class AdminExam {
  id = 0;
  duration = '';
  modelName = '';
  questions: AdminQuestion[] = [];
}

export class StudentExam {
  id = 0;
  duration = '';
  modelName = '';
  examResultId = 0;
  questions: StudentQuestion[] = [];
}

export interface StudentSolution {
  questionId: number;
  answerId: number;
}

export interface ExamSolution {
  examResultId: number;
  solutions: StudentSolution[];
}
