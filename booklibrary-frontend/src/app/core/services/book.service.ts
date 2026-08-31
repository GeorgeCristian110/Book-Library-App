import { BookResponse, API_BASE_URL } from '../models' 
import { Injectable, inject } from '@angular/core'
import { HttpClient } from '@angular/common/http'

@Injectable({
    providedIn: 'root'
})
export class BookService{
    private http = inject(HttpClient);

    getAll(){
        return this.http.get<BookResponse[]>(`${API_BASE_URL}/api/Books`);
    }

    delete(id: number){
        return this.http.delete<void>(`${API_BASE_URL}/api/Books/${id}`)
    }

    getById(id: number){
        return this.http.get<BookResponse>(`${API_BASE_URL}/api/Books/${id}`)
    }

    create(book: {title: string; author: string; publishedDate: string; genre: string; description: string }){
        return this.http.post<BookResponse>(`${API_BASE_URL}/api/Books`, book)
    }

    update(id: number, book:{title: string; author: string; publishedDate: string; genre: string; description: string }){
        return this.http.put<void>(`${API_BASE_URL}/api/Books/${id}`, book)
    }
}