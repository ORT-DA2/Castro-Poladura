import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { IBuyer } from 'src/app/models/request/ticketPurchase/buyer.model';
import { IApiResponse } from 'src/app/models/response/apiResponse.model';
import { ITicket } from 'src/app/models/response/ticket.model';
import { TokenStorageService } from '../storage/token-storage.service';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  headers
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints,
    private token: TokenStorageService
  ) {
    this.headers = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', `Bearer ${token.getToken()}`)
    }
  }
  getTicket(id: number): Observable<ITicket> {
    return this.http.get<ITicket>(`${this.endpoints.TICKETS}/${id}`)
  }

  getTickets(role?: string): Observable<ITicket[]> {
    return this.http.get<ITicket[]>(this.endpoints.TICKETS)
  }

  purchaseTicket(eventId: string, buyer?: IBuyer): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(
      `${this.endpoints.TICKETS}/purchase/${eventId}`,
      buyer,
      this.headers
    )
  }

}