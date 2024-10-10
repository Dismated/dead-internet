import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Post } from '../../models/post-interface';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.css'
})
export class PostItemComponent {
  @Input() post: Post = { id: '', title: '', comments: [] };
  @Input() index: number = 0;
  @Output() deletePost = new EventEmitter<string>();

  onDeletePost() {
    this.deletePost.emit(this.post.id);
  }
}
