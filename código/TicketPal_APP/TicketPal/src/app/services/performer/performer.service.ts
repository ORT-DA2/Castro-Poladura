import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IUpdatePerformer } from 'src/app/models/request/performer/updatePerformer.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { IPerformer } from 'src/app/models/response/performer.model';

@Injectable({
  providedIn: 'root'
})
export class PerformerService {

  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getPerformer(id: number): Observable<IPerformer> {
    return this.http.get<IPerformer>(`${this.endpoints.PERFORMERS}/${id}`)
  }

  getPerformers(performerName: string): Observable<IPerformer[]> {
    let params = new HttpParams().set('performerName', performerName)
    return this.http.get<IPerformer[]>(this.endpoints.PERFORMERS, { params: params })
  }

  addPerformer(request: IUpdatePerformer, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${this.endpoints.PERFORMERS}`,
      request,
      { headers }
    )
  }

  updatePerformer(id: string, request: IUpdatePerformer, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${this.endpoints.PERFORMERS}/${id}`,
      request,
      { headers }
    )
  }

  deletePerformer(id: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${this.endpoints.PERFORMERS}/${id}`)
  }

}
