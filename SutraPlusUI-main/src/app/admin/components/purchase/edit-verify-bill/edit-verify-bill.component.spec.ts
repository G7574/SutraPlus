import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditVerifyBillComponent } from './edit-verify-bill.component';

describe('EditVerifyBillComponent', () => {
  let component: EditVerifyBillComponent;
  let fixture: ComponentFixture<EditVerifyBillComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditVerifyBillComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditVerifyBillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
