import { Component, OnInit } from '@angular/core';
import { Genre } from 'src/app/models/response/genre.model';
import { User } from 'src/app/models/response/user.model';
import { GenreService } from 'src/app/services/genre/genre.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css']
})
export class GenresComponent implements OnInit {
  genres: Genre[];
  fetchedGenres = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: User | null

  constructor(
    private genreService: GenreService, private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
    this.genres = []
    this.genreService.getGenres().subscribe(
      {
        next: data => {
          this.genres = data
          this.fetchedGenres = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

  addGenre(){

  }

  editGenre(id: string){

  }

  deleteGenre(id: string) {
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
          'The genre has been deleted.',
          'success'
        )
        //this.genreService.deleteGenre(id)
      }
    })
  }

}
