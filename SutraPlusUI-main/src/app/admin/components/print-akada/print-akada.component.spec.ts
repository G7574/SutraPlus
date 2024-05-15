import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintAkadaComponent } from './print-akada.component';

describe('PrintAkadaComponent', () => {
  let component: PrintAkadaComponent;
  let fixture: ComponentFixture<PrintAkadaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrintAkadaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrintAkadaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
