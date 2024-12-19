class QuestionBase {
  id = 0;
  text = '';

}

class AnswerBase {
  id = 0;
  text = '';
}

export class AdminQuestion extends QuestionBase {
  answers: AdminAnswer[] = [];
}

export class AdminAnswer extends AnswerBase {
  isCorrect = false;
}

export class StudentQuestion {
  id = 0;
  text = '';
  answers: StudentAnswer[] = [];
}

export class StudentAnswer extends AnswerBase {
}

export class QuestionDto {
  text = '';
  answers: AnswerDto[] = [];
}

export class AnswerDto {
  text = '';
  isCorrect = false;
}
