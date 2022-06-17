import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IBuyer } from 'src/app/models/request/ticket/buyer.model';
import { IUpdateTicket } from 'src/app/models/request/ticket/updateTicket.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { ITicket } from 'src/app/models/response/ticket.model';


@Injectable({
  providedIn: 'root'
})
export class TicketService {

  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) {

  }
  getTicket(id: number): Observable<ITicket> {
    return this.http.get<ITicket>(`${this.endpoints.TICKETS}/${id}`)
  }

  getTicketByCode(code: string): Observable<ITicket> {
    return this.http.get<ITicket>(`${this.endpoints.TICKETS + '/code'}/${code}`)
  }

  getTickets(headers?: HttpHeaders): Observable<ITicket[]> {
    return this.http.get<ITicket[]>(this.endpoints.TICKETS)
  }

  purchaseTicket(eventId: string, buyer?: IBuyer, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(
      `${this.endpoints.TICKETS}/purchase/${eventId}`,
      buyer,
      { headers }
    )
  }

  updateTicket(id: string, request: IUpdateTicket, headers?: HttpHeaders): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${this.endpoints.TICKETS}/${id}`,
      request,
      { headers }
    )
  }

}