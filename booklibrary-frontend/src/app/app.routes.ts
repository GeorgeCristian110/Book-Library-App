import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { Register } from './features/register/register';
import { BookList } from './features/books/book-list/book-list';

export const routes: Routes = [
    {path : 'login', component : Login},
    {path : 'register', component : Register},
    {path : "books", component: BookList},
];
