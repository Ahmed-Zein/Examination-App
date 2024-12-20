import {ErrorHandler, Injectable, NgZone} from '@angular/core';
import {MessageService} from 'primeng/api';

@Injectable({providedIn: 'root'})
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private messageService: MessageService, private zone: NgZone) {
  }

  handleError(error: any): void {
    this.zone.run(() => {
      console.error('Global Error:', error);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'An unexpected error occurred. ' + error.message,
      });
    });
  }
}

