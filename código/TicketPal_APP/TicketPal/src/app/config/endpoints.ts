import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable()
export class Endpoints {
    public readonly API_BASE_URL: string = environment.ticketPal.api.baseUrl;
    public readonly USERS: string = `${this.API_BASE_URL}/users`;
    public readonly PERFORMERS: string = `${this.API_BASE_URL}/performers`;
    public readonly GENRES: string = `${this.API_BASE_URL}/genres`;
    public readonly CONCERTS: string = `${this.API_BASE_URL}/events`;
    public readonly CONCERTSBYPERFORMER: string = `${this.API_BASE_URL}/events/performer`;
    public readonly TICKETS: string = `${this.API_BASE_URL}/tickets`;
}