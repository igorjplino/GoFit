import { Component, OnInit } from '@angular/core';
import { ExerciseService } from '../../core/services/exercise.service';
import { Exercise } from '../../shared/models/exercise';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { MatCard, MatCardContent } from '@angular/material/card';
import { RouterLink } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account.service';
import { Permissions } from '../../core/constants/permissions';

@Component({
  selector: 'app-exercise',
  standalone: true,
  imports: [
    FormsModule,
    MatCard,
    MatCardContent,
    MatFormFieldModule,
    MatIcon,
    MatInputModule,
    MatPaginator,
    MatSelectModule,
    RouterLink
  ],
  templateUrl: './exercise.component.html',
  styleUrl: './exercise.component.scss'
})
export class ExerciseComponent implements OnInit {

  exercises?: Pagination<Exercise>;
  permissions = Permissions;

  constructor(
    private exerciseService: ExerciseService,
    public accountService: AccountService
  ) { }

  ngOnInit(): void {
    this.getExercises();
  }

  getExercises(): void {
    this.exerciseService.getExercises().subscribe({
      next: response => this.exercises = response
    });
  }
}
