import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

interface AuthResponse {
  data: {
    token: string
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7201/api/account';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    this.checkInitialAuthStatus();
  }

  private checkInitialAuthStatus() {
    const token = localStorage.getItem('token');
    this.isAuthenticatedSubject.next(!!token);
  }

  register(username: string, email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { username, email, password });
  }

  login(username: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(res => this.handleAuthResponse(res))
      );
  }

  guestLogin(): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/guest/login`, {})
      .pipe(
        tap(res => {
          console.log(res, res.data.token, 1);
          this.handleAuthResponse(res)
        })
      );
  }

  private handleAuthResponse(res: AuthResponse) {
    if (res && res.data.token) {
      localStorage.setItem('token', res.data.token);
      this.isAuthenticatedSubject.next(true);
      console.log(this.isAuthenticatedSubject.value);
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return this.isAuthenticatedSubject.value;
  }
}
