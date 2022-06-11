import { IConcert } from "./concert.model";
import { IUser } from "./user.model";

export interface ITicket {
    id: string;
    status: string;
    purchaseDate: Date;
    code: string;
    concert: IConcert;
    buyer: IUser;
}