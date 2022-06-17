import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IAddUser } from 'src/app/models/request/user/addUser.model';
import { IUpdateUser } from 'src/app/models/request/user/updateUser.model';
import { IUser } from 'src/app/models/response/user.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { UserService } from 'src/app/services/user/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-edit-modal',
  templateUrl: './user-edit-modal.component.html',
  styleUrls: ['./user-edit-modal.component.css']
})
export class UserEditModalComponent implements OnInit {

  @Input() user: IUser
  @Input() add: Boolean = false
  @Input() refresh: () => void;

  editForm: FormGroup
  responseMessage = ''
  error: boolean = false

  constructor(
    private formBuilder: FormBuilder,
    private token: TokenStorageService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.preloadForm()
  }

  preloadForm = () => {

    this.editForm = this.formBuilder.group({
      firstname: this.user?.firstname,
      lastname: this.user?.lastname,
      password: '',
      email: this.user?.email,
      role: this.user?.role
    });
  }

  onSubmitClicked() {
    this.responseMessage = ""
    this.error = false

    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', `Bearer ${this.token.getToken()}`)

    if (!this.add) {
      this.userService.updateUser(
        this.user.id,
        {
          firstname: this.editForm.controls['firstname'].value,
          lastname: this.editForm.controls['lastname'].value,
          password: this.editForm.controls['password'].value,
          role: this.editForm.controls['role'].value,
          email: this.editForm.controls['email'].value
        } as IUpdateUser,
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
      this.userService.addUser(
        {
          firstname: this.editForm.controls['firstname'].value,
          lastname: this.editForm.controls['lastname'].value,
          password: this.editForm.controls['password'].value,
          role: this.editForm.controls['role'].value,
          email: this.editForm.controls['email'].value
        } as IAddUser,
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
