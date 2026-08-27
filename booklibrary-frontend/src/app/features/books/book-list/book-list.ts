import { Component, inject , OnInit} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookService } from '../../../core/services/book.service';
import { BookResponse } from '../../../core/models';

@Component({
  selector: 'app-book-list',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './book-list.html',
  styleUrl: './book-list.css',
})
export class BookList implements OnInit{
  private bookService = inject(BookService);
  private router = inject(Router);
  
  books: BookResponse[] = [];

  ngOnInit() {
    this.bookService.getAll().subscribe({
      next: (data) =>{
        this.books = data;
      },
      error: () => {
        console.error('Failed to load books');
      },
    });
  }

}

