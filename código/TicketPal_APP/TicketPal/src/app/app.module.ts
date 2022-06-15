import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { BoardAdminComponent } from './components/profile/board-admin/board-admin.component';
import { BoardSellerComponent } from './components/profile/board-seller/board-seller.component';
import { BoardSupervisorComponent } from './components/profile/board-supervisor/board-supervisor.component';
import { authInterceptorProviders } from './helpers/auth/auth.interceptor';
import { Endpoints } from './config/endpoints';
import { DashboardComponent } from './components/home/dashboard/dashboard.component';
import { AuthGuard } from './guards/authGuard.guard';
import { EventComponent } from './components/home/events/event.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule } from '@angular/material/tabs';
import { UsersComponent } from './components/tables/users/users.component';
import { PerformersComponent } from './components/tables/performers/performers.component';
import { GenresComponent } from './components/tables/genres/genres.component';
import { ConcertsComponent } from './components/tables/concerts/concerts.component';
import { TicketsComponent } from './components/tables/tickets/tickets.component';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import { ReviewPurchaseComponent } from './components/ticket/purchase/reviewPurchase.component';
import { ProfileComponent } from './components/profile/profile/profile.component';
import { ConcertEditModalComponent } from './components/profile/edit-modals/concert-edit-modal/concert-edit-modal.component';
import { GenreEditModalComponent } from './components/profile/edit-modals/genre-edit-modal/genre-edit-modal.component';
import { PerformerEditModalComponent } from './components/profile/edit-modals/performer-edit-modal/performer-edit-modal.component';
import { UserEditModalComponent } from './components/profile/edit-modals/user-edit-modal/user-edit-modal.component';
import { ConcertSearchBarComponent } from './components/concert-search-bar/concert-search-bar.component';
import { PerformerSearchBarComponent } from './components/performer-search-bar/performer-search-bar.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    BoardAdminComponent,
    BoardSellerComponent,
    BoardSupervisorComponent,
    DashboardComponent,
    EventComponent,
    UsersComponent,
    ProfileComponent,
    PerformersComponent,
    GenresComponent,
    ConcertsComponent,
    TicketsComponent,
    DatePickerComponent,
    ReviewPurchaseComponent,
    ConcertEditModalComponent,
    GenreEditModalComponent,
    PerformerEditModalComponent,
    UserEditModalComponent,
    ConcertSearchBarComponent,
    PerformerSearchBarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatTabsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    authInterceptorProviders,
    Endpoints,
    AuthGuard,
    FormBuilder
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
