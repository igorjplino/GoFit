import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';

export function permissionGuard(permission: string): CanActivateFn {
  return (route, state) => {
    const accountService = inject(AccountService);
    const router = inject(Router);

    if (!accountService.currentUser()) {
      router.navigate(['/account/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    if (!accountService.hasPermission(permission)) {
      router.navigateByUrl('/forbidden');
      return false;
    }

    return true;
  };
}
