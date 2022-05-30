import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardSellerComponent } from './board-seller.component';

describe('BoardSellerComponent', () => {
  let component: BoardSellerComponent;
  let fixture: ComponentFixture<BoardSellerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoardSellerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardSellerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
