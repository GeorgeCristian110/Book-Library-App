import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private authService = inject(AuthService);
  private router = inject(Router);

  username = '';
  email = '';
  password = '';
  errorMessage = '';

  onSubmit(){
     this.authService.register(this.username, this.email, this.password).subscribe({
      next : () => {
        this.router.navigate(['/login']);
      },
      error : (err) => {
        this.errorMessage = err.error?.title || err.error || "Registration failed. Please try again"
      },

     });
  }



}
