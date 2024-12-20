import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardItemCardComponent } from './dashboard-item-card.component';

describe('DashboardItemCardComponent', () => {
  let component: DashboardItemCardComponent;
  let fixture: ComponentFixture<DashboardItemCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardItemCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardItemCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
