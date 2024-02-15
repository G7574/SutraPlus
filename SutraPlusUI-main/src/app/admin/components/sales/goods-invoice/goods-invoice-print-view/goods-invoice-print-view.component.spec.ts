import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GoodsInvoicePrintViewComponent } from './goods-invoice-print-view.component';

describe('GoodsInvoicePrintViewComponent', () => {
  let component: GoodsInvoicePrintViewComponent;
  let fixture: ComponentFixture<GoodsInvoicePrintViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GoodsInvoicePrintViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GoodsInvoicePrintViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
