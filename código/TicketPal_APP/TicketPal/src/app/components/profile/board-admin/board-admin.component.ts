import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-board-admin',
  templateUrl: './board-admin.component.html',
  styleUrls: ['./board-admin.component.css']
})
export class BoardAdminComponent implements OnInit {
  users: string[]
  errorMessage: string
  fetchedUsers = false
  error = false
  constructor(
    private userService: UserService
  ) { }

  ngOnInit(): void {
  }

}
