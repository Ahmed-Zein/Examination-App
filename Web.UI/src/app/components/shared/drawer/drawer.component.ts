import {Component, EventEmitter, Input, Output} from '@angular/core';
import {DrawerAction} from '../../../app.component';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-drawer',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './drawer.component.html',
  styleUrl: './drawer.component.css',
})
export class DrawerComponent {
  @Input() drawerActions!: DrawerAction[];
  @Output() logout = new EventEmitter<void>();

  async logOut() {
    this.logout.emit();
  }
}

