import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ExerciseComponent } from './features/exercise/exercise.component';
import { ExerciseDetailsComponent } from './features/exercise-details/exercise-details.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'exercise', component: ExerciseComponent },
    { path: 'exercise/:id', component: ExerciseDetailsComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' }
    { path: 'not-found', component: NotFoundComponent },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
