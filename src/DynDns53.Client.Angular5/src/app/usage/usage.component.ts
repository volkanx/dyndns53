import { Component, OnInit } from '@angular/core';
import { LogService } from '../log.service';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'app-usage',
  templateUrl: './usage.component.html',
  styleUrls: ['./usage.component.css']
})

export class UsageComponent implements OnInit {

  toggleUsageButtonText = 'Hide usage';
  showUsage = true;

  constructor(private settingsService: SettingsService,
    private logService: LogService) { }

  ngOnInit() {
    const settings = this.settingsService.getSettings();
    if (settings) {
      this.showUsage = settings.showUsage;
      this.toggleUsageButtonText = (this.showUsage) ? 'Hide usage' : 'Show usage';
      this.logService.addMessage('Updated usage info status');
    }
  }

  toggleUsage(): void {
    this.showUsage = !this.showUsage;
    this.toggleUsageButtonText = (this.showUsage) ? 'Hide usage' : 'Show usage';

    const settings = this.settingsService.getSettings();
    settings.showUsage = this.showUsage;
    this.settingsService.saveSettings(settings);
  }

}
