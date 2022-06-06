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

  getConcert(id: number): Observable<IConcert> {
    return this.http.get<IConcert>(`${this.endpoints.CONCERTS}/${id}`)
  }

  getConcerts(): Observable<IConcert[]> {
    var endpoint = this.endpoints.CONCERTS + '?type=TYPE_CONCERT';
    return this.http.get<IConcert[]>(endpoint)
  }

}