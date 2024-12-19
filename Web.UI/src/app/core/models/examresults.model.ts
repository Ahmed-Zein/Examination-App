import {Student} from './student.model';

export class ExamResult {
  examId = 0;
  // studentEmail: number = 0;
  studentId = '';
  student!: Student;
  totalScore = 0;
  studentScore = 0;
  startTime = '';
  endTime: string | null = null;
  status: ExamResultStatus = ExamResultStatus.UnSubmitted;
}

export enum ExamResultStatus {
  UnSubmitted,
  InEvaluation,
  Evaluated
}

