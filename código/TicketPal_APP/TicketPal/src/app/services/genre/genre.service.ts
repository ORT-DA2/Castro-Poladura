import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IAddGenre } from 'src/app/models/request/genre/addGenre.model';
import { IUpdateGenre } from 'src/app/models/request/genre/updateGenre.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { IGenre } from 'src/app/models/response/genre.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getGenre(id: number): Observable<IGenre> {
    return this.http.get<IGenre>(`${this.endpoints.GENRES}/${id}`)
  }

  getGenres(): Observable<IGenre[]> {
    return this.http.get<IGenre[]>(this.endpoints.GENRES)
  }

  addGenre(request: IAddGenre, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${this.endpoints.GENRES}`,
      request,
      { headers }
    )
  }

  updateGenre(id: string, request: IUpdateGenre, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${this.endpoints.GENRES}/${id}`,
      request,
      { headers }
    )
  }

  deleteGenre(id: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${this.endpoints.GENRES}/${id}`)
  }

}