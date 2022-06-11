import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IPerformer } from 'src/app/models/response/performer.model';
import { PerformerService } from 'src/app/services/performer/performer.service';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-performers',
  templateUrl: './performers.component.html',
  styleUrls: ['./performers.component.css']
})
export class PerformersComponent implements OnInit {
  performers: IPerformer[];
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

  showArtists(id: string): void {
    var info = 'Members: ';
    var performerSelected = this.performers.find(p => p.id == id);
    performerSelected?.members.forEach(p => {
      info += p.userInfo.firstname + ' ' + p.userInfo.lastname + ',' + '\n'
    });
    info = info.substring(0, info.length - 2);
    Swal.fire({
      text: info,
    })
  }
}
