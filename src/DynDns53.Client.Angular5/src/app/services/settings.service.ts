import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { Settings, HostedDomainInfo } from '../models/settings';

@Injectable()
export class SettingsService {

  constructor(private localStorageService: LocalStorageService) { }

  initSettings(): void {
    let settings = this.getSettings();
    if (!settings) {
      settings = new Settings();
      settings.updateInterval = 5;
      settings.accessKey = '';
      settings.secretKey = '';
      settings.showUsage = true;
      settings.numberOfLogEntries = 50;
      settings.domainList = new Array<HostedDomainInfo>();
      this.saveSettings(settings);
    }
  }

  saveSettings(settings: Settings): void {
    const json = JSON.stringify(settings);
    this.localStorageService.setData('settings', json);
  }

  getSettings(): Settings | null {
    const settingsJson: string | null = this.localStorageService.getData('settings');
    let settings: Settings | null = null;
    if (settingsJson) {
      settings = Object.assign(new Settings(), JSON.parse(settingsJson));
    }
    return settings;
  }
}
