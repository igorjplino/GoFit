import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { AccountService } from '@gofit/shared/services/account.service';
import { SnackbarService } from '@gofit/shared/services/snackbar.service';
import { TextInputComponent } from '@gofit/shared/components/text-input/text-input.component';

@Component({
  selector: 'app-personal-information',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton,
    TextInputComponent
  ],
  templateUrl: './personal-information.component.html',
  styleUrl: './personal-information.component.scss'
})
export class PersonalInformationComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountService = inject(AccountService);
  private snack = inject(SnackbarService);

  loaded = signal(false);
  submitting = signal(false);
  email = signal('');
  validationErrors?: string[];

  profileForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]]
  });

  ngOnInit(): void {
    this.accountService.getProfile().subscribe({
      next: profile => {
        this.email.set(profile.email);
        this.profileForm.patchValue({ name: profile.name });
        this.loaded.set(true);
      }
    });
  }

  onSubmit(): void {
    if (this.profileForm.invalid || this.submitting()) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { name } = this.profileForm.value;

    this.accountService.updateProfile({ name: name! }).subscribe({
      next: profile => {
        this.submitting.set(false);
        this.profileForm.patchValue({ name: profile.name });
        this.profileForm.markAsPristine();
        this.snack.success('Profile updated successfully');
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }
}
