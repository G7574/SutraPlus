import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintPartyReportComponent } from './print-party-report.component';

describe('PrintPartyReportComponent', () => {
  let component: PrintPartyReportComponent;
  let fixture: ComponentFixture<PrintPartyReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrintPartyReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrintPartyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
