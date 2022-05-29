import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { UserLogin } from 'src/app/models/auth/userLogin.model';
import { UserRegister } from 'src/app/models/register/userRegister.model';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  login(request: UserLogin): Observable<any> {
    return this.http.post<UserLogin>(`${this.endpoints.USERS}/users`, {
      request
    }, httpOptions);
  }

  register(request: UserRegister): Observable<any> {
    return this.http.post<UserRegister>(this.endpoints.USERS, {
      request
    }, httpOptions);
  }
}
