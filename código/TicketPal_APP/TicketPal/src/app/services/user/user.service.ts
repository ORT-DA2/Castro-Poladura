import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IUser } from 'src/app/models/response/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getUser(id: number): Observable<IUser> {
    return this.http.get<IUser>(`${this.endpoints.USERS}/${id}`)
  }

  getUsers(role?: string): Observable<IUser[]> {

    if (role !== undefined) {
      let params = new HttpParams().set('role', role);
      return this.http.get<IUser[]>(this.endpoints.USERS, { params: params })
    }
    return this.http.get<IUser[]>(this.endpoints.USERS)
  }

}
