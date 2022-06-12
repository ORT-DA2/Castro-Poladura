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
  showAdminBoard = false
  showSpectatorBoard = false
  showSellerBoard = false
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
      this.showAdminBoard = this.roles.includes('ADMIN')
      this.showSpectatorBoard = this.roles.includes('SPECTATOR')
      this.showSellerBoard = this.roles.includes('SELLER')
      this.email = user?.email
    }
  }

  logout(): void {
    this.tokenService.signOut()
    window.location.reload()
  }
}
