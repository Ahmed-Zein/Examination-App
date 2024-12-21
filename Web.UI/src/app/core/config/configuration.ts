import {Injectable} from '@angular/core';

type Environment = 'dev' | 'docker';

@Injectable({providedIn: 'root'})
export default class Configuration {
//   environment: Environment = 'dev';
  environment: Environment = 'docker';
  source: Record<Environment, AppSettingsBase> = {
    dev: new Development(),
    docker: new Docker(),
  };

  get Url(): string {
    const src = this.source[this.environment];
    return `${this.BaseUrl}/api`;
  }

  get SignalrUrl() {
    const src = this.source[this.environment];
    return `${this.BaseUrl}/notification`;
  }

  get BaseUrl(): string {
    return `${this.source[this.environment].baseUrl}:${this.source[this.environment].port}`;
  }

  get CurrentConfig() {
    return this.source[this.environment];
  }
}

interface AppSettingsBase {
  baseUrl: string;
  port: string;

}

class Development implements AppSettingsBase {
  baseUrl = 'http://localhost'
  port = '5270';
}

class Docker implements AppSettingsBase {
  baseUrl = 'http://localhost';
  port = '8080';
}

export class AppRoles {
  static Admin = 'Admin';
  static Student = 'Student';
}
