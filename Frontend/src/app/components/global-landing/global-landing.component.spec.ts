import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalLandingComponent } from './global-landing.component';

describe('GlobalLandingComponent', () => {
  let component: GlobalLandingComponent;
  let fixture: ComponentFixture<GlobalLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GlobalLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GlobalLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
