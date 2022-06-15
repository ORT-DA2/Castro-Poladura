import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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

  @ViewChild('editPerformerView', { static: false })
  performerEditPopup: ElementRef;

  selectedPerformerToEdit: IPerformer
  show = false
  add = false

  constructor(
    private performerService: PerformerService,
    private tokenService: TokenStorageService,
    private genreService: GenreService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser()
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN")
    this.loadPerformers()
  }

  refresh = (): void => {
    this.loadPerformers()
    Swal.close()
  }

  loadPerformers(): void {
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

  addPerformer() {
    this.add = true
    this.show = true
    this.selectedPerformerToEdit = {} as IPerformer;
    Swal.fire({
      html: this.performerEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedPerformerToEdit = {} as IPerformer
        this.show = false
        this.add = false
      }
    })
  }

  editPerformer(selected: IPerformer) {
    this.show = true
    this.add = false
    this.selectedPerformerToEdit = selected;

    Swal.fire({
      html: this.performerEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedPerformerToEdit = {} as IPerformer
        this.show = false
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
