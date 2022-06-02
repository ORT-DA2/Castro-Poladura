import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile/profile.component';
import { BoardAdminComponent } from './components/profile/board-admin/board-admin.component';
import { BoardSpectatorComponent } from './components/profile/board-spectator/board-spectator.component';
import { BoardSellerComponent } from './components/profile/board-seller/board-seller.component';
import { BoardSupervisorComponent } from './components/profile/board-supervisor/board-supervisor.component';
import { authInterceptorProviders } from './helpers/auth/auth.interceptor';
import { Endpoints } from './config/endpoints';
import { DashboardComponent } from './home/dashboard/dashboard.component';
import { AuthGuard } from './guards/authGuard.guard';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule } from '@angular/material/tabs';
import { UsersComponent } from './components/tables/users/users.component';
import { PerformersComponent } from './components/tables/performers/performers.component';
import { GenresComponent } from './components/tables/genres/genres.component';
import { ConcertsComponent } from './components/tables/concerts/concerts.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    BoardAdminComponent,
    BoardSpectatorComponent,
    BoardSellerComponent,
    BoardSupervisorComponent,
    DashboardComponent,
    UsersComponent,
    PerformersComponent,
    GenresComponent,
    ConcertsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatTabsModule
  ],
  providers: [
    authInterceptorProviders,
    Endpoints,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
