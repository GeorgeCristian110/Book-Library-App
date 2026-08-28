import { BookResponse } from '../models' 
import { Injectable, inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'

@Injectable({
    providedIn: 'root'
})
export class BookService{
    private http = inject(HttpClient);

    getAll(){
        return this.http.get<BookResponse[]>(`http://localhost:5222/api/Books`);
    }

    delete(id: number){
        return this.http.delete<void>(`http://localhost:5222/api/Books/${id}`)
    }

    getById(id: number){
        return this.http.get<BookResponse>(`http://localhost:5222/api/Books/${id}`)
    }

    create(book: {title: string; author: string; publishedDate: string; genre: string; description: string }){
        return this.http.post<BookResponse>(`http://localhost:5222/api/Books`, book)
    }

    update(id: number, book:{title: string; author: string; publishedDate: string; genre: string; description: string }){
        return this.http.put<void>(`http://localhost:5222/api/Books/${id}`, book)
    }
}