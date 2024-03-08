import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyBillsComponent } from './verify-bills.component';

describe('VerifyBillsComponent', () => {
  let component: VerifyBillsComponent;
  let fixture: ComponentFixture<VerifyBillsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifyBillsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyBillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
