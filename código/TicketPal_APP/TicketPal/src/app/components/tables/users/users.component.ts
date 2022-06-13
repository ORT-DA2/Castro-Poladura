import { Component, OnInit } from '@angular/core';
import { ITicket } from 'src/app/models/response/ticket.model';
import { IUser } from 'src/app/models/response/user.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';
import { UserService } from 'src/app/services/user/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: IUser[];
  tickets: ITicket[];
  userHasTickets: boolean;
  fetchedUsers = false;
  errorMessage: string;
  adminLoggedIn = false
  currentUser: IUser | null

  constructor(
    private userService: UserService, private tokenService: TokenStorageService, private ticketService: TicketService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser(),
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN"),
    this.loadUsers();
    this.loadTickets();
  }

  loadUsers(): void {
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

  loadTickets(): void{
    this.tickets = []
    this.ticketService.getTickets().subscribe(
      {
        next: data => {
          this.tickets = data
          this.fetchedUsers = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

  async addUser(){
    var result = await Swal.fire({
      html:
      '<h2>User: </h2>'+
      '<form>'+
          '<div class="form-group row">'+
              '<label for="inputFirstName" class="col-sm-2 col-form-label">First Name</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="First name" id="inputFirstName" #firstName>'+
              '</div>'+
            '</div>'+
            '<div class="form-group row">'+
              '<label for="inputLastName" class="col-sm-2 col-form-label">Last Name</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="Last name" id="inputLastName" #lastName>'+
              '</div>'+
            '</div>'+
          '<div class="form-group row">'+
          '<label for="inputEmail" class="col-sm-2 col-form-label">Email</label>'+
          '<div class="col-sm-10">'+
              '<input type="text" class="form-control" placeholder="example@example.com" id="inputEmail" #email>'+
          '</div>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputPassword" class="col-sm-2 col-form-label">Password</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="Password" id="inputPassword" #password>'+
              '</div>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputRole" class="col-sm-2 col-form-label">Role</label>'+
              '<select class="form-control" id="inputRole" #role>'+
                  '<option>ADMIN</option>'+
                  '<option>SELLER</option>'+
                  '<option>SUPERVISOR</option>'+
                  '<option>ARTIST</option>'+
                  '<option>SPECTATOR</option>'+
              '</select>'+
          '</div>'+
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadUsers(),
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

  async editUser(id: string){
    var userSelected = this.users.find(u => u.id == id);
    var result = await Swal.fire({
      html:
      '<h2>User: </h2>'+
      '<form>'+
          '<div class="form-group row">'+
              '<label for="inputFirstName" class="col-sm-2 col-form-label">First Name</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="First name" id="inputFirstName" #firstName>'+
              '</div>'+
            '</div>'+
            '<div class="form-group row">'+
              '<label for="inputLastName" class="col-sm-2 col-form-label">Last Name</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="Last name" id="inputLastName" #lastName>'+
              '</div>'+
            '</div>'+
          '<div class="form-group row">'+
          '<label for="inputEmail" class="col-sm-2 col-form-label">Email</label>'+
          '<div class="col-sm-10">'+
              '<input type="text" class="form-control" placeholder="example@example.com" id="inputEmail" #email>'+
          '</div>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputPassword" class="col-sm-2 col-form-label">Password</label>'+
              '<div class="col-sm-10">'+
                '<input type="text" class="form-control" placeholder="Password" id="inputPassword" #password>'+
              '</div>'+
          '</div>'+
          '<div class="form-group row">'+
              '<label for="inputRole" class="col-sm-2 col-form-label">Role</label>'+
              '<select class="form-control" id="inputRole" #role>'+
                  '<option>ADMIN</option>'+
                  '<option>SELLER</option>'+
                  '<option>SUPERVISOR</option>'+
                  '<option>ARTIST</option>'+
                  '<option>SPECTATOR</option>'+
              '</select>'+
          '</div>'+
        '</form>',
      focusConfirm: false,
      showConfirmButton: true,
      showDenyButton: true,
    }).then((result) => {
      if (result.isConfirmed) {
        this.loadUsers(),
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
        this.userService.deleteUser(id).subscribe(
          {
            next: data => {
              Swal.fire({
                icon: 'success',
                text: data.message,
              })
              this.loadUsers()
            },
            error: err => {
              if(this.tickets.find(t => t.buyer.id == id)){
                Swal.fire({
                  icon: 'error',
                  text: 'No se puede eliminar este usuario porque tiene tickets asociados',
                })
              }
              else {
                Swal.fire({
                  icon: 'error',
                  text: err.error.message,
                })
              }
            }
          }
        )
      }
    })
  }

}
