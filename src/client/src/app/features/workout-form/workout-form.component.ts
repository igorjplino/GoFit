import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { TextInputComponent } from '../../shared/components/text-input/text-input.component';
import { WorkoutExercisesFormComponent } from '../../shared/components/workout-exercises-form/workout-exercises-form.component';
import { WorkoutDraft, WorkoutExerciseDraft } from '../../shared/models/workout-plan';

@Component({
  selector: 'app-workout-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButton,
    MatIconButton,
    MatCard,
    MatIcon,
    TextInputComponent,
    WorkoutExercisesFormComponent
  ],
  templateUrl: './workout-form.component.html',
  styleUrl: './workout-form.component.scss'
})
export class WorkoutFormComponent implements OnInit {
  @Input({ required: true }) draft!: WorkoutDraft;
  @Output() draftChange = new EventEmitter<WorkoutDraft>();
  @Output() remove = new EventEmitter<void>();

  private fb = inject(FormBuilder);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
    description: ['', [Validators.minLength(3), Validators.maxLength(300)]]
  });

  ngOnInit(): void {
    this.form.patchValue({
      name: this.draft.name,
      description: this.draft.description ?? ''
    }, { emitEvent: false });

    this.form.valueChanges.subscribe(value => {
      this.emitDraft(value.name ?? '', value.description ?? '', this.draft.exercises);
    });
  }

  onExercisesChange(entries: WorkoutExerciseDraft[]): void {
    const { name, description } = this.form.value;
    this.emitDraft(name ?? '', description ?? '', entries);
  }

  private emitDraft(name: string, description: string, exercises: WorkoutExerciseDraft[]): void {
    this.draftChange.emit({
      name,
      description: description ? description : null,
      exercises
    });
  }
}
