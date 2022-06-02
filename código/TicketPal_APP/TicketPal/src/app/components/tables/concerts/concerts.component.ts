import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Concert } from 'src/app/models/response/concert.model';
import { ConcertService } from 'src/app/services/concert/concert.service';

@Component({
  selector: 'app-concerts',
  templateUrl: './concerts.component.html',
  styleUrls: ['./concerts.component.css']
})
export class ConcertsComponent implements OnInit {
  concerts: Concert[];
  fetchedConcerts = false;
  errorMessage: string;

  constructor(
    private concertService: ConcertService
  ) { }

  ngOnInit(): void {
    this.concerts = []
    this.concertService.getConcerts().subscribe(
      {
        next: data => {
          this.concerts = data
          this.fetchedConcerts = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

}
