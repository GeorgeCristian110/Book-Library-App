import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  password = '';
  errorMessage = '';

  onSubmit(){
    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        this.router.navigate(['/books']);
      },
      error: () => {
        this.errorMessage= 'Invalid email or password';
      },
    });
    
  }

}
