import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html',
  styleUrls: ['./user-modal.component.css']
})
export class UserModalComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  saveChanges(firstName: string, lastName: string, email: string, password:string, role: string){
    
  }

}
