import { Component, OnInit } from '@angular/core';
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

  constructor(
    private concertService: ConcertService, private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
    this.loadConcerts();
  }

  loadConcerts() {
    this.concerts = []
    if (this.currentUser?.role == "ARTIST"){
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
    this.concertService.getConcerts().subscribe(
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

  addConcert(){

  }

  editConcert(id: string){
    var concertSelected = this.concerts.find(c => c.id == id);
  }

  deleteConcert(id: string){
    /* var concertSelected = this.concerts.find(c => c.id == id);
    if(confirm("Are you sure to delete this concert: " + concertSelected?.tourName)) {
      this.concertService.deleteConcert(id)
    } */
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
        Swal.fire(
          'Deleted!',
          'The concert has been deleted.',
          'success'
        )
        this.concertService.deleteConcert(id)
      }
    })
  }
}

