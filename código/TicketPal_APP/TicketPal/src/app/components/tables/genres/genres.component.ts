import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IGenre } from 'src/app/models/response/genre.model';
import { IUser } from 'src/app/models/response/user.model';
import { GenreService } from 'src/app/services/genre/genre.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css']
})
export class GenresComponent implements OnInit {
  genres: IGenre[]
  fetchedGenres = false
  errorMessage: string
  adminLoggedIn = false
  currentUser: IUser | null

  @ViewChild('editGenreView', { static: false })
  genreEditPopup: ElementRef;

  selectedGenreToEdit: IGenre
  show = false
  add = false

  constructor(
    private genreService: GenreService,
    private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser()
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN")
    this.loadGenres();
  }

  refresh = (): void => {
    this.loadGenres()
    Swal.close()
  }

  loadGenres(): void {
    this.genres = []
    this.genreService.getGenres().subscribe(
      {
        next: data => {
          this.genres = data
          this.fetchedGenres = true
        },
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

  addGenre() {
    this.add = true
    this.show = true
    this.selectedGenreToEdit = {} as IGenre;
    Swal.fire({
      html: this.genreEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedGenreToEdit = {} as IGenre
        this.show = false
        this.add = false
      }
    })
  }

  editGenre(selected: IGenre) {
    this.show = true
    this.add = false
    this.selectedGenreToEdit = selected;

    Swal.fire({
      html: this.genreEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedGenreToEdit = {} as IGenre
        this.show = false
      }
    })
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
        this.genreService.deleteGenre(id).subscribe(
          {
            next: data => {
              Swal.fire({
                icon: 'success',
                text: data.message,
              })
              this.loadGenres()
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

}
