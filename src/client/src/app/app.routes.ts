import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ExerciseComponent } from './features/exercise/exercise.component';
import { ExerciseCreateComponent } from './features/exercise-create/exercise-create.component';
import { ExerciseDetailsComponent } from './features/exercise-details/exercise-details.component';
import { PermissionManagementComponent } from './features/admin/permission-management/permission-management.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ForbiddenComponent } from './shared/components/forbidden/forbidden.component';
import { permissionGuard } from './core/guards/permission.guard';
import { Permissions } from './core/constants/permissions';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'exercise', component: ExerciseComponent },
    { path: 'exercise/create', component: ExerciseCreateComponent, canActivate: [permissionGuard(Permissions.Exercises.Create)] },
    { path: 'exercise/:id', component: ExerciseDetailsComponent },
    { path: 'admin/permissions', component: PermissionManagementComponent, canActivate: [permissionGuard(Permissions.RoleManagement.ManageUserRoles)] },
    { path: 'account', loadChildren: () => import('./features/account/routes').then(r => r.accountRoutes) },
    { path: 'forbidden', component: ForbiddenComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
