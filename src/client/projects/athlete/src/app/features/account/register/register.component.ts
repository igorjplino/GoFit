import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { AccountService } from '@gofit/shared/services/account.service';
import { SnackbarService } from '@gofit/shared/services/snackbar.service';
import { Router } from '@angular/router';
import { TextInputComponent } from '@gofit/shared/components/text-input/text-input.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton,
    TextInputComponent
],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private route = inject(Router);
  private snack = inject(SnackbarService);

  validationErrors?: string[];
  submitting = signal(false);
  
  registerForm: FormGroup = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
  });

  onSubmit() {
    if (this.registerForm.invalid || this.submitting()) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.submitting.set(false);
        this.snack.success('Registration successful');
        this.route.navigateByUrl('/account/login');
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    })
  }
}
