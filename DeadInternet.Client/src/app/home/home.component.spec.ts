import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { PostService } from '../features/services/post.service';
import { ErrorService } from '../core/error-handling/error.service';
import { of, throwError } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { Component, Directive, Input } from '@angular/core';

// Stub component for app-llm-prompt
@Component({
  selector: 'app-llm-prompt',
  template: ''
})
class LlmPromptStubComponent { }

// Stub directive for routerLink
@Directive({
  selector: '[routerLink]'
})
class RouterLinkStubDirective {
  @Input() routerLink: any;
}

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let postServiceSpy: jasmine.SpyObj<PostService>;
  let errorServiceSpy: jasmine.SpyObj<ErrorService>;

  beforeEach(async () => {
    const postServiceSpyObj = jasmine.createSpyObj('PostService', ['getPosts', 'deletePost']);
    const errorServiceSpyObj = jasmine.createSpyObj('ErrorService', ['setErrorMessage']);

    await TestBed.configureTestingModule({
      declarations: [
        HomeComponent,
        LlmPromptStubComponent,
        RouterLinkStubDirective
      ],
      imports: [RouterTestingModule],
      providers: [
        { provide: PostService, useValue: postServiceSpyObj },
        { provide: ErrorService, useValue: errorServiceSpyObj }
      ]
    }).compileComponents();

    postServiceSpy = TestBed.inject(PostService) as jasmine.SpyObj<PostService>;
    errorServiceSpy = TestBed.inject(ErrorService) as jasmine.SpyObj<ErrorService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });



  it('should load posts on init', fakeAsync(() => {
    const mockPosts = [{ id: '1', title: 'Test Post', comments: [] }];
    postServiceSpy.getPosts.and.returnValue(of(mockPosts));


    fixture.detectChanges();
    tick();

    expect(component.posts).toEqual(mockPosts);
    expect(postServiceSpy.getPosts).toHaveBeenCalled();
  }));

  it('should handle error when loading posts', fakeAsync(() => {
    const consoleErrorSpy = spyOn(console, 'error');

    postServiceSpy.getPosts.and.returnValue(throwError(() => new Error('Failed to load posts')));

    component.ngOnInit();
    fixture.detectChanges();
    tick();

    expect(errorServiceSpy.setErrorMessage).toHaveBeenCalledWith('Failed to load posts');
    expect(component.posts).toEqual([]);

    expect(consoleErrorSpy).toHaveBeenCalled();
  }));

  it('should delete post', fakeAsync(() => {
    component.posts = [{ id: '1', title: 'Test Post', comments: [] }];
    postServiceSpy.deletePost.and.returnValue(of(undefined));

    component.deletePost('1');
    tick();

    expect(component.posts).toEqual([]);
    expect(postServiceSpy.deletePost).toHaveBeenCalledWith('1');
  }));

  it('should handle error when deleting post', fakeAsync(() => {
    component.posts = [{ id: '1', title: 'Test Post', comments: [] }];
    postServiceSpy.deletePost.and.returnValue(throwError(() => new Error('Test error')));

    component.deletePost('1');
    tick();

    expect(errorServiceSpy.setErrorMessage).toHaveBeenCalledWith('Failed to delete post');
    expect(component.posts).toEqual([{ id: '1', title: 'Test Post', comments: [] }]);
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
})
