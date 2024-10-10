import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, throwError } from 'rxjs';
import { ErrorService } from '../../core/error-handling/error.service';
import { PostService } from '../../features/services/post.service';

@Component({
  selector: 'app-llm-prompt',
  templateUrl: './llm-prompt.component.html',
  styleUrls: ['./llm-prompt.component.css'] // Note: Fix typo from "styleUrl" to "styleUrls"
})
export class LlmPromptComponent implements OnInit {
  promptText = '';
  response: any = '';
  errorMessage = '';
  placeholder: string = '';
  loading = false;

  constructor(private http: HttpClient, private router: Router, private errorService: ErrorService, private postService: PostService) { }

  ngOnInit(): void {
    this.updatePlaceholder();
    window.addEventListener('resize', this.updatePlaceholder.bind(this)); // Recalculate on resize
  }

  onSubmit(): void {
    if (!this.promptText) return;

    this.loading = true;

    this.postService
      .createPost(this.promptText)
      .pipe(
        catchError((error) => {
          this.errorService.setErrorMessage('Failed to create post. Please try again.');
          return throwError(() => error);
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe((res: any) => this.handleSuccess(res));
  }

  handleKeyDown(event: KeyboardEvent): void {
    const textarea = event.target as HTMLTextAreaElement;

    if (event.key === 'Enter') {
      event.preventDefault();

      if (event.shiftKey) {
        this.insertNewLineAtCursor(textarea);
      } else {
        this.onSubmit();
      }
    }
  }

  private insertNewLineAtCursor(textarea: HTMLTextAreaElement): void {
    const cursorPosition = textarea.selectionStart;
    this.promptText = `${this.promptText.slice(0, cursorPosition)}\n${this.promptText.slice(cursorPosition)}`;
    textarea.value = this.promptText;
    this.adjustTextareaHeight(textarea);
  }

  adjustTextareaHeight(textarea: HTMLTextAreaElement): void {
    textarea.style.height = 'auto';
    textarea.style.height = `${textarea.scrollHeight}px`; // Fix to remove extra space issue
  }

  private handleError(error: HttpErrorResponse): any {
    const errorMessage = error.error instanceof ErrorEvent
      ? `Error: ${error.error.message}`
      : `Error Code: ${error.status}\nMessage: ${error.message}`;

    this.errorService.setErrorMessage(errorMessage);
    return throwError(() => error);
  }

  private handleSuccess(res: any): void {
    this.response = res.data;
    this.router.navigate(['/comments', this.response.prompt.postId]);
  }

  updatePlaceholder(): void {
    const viewportWidth = window.innerWidth;
    const numberOfSpaces = Math.max(Math.floor((viewportWidth / 2 - 380) / 4.4), 0); // Avoid negative spaces
    this.placeholder = `Create a post ${' '.repeat(numberOfSpaces)} (Shift + Enter for new line)`;
  }
}
