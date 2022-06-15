import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ITicket } from 'src/app/models/response/ticket.model';
import { IUser } from 'src/app/models/response/user.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';
import { TicketService } from 'src/app/services/ticket/ticket.service';
import { UserService } from 'src/app/services/user/user.service';
import Swal, { SweetAlertResult } from 'sweetalert2';

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

  @ViewChild('editUserView', { static: false })
  userEditPopup: ElementRef;

  selectedUserToEdit: IUser
  show = false
  add = false

  constructor(
    private userService: UserService,
    private tokenService: TokenStorageService,
    private ticketService: TicketService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.tokenService.getUser()
    this.adminLoggedIn = (this.currentUser?.role == "ADMIN")
    this.loadUsers();
    this.loadTickets();
  }

  refresh = (): void => {
    this.loadUsers()
    Swal.close()
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

  loadTickets(): void {
    this.tickets = []
    this.ticketService.getTickets().subscribe(
      {
        next: data => {
          this.tickets = data
        }
      }
    )
  }

  addUser() {
    this.add = true
    this.show = true
    this.selectedUserToEdit = {} as IUser;
    Swal.fire({
      html: this.userEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedUserToEdit = {} as IUser
        this.show = false
        this.add = false
      }
    })
  }

  editUser(selected: IUser) {
    this.show = true
    this.add = false
    this.selectedUserToEdit = selected;

    Swal.fire({
      html: this.userEditPopup.nativeElement,
      focusConfirm: false,
      showConfirmButton: false,
      showDenyButton: false,
      allowOutsideClick: true,
      backdrop: true
    }).then((result) => {
      if (result.isDismissed) {
        this.selectedUserToEdit = {} as IUser
        this.show = false
      }
    })
  }

  deleteUser(user: IUser) {
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
        this.userService.deleteUser(user.id).subscribe(
          {
            next: data => {
              Swal.fire({
                icon: 'success',
                text: data.message,
              })
              this.loadUsers()
            },
            error: err => {
              if (this.tickets.find(t => t.buyer.id == user.id)) {
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
