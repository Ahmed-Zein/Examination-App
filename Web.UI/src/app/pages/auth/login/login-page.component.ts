import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../../core/services/auth.service';
import {AppRoles} from '../../../core/config/configuration';
import {NgIf} from '@angular/common';
import {Button} from 'primeng/button';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, NgIf, Button],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export class LoginPageComponent {
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
    });
  }

  onSubmit() {
    if (!this.formGroup.valid) {
      return;
    }
    this.isLoading = true;
    this.authService.Login(this.formGroup.value).subscribe({
      complete: () => {
        this.isLoading = false;
        console.log('Successful logged in');

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
