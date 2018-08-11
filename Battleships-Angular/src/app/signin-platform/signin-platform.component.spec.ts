import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninPlatformComponent } from './signin-platform.component';

describe('SigninPlatformComponent', () => {
  let component: SigninPlatformComponent;
  let fixture: ComponentFixture<SigninPlatformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SigninPlatformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SigninPlatformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
