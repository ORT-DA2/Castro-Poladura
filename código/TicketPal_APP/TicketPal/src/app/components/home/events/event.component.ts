import { Component, Input, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

  @Input() concerts: IConcert[]

  constructor() { }

  ngOnInit(): void {
    this.concerts = [
      {
        id: "1",
        date: "19/05/2023",
        availableTickets: 2,
        ticketPrice: 200,
        currencyType: "UYU",
        eventType: "CONCERT",
        tourName: "Last Tour",
        location: "Centenario",
        address: "Montevideo",
        country: "Uruguay"
      }
    ]
  }

}
