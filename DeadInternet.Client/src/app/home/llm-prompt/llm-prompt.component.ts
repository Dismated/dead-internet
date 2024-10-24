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
  spaces: string = " "
  placeholder: string = `Create a post ${this.spaces} (Shift + Enter for new line)`
  loading: boolean = false;

  constructor(private http: HttpClient, private router: Router, private errorService: ErrorService, private postService: PostService) { }

  onSubmit(): void {
    if (!this.promptText) return;
    const prompt = { prompt: this.promptText };
    this.loading = true;

  ngOnInit(): void {
    this.updatePlaceholder();
  }

  onSubmit() {
    if (this.promptText) {
      this.loading = true
      this.http.post('https://localhost:7201/api/home/prompt', { prompt: this.promptText })
        .pipe(
          catchError((error: HttpErrorResponse) => {
            let errorMessage = 'An unknown error occurred';
            if (error.error instanceof ErrorEvent) {
              errorMessage = `Error: ${error.error.message}`;
            } else {
              errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
            }
            this.errorService.setErrorMessage(errorMessage);
            return throwError(() => error);
          })
          ,
          finalize(() => this.loading = false))
        .subscribe(
          (res: any) => {
            this.response = res.response;
            this.router.navigate(['/comments', res.prompt.postId]);
          }
        );
    }
  }

  handleKeyDown(event: KeyboardEvent): void {
    const textarea = event.target as HTMLTextAreaElement;

    if (event.key === 'Enter' && event.shiftKey) {
      event.preventDefault();
      const cursorPosition = textarea.selectionStart;
      this.promptText =
        this.promptText.slice(0, cursorPosition) + '\n' + this.promptText.slice(cursorPosition);
      textarea.value = this.promptText;
      this.adjustTextareaHeight(textarea);
    }

    else if (event.key === 'Enter') {
      event.preventDefault();
      this.onSubmit();
    }
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
