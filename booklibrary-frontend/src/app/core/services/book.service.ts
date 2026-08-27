import { BookResponse } from '../models' 
import { Injectable, inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'

@Injectable({
    providedIn: 'root'
})
export class BookService{
    private http = inject(HttpClient);

    getAll(){
        return this.http.get<BookResponse[]>('http://localhost:5222/api/Books');
    }
}