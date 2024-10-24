import { Component, OnDestroy, OnInit } from '@angular/core';
import { PostService } from '../../features/services/post.service';
import { Subscription, catchError, throwError } from 'rxjs';
import { ErrorService } from '../../core/error-handling/error.service';
import { PostData } from '../../models/post.model';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrl: './post-list.component.css'
})
export class PostListComponent implements OnInit, OnDestroy {
  posts: PostData['data'] = {} as PostData['data'];
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
        (res: PostData) => {
          this.posts = res.data
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
