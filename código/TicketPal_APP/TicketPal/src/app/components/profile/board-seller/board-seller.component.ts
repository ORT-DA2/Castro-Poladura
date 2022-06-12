import { InputModalityDetector } from '@angular/cdk/a11y';
import { Component, OnInit } from '@angular/core';
import { IBuyer } from 'src/app/models/request/ticketPurchase/buyer.model';
import { ITicket } from 'src/app/models/response/ticket.model';
import { TicketService } from 'src/app/services/ticket/ticket.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-board-seller',
  templateUrl: './board-seller.component.html',
  styleUrls: ['./board-seller.component.css']
})
export class BoardSellerComponent implements OnInit {
  tickets: ITicket[];
  fetchedTickets = false;
  errorMessage: string;
  newBuyer: IBuyer = {firstname: '', lastName: '', email: ''};

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

  registerPurchase(id: string, firstname: string, lastname: string, email: string){   
    //if (firstname != "" && lastname != "" && email != "" && id != ""){
      this.newBuyer.firstname = firstname;
      this.newBuyer.lastName = lastname;
      this.newBuyer.email = email;

      Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, buy it!'
      }).then((result) => {
        if (result.isConfirmed) {
          this.ticketService.purchaseTicket(id, this.newBuyer as IBuyer).subscribe({
            next: data => {
              Swal.fire({
                title: `Your purchase code:`
              })
              Swal.fire({
                title: 'Ticket Purchased',
                icon: 'success',
                html: `<h2>Purchase Code:</h2><br><strong><u>${data.message}</u></strong>`,
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText:
                  '<i class="fa fa-thumbs-up"></i> Close',
                confirmButtonAriaLabel: 'Thumbs up, great!'
              }).then(function (isConfirm) {
                if (isConfirm) {
                  window.location.reload();
                }
              })
            },
            error: err => {
              Swal.fire(
                `Request failed: ${err.error.message}`
              )
              Swal.fire({
                icon: 'error',
                title: err.error.statusDescription,
                text: err.error.message
              })
            }
          });
          Swal.fire(
            'Ticket purchased!',
            'Your ticket has been purchased.',
            'success'
          )
        }
      })
    //}
  }
    
}
