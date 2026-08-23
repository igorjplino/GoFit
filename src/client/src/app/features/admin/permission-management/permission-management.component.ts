import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { RoleService } from '../../../core/services/role.service';
import { UserManagementService } from '../../../core/services/user-management.service';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { AccountService } from '../../../core/services/account.service';
import { Role } from '../../../shared/models/role';
import { UserSummary } from '../../../shared/models/user-summary';
import { PermissionCatalog } from '../../../core/constants/permissions';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-permission-management',
  standalone: true,
  imports: [
    MatCard,
    MatIcon,
    MatFormFieldModule,
    MatSelectModule
  ],
  templateUrl: './permission-management.component.html',
  styleUrl: './permission-management.component.scss'
})
export class PermissionManagementComponent implements OnInit {
  private roleService = inject(RoleService);
  private userManagementService = inject(UserManagementService);
  private snack = inject(SnackbarService);
  private dialog = inject(MatDialog);
  accountService = inject(AccountService);

  roles = signal<Role[]>([]);
  users = signal<UserSummary[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  savingUserId = signal<string | null>(null);

  permissionCatalog = PermissionCatalog;
  categories = [...new Set(PermissionCatalog.map(p => p.category))];

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.roleService.getRoles().subscribe({
      next: roles => this.roles.set(roles),
      error: () => this.error.set('Could not load roles.')
    });

    this.userManagementService.getUsers().subscribe({
      next: users => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load users.');
        this.loading.set(false);
      }
    });
  }

  roleGrants(roleName: string, permission: string): boolean {
    return this.roles().find(r => r.name === roleName)?.permissions.includes(permission) ?? false;
  }

  permissionsFor(category: string) {
    return this.permissionCatalog.filter(p => p.category === category);
  }

  onRoleChange(user: UserSummary, event: MatSelectChange): void {
    const newRole = event.value;
    if (newRole === user.role) {
      return;
    }

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Change user role',
        message: `Change ${user.displayName}'s role from ${user.role} to ${newRole}? This immediately changes what they can access.`,
        confirmLabel: 'Change role'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        event.source.value = user.role;
        return;
      }

      this.savingUserId.set(user.id);

      this.userManagementService.updateUserRole(user.id, newRole).subscribe({
        next: () => {
          this.users.update(users =>
            users.map(u => u.id === user.id ? { ...u, role: newRole } : u));
          this.savingUserId.set(null);
          this.snack.success(`${user.displayName}'s role is now ${newRole}`);
        },
        error: () => {
          event.source.value = user.role;
          this.savingUserId.set(null);
        }
      });
    });
  }
}
