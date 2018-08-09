import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameSurrenderComponent } from './game-surrender.component';

describe('GameSurrenderComponent', () => {
  let component: GameSurrenderComponent;
  let fixture: ComponentFixture<GameSurrenderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameSurrenderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameSurrenderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
