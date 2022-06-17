import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ITicket } from 'src/app/models/response/ticket.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  tickets: ITicket[];
  fetchedTickets = false;
  errorMessage: string;

  constructor(
    private ticketService: TicketService,
    private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.tickets = []
    this.ticketService.getTickets(
      new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', `Bearer ${this.tokenService.getToken()}`)
    ).subscribe(
      {
        next: data => {
          this.tickets = data
          this.fetchedTickets = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

}
