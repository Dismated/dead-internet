import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from '../core/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  posts: any[] | undefined;

  constructor(private router: Router, private dataService: DataService) {
  }



  ngOnInit(): void {
    this.dataService.getPosts().subscribe(
      (response) => {
        this.posts = response

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

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
