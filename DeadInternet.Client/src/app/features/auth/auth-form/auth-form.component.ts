import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.css']
})
export class AuthFormComponent {
  @Input() form!: FormGroup;
  @Input() formType: 'register' | 'login' = 'login';
  @Output() submitForm = new EventEmitter<void>();


  getErrorMessage(controlName: string): string {
    const control = this.form.get(controlName);
    if (control?.hasError('required')) {
      return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required.`;
    }
    if (control?.hasError('minlength')) {
      return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} must be at least 8 characters long.`;
    }
    if (control?.hasError('email')) {
      return 'Please enter a valid email address.';
    }
    if (control?.hasError('pattern')) {
      return 'Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be at least 8 characters long.';
    }
    return '';
  }

}
