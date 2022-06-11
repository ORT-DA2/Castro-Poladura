import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { Concert } from 'src/app/models/response/concert.model';

@Injectable({
  providedIn: 'root'
})
export class ConcertService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getConcert(id: number): Observable<Concert> {
    return this.http.get<Concert>(`${this.endpoints.CONCERTS}/${id}`)
  }

  getConcerts(): Observable<Concert[]> {
    let params = new HttpParams().set('type', "TYPE_CONCERT");
    return this.http.get<Concert[]>(this.endpoints.CONCERTS, { params: params })
  }
  //REVISAR ESTE METODO
  updateConcerts(id: string): Observable<Concert[]> {
    let params = new HttpParams().set('type', id);
    return this.http.put<Concert[]>(this.endpoints.CONCERTS, { params: params })
  }
  //REVISAR ESTE METODO
  deleteConcert(id: string): Observable<Concert[]> {
    //let params = new HttpParams().set('type', id);
    return this.http.delete<Concert[]>(this.endpoints.CONCERTS + '/' + id)
  }
} 
