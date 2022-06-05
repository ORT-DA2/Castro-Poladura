import { Component, OnInit } from '@angular/core';
import { Ticket } from 'src/app/models/response/ticket.model';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-tickets',
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit {
  tickets: Ticket[];
  fetchedTickets = false;
  errorMessage: string;

  constructor(
    private ticketService: TicketService
  ) { }

  ngOnInit(): void {
    this.tickets = []
    this.ticketService.getTickets().subscribe(
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
