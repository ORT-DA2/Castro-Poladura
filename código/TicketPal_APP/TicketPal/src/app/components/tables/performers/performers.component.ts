import { Component, OnInit } from '@angular/core';
import { IGenre } from 'src/app/models/response/genre.model';
import { IPerformer } from 'src/app/models/response/performer.model';
import { IUser } from 'src/app/models/response/user.model';
import { GenreService } from 'src/app/services/genre/genre.service';
import { PerformerService } from 'src/app/services/performer/performer.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-performers',
  templateUrl: './performers.component.html',
  styleUrls: ['./performers.component.css']
})
export class PerformersComponent implements OnInit {
  performers: IPerformer[];
  genres: IGenre[];
  fetchedPerformers = false;
  fetchedGenres = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: IUser | null

  constructor(
    private performerService: PerformerService, private tokenService: TokenStorageService, private genreService: GenreService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
    this.loadPerformers();
    this.loadGenres();
  }

  loadPerformers(): void{
    this.performers = []
    this.performerService.getPerformers().subscribe(
      {
        next: data => {
          this.performers = data
          this.fetchedPerformers = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

  loadGenres(): void {
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

  showArtists(id: string): void {
    var info = 'Members: ';
    var performerSelected = this.performers.find(p => p.id == id);
    performerSelected?.members.forEach(p => {
      info += p.userInfo.firstname + ' ' + p.userInfo.lastname + ',' + '\n'
    });
    info = info.substring(0, info.length - 2);
    Swal.fire({
      text: info,
    })
  }

  async addPerformer(){
    var result = await Swal.fire({
      html:
      '<h2>Performer: </h2>'+
      '<form>'+
          '<div class="form-group row">'+
              '<label for="inputPerformerType" class="col-sm-2 col-form-label">Performer Type</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="SOLO_ARTIST" id="inputPerformerType" #performerType>'+
              '</div>'+
            '</div>'+
            '<div class="form-group row">'+
              '<label for="inputYear" class="col-sm-2 col-form-label">Start Year</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="1981" id="inputYear" #year>'+
              '</div>'+
            '</div>'+
          '<div class="form-group row">'+
              '<label for="inputGenre">Genres</label>'+
              '<select class="form-control" id="inputGenre" *ngFor="let genre of genres">'+
                  '<option value="genre.id" #genre>{{genre.name}}</option>'+
              '</select>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputMember">Members</label>'+
              '<select class="custom-select" multiple d="inputMember" *ngFor="let performer of performers">'+
                  '<option selected #performer>None</option>'+
                  '<option value="performer.id" #performer>{{performer.userInfo.firstname}} {{performer.userInfo.lastname}}</option>'+
                '</select>'+
          '</div>'+
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadPerformers(),
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

  async editPerformer(id: string){
    var performerSelected = this.performers.find(p => p.id == id);
    var result = await Swal.fire({
      html:
      '<h2>Performer: </h2>'+
      '<form>'+
          '<div class="form-group row">'+
              '<label for="inputPerformerType" class="col-sm-2 col-form-label">Performer Type</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="SOLO_ARTIST" id="inputPerformerType" #performerType>'+
              '</div>'+
            '</div>'+
            '<div class="form-group row">'+
              '<label for="inputYear" class="col-sm-2 col-form-label">Start Year</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="1981" id="inputYear" #year>'+
              '</div>'+
            '</div>'+
          '<div class="form-group row">'+
              '<label for="inputGenre">Genres</label>'+
              '<select class="form-control" id="inputGenre" *ngFor="let genre of genres">'+
                  '<option value="genre.id" #genre>{{genre.name}}</option>'+
              '</select>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputMember">Members</label>'+
              '<select class="custom-select" multiple d="inputMember" *ngFor="let performer of performers">'+
                  '<option selected #performer>None</option>'+
                  '<option value="performer.id" #performer>{{performer.userInfo.firstname}} {{performer.userInfo.lastname}}</option>'+
                '</select>'+
          '</div>'+
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadPerformers(),
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

  deletePerformer(id: string) {
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
        this.performerService.deletePerformer(id).subscribe(
          {
            next: data => {
              Swal.fire({
                icon: 'success',
                text: data.message,
              })
              this.loadPerformers()
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
