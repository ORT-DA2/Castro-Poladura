import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from './services/storage/token-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  private roles: string[] = []
  isLoggedIn = false
  adminLogged = false
  spectatorLogged = false
  supervisorLogged = false
  email?: string

  constructor(
    private tokenService: TokenStorageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.isLoggedIn = !!this.tokenService.getToken()

    this.router.navigate(['home']);
    if (this.isLoggedIn) {
      const user = this.tokenService.getUser()
      if (user?.role) {
        this.roles.push(user?.role)
      }
      this.adminLogged = this.roles.includes('ADMIN')
      this.spectatorLogged = this.roles.includes('SPECTATOR')
      this.supervisorLogged = this.roles.includes('SUPERVISOR')
      this.email = user?.email
    }
  }

  logout(): void {
    this.tokenService.signOut()
    window.location.reload()
  }
}
