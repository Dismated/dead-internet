import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Comment, CommentInput } from '../../models/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private apiUrl = 'https://localhost:7201/api/comments';

  constructor(private http: HttpClient) { }

  getComments(id: string | null): Observable<Comment> {
    return this.http.get<Comment>(`${this.apiUrl}/post/${id}`)
  }

  deleteCommentChain(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
  }

  editComment(id: string, updatedContent: string): Observable<void> {
    const payload = { content: updatedContent };

    return this.http.put<void>(`${this.apiUrl}/${id}`, payload)
  }

  createComment(payload: CommentInput): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/reply`, payload)
  }
}
