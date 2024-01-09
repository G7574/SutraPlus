import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportEInvoiceComponent } from './import-e-invoice.component';

describe('ImportEInvoiceComponent', () => {
  let component: ImportEInvoiceComponent;
  let fixture: ComponentFixture<ImportEInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImportEInvoiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ImportEInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
