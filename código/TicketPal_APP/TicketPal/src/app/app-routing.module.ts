import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { BoardAdminComponent } from './components/profile/board-admin/board-admin.component';
import { AuthGuard } from './guards/authGuard.guard';
import { DashboardComponent } from './components/home/dashboard/dashboard.component';
import { ProfileComponent } from './components/profile/profile/profile.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: BoardAdminComponent, canActivate: [AuthGuard] },
  { path: 'home', component: DashboardComponent },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
