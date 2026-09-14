import { Component, ViewChild, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroupDirective, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { AccountService } from '@gofit/shared/services/account.service';
import { SnackbarService } from '@gofit/shared/services/snackbar.service';
import { TextInputComponent } from '@gofit/shared/components/text-input/text-input.component';

function passwordsMatchValidator(group: AbstractControl): ValidationErrors | null {
  const newPassword = group.get('newPassword')?.value;
  const confirmPassword = group.get('confirmPassword')?.value;
  return newPassword === confirmPassword ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-account-security',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton,
    TextInputComponent
  ],
  templateUrl: './account-security.component.html',
  styleUrl: './account-security.component.scss'
})
export class AccountSecurityComponent {
  @ViewChild('passwordFormDirective') private passwordFormDirective!: FormGroupDirective;

  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private snack = inject(SnackbarService);

  submitting = signal(false);
  validationErrors?: string[];

  passwordForm = this.fb.group({
    currentPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required]]
  }, { validators: passwordsMatchValidator });

  onSubmit(): void {
    if (this.passwordForm.invalid || this.submitting()) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { currentPassword, newPassword } = this.passwordForm.value;

    this.accountService.changePassword({ currentPassword: currentPassword!, newPassword: newPassword! }).subscribe({
      next: () => {
        this.submitting.set(false);
        this.passwordFormDirective.resetForm({ currentPassword: '', newPassword: '', confirmPassword: '' });
        this.snack.success('Password changed successfully');
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }
}
