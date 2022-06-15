import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConcertSearchBarComponent } from './concert-search-bar.component';

describe('SearchBarComponent', () => {
  let component: ConcertSearchBarComponent;
  let fixture: ComponentFixture<ConcertSearchBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConcertSearchBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConcertSearchBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
