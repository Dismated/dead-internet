import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentComponent } from './comment.component';
import { ErrorService } from '../../core/error-handling/error.service';
import { CommentsService } from '../../features/services/comments.service';
import { of } from 'rxjs';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: "app-error-message",
  template: ""
})
class ErrorMessageComponentStub { }

@Component({
  selector: "app-loading",
  template: ""
})
class LoadingComponentStub {
  @Input() isLoading: boolean = false;
}

@Component({
  selector: "app-text-button",
  template: ""
})
class TextButtonComponentStub {
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: any } = {};
  @Input() disabled: boolean = false;

  @Output() clickEvent = new EventEmitter<Event>();
}

@Component({
  selector: "app-button",
  template: ""
})
class ButtonComponentStub {
  @Input() submitType: boolean = false;
  @Input() btnClass: string = '';
  @Input() btnStyles: { [key: string]: string } = {};
  @Input() disabled: boolean = false;

  @Output() clickEvent = new EventEmitter<Event>();
}

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
      imports: [HttpClientTestingModule, FormsModule],
      declarations: [
        CommentComponent,
        ErrorMessageComponentStub,
        LoadingComponentStub,
        TextButtonComponentStub,
        ButtonComponentStub
      ],
      providers: [
        { provide: CommentsService, useValue: commentsServiceMock },
        { provide: ErrorService, useValue: errorServiceMock },
      ]
    })
      .compileComponents();

  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentComponent);
    component = fixture.componentInstance;
    component.comment = { id: '123', content: 'Test comment' };
    fixture.detectChanges();
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



  it('should toggle isMinimized on onMinimizeClick()', () => {
    expect(component.isMinimized).toBeFalse();
    component.onMinimizeClick();
    expect(component.isMinimized).toBeTrue();
  });
});
