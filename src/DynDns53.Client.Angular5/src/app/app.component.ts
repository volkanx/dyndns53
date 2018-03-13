import { Component } from '@angular/core';
import { SettingsService } from './services/settings.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DynDns53 | AWS Route53-Based Dynamic DNS Tool';

  constructor(private settingsService: SettingsService) {
    settingsService.initSettings();
  }

}
