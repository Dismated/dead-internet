
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, throwError } from 'rxjs';
import { ErrorService } from '../../core/services/error.service';


@Component({
  selector: 'app-llm-prompt',
  templateUrl: './llm-prompt.component.html',
  styleUrl: './llm-prompt.component.css'
})
export class LlmPromptComponent implements OnInit {
  promptText = '';
  response = '';
  errorMessage = '';
  spaces: string = " "
  placeholder: string = `Create a post ${this.spaces} (Shift + Enter for new line)`
  loading: boolean = false;


  constructor(private http: HttpClient, private router: Router, private errorService: ErrorService) { }

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


  adjustTextareaHeight(textarea: HTMLTextAreaElement): void {
    textarea.style.height = 'auto';
    textarea.style.height = `${textarea.scrollHeight} px`;
  }

  updatePlaceholder(): void {
    const viewportWidth = window.innerWidth;
    const numberOfSpaces = Math.floor((viewportWidth / 2 - 380) / 4.4);
    this.spaces = ' '.repeat(numberOfSpaces);

    this.placeholder = `Create a post ${this.spaces} (Shift + Enter for new line)`
    console.log(this.placeholder, this.spaces, numberOfSpaces)
  }


}
