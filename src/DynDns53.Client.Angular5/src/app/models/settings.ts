export class Settings {
  showUsage: boolean;
  numberOfLogEntries: number;
  updateInterval: number;
  accessKey: string;
  secretKey: string;
  domainList: HostedDomainInfo[];
}

export class HostedDomainInfo {
  zoneId: string;
  domainName: string;

  constructor (zoneId: string, domainName: string) {
    this.zoneId = zoneId;
    this.domainName = domainName;
  }
}
