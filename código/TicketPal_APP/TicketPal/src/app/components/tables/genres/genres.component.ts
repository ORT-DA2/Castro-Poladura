import { Component, OnInit } from '@angular/core';
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
  genres: IGenre[];
  fetchedGenres = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: IUser | null

  constructor(
    private genreService: GenreService, private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
    this.loadGenres();
  }

  loadGenres(): void{
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

  async addGenre(){
    var result = await Swal.fire({
      html:
      '<h2>Genre: </h2>' +
      '<form>' +
          '<div class="form-group row">' +
              '<label for="inputName" class="col-sm-2 col-form-label">Genre Name</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="Genre name" id="inputName" #genreName>' +
              '</div>' +
            '</div>' +
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadGenres(),
        Swal.fire('Saved!', '', 'success')
        return [
          document.getElementById('inputName')?.ariaValueText,
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

  async editGenre(id: string){
    var genreSelected = this.genres.find(g => g.id == id);
    var result = await Swal.fire({
      html:
      '<h2>Genre: </h2>' +
      '<form>' +
          '<div class="form-group row">' +
              '<label for="inputName" class="col-sm-2 col-form-label">Genre Name</label>' +
              '<div class="col-sm-10">' +
                '<input type="text" class="form-control" placeholder="Genre name" id="inputName" #genreName>' +
              '</div>' +
            '</div>' +
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadGenres(),
        Swal.fire('Saved!', '', 'success')
        return [
          document.getElementById('inputName')?.ariaValueText,
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
        this.genreService.deleteGenre(id).subscribe(),
        this.loadGenres(),
        Swal.fire(
          'Deleted!',
          'The genre has been deleted.',
          'success'
        )
      }
    })
  }

}
