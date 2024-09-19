import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommentsService } from '../../services/comments.service';

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

  constructor(private commentsService: CommentsService) { }

  onEditClick() {
    this.isEditing = true;
    this.editContent = this.comment.content;
  }

  onSaveEdit() {
    if (this.editContent.trim()) {
      this.commentsService.editComment(this.comment.id, this.editContent).subscribe(() => {
        this.comment.content = this.editContent;
        this.isEditing = false;
      }, error => {
        console.error('Error updating comment:', error);
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
      this.commentsService.createComment({ content: this.replyContent, parentCommentId: this.comment.id }).subscribe(() => {
        console.log('replyyea')
        this.isReplying = false;
      }, error => { console.error('Error creating comment:', error); });
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
