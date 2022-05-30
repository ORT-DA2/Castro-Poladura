import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardSupervisorComponent } from './board-supervisor.component';

describe('BoardSupervisorComponent', () => {
  let component: BoardSupervisorComponent;
  let fixture: ComponentFixture<BoardSupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoardSupervisorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardSupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
