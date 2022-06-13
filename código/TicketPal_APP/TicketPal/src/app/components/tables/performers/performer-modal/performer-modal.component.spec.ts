import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformerModalComponent } from './performer-modal.component';

describe('PerformereModalComponent', () => {
  let component: PerformerModalComponent;
  let fixture: ComponentFixture<PerformerModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformerModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformerModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
