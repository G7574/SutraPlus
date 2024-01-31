import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemWiseReportViewComponent } from './item-wise-report-view.component';

describe('ItemWiseReportViewComponent', () => {
  let component: ItemWiseReportViewComponent;
  let fixture: ComponentFixture<ItemWiseReportViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemWiseReportViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemWiseReportViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
