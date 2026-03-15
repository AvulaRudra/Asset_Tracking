import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';

export interface AuthResponseDto {
  access_token: string;
  token_type: string;
}

export interface SignUpRequestDto {
  username: string;
  email?: string | null;
  password: string;
}

export interface SignInRequestDto {
  username: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) {}

  signUpLocal(body: SignUpRequestDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/local/signup`, body);
  }

  signInLocal(body: SignInRequestDto): Observable<AuthResponseDto> {
    return this.http.post<AuthResponseDto>(`${this.baseUrl}/auth/local/login`, body);
  }
}
