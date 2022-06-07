import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Endpoints } from 'src/app/config/endpoints';
import { Ticket } from 'src/app/models/response/ticket.model';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  constructor(
    private http: HttpClient,
    private endpoints: Endpoints
  ) { }

  getTicket(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.endpoints.TICKETS}/${id}`)
  }

  getTickets(role?: string): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.endpoints.TICKETS)
  }

}