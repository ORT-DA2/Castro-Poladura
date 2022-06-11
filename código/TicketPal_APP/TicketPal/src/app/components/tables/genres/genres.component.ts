import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IGenre } from 'src/app/models/response/genre.model';
import { GenreService } from 'src/app/services/genre/genre.service';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css']
})
export class GenresComponent implements OnInit {
  genres: IGenre[];
  fetchedGenres = false;
  errorMessage: string;

  constructor(
    private genreService: GenreService
  ) { }

  ngOnInit(): void {
    this.genres = []
    this.genreService.getGenres().subscribe(
      {
        next: data => {
          this.genres = data
          this.fetchedGenres = true
        }
        ,
        error: err => {
          this.errorMessage = err.error.message
        }
      }
    )
  }

}
