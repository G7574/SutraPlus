import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillExpenseRateDailogComponent } from './bill-expense-rate-dailog.component';

describe('BillExpenseRateDailogComponent', () => {
  let component: BillExpenseRateDailogComponent;
  let fixture: ComponentFixture<BillExpenseRateDailogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BillExpenseRateDailogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BillExpenseRateDailogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
