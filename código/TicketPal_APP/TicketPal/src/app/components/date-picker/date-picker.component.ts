import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.css']
})
export class DatePickerComponent implements OnInit {

startDate: string;
endDate: string | null = null;

  constructor() {
  }
 
  ngOnInit(): void {

  }

  loadDates(startDate: string, endDate: string){
    this.startDate = startDate;
    this.endDate = endDate;
  }

}