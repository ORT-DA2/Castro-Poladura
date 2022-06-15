import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IAddPerformer } from 'src/app/models/request/performer/addPerformer.model';
import { IUpdatePerformer } from 'src/app/models/request/performer/updatePerformer.model';
import { IGenre } from 'src/app/models/response/genre.model';
import { IPerformer } from 'src/app/models/response/performer.model';
import { IUser } from 'src/app/models/response/user.model';
import { GenreService } from 'src/app/services/genre/genre.service';
import { PerformerService } from 'src/app/services/performer/performer.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { UserService } from 'src/app/services/user/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-performer-edit-modal',
  templateUrl: './performer-edit-modal.component.html',
  styleUrls: ['./performer-edit-modal.component.css']
})
export class PerformerEditModalComponent implements OnInit {

  @Input() performer: IPerformer
  @Input() add: Boolean = false
  @Input() refresh: () => void;

  editForm: FormGroup
  responseMessage = ''
  error: boolean = false

  accounts: IUser[]
  selectedAccount: IUser
  genres: IGenre[]
  selectedGenre: IGenre

  members: IPerformer[]
  tempMembers: IPerformer[]

  constructor(
    private userService: UserService,
    private performerService: PerformerService,
    private genreService: GenreService,
    private formBuilder: FormBuilder,
    private token: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.tempMembers = []
    this.performer.members.forEach(val => this.tempMembers.push(Object.assign({}, val)));

    this.userService.getUsers().subscribe(
      {
        next: data => {
          this.accounts = data
        }
      }
    )
    this.performerService.getPerformers().subscribe(
      {
        next: data => {
          this.members = data
        }
      })
    this.genreService.getGenres().subscribe(
      {
        next: data => {
          this.genres = data
        }
      }
    )
    this.preloadForm()
  }

  preloadForm = () => {
    this.selectedAccount = this.performer.userInfo
    this.editForm = this.formBuilder.group({
      performerType: this.performer.performerType,
      startYear: this.performer.startYear,
      selectedAccount: this.performer.userInfo.email,
      members: this.performer.members,
      tempMembers: this.tempMembers,
      selectedGenre: this.performer.genre,
      selectedMember: {} as IPerformer
    });
  }

  onSubmitClicked() {
    this.responseMessage = ""
    this.error = false

    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', `Bearer ${this.token.getToken()}`)

    if (!this.add) {
      const genre = this.editForm.controls["selectedGenre"].value as IGenre
      const request = {
        performerType: this.editForm.controls["performerType"].value,
        artistsIds: this.tempMembers.map(m => Number(m.id)),
        genreId: Number(genre.id),
        startYear: this.editForm.controls["startYear"].value
      } as IUpdatePerformer;
      console.log(request)
      this.performerService.updatePerformer(
        this.performer.id,
        request,
        headers
      ).subscribe(
        {
          next: data => {
            this.refresh()
            this.responseMessage = data.message
            this.successToast(this.responseMessage)
          },
          error: err => {
            this.error = true
            this.responseMessage = err.error.message
          }
        }
      )
    } else {
      const request = {
        userId: +this.selectedAccount.id,
        performerType: this.editForm.controls["performerType"].value,
        memberIds: this.tempMembers.map(m => +m.id),
        genre: +this.selectedGenre.id,
        startYear: this.editForm.controls["startYear"].value
      } as IAddPerformer
      console.log(request)
      this.performerService.addPerformer(
        request,
        headers
      ).subscribe(
        {
          next: data => {
            this.responseMessage = data.message
            this.refresh()
          },
          error: err => {
            this.error = true
            this.responseMessage = err.error.message
          }
        }
      )
    }
  }

  removeMember(member: IPerformer) {
    const index = this.tempMembers.indexOf(member)
    if (index !== -1) {
      this.tempMembers.splice(index, 1);
    }
  }
  addMember() {
    const selected = this.editForm.controls['selectedMember'].value as IPerformer
    this.tempMembers.push(selected)
  }

  isResponse = (): boolean => {
    return this.responseMessage !== null && this.responseMessage.length > 0;
  }

  successToast = (message: string) => {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })

    Toast.fire({
      icon: 'success',
      title: message
    })

  }

}
