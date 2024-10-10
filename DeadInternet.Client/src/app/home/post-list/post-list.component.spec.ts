import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PostListComponent } from './post-list.component';
import { PostService } from '../../features/services/post.service';
import { ErrorService } from '../../core/error-handling/error.service';
import { of, throwError } from 'rxjs';

// Stub components
@Component({
  selector: 'app-post-item',
  template: ''
})
class PostItemComponentStub {
  @Input() post: any;
  @Input() index: number = 0
  @Output() deletePost = new EventEmitter<string>();
}

@Component({
  selector: 'app-llm-prompt',
  template: ''
})
class LlmPromptComponentStub { }

@Component({
  selector: 'app-button',
  template: ''
})
class ButtonComponentStub {
  @Input() type: string = ''
  @Input() disabled: boolean = false;
}



describe('PostListComponent', () => {
  let component: PostListComponent;
  let fixture: ComponentFixture<PostListComponent>;
  let postServiceSpy: jasmine.SpyObj<PostService>;

  beforeEach(async () => {
    const postServiceSpyObj = jasmine.createSpyObj('PostService', ['getPosts', 'deletePost']);
    const errorServiceSpyObj = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [
        PostListComponent,
        PostItemComponentStub,
        LlmPromptComponentStub,
        ButtonComponentStub
      ],
      providers: [
        { provide: PostService, useValue: postServiceSpyObj },
        { provide: ErrorService, useValue: errorServiceSpyObj }
      ]
    }).compileComponents();

    postServiceSpy = TestBed.inject(PostService) as jasmine.SpyObj<PostService>;
    errorServiceSpy = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch posts on init', fakeAsync(() => {
    const mockPosts = { data: [{ id: '1', title: 'Test Post' }] };
    postServiceSpy.getPosts.and.returnValue(of(mockPosts));

    fixture.detectChanges();
    tick();

    expect(postServiceSpy.getPosts).toHaveBeenCalled();
    expect(component.posts).toEqual(mockPosts.data);
  }));

  it('should handle error when fetching posts fails', fakeAsync(() => {
    postServiceSpy.getPosts.and.returnValue(throwError(() => new Error('Error')));

    postServiceSpy.getPosts().subscribe({
      next: () => fail('Expected error, but got a success response'),
      error: (err) => {
        expect(err.message).toBe('Error');
      }
    })
  }));

  it('should delete post', fakeAsync(() => {
    component.posts = [{ id: '1', title: 'Test Post' }];
    postServiceSpy.deletePost.and.returnValue(of(undefined));

    component.deletePost('1');
    tick();

    expect(postServiceSpy.deletePost).toHaveBeenCalledWith('1');
    expect(component.posts).toEqual([]);
  }));


  it('should handle error when deleting post fails', fakeAsync(() => {
    component.posts = [{ id: '1', title: 'Test Post' }];
    postServiceSpy.deletePost.and.returnValue(throwError(() => new Error('Delete Error')));

    postServiceSpy.deletePost('1').subscribe({
      next: () => fail('Expected error, but got a success response'),
      error: (err) => {
        expect(err.message).toBe('Delete Error');
      }
    });
  }));

  it('should check if user is authenticated', () => {
    spyOn(localStorage, 'getItem').and.returnValue('token');
    expect(component.isUserAuthenticated()).toBeTrue();

    (localStorage.getItem as jasmine.Spy).and.returnValue(null);
    expect(component.isUserAuthenticated()).toBeFalse();
  });

  it('should unsubscribe on destroy', () => {
    spyOn(component['subscriptions'], 'unsubscribe');
    component.ngOnDestroy();
    expect(component['subscriptions'].unsubscribe).toHaveBeenCalled();
  });
});
