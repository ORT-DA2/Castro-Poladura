import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { IBuyer } from 'src/app/models/request/ticketPurchase/buyer.model';
import { IConcert } from 'src/app/models/response/concert.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

  userLoggedIn = false

  @Input() concerts: IConcert[]
  @ViewChild('purchaseView', { static: false })
  myAlert: ElementRef;

  selectedConcert: IConcert

  constructor(
    private tokenService: TokenStorageService,
    private ticketService: TicketService
  ) { }

  ngOnInit(): void {
    this.selectedConcert = <IConcert>{}
    this.userLoggedIn = !!this.tokenService.getToken()
  }

  showConcertDetails(id: string): void {
    var concertSelected = this.concerts.find(c => c.id == id);
    var info = 'Tour name: ' + concertSelected?.tourName + 'Artist: ' +  + '. Event type: ' + concertSelected?.eventType 
    + '. Date: ' + concertSelected?.date + '. Available tickets: ' + concertSelected?.availableTickets 
    + '. Ticket price: ' + concertSelected?.currencyType + ' ' + concertSelected?.ticketPrice 
    + '. Location: ' + concertSelected?.address + ', ' + concertSelected?.location + ' - ' + concertSelected?.country + '.';
    Swal.fire({
      title: 'Concert details:',
      text: info,
    })
  }

  onPurchaseClicked(concert: IConcert) {
    this.selectedConcert = concert
    Swal.fire({
      html: this.myAlert.nativeElement,
      showClass: {
        popup: 'animate__animated animate__fadeInDown'
      },
      hideClass: {
        popup: 'animate__animated animate__fadeOutUp'
      },
      showCancelButton: true,
      showConfirmButton: true,
      confirmButtonText: "Purchase",
      cancelButtonText: "Cancel",
      showLoaderOnConfirm: true,
      backdrop: true,
      preConfirm: (login) => {
        return this.ticketService.purchaseTicket(concert.id, {} as IBuyer)
          .subscribe(
            {
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
            }
          )
      },
      allowOutsideClick: () => !Swal.isLoading()
    })
  }
}
