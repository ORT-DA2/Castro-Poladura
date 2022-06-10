import { Component, OnInit } from '@angular/core';
import { Ticket } from 'src/app/models/response/ticket.model';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-board-seller',
  templateUrl: './board-seller.component.html',
  styleUrls: ['./board-seller.component.css']
})
export class BoardSellerComponent implements OnInit {
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

  registerPurchase(id: string){
    
  }

}
