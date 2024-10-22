import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdjustableTextareaComponent } from './adjustable-textarea.component';

describe('AdjustableTextareaComponent', () => {
  let component: AdjustableTextareaComponent;
  let fixture: ComponentFixture<AdjustableTextareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdjustableTextareaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdjustableTextareaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
