import {interval, Subject} from 'rxjs';
import {Injectable} from '@angular/core';

@Injectable({providedIn: 'root'})
export class TimerDataService {
  timerSubject: Subject<number> = new Subject<number>();
  tick;

  constructor() {
    this.tick = interval(1000).subscribe(x => this.timerSubject.next(x));
  }

  public Observable() {
    return this.timerSubject.asObservable();
  }
}
