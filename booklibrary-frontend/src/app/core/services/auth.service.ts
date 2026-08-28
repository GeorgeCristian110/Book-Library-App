import {Injectable, signal, computed, inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthResponse } from '../models';
import { tap } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly tokenKey = 'jwt_token';
    private readonly usernameKey = 'username';

    private tokenSignal = signal<string | null>(localStorage.getItem(this.tokenKey));
    private usernameSignal = signal<string | null>(localStorage.getItem(this.usernameKey));
   
    readonly isLoggedIn = computed(() => this.tokenSignal() !== null);
    
    getToken() {
        return this.tokenSignal();
    }

    getUsername(){
        return this.usernameSignal();
    }

    private http = inject(HttpClient);

    login(email: string, password:string){
        return this.http.post<AuthResponse>('http://localhost:5222/api/Auth/login', 
            {email, password}).pipe(
            tap((response : AuthResponse) => {
                this.tokenSignal.set(response.token);
                this.usernameSignal.set(response.username);

                localStorage.setItem(this.tokenKey, response.token);
                localStorage.setItem(this.usernameKey, response.username);
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

                    localStorage.setItem(this.tokenKey, response.token);
                    localStorage.setItem(this.usernameKey, response.username);
                })
            );
    }

    logout(){
        this.tokenSignal.set(null);
        this.usernameSignal.set(null);

        localStorage.removeItem(this.tokenKey);
        localStorage.removeItem(this.usernameKey);
    }
}

