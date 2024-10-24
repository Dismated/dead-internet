import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CommentsService } from './comments.service';

describe('CommentsService', () => {
  let service: CommentsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CommentsService]
    });
    service = TestBed.inject(CommentsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getComments', () => {
    it('should retrieve comments for a post', () => {
      const mockId = '123';
      const mockComments = [{ id: 1, content: 'Test comment' }];

      service.getComments(mockId).subscribe(comments => {
        expect(comments).toEqual(mockComments);
      });

      const req = httpMock.expectOne(`https://localhost:7201/api/comments/post/${mockId}`);
      expect(req.request.method).toBe('GET');
      req.flush(mockComments);
    });
  });

  describe('deleteCommentChain', () => {
    it('should delete a comment chain', () => {
      const mockId = '456';

      service.deleteCommentChain(mockId).subscribe();

      const req = httpMock.expectOne(`https://localhost:7201/api/comments/${mockId}`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });

  describe('editComment', () => {
    it('should edit a comment', () => {
      const mockId = '789';
      const mockContent = 'Updated content';

      service.editComment(mockId, mockContent).subscribe();

      const req = httpMock.expectOne(`https://localhost:7201/api/comments/${mockId}`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual({ content: mockContent });
      req.flush(null);
    });
  });

  describe('createComment', () => {
    it('should create a new comment', () => {
      const mockPayload = { postId: '123', content: 'New comment' };
      const mockResponse = { id: '1', ...mockPayload };

      service.createComment(mockPayload).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`https://localhost:7201/api/comments/reply`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockPayload);
      req.flush(mockResponse);
    });
  });
});
