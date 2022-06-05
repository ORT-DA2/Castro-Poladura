import { Genre } from "./genre.model";
import { User } from "./user.model";

export class Performer {
    id: string;
    userInfo: User;
    performerType: string;
    startYear: string;
    genre: Genre;
    members: Performer[]; 
}