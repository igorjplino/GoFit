import { Component, OnInit } from '@angular/core';
import { ExerciseService } from '../../core/services/exercise.service';
import { Exercise } from '../../shared/models/exercise';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { MatCard, MatCardContent } from '@angular/material/card';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-exercise',
  standalone: true,
  imports: [
    MatCard,
    MatCardContent,
    MatPaginator,
    RouterLink
  ],
  templateUrl: './exercise.component.html',
  styleUrl: './exercise.component.scss'
})
export class ExerciseComponent implements OnInit {

  exercises?: Pagination<Exercise>;

  constructor(private exerciseService: ExerciseService) { }

  ngOnInit(): void {
    this.getExercises();
  }

  getExercises(): void {
    this.exerciseService.getExercises().subscribe({
      next: response => this.exercises = response
    });
  }
}
