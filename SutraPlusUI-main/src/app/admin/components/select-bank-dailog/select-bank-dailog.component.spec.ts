import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectBankDailogComponent } from './select-bank-dailog.component';

describe('SelectBankDailogComponent', () => {
  let component: SelectBankDailogComponent;
  let fixture: ComponentFixture<SelectBankDailogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectBankDailogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectBankDailogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
