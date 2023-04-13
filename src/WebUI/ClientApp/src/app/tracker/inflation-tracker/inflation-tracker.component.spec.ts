import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InflationTrackerComponent } from './inflation-tracker.component';

describe('InflationTrackerComponent', () => {
  let component: InflationTrackerComponent;
  let fixture: ComponentFixture<InflationTrackerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InflationTrackerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InflationTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
