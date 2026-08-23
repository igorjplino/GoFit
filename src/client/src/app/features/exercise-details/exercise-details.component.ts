import { Component, inject } from '@angular/core';
import { Exercise } from '../../shared/models/exercise';
import { ExerciseService } from '../../core/services/exercise.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCard } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '../../core/services/account.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { Permissions } from '../../core/constants/permissions';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-exercise-details',
  standalone: true,
  imports: [MatCard, MatButton],
  templateUrl: './exercise-details.component.html',
  styleUrl: './exercise-details.component.scss'
})
export class ExerciseDetailsComponent {
  exercise!: Exercise;
  permissions = Permissions;
  deleting = false;

  private exerciseService = inject(ExerciseService);
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private snack = inject(SnackbarService);
  accountService = inject(AccountService);

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

  onDelete(): void {
    if (this.deleting) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete exercise',
        message: `Delete "${this.exercise.name}"? This cannot be undone.`,
        confirmLabel: 'Delete'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.deleting = true;

      this.exerciseService.deleteExercise(this.exercise.id).subscribe({
        next: () => {
          this.snack.success('Exercise deleted');
          this.router.navigateByUrl('/exercise');
        },
        error: () => this.deleting = false
      });
    });
  }
}
