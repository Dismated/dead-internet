import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PostService } from './post.service';

describe('PostService', () => {
  let service: PostService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PostService]
    });
    service = TestBed.inject(PostService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a post', () => {
    const dummyPrompt = { content: 'Test prompt' };
    const dummyResponse = { id: '1', content: 'Test prompt' };

    service.createPost(dummyPrompt).subscribe(response => {
      expect(response).toEqual(dummyResponse);
    });

    const req = httpMock.expectOne('https://localhost:7201/api/home/prompt');
    expect(req.request.method).toBe('POST');
    req.flush(dummyResponse);
  });

  it('should get posts', () => {
    const dummyPosts = [
      { id: '1', content: 'Post 1' },
      { id: '2', content: 'Post 2' }
    ];

    service.getPosts().subscribe(posts => {
      expect(posts).toEqual(dummyPosts);
    });

    const req = httpMock.expectOne('https://localhost:7201/api/home');
    expect(req.request.method).toBe('GET');
    req.flush(dummyPosts);
  });

  it('should delete a post', () => {
    const dummyId = '1';

    service.deletePost(dummyId).subscribe(response => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne('https://localhost:7201/api/home/1');
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
