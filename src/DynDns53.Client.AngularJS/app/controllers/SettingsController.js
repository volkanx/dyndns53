app.controller('SettingsController', ['$scope', '$rootScope', 'LocalStorage', function($scope, $rootScope, LocalStorage) {
  if (LocalStorage.getData('formSaved') == null || LocalStorage.getData('formSaved') == "undefined") {
    LocalStorage.setData('formSaved', "false");
  }

  $scope.loadValues = function() {
     $rootScope.$emit('rootScope:log', 'Loading configuration values from the local storage');
     
     $rootScope.updateInterval = isNaN(parseInt(LocalStorage.getData('updateInterval'))) ? 5 : parseInt(LocalStorage.getData('updateInterval'));
     $rootScope.maxLogRowCount = isNaN(parseInt(LocalStorage.getData('maxLogRowCount'))) ? 50 : parseInt(LocalStorage.getData('maxLogRowCount'));
     $rootScope.accessKey = LocalStorage.getData('accessKey');
     $rootScope.secretKey = LocalStorage.getData('secretKey');
     $rootScope.domainList = JSON.parse(LocalStorage.getData('domainList'));
     if ($rootScope.domainList == null) {
        $rootScope.domainList = { "domains": [] }
     }
  };

  $scope.saveValues = function() {
    LocalStorage.setData('updateInterval', $rootScope.updateInterval)
    LocalStorage.setData('maxLogRowCount', $rootScope.maxLogRowCount)
    LocalStorage.setData('accessKey', $rootScope.accessKey)
    LocalStorage.setData('secretKey', $rootScope.secretKey)
    LocalStorage.setData('domainList', JSON.stringify($rootScope.domainList))
    LocalStorage.setData('formSaved', "true")
    $rootScope.$emit('rootScope:log', 'Saved configuration values to the local storage');
  };

  $scope.addDomain = function() {
    $rootScope.domainList.domains.push({ 'name': '', 'zoneId': '' })
    $scope.settingsForm.domainListHidden.$validate();
  };

  $scope.deleteDomain = function(index) {
    $rootScope.domainList.domains.splice(index, 1);
    $scope.settingsForm.domainListHidden.$validate();
  };

  $scope.domainsUpdated = function() {
    $scope.settingsForm.domainListHidden.$validate();
  }
}]);
