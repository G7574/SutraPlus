import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportsMainComponent } from './reports-main.component';

describe('ReportsMainComponent', () => {
  let component: ReportsMainComponent;
  let fixture: ComponentFixture<ReportsMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportsMainComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportsMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
