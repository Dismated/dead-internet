import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LlmPromptComponent } from './LlmPromptComponent';

describe('LlmPromptComponent', () => {
  let component: LlmPromptComponent;
  let fixture: ComponentFixture<LlmPromptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LlmPromptComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(LlmPromptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
