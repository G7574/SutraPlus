import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartyWiseCommHamaliComponent } from './party-wise-comm-hamali.component';

describe('PartyWiseCommHamaliComponent', () => {
  let component: PartyWiseCommHamaliComponent;
  let fixture: ComponentFixture<PartyWiseCommHamaliComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PartyWiseCommHamaliComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PartyWiseCommHamaliComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
