import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IAddUser } from 'src/app/models/request/user/addUser.model';
import { IUpdateUser } from 'src/app/models/request/user/updateUser.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { IUser } from 'src/app/models/response/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getUser(id: string, headers?: HttpHeaders): Observable<IUser> {
    return this.http.get<IUser>(
      `${this.endpoints.USERS}/${id}`,
      { headers }
    )
  }

  getUsers(role?: string): Observable<IUser[]> {

    if (role !== undefined) {
      let params = new HttpParams().set('role', role);
      return this.http.get<IUser[]>(this.endpoints.USERS, { params: params })
    }
    return this.http.get<IUser[]>(this.endpoints.USERS)
  }

  updateUser(id: string, request: IUpdateUser, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${this.endpoints.USERS}/${id}`,
      request,
      { headers }
    )
  }

  addUser(request: IAddUser, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${this.endpoints.USERS}`,
      request,
      { headers }
    )
  }

  deleteUser(id: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${this.endpoints.USERS}/${id}`)
  }

}
