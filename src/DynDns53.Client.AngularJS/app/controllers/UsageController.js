
app.controller('UsageController', ['$scope', 'LocalStorage', function($scope, LocalStorage) {
  if (LocalStorage.getData('hideUsage') == null || LocalStorage.getData('hideUsage') == "undefined") {
    LocalStorage.setData('hideUsage', "false");
  }

  $scope.hideUsage = LocalStorage.getData('hideUsage');
  $scope.toggleUsageButtonText = ($scope.hideUsage == 'true') ? "Show usage" : "Hide usage";

  $scope.toggleUsage = function() {
    $scope.hideUsage = ($scope.hideUsage == 'true') ? 'false' : 'true';
    $scope.toggleUsageButtonText = ($scope.hideUsage == 'true') ? "Show usage" : "Hide usage";
    LocalStorage.setData('hideUsage', $scope.hideUsage)
  }
}]);