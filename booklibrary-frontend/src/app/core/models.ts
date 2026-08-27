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