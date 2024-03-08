import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AkadaEntryOptionsDialogComponent } from './akada-entry-options-dialog.component';

describe('AkadaEntryOptionsDialogComponent', () => {
  let component: AkadaEntryOptionsDialogComponent;
  let fixture: ComponentFixture<AkadaEntryOptionsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AkadaEntryOptionsDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AkadaEntryOptionsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
