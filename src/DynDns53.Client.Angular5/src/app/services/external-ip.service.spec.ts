import { TestBed, inject } from '@angular/core/testing';

import { ExternalIpService } from './external-ip.service';

describe('ExternalIpService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExternalIpService]
    });
  });

  it('should be created', inject([ExternalIpService], (service: ExternalIpService) => {
    expect(service).toBeTruthy();
  }));
});
