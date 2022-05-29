import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { UserLogin } from 'src/app/models/request/auth/userLogin.model';
import { UserRegister } from 'src/app/models/request/register/userRegister.model';
import { ApiResponse } from 'src/app/models/response/apiResponse.model';
import { User } from 'src/app/models/response/user.model';

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

  login(request: UserLogin): Observable<User> {
    return this.http.post<User>(`${this.endpoints.USERS}/login`, {
      request
    }, httpOptions);
  }

  register(request: UserRegister): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.endpoints.USERS, {
      request
    }, httpOptions);
  }
}
