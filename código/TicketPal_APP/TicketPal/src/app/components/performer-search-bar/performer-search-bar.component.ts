import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-performer-search-bar',
  templateUrl: './performer-search-bar.component.html',
  styleUrls: ['./performer-search-bar.component.css']
})
export class PerformerSearchBarComponent implements OnInit {
  performerName: string;

  constructor() { }

  ngOnInit(): void {
  }

  loadPerformerName(name: string){
    this.performerName = name;
  }

}
