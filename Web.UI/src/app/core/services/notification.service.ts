import {Injectable} from '@angular/core';
import Configuration from '../config/configuration';
import {AuthService} from './auth.service';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {Subject} from 'rxjs';


type Action = (data: string, level: number) => void;

class ClientHubAPI {
  static receiveNotification = "ReceiveNotification"
}

@Injectable({providedIn: 'root'})
export class NotificationService {
  hasNewNotification = new Subject<void>();
  connection!: HubConnection;

  constructor(private configuration: Configuration, private authService: AuthService) {
  }

  public OnReceiveNotification(fc: Action): void {
    this.connection.off(ClientHubAPI.receiveNotification, fc)
    this.connection.on(ClientHubAPI.receiveNotification, fc)
  }

  public DestroyOnReceiveNotification(fc: Action): void {
    this.connection.off(ClientHubAPI.receiveNotification, fc)
  }

  public async establishConnection() {
    const jwtToken = this.authService.GetAuthToken();

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.configuration.SignalrUrl, {
        accessTokenFactory(): string | Promise<string> {
          return jwtToken;
        }
      }).build();

    this.connection.onclose(error => {
      console.error("signalr connection is closed");
      console.error(error);
    })

    this.OnReceiveNotification(() => {
      this.hasNewNotification.next();
    });
    await this.connection.start();
    return
  }

}
