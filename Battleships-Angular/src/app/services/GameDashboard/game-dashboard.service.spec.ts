import { TestBed, inject } from '@angular/core/testing';

import { GameDashboardService } from './game-dashboard.service';

describe('GameDashboardService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GameDashboardService]
    });
  });

  it('should be created', inject([GameDashboardService], (service: GameDashboardService) => {
    expect(service).toBeTruthy();
  }));
});
