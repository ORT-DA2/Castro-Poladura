import { Concert } from "./concert.model";
import { User } from "./user.model";

export class Ticket {
    id: string;
    status: string;
    purchaseDate: Date;
    code: string;
    event: Concert;
    buyer: User;
}