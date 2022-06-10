import { Component, Input, OnInit } from '@angular/core';
import { Concert } from 'src/app/models/response/concert.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

  userLoggedIn = false

  @Input() concerts: Concert[]

  constructor(
    private tokenService: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.userLoggedIn = !!this.tokenService.getToken()
  }

  showConcertDetails(id: string): void {
    var concertSelected = this.concerts.find(c => c.id == id);
    var info = 'Tour name: ' + concertSelected?.tourName + '. Event type: ' + concertSelected?.eventType 
    + '. Date: ' + concertSelected?.date + '. Available tickets: ' + concertSelected?.availableTickets 
    + '. Ticket price: ' + concertSelected?.currencyType + ' ' + concertSelected?.ticketPrice 
    + '. Location: ' + concertSelected?.address + ', ' + concertSelected?.location + ' - ' + concertSelected?.country + '.';
    Swal.fire({
      title: 'Concert details:',
      text: info,
    })
  }
}
