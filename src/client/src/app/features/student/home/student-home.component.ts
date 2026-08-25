import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-student-home',
  standalone: true,
  imports: [RouterLink, MatButton],
  templateUrl: './student-home.component.html',
  styleUrl: './student-home.component.scss'
})
export class StudentHomeComponent {
  accountService = inject(AccountService);
}
