import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {TimerDataService} from '../../core/services/timer.data.service';
import {DecimalPipe} from '@angular/common';
import {Subscription} from 'rxjs';
import {ProgressBar} from 'primeng/progressbar';

@Component({
  selector: 'app-timer',
  standalone: true,
  imports: [
    DecimalPipe,
    ProgressBar
  ],
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css'
})
export class TimerComponent implements OnInit, OnDestroy {
  @Input({required: true}) totalSeconds!: number;
  @Output() Completed = new EventEmitter<void>();
  totalBar!: number;
  hours: number = 0;
  minutes: number = 0;
  seconds: number = 0;

  private subscription?: Subscription;

  constructor(private timeService: TimerDataService) {
  }

  get barValue(): number {
    return ((this.totalSeconds / this.totalBar) * 100);
  }

  ngOnInit(): void {
    this.totalBar = this.totalSeconds;
    this.updateTimeUnits(this.totalSeconds);
    this.startTicking();
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  private startTicking(): void {
    this.subscription = this.timeService.Observable().subscribe({
      next: () => {
        if (this.totalSeconds > 0) {
          this.totalSeconds--;
          this.updateTimeUnits(this.totalSeconds);
        } else {
          this.Completed.emit();
          this.subscription?.unsubscribe();
        }
      }
    });
  }

  private updateTimeUnits(totalSeconds: number): void {
    const {hours, minutes, seconds} = this.convertSeconds(totalSeconds);
    this.hours = hours;
    this.minutes = minutes;
    this.seconds = seconds;
  }

  private convertSeconds(totalSeconds: number): { hours: number; minutes: number; seconds: number } {
    return {
      hours: Math.floor(totalSeconds / 3600),
      minutes: Math.floor((totalSeconds % 3600) / 60),
      seconds: totalSeconds % 60
    };
  }
}
