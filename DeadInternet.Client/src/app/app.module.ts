import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/auth/login/login.component';
import { PostListComponent } from './home/post-list/post-list.component';
import { JwtModule } from '@auth0/angular-jwt';
import { RegisterComponent } from './features/auth/register/register.component';
import { AuthInterceptor } from './core/auth/auth.interceptor';
import { LlmPromptComponent } from './home/llm-prompt/llm-prompt.component';
import { CommentsComponent } from './comments/comments.component';
import { CommentComponent } from './comments/comment/comment.component';
import { ErrorMessageComponent } from "./core/error-handling/error-display.component";
import { ErrorPopupComponent } from './core/error-handling/error-popup/error-popup.component';
import { LoadingComponent } from './core/loading/loading.component';
import { ButtonComponent } from './shared/components/button/button.component';
import { TextButtonComponent } from './shared/components/text-button/text-button.component';
import { PostItemComponent } from './home/post-item/post-item.component';
import { FormInputComponent } from './shared/components/form-input/form-input.component';
import { AuthFormComponent } from './features/auth/auth-form/auth-form.component

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PostListComponent,
    RegisterComponent,
    LlmPromptComponent,
    CommentsComponent,
    CommentComponent,
    ErrorMessageComponent,
    ErrorPopupComponent,
    LoadingComponent,
    ButtonComponent,
    TextButtonComponent,
    PostItemComponent,
    FormInputComponent,
    AuthFormComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,

    JwtModule.forRoot({
      config: {
        allowedDomains: ['localhost:5001'],
      }
    }),

  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
