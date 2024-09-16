import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './home/home.component';
import { JwtModule } from '@auth0/angular-jwt';
import { RegisterComponent } from './auth/register/register.component';
import { AuthInterceptor } from './core/auth.interceptor';
import { LlmPromptComponent } from './home/llm-prompt/llm-prompt.component';
import { CommentsComponent } from './comments/comments.component';
import { SharedModule } from './shared/shared.module';
import { CommentComponent } from './comments/comment/comment.component';





@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    LlmPromptComponent,
    CommentsComponent,
    CommentComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,
    FormsModule,
    SharedModule,
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
