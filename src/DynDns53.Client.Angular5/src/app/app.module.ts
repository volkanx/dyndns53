import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { UsageComponent } from './usage/usage.component';
import { SettingsComponent } from './settings/settings.component';
import { LocalStorageService } from './services/local-storage.service';
import { LogService } from './services/log.service';
import { EventLogComponent } from './event-log/event-log.component';
import { SettingsService } from './services/settings.service';
import { UpdateComponent } from './update/update.component';
import { ExternalIpService } from './services/external-ip.service';
import { HttpClientModule } from '@angular/common/http';
import { TimerService } from './services/timer.service';

@NgModule({
  declarations: [
    AppComponent,
    UsageComponent,
    SettingsComponent,
    EventLogComponent,
    UpdateComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    LocalStorageService,
    LogService,
    SettingsService,
    ExternalIpService,
    TimerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
