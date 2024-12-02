import { Component, OnInit } from '@angular/core';
import { ExerciseService } from '../../core/services/exercise.service';
import { Exercise } from '../../shared/models/exercise';

@Component({
  selector: 'app-exercise',
  standalone: true,
  imports: [],
  templateUrl: './exercise.component.html',
  styleUrl: './exercise.component.scss'
})
export class ExerciseComponent implements OnInit {

  products: Exercise[] = [];

  constructor(private exerciseService: ExerciseService) {}

  ngOnInit(): void {
    this.exerciseService.getExercises().subscribe({
      next: response => this.products = response.data,
      error: error => console.log(error)
    });
  }
}
