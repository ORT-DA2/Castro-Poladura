import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { UserLogin } from 'src/app/models/request/auth/userLogin.model';
import { UserRegister } from 'src/app/models/request/register/userRegister.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { IUser } from 'src/app/models/response/user.model';

const httpOptions = {
  headers: new HttpHeaders().set('Content-Type', 'application/json')
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  login(request: UserLogin): Observable<IUser> {
    return this.http.post<any>(`${this.endpoints.USERS}/login`,
      request,
      httpOptions
    );
  }

  register(request: UserRegister): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(this.endpoints.USERS, {
      request
    }, httpOptions);
  }
}
