import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { QuoteResponse } from "../models";


@Injectable({
    providedIn: 'root'
})
export class QuoteService {
    private http = inject(HttpClient);

    getMyQuotes(){
        return this.http.get<QuoteResponse[]>('http://localhost:5222/api/Quotes');
    }

    delete(id : number) {
        return this.http.delete<void>(`http://localhost:5222/api/Quotes/${id}`)
    }

    getById(id : number) {
        return this.http.get<QuoteResponse>(`http://localhost:5222/api/Quotes/${id}`)
    }

    create(quote : {text: string; author: string; bookId: number | null})
    {
        return this.http.post<QuoteResponse>('http://localhost:5222/api/Quotes', quote)
    }

    update(id : number, quote: {text: string; author: string; bookId: number | null}) 
    {
        return this.http.put<void>(`http://localhost:5222/api/Quotes/${id}`, quote)
    }
}