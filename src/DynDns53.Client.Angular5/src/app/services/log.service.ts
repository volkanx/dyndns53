import { Injectable } from '@angular/core';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';

@Injectable()
export class LogService {

  messages: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor() {
  }

  addMessage(message: string) {
    const logLine = new Date().toTimeString() + '\t' + message;
    this.messages.next(logLine);
  }

  getMessages(): BehaviorSubject<string> {
    return this.messages;
  }
}
