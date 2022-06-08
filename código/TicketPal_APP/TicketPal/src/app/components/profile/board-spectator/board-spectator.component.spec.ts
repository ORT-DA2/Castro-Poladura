import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardSpectatorComponent } from './board-spectator.component';

describe('BoardSpectatorComponent', () => {
  let component: BoardSpectatorComponent;
  let fixture: ComponentFixture<BoardSpectatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoardSpectatorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardSpectatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
