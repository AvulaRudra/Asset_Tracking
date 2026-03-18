import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  private readonly key = 'access_token';
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  getAccessToken(): string | null {
    if (!this.isBrowser) return null;
    return localStorage.getItem(this.key);
  }

  setAccessToken(token: string): void {
    if (!this.isBrowser) return;
    localStorage.setItem(this.key, token);
  }

  clear(): void {
    if (!this.isBrowser) return;
    localStorage.removeItem(this.key);
  }
}