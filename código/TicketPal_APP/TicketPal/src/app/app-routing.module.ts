import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { BoardAdminComponent } from './components/profile/board-admin/board-admin.component';
import { AuthGuard } from './guards/authGuard.guard';
import { DashboardComponent } from './components/home/dashboard/dashboard.component';
import { ProfileComponent } from './components/profile/profile/profile.component';
import { TicketsComponent } from './components/tables/tickets/tickets.component';
import { BoardSupervisorComponent } from './components/profile/board-supervisor/board-supervisor.component';
import { BoardSellerComponent } from './components/profile/board-seller/board-seller.component';
import { ConcertsComponent } from './components/tables/concerts/concerts.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: BoardAdminComponent, canActivate: [AuthGuard] },
  { path: 'home', component: DashboardComponent },
  { path: 'purchases', component: TicketsComponent, canActivate: [AuthGuard] },
  { path: 'supervisor', component: BoardSupervisorComponent, canActivate: [AuthGuard] },
  { path: 'seller', component: BoardSellerComponent, canActivate: [AuthGuard] },
  { path: 'concerts', component: ConcertsComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
