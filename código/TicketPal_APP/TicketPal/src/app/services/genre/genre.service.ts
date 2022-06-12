import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
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

  /* addGenre(): Observable<IGenre[]> {
    
  } */

  //REVISAR ESTE METODO
  updateGenre(id: string): Observable<IGenre[]> {
    let params = new HttpParams().set('type', id);
    return this.http.put<IGenre[]>(this.endpoints.GENRES, { params: params })
  }
  //REVISAR ESTE METODO
  deleteGenre(id: string): Observable<IGenre> {
    return this.http.delete<IGenre>(`${this.endpoints.GENRES}/${id}`)
  }

}