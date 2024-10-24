
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommentsService } from "../features/services/comments.service";
import { ErrorService } from '../core/error-handling/error.service';
import { Subscription, catchError, finalize, throwError } from 'rxjs';
import { Reply } from "../models/comment.model";


@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit, OnDestroy {
  postId: string | null = null
  commentData: Reply[] = []
  promptText = ""
  private errorSubscription: Subscription | undefined;

  private subscriptions = new Subscription();

  constructor(
    private route: ActivatedRoute,
    private commentsService: CommentsService,
    private errorService: ErrorService
  ) { }

  ngOnInit() {
    this.postId = this.route.snapshot.paramMap.get('id');
    if (this.postId == null) {
      this.subscriptions.add(
        this.commentsService.getComments(this.postId!).pipe(
          catchError(error => {
            this.errorService.setErrorMessage('Failed to load comments');
            return throwError(() => error);
          })
        ).subscribe(
          res => {
            this.promptText = res.data.prompt.content
            this.commentData = res.data.replies;

          }
        ));
    }
    else {
      this.errorService.setErrorMessage('No post ID provided');
      this.loading = false;
    }
  }

  ngOnDestroy() {
    if (this.errorSubscription) {
      this.errorSubscription.unsubscribe();
    }
  }

  deleteCommentChain(commentId: string) {
    this.commentsService.deleteCommentChain(commentId).subscribe(() => {
      this.removeCommentRecursively(this.commentData, commentId);
    },
      (error) => {
        console.error('Error deleting comment:', error);
      });
  }

  private removeCommentRecursively(comments: Reply[], commentId: string): boolean {
    for (let i = 0; i < comments.length; i++) {
      if (comments[i].id === commentId) {
        comments.splice(i, 1);
        return true;
      }
      if (comments[i].replies && this.removeCommentRecursively(comments[i].replies, commentId)) {
        return true;
      }
    }
    return false;
  }
}


function throwError(arg0: () => any): any {
  throw new Error('Function not implemented.');
}
