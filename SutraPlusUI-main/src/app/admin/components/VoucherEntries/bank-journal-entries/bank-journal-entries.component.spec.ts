import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankJournalEntriesComponent } from './bank-journal-entries.component';

describe('BankJournalEntriesComponent', () => {
  let component: BankJournalEntriesComponent;
  let fixture: ComponentFixture<BankJournalEntriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BankJournalEntriesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BankJournalEntriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
