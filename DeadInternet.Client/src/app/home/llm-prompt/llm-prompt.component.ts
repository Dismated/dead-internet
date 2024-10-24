import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, throwError } from 'rxjs';
import { ErrorService } from '../../core/error-handling/error.service';
import { PostService } from '../../features/services/post.service';
import { Comment } from '../../models/comment.model';
import { PromptNRepliesData } from '../../models/post.model';

@Component({
  selector: 'app-llm-prompt',
  templateUrl: './llm-prompt.component.html',
  styleUrls: ['./llm-prompt.component.css'] // Note: Fix typo from "styleUrl" to "styleUrls"
})
export class LlmPromptComponent {
  promptText = '';
  response: PromptNRepliesData['data'] = {} as PromptNRepliesData['data'];
  errorMessage = '';
  loading = false;

  constructor(private http: HttpClient, private router: Router, private errorService: ErrorService, private postService: PostService) { }

  onSubmit(): void {
    if (!this.promptText) return;
    const prompt = { prompt: this.promptText };
    this.loading = true;

    this.postService
      .createPost(prompt)
      .pipe(
        catchError((error) => {
          this.errorService.setErrorMessage('Failed to create post. Please try again.');
          return throwError(() => error);
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe((res: PromptNRepliesData) => this.handleSuccess(res));
  }

  private handleError(error: HttpErrorResponse): any {
    const errorMessage = error.error instanceof ErrorEvent
      ? `Error: ${error.error.message}`
      : `Error Code: ${error.status}\nMessage: ${error.message}`;

    this.errorService.setErrorMessage(errorMessage);
    return throwError(() => error);
  }

  private handleSuccess(res: PromptNRepliesData): void {
    this.response = res.data;
    this.router.navigate(['/comments', this.response.prompt.postId]);
  }
}
