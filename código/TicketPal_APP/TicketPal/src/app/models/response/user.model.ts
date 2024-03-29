
export interface IUser {
    id: string;
    firstname: string;
    lastname: string;
    email: string;
    password: string;
    token: string;
    role: string;
    activeAccount: boolean;
}