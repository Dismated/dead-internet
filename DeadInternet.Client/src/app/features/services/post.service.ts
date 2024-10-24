import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostData, PromptNRepliesData } from '../../models/post.model';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private apiUrl = 'https://localhost:7201/api/home';

  constructor(private http: HttpClient) { }

  createPost(prompt: { prompt: string }): Observable<PromptNRepliesData> {
    return this.http.post<PromptNRepliesData>(`${this.apiUrl}/prompt`, prompt)
  }
  getPosts(): Observable<PostData> {
    return this.http.get<PostData>(this.apiUrl)
  }

  deletePost(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
