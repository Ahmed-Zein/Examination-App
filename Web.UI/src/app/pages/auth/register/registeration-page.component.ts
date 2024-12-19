import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../core/services/auth.service';
import {AppRoles} from '../../../core/config/configuration';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './registration-page.component.html',
  styleUrl: './registration-page.component.css'
})
export class RegistrationPageComponent {

  isLoading = false;
  formGroup: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.formGroup = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
    });
  }

  isInValidFormControl(formName: string): boolean {
    return ((this.formGroup.get(formName)?.invalid && this.formGroup.get(formName)?.touched) as boolean);
  }

  onSubmit() {
    if (!this.formGroup.valid) {
      this.formGroup.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    this.authService.Register(this.formGroup.value).subscribe({
      next: () => {
        this.isLoading = false;
        console.log('Successful register');

        if (this.authService.UserRole() === AppRoles.Admin) {
          const _ = this.router.navigate(['/admin/dashboard']);
        } else if (this.authService.UserRole() === AppRoles.Student) {
          const _ = this.router.navigate(['/student/dashboard']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.log('Unsuccessful Login', error);
      },
    });
  }
}
