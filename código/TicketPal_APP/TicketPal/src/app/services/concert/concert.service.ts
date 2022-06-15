import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { ICreateConcert } from 'src/app/models/request/concert/createConcert.model';
import { IUpdateConcert } from 'src/app/models/request/concert/updateConcert.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
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

  getConcerts(startDate: string, endDate: string, tourName: string): Observable<IConcert[]> {
    let params = new HttpParams().set('type', "TYPE_CONCERT").set('startDate', startDate).set('endDate', endDate).set('tourName', tourName);
    return this.http.get<IConcert[]>(this.endpoints.CONCERTS, { params: params })
  }

  getConcertsByPerformer(id: string): Observable<IConcert[]>{
    let params = new HttpParams().set('performerId', id).set('type', "TYPE_CONCERT");
    return this.http.get<IConcert[]>(this.endpoints.CONCERTSBYPERFORMER, { params: params })
  }

  addConcert(request: ICreateConcert, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${this.endpoints.CONCERTS}`,
      request,
      { headers }
    )
  }

  updateConcert(id: string, request: IUpdateConcert, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${this.endpoints.CONCERTS}/${id}`,
      request,
      { headers }
    )
  }

  deleteConcert(id: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${this.endpoints.CONCERTS}/${id}`)
  }
} 
