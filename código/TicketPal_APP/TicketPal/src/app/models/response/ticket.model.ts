import { IConcert } from "./concert.model";
import { User } from "./user.model";

export class Ticket {
    id: string;
    status: string;
    purchaseDate: Date;
    code: string;
    concert: IConcert;
    buyer: User;
}