import { Component, OnInit } from '@angular/core';
import { LogService } from '../log.service';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
  styleUrls: ['./event-log.component.css']
})
export class EventLogComponent implements OnInit {

  logs: string[] = [];
  logString: string;
  numberOfLogEntries: number;

  constructor(private logService: LogService, private settingsService: SettingsService) { }

  ngOnInit() {
    this.numberOfLogEntries = this.settingsService.getNumberOfLogEntries();

    this.logService.getMessages().subscribe(message => {
      this.logs.unshift(message);
      this.logs = this.logs.slice(0, this.numberOfLogEntries);
      this.logString = this.logs.join('\n');
    });
  }

  clearLog(): void {
    this.logs = [];
    this.logString = '';
  }

  sendMessage(message): void {
    this.logService.addMessage(message);
  }
}
