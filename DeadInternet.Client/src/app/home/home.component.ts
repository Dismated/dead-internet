import { Component, OnDestroy, OnInit } from '@angular/core';
import { PostService } from '../features/services/post.service';
import { Subscription, catchError, throwError } from 'rxjs';
import { ErrorService } from '../core/error-handling/error.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, OnDestroy {
  posts: any[] = []
  private subscriptions = new Subscription();


  constructor(private postService: PostService, private errorService: ErrorService) {
  }

  ngOnInit(): void {
    this.subscriptions.add(
      this.postService.getPosts().pipe(
        catchError(error => {
          this.errorService.setErrorMessage('Failed to load posts');
          return throwError(() => error);
        })
      ).subscribe(
        res => {
          this.posts = res
        }
      )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  isUserAuthenticated() {
    return !!localStorage.getItem('token');
  }

  deletePost(id: string) {
    this.subscriptions.add(
      this.postService.deletePost(id).pipe(
        catchError(error => {
          this.errorService.setErrorMessage('Failed to delete post');
          return throwError(() => error);
        })
      ).subscribe(
        () => {
          this.posts = this.posts.filter((post) => post.id !== id);
        }
      )
    );
  }
}
