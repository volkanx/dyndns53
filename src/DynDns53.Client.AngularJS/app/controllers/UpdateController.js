app.controller('UpdateController', ['$scope', '$rootScope', '$http', 'GetExternalIP', '$interval', 'LocalStorage', function($scope, $rootScope, $http, GetExternalIP, $interval, LocalStorage) {

  intervalfunc = function(){ 
    $scope.$broadcast('auto-update-timer-restart');
    $scope.updateAllDomains();
  }

  $scope.toggleAutoUpdate = function() {
    if ($scope.domainList.domains.length == 0) {
      $rootScope.$emit('rootScope:log', 'No domains to update');
      return;
    }

    if ($scope.updating) {
      // Stop
      $scope.updating = false;
      $interval.cancel(intervalPromise);
      $scope.$broadcast('auto-update-stop');
      var logMessage = "Stopping auto-update";
      $rootScope.$emit('rootScope:log', logMessage);
    } else {
      // Start
      $scope.updating = true;
      intervalPromise = $interval(intervalfunc, ($scope.updateInterval * 60 * 1000));
      $scope.$broadcast('auto-update-start');
      var logMessage = "Starting auto-update at every: " + $scope.updateInterval + " minutes";
      $rootScope.$emit('rootScope:log', logMessage);
    }
  }

  $scope.updateAllDomains = function() {
    if ($scope.domainList.domains.length == 0) {
      $rootScope.$emit('rootScope:log', 'No domains to update');
      return;
    }

    angular.forEach($scope.domainList.domains, function(value, key) {
      $scope.updateDomainInfo(value.name, value.zoneId);
    });
  }

  $scope.updateDomainInfo = function(domainName, zoneId) {
    var options = {
      'accessKeyId': $scope.accessKey,
      'secretAccessKey': $scope.secretKey
    };
    var route53 = new AWS.Route53(options);
    
    var params = {
      HostedZoneId: zoneId
    };

    route53.listResourceRecordSets(params, function(err, data) {
        if (err) { 
          $rootScope.$emit('rootScope:log', err.message);
          $rootScope.$apply();
        } else {
          angular.forEach(data.ResourceRecordSets, function(value, key) {
              if (value.Name.slice(0, -1) == domainName) {
                var externalIPAddress = "";
                GetExternalIP
                  .then(function(response) {
                     externalIPAddress = response.data.ip;
                     if (value.ResourceRecords[0].Value != externalIPAddress) {
                       $scope.changeIP(domainName, zoneId, externalIPAddress)
                     } else {
                        $rootScope.$emit('rootScope:log', 'IP address is up-to-date. Skipping update.');
                     }
                 });
              }
          });
        }
    });
  }

  $scope.changeIP = function(domainName, zoneId, newIPAddress) {
    var options = {
      'accessKeyId': $scope.accessKey,
      'secretAccessKey': $scope.secretKey
    };

    var route53 = new AWS.Route53(options);
    var params = {
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

    route53.changeResourceRecordSets(params, function(err, data) {
      if (err) { 
        $rootScope.$emit('rootScope:log', err.message); 
      }
      else { 
        var logMessage = "Updated domain: " + domainName + " ZoneID: " + zoneId + " with IP Address: " + newIPAddress;
        $rootScope.$emit('rootScope:log', logMessage);
      }

      $rootScope.$apply();
    });
  }
}]);