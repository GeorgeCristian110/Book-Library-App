import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { QuoteResponse , API_BASE_URL } from "../models";


@Injectable({
    providedIn: 'root'
})
export class QuoteService {
    private http = inject(HttpClient);

    getMyQuotes(){
        return this.http.get<QuoteResponse[]>(`${API_BASE_URL}/api/Quotes`);
    }

    delete(id : number) {
        return this.http.delete<void>(`${API_BASE_URL}/api/Quotes/${id}`)
    }

    getById(id : number) {
        return this.http.get<QuoteResponse>(`${API_BASE_URL}/api/Quotes/${id}`)
    }

    create(quote : {text: string; author: string; bookId: number | null})
    {
        return this.http.post<QuoteResponse>(`${API_BASE_URL}/api/Quotes`, quote)
    }

    update(id : number, quote: {text: string; author: string; bookId: number | null}) 
    {
        return this.http.put<void>(`${API_BASE_URL}/api/Quotes/${id}`, quote)
    }
}