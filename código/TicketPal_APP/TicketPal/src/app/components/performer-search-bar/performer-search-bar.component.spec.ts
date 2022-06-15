import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformerSearchBarComponent } from './performer-search-bar.component';

describe('PerformerSearchBarComponent', () => {
  let component: PerformerSearchBarComponent;
  let fixture: ComponentFixture<PerformerSearchBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerformerSearchBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformerSearchBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
