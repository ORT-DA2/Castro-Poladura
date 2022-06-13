import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IConcert } from 'src/app/models/response/concert.model';
import { IUser } from 'src/app/models/response/user.model';
import { ConcertService } from 'src/app/services/concert/concert.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-concerts',
  templateUrl: './concerts.component.html',
  styleUrls: ['./concerts.component.css']
})
export class ConcertsComponent implements OnInit {
  concerts: IConcert[];
  fetchedConcerts = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: IUser | null
  editedConcert: IConcert
  newConcert: IConcert
  notHome = false;
  @Input() fromDate: string;
  @Input() toDate: string;

  constructor(
    private concertService: ConcertService, private tokenService: TokenStorageService, private router: Router
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser();
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN");
    this.notHome = this.router.url != '/home';
    this.loadConcerts(this.fromDate, this.toDate);
  }

  loadConcerts(startDate: string, endDate: string) {
    this.concerts = []
    if (this.currentUser?.role == "ARTIST" && this.notHome){
      this.concertService.getConcertsByPerformer(this.currentUser.id).subscribe(
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
    else if (startDate != undefined && startDate != ""){
      var formattedStartDate = this.parseDate(startDate);
      var formattedEndDate = this.parseDate(endDate);
      this.concertService.getConcerts(formattedStartDate, formattedEndDate).subscribe(
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
    else {
      this.concertService.getConcerts("", "").subscribe(
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

  async addConcert(){
    var result = await Swal.fire({
      html:
      '<h2>Concert: </h2>' +
      '<form>' +
          '<div class="form-group row">' +
              '<label for="inputTourName" class="col-sm-2 col-form-label">Tour Name</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="Tour name" id="inputTourName" #tourName>' +
              '</div>' +
            '</div>' +
            '<div class="form-group row">' +
              '<label for="inputDate" class="col-sm-2 col-form-label">Date</label>' +
              '<div class="col-sm-10">' +
                '<input type="date" class="form-control" id="inputDate" #date>' +
              '</div>' +
            '</div>' +
          '<div class="form-group row">' +
          '<label for="inputTicketPrice" class="col-sm-2 col-form-label">Ticket price</label>' +
          '<div class="col-sm-10">' +
              '<input type="text" class="form-control" placeholder="0" id="inputTicketPrice" #ticketPrice>' +
          '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputCurrencyType" class="col-sm-2 col-form-label">Currency type</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="UYU" id="inputCurrencyType" #currencyType>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputAddress" class="col-sm-2 col-form-label">Address</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Address" id="inputAddress" #address>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputLocation" class="col-sm-2 col-form-label">Location</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Location" id="inputLocation" #location>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputCountry" class="col-sm-2 col-form-label">Country</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Country" id="inputCountry" #country>' +
              '</div>' +
          '</div>' +
       '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadConcerts("",""),
        Swal.fire('Saved!', '', 'success')
        return [
          document.getElementById('inputTourName')?.ariaValueText,
          document.getElementById('inputDate')?.ariaValueText,
          document.getElementById('inputTicketPrice')?.ariaValueText,
          document.getElementById('inputCurrencyType')?.ariaValueText,
          document.getElementById('inputAddress')?.ariaValueText,
          document.getElementById('inputLocation')?.ariaValueText,
          document.getElementById('inputCountry')?.ariaValueText,
        ]
      } else if (result.isDenied) {
        Swal.fire('Changes are not saved', '', 'info')
        return null;
      }
      else{
        return null;
      }
    })
  }

  async editConcert(id: string){
    var concertSelected = this.concerts.find(c => c.id == id);
    var result = await Swal.fire({
      html:
      '<h2>Concert: </h2>' +
      '<form>' +
          '<div class="form-group row">' +
              '<label for="inputTourName" class="col-sm-2 col-form-label">Tour Name</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="Tour name" id="inputTourName" #tourName>' +
              '</div>' +
            '</div>' +
            '<div class="form-group row">' +
              '<label for="inputDate" class="col-sm-2 col-form-label">Date</label>' +
              '<div class="col-sm-10">' +
                '<input type="date" class="form-control" id="inputDate" #date>' +
              '</div>' +
            '</div>' +
          '<div class="form-group row">' +
          '<label for="inputTicketPrice" class="col-sm-2 col-form-label">Ticket price</label>' +
          '<div class="col-sm-10">' +
              '<input type="text" class="form-control" placeholder="0" id="inputTicketPrice" #ticketPrice>' +
          '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputCurrencyType" class="col-sm-2 col-form-label">Currency type</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="UYU" id="inputCurrencyType" #currencyType>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputAddress" class="col-sm-2 col-form-label">Address</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Address" id="inputAddress" #address>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputLocation" class="col-sm-2 col-form-label">Location</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Location" id="inputLocation" #location>' +
              '</div>' +
          '</div>' +
          '<div class="form-group row">' +
              '<label for="inputCountry" class="col-sm-2 col-form-label">Country</label>' +
              '<div class="col-sm-10">' +
                  '<input type="text" class="form-control" placeholder="Country" id="inputCountry" #country>' +
              '</div>' +
          '</div>' +
          /* '<button id="saveButton" type="button" class="btn btn-primary" (click)="saveChanges(tourName.value, date.value, ticketPrice.value, currencyType.value, address.value, location.value, country.value)"> Save </button><button type="cancel" class="btn btn-danger">Cancel</button>' + */
       '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
      /* preConfirm: () => {
        return [
          document.getElementById('inputTourName')?.ariaValueText,
          document.getElementById('inputDate')?.ariaValueText,
          document.getElementById('inputTicketPrice')?.ariaValueText,
          document.getElementById('inputCurrencyType')?.ariaValueText,
          document.getElementById('inputAddress')?.ariaValueText,
          document.getElementById('inputLocation')?.ariaValueText,
          document.getElementById('inputCountry')?.ariaValueText,
        ]
      } */
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadConcerts("",""),
        Swal.fire('Saved!', '', 'success')
        return [
          document.getElementById('inputTourName')?.ariaValueText,
          document.getElementById('inputDate')?.ariaValueText,
          document.getElementById('inputTicketPrice')?.ariaValueText,
          document.getElementById('inputCurrencyType')?.ariaValueText,
          document.getElementById('inputAddress')?.ariaValueText,
          document.getElementById('inputLocation')?.ariaValueText,
          document.getElementById('inputCountry')?.ariaValueText,
        ]
      } else if (result.isDenied) {
        Swal.fire('Changes are not saved', '', 'info')
        return null;
      }
      else{
        return null;
      }
    })
  }

  deleteConcert(id: string){
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.concertService.deleteConcert(id).subscribe(
          {
            next: data => {
              Swal.fire({
                icon: 'success',
                text: data.message,
              })
              this.loadConcerts("","")
            },
            error: err => {
              Swal.fire({
                icon: 'error',
                text: err.error.message,
              })
            }
          }
        )
      }
    })
  }

  onSearch(startDate: string, endDate: string | null){
    this.loadConcerts(this.fromDate, this.toDate)
  }

  parseDate(date: string){
    const [year, month, day] = date.split('-');
    var newDate = day + '/' + month + '/' + year + ' ' + '00:00';
    return newDate;
  }
}

