import { IGenre } from "./genre.model";
import { IUser } from "./user.model";

export interface IPerformer {
    id: string;
    userInfo: IUser;
    performerType: string;
    startYear: string;
    genre: IGenre;
    members: IPerformer[];
}