import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { By } from '@angular/platform-browser';
import { PostItemComponent } from './post-item.component';
import { TextButtonComponent } from '../../shared/components/text-button/text-button.component';

describe('PostItemComponent', () => {
  let component: PostItemComponent;
  let fixture: ComponentFixture<PostItemComponent>;
  let debugEl: any;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [PostItemComponent, TextButtonComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostItemComponent);
    component = fixture.componentInstance;
    component.post = { id: '1', title: 'Test Post', comments: [{}, {}, {}] };
    component.index = 0;
    fixture.detectChanges();

    debugEl = fixture.debugElement;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the correct post index', () => {
    const indexElement = debugEl.query(By.css('p.mb-0')).nativeElement;
    expect(indexElement.textContent).toBe('1');
  });

  it('should display the correct post title', () => {
    const titleElement = debugEl.query(By.css('strong.mb-0')).nativeElement;
    expect(titleElement.textContent).toBe('Test Post');
  });

  it('should display the correct number of comments', () => {
    const commentsElement = debugEl.query(By.css('app-text-button')).nativeElement;
    expect(commentsElement.textContent).toContain('2 comments');
  });

  it('should have the correct routerLink for comments', () => {
    const routerLinkElements = debugEl.queryAll(By.css('[ng-reflect-router-link]'));
    expect(routerLinkElements.length).toBeGreaterThan(0);

    if (routerLinkElements.length > 0) {
      const routerLink = routerLinkElements[0].attributes['ng-reflect-router-link'];
      expect(routerLink).toBe('/comments,1');
    }
  });

  it('should emit deletePost event when delete button is clicked', () => {
    spyOn(component.deletePost, 'emit');

    const deleteButtons = debugEl.queryAll(By.css('app-text-button'));
    expect(deleteButtons.length).toBeGreaterThan(1);

    const deleteButton = deleteButtons[1];
    expect(deleteButton).toBeTruthy();

    // Trigger the clickEvent on the TextButtonComponent
    const textButtonComponent = deleteButton.componentInstance;
    textButtonComponent.clickEvent.emit();

    fixture.detectChanges();

    expect(component.deletePost.emit).toHaveBeenCalledWith('1');
  });

  it('should apply correct styles to elements', () => {
    const flexContainer = debugEl.query(By.css('.d-flex')).nativeElement;
    expect(flexContainer.classList.contains('d-flex')).toBeTruthy();

    const thumbnailContainer = debugEl.query(By.css('div[style*="width: 80px"]')).nativeElement;
    expect(thumbnailContainer.style.backgroundColor).toBe('var(--bs-secondary-bg)');
    expect(thumbnailContainer.style.borderRadius).toBe('var(--bs-border-radius)');
  });
});
