import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { Genre } from 'src/app/models/response/genre.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getGenre(id: number): Observable<Genre> {
    return this.http.get<Genre>(`${this.endpoints.GENRES}/${id}`)
  }

  getGenres(): Observable<Genre[]> {
    return this.http.get<Genre[]>(this.endpoints.GENRES)
  }

}