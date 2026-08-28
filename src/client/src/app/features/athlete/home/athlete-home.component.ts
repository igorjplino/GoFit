import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-athlete-home',
  standalone: true,
  imports: [RouterLink, MatButton],
  templateUrl: './athlete-home.component.html',
  styleUrl: './athlete-home.component.scss'
})
export class AthleteHomeComponent {
  accountService = inject(AccountService);
}
