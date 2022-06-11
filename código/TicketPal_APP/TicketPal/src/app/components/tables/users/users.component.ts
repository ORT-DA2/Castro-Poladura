import { Component, OnInit } from '@angular/core';
import { IUser } from 'src/app/models/response/user.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { UserService } from 'src/app/services/user/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: IUser[];
  fetchedUsers = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: User | null

  constructor(
    private userService: UserService, private tokenService: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
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

  addUser(){

  }

  editUser(id: string){

  }

  deleteUser(id: string) {
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
          'The user has been deleted.',
          'success'
        )
        //this.userService.deleteUser(id)
      }
    })
  }

}
