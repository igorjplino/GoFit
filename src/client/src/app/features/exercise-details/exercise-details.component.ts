import { Component, inject } from '@angular/core';
import { Exercise } from '../../shared/models/exercise';
import { ExerciseService } from '../../core/services/exercise.service';
import { ActivatedRoute } from '@angular/router';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-exercise-details',
  standalone: true,
  imports: [MatCard],
  templateUrl: './exercise-details.component.html',
  styleUrl: './exercise-details.component.scss'
})
export class ExerciseDetailsComponent {
  exercise!: Exercise;

  private exerciseService = inject(ExerciseService);
  private activatedRoute = inject(ActivatedRoute);

  constructor() { }

  ngOnInit(): void {
    this.loadExercise();
  }

  loadExercise(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (!id) {
      return;
    }

    this.exerciseService.getExercise(id).subscribe({
      next: response => this.exercise = response
    });
  }
}
