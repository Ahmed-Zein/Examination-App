import {Component, EventEmitter, OnDestroy, Output} from '@angular/core';
import {RouterLink} from '@angular/router';
import {DrawerService} from './drawer.service';

@Component({
  selector: 'app-drawer',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './drawer.component.html',
  styleUrl: './drawer.component.css',
})
export class DrawerComponent implements OnDestroy {
  @Output() logout = new EventEmitter<void>();
  selectedTab = 0;

  constructor(private drawerService: DrawerService) {
  }

  get actions() {
    return this.drawerService.GetActions;
  }

  ngOnDestroy(): void {
    this.logout.complete();
  }

  setSelectedTab(tab: number) {
    this.selectedTab = tab;
  }

  async logOut() {
    this.logout.emit();
  }
}

