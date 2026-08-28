import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { QuoteService } from '../../../core/services/quote.service';
import { QuoteResponse } from '../../../core/models';

@Component({
  selector: 'app-quote-list',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './quote-list.html',
  styleUrl: './quote-list.css',
})
export class QuoteList implements OnInit {
  private quoteService = inject(QuoteService);

  quotes: QuoteResponse[] = [];

  ngOnInit()  {
    
    this.quoteService.getMyQuotes().subscribe({
      next: (data) => {
        this.quotes = data;
      },
      error: () => {
        console.error('Failed to load quotes');
      },
    });
  }

  deleteQuote(id : number){

    if(confirm('Are you sure you want to delete the quote?')){
      this.quoteService.delete(id).subscribe({
        next: () => {
          this.quotes = this.quotes.filter((q) => q.id !== id);
        },
      });
    }

  }


}
