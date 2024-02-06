import { Routes } from '@angular/router';
import { LoginComponent } from './common/components/auth/login/login.component';
import { RegisterComponent } from './common/components/auth/register/register.component';
import { DashboardComponent } from './features/components/dashboard/dashboard.component';
import { AuthGuard } from './common/services/authentication.service';

export const routes: Routes = [
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegisterComponent
    },
    {
        path: 'dashboard',
        canActivate: [AuthGuard],
        component: DashboardComponent
    },
    {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full'
    },
];
