import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  loginWithGoogle(): void {
    window.location.href = `${this.apiBaseUrl}/auth/google/login`;
  }

  loginWithAzureAd(): void {
    window.location.href = `${this.apiBaseUrl}/auth/azure/login`;
  }
}
