import { Component, Input, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';
import { ConcertService } from 'src/app/services/concert/concert.service';

@Component({
  selector: 'app-concert-modal',
  templateUrl: './concert-modal.component.html',
  styleUrls: ['./concert-modal.component.css']
})
export class ConcertModalComponent implements OnInit {
  @Input() concert: IConcert;

  constructor(private concertService: ConcertService) { }

  ngOnInit(): void {
    //this.concert = <IConcert>{}
  }

  saveChanges(tourName: string, date: string, ticketPrice: string, currencyType: string, address: string, location: string, country: string){
    //this.concertService.updateConcert();
  }
}
