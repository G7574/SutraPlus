import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentListReportViewComponent } from './payment-list-report-view.component';

describe('PaymentListReportViewComponent', () => {
  let component: PaymentListReportViewComponent;
  let fixture: ComponentFixture<PaymentListReportViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PaymentListReportViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PaymentListReportViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
