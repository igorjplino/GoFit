import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  private duration: number = 5000;

  constructor(private snackbar: MatSnackBar) { }

  error(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.duration,
      panelClass: ['snack-error']
    })
  }

  success(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: this.duration,
      panelClass: ['snack-success']
    })
  }
}
