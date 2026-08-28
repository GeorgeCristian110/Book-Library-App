import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule, ɵnormalizeQueryParams } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { QuoteService } from '../../../core/services/quote.service';

@Component({
  selector: 'app-quote-form',
  imports: [FormsModule, CommonModule],
  templateUrl: './quote-form.html',
  styleUrl: './quote-form.css',
})
export class QuoteForm implements OnInit{
  private quoteService = inject(QuoteService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  quoteId: number | null = null;
  isEditMode = false;

  text = '';
  author = '';
  bookId: number | null = null;

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');

    if(idParam){
      this.isEditMode = true;
      this.quoteId = Number(idParam);
      
      this.quoteService.getById(this.quoteId).subscribe({
        next: (quote) => {
          this.text = quote.text;
          this.author = quote.author ?? '';
          this.bookId = quote.bookId;
        },
      });
    }
  }

  onSubmit() {
    const quoteData = {
      text: this.text,
      author: this.author,
      bookId: this.bookId,
    };

    if(this.isEditMode && this.quoteId){
      this.quoteService.update(this.quoteId, quoteData).subscribe({
        next: () => this.router.navigate(['/quotes']),
      });
    } else {
      this.quoteService.create(quoteData).subscribe({
        next: () => this.router.navigate(['/quotes']),
      });
    }
  }

}
