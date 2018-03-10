import { Component, OnInit } from '@angular/core';
import { AWS } from 'aws-sdk/dist/aws-sdk';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {

  updating: boolean;

  constructor() { }

  ngOnInit() {
  }

  updateTest() {
    this.changeIP('domainName', 'zoneId', '1.1.1.1');


    const options = {
      'accessKeyId': 'accessKey',
      'secretAccessKey': 'secretKey'
    };
    const route53 = new AWS.Route53(options);


    const params = {
      HostedZoneId: 'zoneId'
    };

    route53.listResourceRecordSets(params, function(err, data) {
        if (err) {
        //  $rootScope.$emit('rootScope:log', err.message);
       //   $rootScope.$apply();
        } else {
          data.ResourceRecordSets.forEach(function(value, key) {
              if (value.Name.slice(0, -1) === 'domainName') {
                const externalIPAddress = '6.6.6.6';
                this.changeIP('domainName', 'zoneId', externalIPAddress);
              }
          });
        }
    });
  }

  changeIP(domainName, zoneId, newIPAddress) {
    const options = {
      'accessKeyId': 'AKIAIUMVVHICMG2Q4BTQ',
      'secretAccessKey': '/22YMSJ52g5MEjFITo1f9+N0vSW69DraEKdo2yZ'
    };

    console.log('AWS');
    console.log(new AWS.Route53());

    const route53 = new AWS.Route53();

    console.log('route53');
    console.log(route53);

    const params = {
      ChangeBatch: {
        Changes: [
          {
            Action: 'UPSERT',
            ResourceRecordSet: {
              Name: 'test1.vlkn.me',
              Type: 'A',
              TTL: 300,
              ResourceRecords: [ {
                  Value: '6.6.6.6'
                }
              ]
            }
          }
        ]
      },
      HostedZoneId: 'Z5E5IXDGLAKX6'
    };

    route53.changeResourceRecordSets(params, function(err, data) {

      console.log(data);

      // if (err) {
      // // //  $rootScope.$emit('rootScope:log', err.message);
      // }
      // else {
      // // //  var logMessage = "Updated domain: " + domainName + " ZoneID: " + zoneId + " with IP Address: " + newIPAddress;
      // //  // $rootScope.$emit('rootScope:log', logMessage);


      // }

     // $rootScope.$apply();
    });
  }


}
