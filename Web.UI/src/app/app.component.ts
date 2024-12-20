import {Component, OnDestroy, OnInit} from '@angular/core';
import {ReactiveFormsModule} from '@angular/forms';
import {AuthService} from './core/services/auth.service';
import {MessageService} from 'primeng/api';
import {ToastModule} from 'primeng/toast';
import {CommonModule} from '@angular/common';
import {RouterOutlet} from '@angular/router';
import {DrawerComponent} from './components/shared/drawer/drawer.component';
import {NotificationService} from './core/services/notification.service';
import {Utils} from './core/utils/utils';

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
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'ExaminationSystem.UI';

  constructor(
    private authService: AuthService,
    private notificationService: NotificationService,
    private messageService: MessageService
  ) {
  }


  get notificationAction() {
    return (data: string, level: number) => {
      this.messageService.add({
        severity: Utils.severity(level),
        summary: 'Notification Received',
        detail: data,
      });
    };
  }

  ngOnDestroy(): void {
    this.notificationService.DestroyOnReceiveNotification(this.notificationAction)
  }

  ngOnInit() {
    this.notificationService.establishConnection()
      .then(r => console.log("Notification started"));

    this.notificationService.OnReceiveNotification(this.notificationAction)
  }

  async Logout() {
    const _ = await this.authService.Logout();
  }
}
