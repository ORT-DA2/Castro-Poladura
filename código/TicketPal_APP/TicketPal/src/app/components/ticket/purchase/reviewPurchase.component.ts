import { Component, Input, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';

@Component({
  selector: 'app-purchase',
  templateUrl: './reviewPurchase.component.html',
  styleUrls: ['./reviewPurchase.component.css']
})
export class ReviewPurchaseComponent implements OnInit {

  @Input() concert: IConcert

  constructor() { }

  ngOnInit(): void {
  }

}
