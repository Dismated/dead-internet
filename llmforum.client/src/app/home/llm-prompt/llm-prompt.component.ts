
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';


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


  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.updatePlaceholder();
  }

  onSubmit() {
    if (this.promptText) {
      this.http.post('https://localhost:7201/api/home/prompt', { prompt: this.promptText })
        .subscribe(
          (res: any) => {
            this.response = res.response;
            console.log(res)
            this.router.navigate(['/comments', res.prompt.postId]);

          },
          (error) => {
            console.error('Error:', error);
            this.errorMessage = 'An error occurred while processing your request.';
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
