import { Component, Input, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';
import { ConcertService } from 'src/app/services/concert/concert.service';

@Component({
  selector: 'app-concert-edit-modal',
  templateUrl: './concert-edit-modal.component.html',
  styleUrls: ['./concert-edit-modal.component.css']
})
export class ConcertEditModalComponent implements OnInit {
  @Input() concert: IConcert;

  constructor(private concertService: ConcertService) { }

  ngOnInit(): void {
    //this.concert = <IConcert>{}
  }

  saveChanges(tourName: string, date: string, ticketPrice: string, currencyType: string, address: string, location: string, country: string) {
    //this.concertService.updateConcert();
  }
}
