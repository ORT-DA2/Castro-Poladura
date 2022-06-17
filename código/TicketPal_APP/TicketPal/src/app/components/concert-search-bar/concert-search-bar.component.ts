import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-concert-search-bar',
  templateUrl: './concert-search-bar.component.html',
  styleUrls: ['./concert-search-bar.component.css']
})
export class ConcertSearchBarComponent implements OnInit {
  tourName: string;

  constructor() { }

  ngOnInit(): void {
  }

  loadTourName(name: string){
    this.tourName = name;
  }

}
