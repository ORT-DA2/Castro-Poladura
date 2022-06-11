import { Component, OnInit } from '@angular/core';
import { IPerformer } from 'src/app/models/response/performer.model';
import { User } from 'src/app/models/response/user.model';
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
  fetchedPerformers = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: User | null

  constructor(
    private performerService: PerformerService, private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
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

  addPerformer(){

  }

  editPerformer(id: string){

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
        Swal.fire(
          'Deleted!',
          'The performer has been deleted.',
          'success'
        )
        //this.performerService.deletePerformer(id)
      }
    })
  }
}
