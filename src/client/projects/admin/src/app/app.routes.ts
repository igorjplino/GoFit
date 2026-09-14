import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ExerciseComponent } from './features/exercise/exercise.component';
import { ExerciseCreateComponent } from './features/exercise-create/exercise-create.component';
import { ExerciseDetailsComponent } from './features/exercise-details/exercise-details.component';
import { PermissionManagementComponent } from './features/permission-management/permission-management.component';
import { NotFoundComponent } from '@gofit/shared/components/not-found/not-found.component';
import { ForbiddenComponent } from '@gofit/shared/components/forbidden/forbidden.component';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';
import { permissionGuard } from '@gofit/shared/guards/permission.guard';
import { Permissions } from '@gofit/shared/constants/permissions';

export const routes: Routes = [
    {
        path: '',
        component: AdminLayoutComponent,
        children: [
            { path: '', component: HomeComponent },
            { path: 'exercise', component: ExerciseComponent, canActivate: [permissionGuard(Permissions.Exercises.View)] },
            { path: 'exercise/create', component: ExerciseCreateComponent, canActivate: [permissionGuard(Permissions.Exercises.Create)] },
            { path: 'exercise/:id', component: ExerciseDetailsComponent, canActivate: [permissionGuard(Permissions.Exercises.View)] },
            { path: 'permissions', component: PermissionManagementComponent, canActivate: [permissionGuard(Permissions.RoleManagement.ManageUserRoles)] }
        ]
    },
    { path: 'account', loadChildren: () => import('./features/account/routes').then(r => r.accountRoutes) },
    { path: 'forbidden', component: ForbiddenComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
