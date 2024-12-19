import {Component, OnDestroy, OnInit} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {AppRoles} from './core/config/configuration';
import {AuthService} from './core/services/auth.service';
import {MessageService} from 'primeng/api';
import {ToastModule} from 'primeng/toast';
import {CommonModule} from '@angular/common';
import {RouterOutlet} from '@angular/router';
import {DrawerComponent} from './components/shared/drawer/drawer.component';
import {DrawerConfiguration} from './core/config/DrawerConfiguration';
import {NotificationService} from './core/services/notification.service';
import {HubConnection} from '@microsoft/signalr';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    ToastModule,
    ReactiveFormsModule,
    CommonModule,
    RouterOutlet,
    DrawerComponent
  ],
  providers: [MessageService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'ExaminationSystem.UI';
  connection!: HubConnection;

  constructor(
    private authService: AuthService,
    private drawerConfiguration: DrawerConfiguration,
    private notificationService: NotificationService,
    private messageService: MessageService
  ) {
  }

  get GetActions(): DrawerAction[] {
    switch (this.authService.UserRole()) {
      case AppRoles.Admin:
        return this.drawerConfiguration.Admin;
      case AppRoles.Student:
        return this.drawerConfiguration.Student;
      default:
        return [];
    }
  }

  ngOnDestroy(): void {
    console.log('ngOnDestroy is Called');
  }

  ngOnInit() {
    this.startNotification()
      .then(r => console.log("Notification started"));
  }

  async startNotification() {
    this.connection = this.notificationService.Connection;

    this.connection.on("ReceiveNotification", (data: string, level: number) => {
      this.messageService.add({
        severity: this._severity(level),
        summary: 'Notification Received',
        detail: data,
      });
    });
  }

  async Logout() {
    const _ = await this.authService.Logout();
  }

  private _severity(number: number) {
    switch (number) {
      case 0:
        return 'info';
      case 1:
        return 'success';
      case 2:
        return 'warn';
      default:
        return 'error';
    }
  }
}

export interface DrawerAction {
  title: string;
  icon: string;
  path: string;
}

