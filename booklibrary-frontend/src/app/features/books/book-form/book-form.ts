import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute} from '@angular/router'
import { BookService } from '../../../core/services/book.service';

@Component({
  selector: 'app-book-form',
  imports: [FormsModule, CommonModule],
  templateUrl: './book-form.html',
  styleUrl: './book-form.css',
})
export class BookForm implements OnInit {
  private bookService = inject(BookService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  bookId: number | null = null;
  isEditMode = false;

  title = '';
  author = '';
  publishedDate = '';
  genre = '';
  description = '';


  ngOnInit(){
    const idParam = this.route.snapshot.paramMap.get('id');

    if(idParam){
      this.isEditMode = true;
      this.bookId = Number(idParam);
   

      this.bookService.getById(this.bookId).subscribe({
        next: (book) => {
          console.log('Book loaded from server:', book);
          this.title = book.title;
          this.author = book.author;
          this.publishedDate = book.publishedDate;
          this.genre = book.genre;
          this.description = book.description ?? '';

        },
      });
    }
  }

  onSubmit(){
    const bookData = {
      title : this.title,
      author : this.author,
      publishedDate : this.publishedDate,
      genre : this.genre,
      description : this.description,
    };

    if (this.isEditMode && this.bookId){
      this.bookService.update(this.bookId, bookData).subscribe({
        next: () => this.router.navigate(['/books'])
      });
    } else {
      this.bookService.create(bookData).subscribe({
        next : () => this.router.navigate(['/books']),
      });
    }
  }

}
