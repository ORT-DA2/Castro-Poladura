import { Component, OnInit } from '@angular/core';
import { UserLogin } from 'src/app/models/request/auth/userLogin.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  form: any = {
    email: null,
    password: null
  }
  isLoggedIn = false
  error = false
  errorMessage = ''
  roles: string[] = []
  welcomeMessage: string = ''

  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService
  ) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true
      let user = this.tokenStorage.getUser()
      if (user) {
        this.roles.push(user.role)
      }
    }
  }

  onSubmit(): void {
    const request: UserLogin = {
      email: this.form.email,
      password: this.form.password
    }

    this.authService.login(
      request
    ).subscribe({
      next: data => {
        this.tokenStorage.saveToken(data.token)
        this.tokenStorage.saveUser(data)
        this.error = false
        this.isLoggedIn = true
        const user = this.tokenStorage.getUser()
        if (user) {
          this.welcomeMessage = `Welcome ${user.firstname}.`
          this.roles.push(user.role)
        }
        this.reloadPage()
      },
      error: err => {
        this.errorMessage = err.error.message
        this.error = true
      }
    })
  }
  reloadPage(): void {
    window.location.reload()
  }
}

