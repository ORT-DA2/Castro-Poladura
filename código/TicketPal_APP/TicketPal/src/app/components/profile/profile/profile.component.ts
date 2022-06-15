import { Component, OnInit } from '@angular/core';
import { IUser } from 'src/app/models/response/user.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { UserService } from 'src/app/services/user/user.service';
import { IUpdateUser } from 'src/app/models/request/user/updateUser.model';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  currentUser: IUser | null
  profileForm: FormGroup
  responseMessage = ''
  error: boolean = false

  constructor(
    private token: TokenStorageService,
    private formBuilder: FormBuilder,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.token.getUser()
    this.preloadForm()
  }

  preloadForm = () => {
    this.profileForm = this.formBuilder.group({
      firstname: this.currentUser?.firstname,
      lastname: this.currentUser?.lastname,
      password: ''
    });
  }

  onUpdateProfileClicked = () => {

    var request: IUpdateUser = {} as IUpdateUser
    const headers = new HttpHeaders()
      .set('Content-Type', 'application/json')
      .set('Authorization', `Bearer ${this.token.getToken()}`)

    if (this.currentUser) {
      request = {
        firstname: this.profileForm.controls['firstname'].value,
        lastname: this.profileForm.controls['lastname'].value,
        password: this.profileForm.controls['password'].value,
        role: this.currentUser?.role
      }
    }

    if (this.currentUser) {
      this.userService.updateUser(
        this.currentUser.id,
        request,
        headers
      )
        .subscribe(
          {
            next: data => {
              this.responseMessage = data.message
              if (this.currentUser) {
                this.userService.getUser(
                  this.currentUser.id,
                  headers
                ).subscribe(
                  {
                    next: data => {
                      this.token.saveUser(
                        {
                          id: this.currentUser?.id,
                          firstname: data.firstname,
                          lastname: data.lastname,
                          password: data.password,
                          role: this.currentUser?.role,
                          activeAccount: this.currentUser?.activeAccount,
                          email: this.currentUser?.email
                        } as IUser
                      )
                    }
                  }
                )
              }
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

}
