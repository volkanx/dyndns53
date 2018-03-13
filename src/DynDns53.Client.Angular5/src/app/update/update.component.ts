import { Component, OnInit } from '@angular/core';
import * as AWS from 'aws-sdk';
import { LogService } from '../services/log.service';
import { SettingsService } from '../services/settings.service';
import { Settings } from '../models/settings';
import { ExternalIpService } from '../services/external-ip.service';
import { TimerObservable } from 'rxjs/observable/TimerObservable';
import { Subscription } from 'rxjs/Subscription';
import { count } from 'rxjs/operators';
import { TimerService } from '../services/timer.service';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {

  autoUpdateEnabled: boolean;
  settings: Settings | null;
  route53Client: AWS.Route53;
  tick: string;
  private subscription: Subscription;

  constructor(private settingsService: SettingsService, private logService: LogService,
    private externalIPService: ExternalIpService, public timerService: TimerService) {
    }

  ngOnInit() {
    this.autoUpdateEnabled = false;
  }

  toggleAutoUpdate(): void {
    if (this.autoUpdateEnabled) {
      // Stop
      this.autoUpdateEnabled = false;
      this.subscription.unsubscribe();
      this.logService.addMessage('Disabled auto-update');
    } else {
      // Start
      const currentSettings = this.settingsService.getSettings();
      if (currentSettings) {
          this.autoUpdateEnabled = true;
          const logMessage = 'Enabled auto-update at every: ' + currentSettings.updateInterval + ' minutes';
          this.logService.addMessage(logMessage);

          const timer = TimerObservable.create(0, currentSettings.updateInterval * 60 * 1000);
          this.subscription = timer.subscribe(t => {
              this.timerService.stop();
              this.logService.addMessage('Updating...');
              this.updateAllDomains();
              this.timerService.start(currentSettings.updateInterval * 60);

          });
      } else {
          this.logService.addMessage('ERROR: Couldn\'t retrieve settings');
      }
    }
  }

  updateAllDomains() {
    this.settings = this.settingsService.getSettings();
    if (this.settings) {
      const domains = this.settings.domainList;
      const options = {
        'accessKeyId': this.settings.accessKey,
        'secretAccessKey': this.settings.secretKey
      };
      this.route53Client = new AWS.Route53(options);

      if (domains === undefined || domains.length === 0) {
        this.logService.addMessage('No domains to update');
        return;
      }

      domains.forEach((domain) => {
        this.updateDomainInfo(domain.domainName, domain.zoneId);
      });
    } else {
      this.logService.addMessage('ERROR: Couldn\'t retrieve settings');
    }
  }

  updateDomainInfo(domainName: string, zoneId: string) {
    const params = {
      HostedZoneId: zoneId
    };

    this.route53Client.listResourceRecordSets(params, (err, data) => {
        if (err) {
          const errorMessage = 'Error: ' + err.message;
          this.logService.addMessage(errorMessage);
        } else {
          data.ResourceRecordSets.forEach((resourceRecordSet) => {
            if (resourceRecordSet.Name.slice(0, -1) === domainName) {
              this.externalIPService
                .getIP()
                .subscribe( (response) => {
                    const externalIPAddress = response.ip;
                    // Only update if the current IP is different
                    if (resourceRecordSet && resourceRecordSet.ResourceRecords) {
                      if (resourceRecordSet.ResourceRecords[0].Value !== externalIPAddress) {
                        this.changeIP(domainName, zoneId, externalIPAddress);
                      } else {
                        const logMessage = 'IP Address hasn\'t changed. Skipping update.';
                        this.logService.addMessage(logMessage);
                      }
                    } else {
                        const logMessage = 'Error: Couldn\'t get resource record set';
                        this.logService.addMessage(logMessage);
                    }
                });
            }
          });
        }
    });
  }

  changeIP(domainName: string, zoneId: string, newIPAddress: string) {
    const params = {
      ChangeBatch: {
        Changes: [
          {
            Action: 'UPSERT',
            ResourceRecordSet: {
              Name: domainName,
              Type: 'A',
              TTL: 300,
              ResourceRecords: [ {
                  Value: newIPAddress
                }
              ]
            }
          }
        ]
      },
      HostedZoneId: zoneId
    };

    this.route53Client.changeResourceRecordSets(params, (err, data) => {
      if (err) {
        const errorMessage = 'Error: ' + err.message;
        this.logService.addMessage(errorMessage);
      } else {
        const logMessage = 'Updated domain: ' + domainName + ' ZoneID: ' + zoneId + ' with IP Address: ' + newIPAddress;
        this.logService.addMessage(logMessage);
      }
    });
  }


}
