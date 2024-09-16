import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PostService } from '../services/post.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  posts: any[] | undefined;

  constructor(private router: Router, private postService: PostService) {
  }



  ngOnInit(): void {
    this.postService.getPosts().subscribe(
      (response) => {
        this.posts = response
        console.log(this.posts)

      },
      (error) => console.error(error)
    );
  }
  isUserAuthenticated() {
    const token: string | null = localStorage.getItem('token');

    if (token) {
      return true;
    }
    else {
      return false;
    }
  }

  deletePost(id: string) {
    this.postService.deletePost(id).subscribe(
      (response) => {
        this.posts = this.posts?.filter((post) => post.id !== id)
      },
      (error) => console.error(error))
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
