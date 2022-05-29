import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable()
export class Endpoints {
    public readonly API_BASE_URL: string = environment.ticketPal.api.baseUrl;
    public readonly USERS: string = `${this.API_BASE_URL}/users`;
}