import { Component, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';
import { ConcertService } from 'src/app/services/concert/concert.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  concerts: IConcert[]

  constructor(
    private concertService: ConcertService
  ) {
  }

  ngOnInit(): void {
    this.concerts = []
    this.concertService.getConcerts().subscribe(
      {
        next: data => {
          this.concerts = data
        }
        ,
        error: err => {

        }
      }
    )
  }

}


