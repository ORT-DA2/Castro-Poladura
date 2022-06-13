import { Component, Input, OnInit } from '@angular/core';
import { IGenre } from 'src/app/models/response/genre.model';
import { IPerformer } from 'src/app/models/response/performer.model';

@Component({
  selector: 'app-performer-modal',
  templateUrl: './performer-modal.component.html',
  styleUrls: ['./performer-modal.component.css']
})
export class PerformerModalComponent implements OnInit {
  @Input() genres: IGenre[]
  @Input() performers: IPerformer[]

  constructor() { }

  ngOnInit(): void {
  }

  saveChanges(performerType: string, year: string, genre: string, performer: string){

  }

}
