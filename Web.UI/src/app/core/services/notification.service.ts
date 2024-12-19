import {Injectable} from '@angular/core';
import Configuration from '../config/configuration';
import {AuthService} from './auth.service';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';

@Injectable({providedIn: 'root'})
export class NotificationService {

  connection!: HubConnection;

  constructor(private configuration: Configuration, private authService: AuthService) {
    this.establishConnection().then(connection => {
      console.log("New Connection" + connection);
    })
  }

  get Connection(): HubConnection {
    return this.connection;
  }

  private async establishConnection() {
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
}
