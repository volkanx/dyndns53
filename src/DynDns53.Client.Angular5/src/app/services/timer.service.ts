import { Injectable } from '@angular/core';
import { timer } from 'rxjs/observable/timer';
import { TimerObservable } from 'rxjs/observable/TimerObservable';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class TimerService {

  remainingSeconds: number;
  private timer: Observable<number>;
  private subscription: Subscription;

  constructor() {
  }

  start(startValue: number): void {
    this.remainingSeconds = startValue;
    this.timer = TimerObservable.create(0, 1000);
    this.subscription = this.timer.subscribe(t => {
      this.remainingSeconds = this.remainingSeconds - 1;
    });
  }

  stop(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
