import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentsService } from '../../features/services/comments.service';
import { catchError, throwError, finalize } from 'rxjs';
import { ErrorService } from '../../core/error-handling/error.service';
import { Reply } from '../../models/comment.model';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() comment: Reply = {} as Reply
  @Input() depth: number = 0;
  @Output() deleteComment = new EventEmitter<string>();

  isReplying: boolean = false;
  replyContent: string = '';
  isMinimized: boolean = false;
  loading: boolean = false;

  constructor(private commentsService: CommentsService, private errorService: ErrorService, private cdr: ChangeDetectorRef) { }

  onReplyClick() {
    this.isReplying = true;
    this.cdr.detectChanges()
  }

  onSaveReply() {
    if (this.replyContent.trim()) {
      this.loading = true;
      this.commentsService.createComment({ content: this.replyContent, parentCommentId: this.comment.id }).pipe(
        catchError(error => {
          this.errorService.setErrorMessage('Failed to create reply');
          return throwError(() => error);
        }), finalize(() => { this.loading = false; })
      ).subscribe(() => {
        this.isReplying = false;
        this.replyContent = '';
        window.location.reload();
      });
    }
  }

  onCancelReply() {
    this.isReplying = false;
  }

  onDeleteClick() {
    this.deleteComment.emit(this.comment.id);
  }

  onDeleteComment(commentId: string) {
    this.deleteComment.emit(commentId);
  }

  onMinimizeClick() {
    this.isMinimized = !this.isMinimized;
  }
}
