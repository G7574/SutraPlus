import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartyWiseCaseReportComponent } from './party-wise-case-report.component';

describe('PartyWiseCaseReportComponent', () => {
  let component: PartyWiseCaseReportComponent;
  let fixture: ComponentFixture<PartyWiseCaseReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PartyWiseCaseReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PartyWiseCaseReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
