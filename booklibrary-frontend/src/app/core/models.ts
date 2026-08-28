export interface AuthResponse {
    id : number;
    username: string;
    email : string;
    token : string;
}

export interface BookResponse {
    id: number;
    title: string;
    author: string;
    publishedDate: string;
    genre: string;
    description: string | null;
    ownerUsername: string;
}

export interface QuoteResponse {
    id: number;
    text: string;
    author: string | null;
    ownerId: number;
    bookId: number | null;
    bookTitle: string | null;
}