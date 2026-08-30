import { Component, EventEmitter, Input, OnInit, Output, ViewChild, inject, signal } from '@angular/core';
import { FormBuilder, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldModule } from '@angular/material/form-field';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { ExerciseService } from '../../../core/services/exercise.service';
import { Exercise } from '../../models/exercise';
import { WorkoutExerciseDraft } from '../../models/workout-plan';

// This form relies solely on the submit button's disabled state to gate invalid input -
// fields never show as required or in an error state, they just don't let you submit yet.
class NeverErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(): boolean {
    return false;
  }
}

@Component({
  selector: 'app-workout-exercises-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    DragDropModule
  ],
  providers: [
    { provide: ErrorStateMatcher, useClass: NeverErrorStateMatcher },
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { hideRequiredMarker: true } }
  ],
  templateUrl: './workout-exercises-form.component.html',
  styleUrl: './workout-exercises-form.component.scss'
})
export class WorkoutExercisesFormComponent implements OnInit {
  @Input() entries: WorkoutExerciseDraft[] = [];
  @Output() entriesChange = new EventEmitter<WorkoutExerciseDraft[]>();
  @ViewChild('entryFormDirective') private entryFormDirective!: FormGroupDirective;

  private fb = inject(FormBuilder);
  private exerciseService = inject(ExerciseService);

  exercises = signal<Exercise[]>([]);
  editingIndex = signal<number | null>(null);

  entryForm = this.fb.group({
    exerciseId: ['', Validators.required],
    numberOfSets: [3, [Validators.required, Validators.min(1), Validators.max(20)]],
    weight: [0, [Validators.required, Validators.min(0)]],
    repetitions: [10, [Validators.required, Validators.min(1), Validators.max(100)]]
  });

  ngOnInit(): void {
    this.exerciseService.getExercises({ pageSize: 50 }).subscribe({
      next: response => this.exercises.set(response.items)
    });
  }

  onSave(): void {
    if (this.entryForm.invalid) {
      return;
    }

    const { exerciseId, numberOfSets, weight, repetitions } = this.entryForm.value;
    const exerciseName = this.exercises().find(e => e.id === exerciseId)?.name ?? '';

    const draft: WorkoutExerciseDraft = {
      exerciseId: exerciseId!,
      exerciseName,
      numberOfSets: numberOfSets!,
      weight: weight!,
      repetitions: repetitions!,
      order: 0
    };

    const index = this.editingIndex();
    const updated = index === null
      ? [...this.entries, draft]
      : this.entries.map((entry, i) => i === index ? draft : entry);

    this.emitEntries(updated);
    this.resetForm();
  }

  onEdit(index: number): void {
    const entry = this.entries[index];
    if (!entry) {
      return;
    }

    this.editingIndex.set(index);
    this.entryForm.patchValue({
      exerciseId: entry.exerciseId,
      numberOfSets: entry.numberOfSets,
      weight: entry.weight,
      repetitions: entry.repetitions
    });
  }

  onRemove(index: number): void {
    const updated = this.entries.filter((_, i) => i !== index);
    this.emitEntries(updated);

    if (this.editingIndex() === index) {
      this.resetForm();
    }
  }

  onCancelEdit(): void {
    this.resetForm();
  }

  onDrop(event: CdkDragDrop<WorkoutExerciseDraft[]>): void {
    const updated = [...this.entries];
    moveItemInArray(updated, event.previousIndex, event.currentIndex);
    this.emitEntries(updated);
  }

  private emitEntries(entries: WorkoutExerciseDraft[]): void {
    const reordered = entries.map((entry, i) => ({ ...entry, order: i }));
    this.entriesChange.emit(reordered);
  }

  private resetForm(): void {
    this.editingIndex.set(null);
    this.entryFormDirective.resetForm({ exerciseId: '', numberOfSets: 3, weight: 0, repetitions: 10 });
  }
}
