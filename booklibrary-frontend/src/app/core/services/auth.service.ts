import {Injectable, signal, computed, inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthResponse } from '../models';
import { tap } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private tokenSignal = signal<string | null>(null);
    private usernameSignal = signal<string | null>(null);
    readonly isLoggedIn = computed(() => this.tokenSignal() !== null);

    private http = inject(HttpClient);

    login(email: string, password:string){
        return this.http.post<AuthResponse>('http://localhost:5222/api/Auth/login', 
            {email, password}).pipe(
            tap((response : AuthResponse) => {
                this.tokenSignal.set(response.token);
                this.usernameSignal.set(response.username);
            })
        );
    }
    
    register(username : string, email: string, password: string){
        return this.http.post<AuthResponse>('http://localhost:5222/api/Auth/register', 
            {username, email, password})
            .pipe(
                tap((response : AuthResponse) => {
                    this.tokenSignal.set(response.token);
                    this.usernameSignal.set(response.username);
                })
            );
    }
}

