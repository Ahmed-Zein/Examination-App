import {Injectable} from '@angular/core';
import Configuration from '../config/configuration';
import {AuthService} from './auth.service';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';


class ClientHubAPI {
  static receiveNotification = "ReceiveNotification"
}

@Injectable({providedIn: 'root'})
export class NotificationService {
  connection!: HubConnection;

  constructor(private configuration: Configuration, private authService: AuthService) {
  }

  public OnReceiveNotification(fc: (data: string, level: number) => void): void {
    this.connection.on(ClientHubAPI.receiveNotification, fc)
  }

  public DestroyOnReceiveNotification(fc: (data: string, level: number) => void): void {
    console.info("CALLED: DestroyOnReceiveNotification")
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
    await this.connection.start();
    return
  }
}
