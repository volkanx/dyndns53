import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { Settings } from './settings';

@Injectable()
export class SettingsService {

  constructor(private localStorageService: LocalStorageService) { }

  saveSettings(settings: Settings): void {
    const json = JSON.stringify(settings);
    this.localStorageService.setData('settings', json);
  }

  getSettings(): Settings {
    const settingsJson: string = this.localStorageService.getData('settings');
    const settings: Settings = Object.assign(new Settings(), JSON.parse(settingsJson));
    return settings;
  }

  getNumberOfLogEntries(): number {
    return 10;
  }

}
