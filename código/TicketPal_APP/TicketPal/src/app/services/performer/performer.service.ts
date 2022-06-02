import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { Performer } from 'src/app/models/response/performer.model';

@Injectable({
  providedIn: 'root'
})
export class PerformerService {

  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getPerformer(id: number): Observable<Performer> {
    return this.http.get<Performer>(`${this.endpoints.PERFORMERS}/${id}`)
  }

  getPerformers(): Observable<Performer[]> {
    return this.http.get<Performer[]>(this.endpoints.PERFORMERS)
  }

}
