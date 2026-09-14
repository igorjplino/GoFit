import { Route } from "@angular/router";
import { LoginComponent } from "@gofit/shared/components/login/login.component";
import { RegisterComponent } from "./register/register.component";

export const accountRoutes: Route[] = [
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
]