import { Component, OnInit } from '@angular/core';
import { IUser } from 'src/app/models/response/user.model';
import { TokenStorageService } from 'src/app/services/storage/token-storage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  currentUser: IUser | null
  sellerLoggedIn = false
  artistLoggedIn = false

  constructor(
    private token: TokenStorageService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.token.getUser(),
    this.sellerLoggedIn = this.isSellerLogged(),
    this.artistLoggedIn = this.isArtistLogged()
  }

  isSellerLogged(): boolean{
    return this.sellerLoggedIn = (this.currentUser?.role == "SELLER")
  }

  isArtistLogged(): boolean{
    return this.artistLoggedIn = (this.currentUser?.role == "ARTIST")
  }

}
