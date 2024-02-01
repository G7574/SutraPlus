import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonReportGeneratorComponent } from './common-report-generator.component';

describe('CommonReportGeneratorComponent', () => {
  let component: CommonReportGeneratorComponent;
  let fixture: ComponentFixture<CommonReportGeneratorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommonReportGeneratorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonReportGeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
