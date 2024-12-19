import {ExamResult} from './examresults.model';

export class Student {
  isLocked = false;
  id = '';
  email = '';
  lastName = '';
  firstName = '';
  examResults: ExamResult[] = [];
}
