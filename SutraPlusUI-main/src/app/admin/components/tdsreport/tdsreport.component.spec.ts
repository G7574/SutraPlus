import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TDSReportComponent } from './tdsreport.component';

describe('TDSReportComponent', () => {
  let component: TDSReportComponent;
  let fixture: ComponentFixture<TDSReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TDSReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TDSReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
