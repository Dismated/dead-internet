import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() comment: any;
  @Input() depth: number = 0;

  deleteCommentChain(commentId: string) {
    this.commentService.deleteCommentChain(commentId).subscribe((res) => {
    })
  }
}
