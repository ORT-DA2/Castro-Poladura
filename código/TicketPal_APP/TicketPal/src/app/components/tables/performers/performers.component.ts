import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Performer } from 'src/app/models/response/performer.model';
import { PerformerService } from 'src/app/services/performer/performer.service';

@Component({
  selector: 'app-performers',
  templateUrl: './performers.component.html',
  styleUrls: ['./performers.component.css']
})
export class PerformersComponent implements OnInit {
  performers: Performer[];
  fetchedPerformers = false;
  errorMessage: string;

  constructor(
    private performerService: PerformerService
  ) { }

  ngOnInit(): void {
    this.performers = []
    this.performerService.getPerformers().subscribe(
      {
        next: data => {
          this.performers = data
          this.fetchedPerformers = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }
}
