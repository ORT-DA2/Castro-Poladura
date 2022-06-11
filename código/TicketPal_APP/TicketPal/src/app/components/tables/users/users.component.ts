import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IUser } from 'src/app/models/response/user.model';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: IUser[];
  fetchedUsers = false;
  errorMessage: string;

  constructor(
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.users = []
    this.userService.getUsers().subscribe(
      {
        next: data => {
          this.users = data
          this.fetchedUsers = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

}
