import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentComponent } from './comment.component';
import { ErrorService } from '../../core/error-handling/error.service';
import { CommentsService } from '../../features/services/comments.service';
import { of, throwError } from 'rxjs';

describe('CommentComponent', () => {
  let component: CommentComponent;
  let fixture: ComponentFixture<CommentComponent>;
  let commentsServiceMock: any;
  let errorServiceMock: any;

  beforeEach(async () => {
    commentsServiceMock = {
      editComment: jasmine.createSpy('editComment').and.returnValue(of(null)),
      createComment: jasmine.createSpy('createComment').and.returnValue(of(null)),
    };

    errorServiceMock = {
      setErrorMessage: jasmine.createSpy('setErrorMessage'),
    };

    await TestBed.configureTestingModule({
      declarations: [CommentComponent],
      providers: [
        { provide: CommentsService, useValue: commentsServiceMock },
        { provide: ErrorService, useValue: errorServiceMock }
      ]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentComponent);
    component = fixture.componentInstance;
    component.comment = { id: '123', content: 'Test comment' };
    fixture.detectChanges();
    spyOn(window.location, 'reload');
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have the correct initial comment and depth', () => {
    expect(component.comment.id).toBe('123');
    expect(component.depth).toBe(0);
  });

  it('should enable editing mode and set editContent on onEditClick()', () => {
    component.onEditClick();
    expect(component.isEditing).toBeTrue();
    expect(component.editContent).toBe('Test comment');
  });

  it('should call commentsService.editComment and save content on onSaveEdit()', () => {
    component.editContent = 'Updated content';
    component.onSaveEdit();

    expect(component.loading).toBeTrue();
    expect(commentsServiceMock.editComment).toHaveBeenCalledWith('123', 'Updated content');
    expect(component.isEditing).toBeFalse();
  });

  it('should handle error on save edit and call errorService', () => {
    commentsServiceMock.editComment.and.returnValue(throwError(() => new Error('error')));

    component.editContent = 'Updated content';
    component.onSaveEdit();

    expect(errorServiceMock.setErrorMessage).toHaveBeenCalledWith('Failed to update comment');
    expect(component.loading).toBeFalse();
  });

  it('should cancel editing mode on onCancelEdit()', () => {
    component.isEditing = true;
    component.onCancelEdit();
    expect(component.isEditing).toBeFalse();
  });

  it('should enable replying mode on onReplyClick()', () => {
    component.onReplyClick();
    expect(component.isReplying).toBeTrue();
  });

  it('should call commentsService.createComment on onSaveReply()', () => {
    component.replyContent = 'Reply content';
    component.onSaveReply();

    expect(component.loading).toBeTrue();
    expect(commentsServiceMock.createComment).toHaveBeenCalledWith({
      content: 'Reply content',
      parentCommentId: '123',
    });
    expect(component.isReplying).toBeFalse();
    expect(component.replyContent).toBe('');
  });

  it('should emit deleteComment event on onDeleteClick()', () => {
    spyOn(component.deleteComment, 'emit');
    component.onDeleteClick();
    expect(component.deleteComment.emit).toHaveBeenCalledWith('123');
  });

  it('should toggle isMinimized on onMinimizeClick()', () => {
    expect(component.isMinimized).toBeFalse();
    component.onMinimizeClick();
    expect(component.isMinimized).toBeTrue();
  });
});
