import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRoles } from '../../../core/constants/roles';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatInput,
    MatLabel,
    MatButton
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  returnUrl = '/';
  hasExplicitReturnUrl = false;

  constructor(
    fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    activatedRoute: ActivatedRoute
  ) {
    this.loginForm = fb.group({
      email: [''],
      password: ['']
    });

    const url = activatedRoute.snapshot.queryParams['returnUrl'];
    if (url) {
      this.returnUrl = url;
      this.hasExplicitReturnUrl = true;
    }
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.accountService.getUserInfo().subscribe(user => {
          let target: string = '/';
          
          if (this.hasExplicitReturnUrl) {
            target = this.returnUrl;
          } else if (user.role === AppRoles.Admin) {
            target = '/admin';
          }

          this.router.navigateByUrl(target);
        });
      }
    });
  }
}
