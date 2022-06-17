import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IAddGenre } from 'src/app/models/request/genre/addGenre.model';
import { IUpdateGenre } from 'src/app/models/request/genre/updateGenre.model';
import { IGenre } from 'src/app/models/response/genre.model';
import { GenreService } from 'src/app/services/genre/genre.service';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-genre-edit-modal',
  templateUrl: './genre-edit-modal.component.html',
  styleUrls: ['./genre-edit-modal.component.css']
})
export class GenreEditModalComponent implements OnInit {

  @Input() genre: IGenre
  @Input() add: Boolean = false
  @Input() refresh: () => void;

  editForm: FormGroup
  responseMessage = ''
  error: boolean = false

  constructor(
    private token: TokenStorageService,
    private formBuilder: FormBuilder,
    private genreService: GenreService
  ) { }

  ngOnInit(): void {
    this.preloadForm()
  }

  onSubmitClicked() {
    this.responseMessage = ""
    this.error = false

    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', `Bearer ${this.token.getToken()}`)

    if (!this.add) {
      this.genreService.updateGenre(
        this.genre.id,
        {
          name: this.editForm.controls['genreName'].value
        } as IUpdateGenre,
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
      this.genreService.addGenre(
        {
          name: this.editForm.controls['genreName'].value
        } as IAddGenre,
        headers
      ).subscribe(
        {
          next: data => {
            this.responseMessage = data.message
            this.refresh()
            this.successToast(this.responseMessage)
          },
          error: err => {
            this.error = true
            this.responseMessage = err.error.message
          }
        }
      )
    }
  }

  preloadForm = () => {
    this.editForm = this.formBuilder.group({
      genreName: this.genre?.name
    });
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
