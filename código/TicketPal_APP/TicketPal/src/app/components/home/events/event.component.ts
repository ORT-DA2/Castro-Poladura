import { Component, Input, OnInit } from '@angular/core';
import { IConcert } from 'src/app/models/response/concert.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.css']
})
export class EventComponent implements OnInit {

  userLoggedIn = false

  @Input() concerts: IConcert[]

  constructor(
    private tokenService: TokenStorageService,
  ) { }

  ngOnInit(): void {
    this.userLoggedIn = !!this.tokenService.getToken()
  }

}
