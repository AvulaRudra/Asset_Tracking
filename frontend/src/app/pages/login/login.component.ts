import { Component, OnInit, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../services/auth.service';
import { AuthApiService } from '../../services/auth-api.service';
import { TokenStorageService } from '../../services/token-storage.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  mode: 'signin' | 'signup' = 'signin';

  signin = { username: '', password: '' };
  signup = { username: '', email: '', password: '' };

  errorMessage: string | null = null;
  isSubmitting = false;

  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  constructor(
    private readonly authService: AuthService,
    private readonly authApi: AuthApiService,
    private readonly tokenStorage: TokenStorageService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  // ✅ Moved out of constructor — ngOnInit only runs in the browser during navigation
  ngOnInit(): void {
    if (!this.isBrowser) return;

    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      this.tokenStorage.setAccessToken(token);
      this.router.navigate(['/dashboard']); // go straight to dashboard
    }
  }

  setMode(mode: 'signin' | 'signup'): void {
    this.mode = mode;
    this.errorMessage = null;
  }

  onLocalSignIn(): void {
    this.errorMessage = null;
    this.isSubmitting = true;
    this.authApi.signInLocal({ username: this.signin.username, password: this.signin.password }).subscribe({
      next: (res) => {
        this.tokenStorage.setAccessToken(res.AccessToken);
        this.isSubmitting = false;
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.errorMessage = err?.error?.detail ?? 'Sign in failed';
        this.isSubmitting = false;
      }
    });
  }

  onLocalSignUp(): void {
    this.errorMessage = null;
    this.isSubmitting = true;
    this.authApi
      .signUpLocal({ username: this.signup.username, email: this.signup.email || null, password: this.signup.password })
      .subscribe({
        next: (res) => {
          this.tokenStorage.setAccessToken(res.AccessToken);
          this.isSubmitting = false;
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.errorMessage = err?.error?.detail ?? 'Sign up failed';
          this.isSubmitting = false;
        }
      });
  }

  onGoogleLogin(): void {
    this.authService.loginWithGoogle();
  }

  onAzureAdLogin(): void {
    this.authService.loginWithAzureAd();
  }
}