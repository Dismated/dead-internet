
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
          this.promptText = response.prompt.content
          this.commentData = response.replies
        },
        (error) => console.error(error))
    }
  }
}

