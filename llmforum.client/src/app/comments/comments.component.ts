
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentsService } from "../services/comments.service";


@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit {
  postId: string | null = null
  commentData: any = null
  promptText = ""

  constructor(
    private route: ActivatedRoute,
    private commentsService: CommentsService
  ) { }

  ngOnInit() {
    this.postId = this.route.snapshot.paramMap.get('id');
    if (this.commentData == null) {
      this.commentsService.getComments(this.postId).subscribe(
        (response) => {
          console.log(response, 'lol')

          this.promptText = response.prompt.content
          this.commentData = response.replies
        },
        (error) => console.error(error))
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

  private removeCommentRecursively(comments: any, commentId: string): boolean {
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

