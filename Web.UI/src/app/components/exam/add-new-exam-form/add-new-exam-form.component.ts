import {Component, EventEmitter, Output} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AdminExam} from '../../../core/models/exam.model';
import {Utils} from '../../../core/utils/utils';

@Component({
  selector: 'app-add-new-exam-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,

  ],
  templateUrl: './add-new-exam-form.component.html',
  styleUrl: './add-new-exam-form.component.css'
})
export class AddNewExamFormComponent {
  @Output() submitExam = new EventEmitter<AdminExam>();
  isSubmitting = false;
  formCtrl = new FormGroup(
    {
      modelName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      hours: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(4)]),
      minutes: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(59)]),
    }
  )

  onSubmit() {
    this.isSubmitting = true;
    if (this.formCtrl.invalid) {
      console.error(this.formCtrl.errors);
      this.isSubmitting = false;
      return;
    }

    const exam = new AdminExam();
    const {modelName, hours, minutes} = this.formCtrl.value;
    exam.modelName = modelName!;
    exam.duration = `${Utils.leftPad(hours!, 2)}:${Utils.leftPad(minutes!, 2)}:00`;
    console.log(exam);
    this.isSubmitting = false;
    this.submitExam.emit(exam);
  }
}

