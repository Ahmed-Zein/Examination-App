import {Injectable} from '@angular/core';
import Configuration from '../config/configuration';
import {AuthService} from './auth.service';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';

@Injectable({providedIn: 'root'})
export class NotificationService {

  connection!: HubConnection;

  constructor(private configuration: Configuration, private authService: AuthService) {
  }

  get Connection(): HubConnection {
    return this.connection;
  }

  public OnReceiveNotification(fc: (data: string, level: number) => void): void {
    this.connection.on("ReceiveNotification", fc)
  }

  public DestroyOnReceiveNotification(fc: (data: string, level: number) => void): void {
    this.connection.off("ReceiveNotification", fc)
  }

  public async establishConnection() {
    const jwtToken = this.authService.GetAuthToken();

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.configuration.BaseUrl}/notification`, {
        accessTokenFactory(): string | Promise<string> {
          return jwtToken;
        }
      }).build();
    this.connection.onclose(error => {
      console.error("signalr connection is closed");
      console.error(error);
    })
    await this.connection.start();
    return
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

//   this.connection.on("ReceiveNotification", (data: string, level: number) => {
//   this.messageService.add({
//                             severity: this._severity(level),
//   summary: 'Notification Received',
//   detail: data,
// });
// });
}
