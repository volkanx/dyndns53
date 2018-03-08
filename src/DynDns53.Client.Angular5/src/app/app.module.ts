import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { UsageComponent } from './usage/usage.component';
import { SettingsComponent } from './settings/settings.component';
import { LocalStorageService } from './local-storage.service';
import { LogService } from './log.service';
import { EventLogComponent } from './event-log/event-log.component';
import { SettingsService } from './settings.service';
import { UpdateComponent } from './update/update.component';


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
    FormsModule
  ],
  providers: [
    LocalStorageService,
    LogService,
    SettingsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
