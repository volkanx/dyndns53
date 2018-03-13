import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { LogService } from '../services/log.service';
import { Settings } from '../models/settings';
import { HostedDomainInfo } from '../models/settings';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})

export class SettingsComponent implements OnInit {

  settings: Settings;

  constructor(private settingsService: SettingsService,
    private logService: LogService) { }

  ngOnInit() {
    this.loadValues();
  }

  loadValues(): void {
    this.logService.addMessage('Loading configuration values from the local storage');
    const tempSettings = this.settingsService.getSettings();
    if (tempSettings) {
      this.settings = tempSettings;
    }
  }

  saveValues(): void {
    if (this.settings) {
      this.settingsService.saveSettings(this.settings);
      this.logService.addMessage('Saved configuration values to the local storage');
    }
  }

  addDomain(domainName: string, zoneId: string): void {
      const hostedDomainInfo = new HostedDomainInfo(zoneId, domainName);
      this.settings.domainList.push(hostedDomainInfo);
  }

  deleteDomain(index: number): void {
    this.settings.domainList.splice(index, 1);
  }
}
