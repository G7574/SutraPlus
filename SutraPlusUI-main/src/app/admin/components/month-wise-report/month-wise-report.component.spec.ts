import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthWiseReportComponent } from './month-wise-report.component';

describe('MonthWiseReportComponent', () => {
  let component: MonthWiseReportComponent;
  let fixture: ComponentFixture<MonthWiseReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthWiseReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthWiseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
