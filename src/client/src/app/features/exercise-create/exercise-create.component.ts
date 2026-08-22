import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { ExerciseService } from '../../core/services/exercise.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';

@Component({
  selector: 'app-exercise-create',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCard,
    MatButton,
    TextInputComponent
  ],
  templateUrl: './exercise-create.component.html',
  styleUrl: './exercise-create.component.scss'
})
export class ExerciseCreateComponent {
  private fb = inject(FormBuilder);
  private exerciseService = inject(ExerciseService);
  private router = inject(Router);
  private snack = inject(SnackbarService);

  submitting = signal(false);
  validationErrors?: string[];

  exerciseForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  onSubmit(): void {
    if (this.exerciseForm.invalid || this.submitting()) {
      return;
    }

    this.submitting.set(true);
    this.validationErrors = undefined;

    const { name, description } = this.exerciseForm.value;

    this.exerciseService.createExercise({
      name: name!,
      description: description ? description : null
    }).subscribe({
      next: id => {
        this.snack.success('Exercise created successfully');
        this.router.navigateByUrl(`/exercise/${id}`);
      },
      error: errors => {
        this.submitting.set(false);
        this.validationErrors = Array.isArray(errors) ? errors : undefined;
      }
    });
  }
}
