import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentsService } from '../../features/services/comments.service';
import { catchError, throwError } from 'rxjs';
import { ErrorService } from '../../core/services/error.service';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() comment: any;
  @Input() depth: number = 0;
  @Output() deleteComment = new EventEmitter<string>();

  isEditing: boolean = false;
  editContent: string = '';
  isReplying: boolean = false;
  replyContent: string = '';
  isMinimized: boolean = false;

  constructor(private commentsService: CommentsService, private errorService: ErrorService) { }

  onEditClick() {
    this.isEditing = true;
    this.editContent = this.comment.content;
  }

  onSaveEdit() {
    if (this.editContent.trim()) {
      this.commentsService.editComment(this.comment.id, this.editContent).pipe(
        catchError(error => {
          this.errorService.setErrorMessage('Failed to update comment');
          return throwError(() => error);
        })
      ).subscribe(() => {
        this.comment.content = this.editContent;
        this.isEditing = false;
      });
    }
  }

  onCancelEdit() {
    this.isEditing = false;
  }

  onReplyClick() {
    this.isReplying = true;
  }

  onSaveReply() {
    if (this.replyContent.trim()) {
      this.commentsService.createComment({ content: this.replyContent, parentCommentId: this.comment.id }).pipe(
        catchError(error => {
          this.errorService.setErrorMessage('Failed to create reply');
          return throwError(() => error);
        })
      ).subscribe(() => {
        this.isReplying = false;
        this.replyContent = ''; // Clear the reply content
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
