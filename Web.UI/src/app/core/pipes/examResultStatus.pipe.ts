import {Pipe, PipeTransform} from '@angular/core';
import {ExamResultStatus} from '../models/examresults.model';

@Pipe({
  standalone: true,
  name: 'ExamResultStatusPipe'
})
export class ExamResultStatusPipePipe implements PipeTransform {
  transform(value: ExamResultStatus): string {
    switch (value) {
      case ExamResultStatus.Evaluated:
        return 'Evaluated';
      case ExamResultStatus.InEvaluation:
        return 'In Evaluation';
      case ExamResultStatus.UnSubmitted:
        return 'Unsubmitted';
    }
  }
}
