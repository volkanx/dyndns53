import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../settings.service';
import { LogService } from '../log.service';
import { Settings } from '../settings';
import { HostedDomainInfo } from '../settings';

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
    this.settings = this.settingsService.getSettings();
 }

 saveValues(): void {
    this.settingsService.saveSettings(this.settings);
    this.logService.addMessage('Saved configuration values to the local storage');
 }

 addDomain(zoneId, domainName): void {
    const settings = this.settingsService.getSettings();
    const hostedDomainInfo = new HostedDomainInfo(zoneId, domainName);
    settings.domainList.push(hostedDomainInfo);
    this.settingsService.saveSettings(this.settings);
    this.logService.addMessage('New domain has been added. Restarting DNS update.');
 }

 deleteDomain(): void {

 }

 domainsUpdated(): void {

 }

}
