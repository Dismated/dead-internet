
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit {
  postId: string | null = null
  commentData: any;
  promptText = ""

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      this.commentData = navigation.extras.state['commentData'];
      this.promptText = navigation.extras.state['promptText'];
    }
  }

  ngOnInit() {
    this.postId = this.route.snapshot.paramMap.get('id');

  }
}
