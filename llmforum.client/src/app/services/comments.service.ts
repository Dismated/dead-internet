import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private apiUrl = 'https://localhost:7201/api/comments';

  constructor(private http: HttpClient) { }

  getComments(id: string | null): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/post/${id}`)
  }

  deleteCommentChain(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
  }
}
