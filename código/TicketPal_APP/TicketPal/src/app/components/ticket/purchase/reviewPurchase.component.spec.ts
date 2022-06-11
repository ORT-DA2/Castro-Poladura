import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewPurchaseComponent } from './reviewPurchase.component';

describe('PurchaseComponent', () => {
  let component: ReviewPurchaseComponent;
  let fixture: ComponentFixture<ReviewPurchaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReviewPurchaseComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewPurchaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
