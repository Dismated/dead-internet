
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
    private router: Router,
    private commentsService: CommentsService
  ) {
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      this.commentData = navigation.extras.state['commentData'];
      this.promptText = navigation.extras.state['promptText'];
    }
  }

  ngOnInit() {
    this.postId = this.route.snapshot.paramMap.get('id');
    if (this.commentData == null) {
      this.commentsService.getComments(this.postId).subscribe(
        (response) => {
          console.log(response)
          this.promptText = response[0].content
          this.commentData = response.splice(1)
        },
        (error) => console.error(error))
    }
  }
}

