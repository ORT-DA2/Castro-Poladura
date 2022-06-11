import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IConcert } from 'src/app/models/response/concert.model';

@Injectable({
  providedIn: 'root'
})
export class ConcertService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getConcert(id: string): Observable<IConcert> {
    return this.http.get<IConcert>(`${this.endpoints.CONCERTS}/${id}`)
  }

  getConcerts(): Observable<IConcert[]> {
    let params = new HttpParams().set('type', "TYPE_CONCERT");
    return this.http.get<IConcert[]>(this.endpoints.CONCERTS, { params: params })
  }

  getConcertsByPerformer(name: string){
    let params = new HttpParams().set('artistName', name).set('type', "TYPE_CONCERT");
    return this.http.get<IConcert[]>(this.endpoints.CONCERTS, { params: params })
  }

  //REVISAR ESTE METODO
  updateConcert(id: string): Observable<IConcert[]> {
    let params = new HttpParams().set('type', id);
    return this.http.put<IConcert[]>(this.endpoints.CONCERTS, { params: params })
  }
  //REVISAR ESTE METODO
  deleteConcert(id: string): Observable<IConcert> {
    return this.http.delete<IConcert>(`${this.endpoints.CONCERTS}/${id}`)
  }
} 
