import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IUpdateTicket } from 'src/app/models/request/ticket/updateTicket.model';
import { ITicket } from 'src/app/models/response/ticket.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';

@Component({
  selector: 'app-board-supervisor',
  templateUrl: './board-supervisor.component.html',
  styleUrls: ['./board-supervisor.component.css']
})
export class BoardSupervisorComponent implements OnInit {

  ticket: ITicket
  responseMessage = ''
  fetchedTicket: boolean = false
  errorUpdate = false
  errorFetchTicket = false

  searchCodeForm = this.formBuilder.group({
    input: ''
  })

  constructor(
    private formBuilder: FormBuilder,
    private ticketService: TicketService,
    private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {

  }

  onSearchClicked = () => {
    const search = this.searchCodeForm.controls['input'].value
    this.ticketService.getTicketByCode(
      search
    ).subscribe(
      {
        next: data => {
          if (data) {
            this.fetchedTicket = true
            this.ticket = data
          } else {
            this.fetchedTicket = false
            this.errorFetchTicket = true
            this.responseMessage = 'Ticket code not found.'
          }
        },
        error: err => {
          this.fetchedTicket = false
        }
      }
    )
  }

  onUpdateClicked = (value: string) => {
    var request: IUpdateTicket = {
      status: value
    } as IUpdateTicket

    this.ticketService.updateTicket(
      this.ticket.id,
      request,
      new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Authorization', `Bearer ${this.tokenService.getToken()}`)
    )
      .subscribe(
        {
          next: data => {
            this.errorUpdate = false
            this.responseMessage = data.message
          },
          error: err => {
            this.errorUpdate = true
            this.responseMessage = err.error.message
          }
        }
      )
  }

  isResponse = (): boolean => {
    return this.responseMessage !== null && this.responseMessage.length > 0;
  }

}
