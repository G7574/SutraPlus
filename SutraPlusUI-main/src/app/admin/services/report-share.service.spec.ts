import { TestBed } from '@angular/core/testing';

import { ReportShareService } from './report-share.service';

describe('ReportShareService', () => {
  let service: ReportShareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReportShareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
