import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() comment: any;
  @Input() depth: number = 0;
  @Output() deleteComment = new EventEmitter<string>();

  onDeleteClick() {
    this.deleteComment.emit(this.comment.id);
  }

  onDeleteComment(commentId: string) {
    this.deleteComment.emit(commentId);
  }
}
