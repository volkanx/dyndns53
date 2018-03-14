app.controller('TimerController', ['$scope', '$rootScope', function($scope, $rootScope) {
  $scope.$on('auto-update-start', function (event, data) {
      $scope.countdown = $scope.updateInterval * 60
      $scope.$broadcast('timer-reset');
      $scope.$broadcast('timer-add-cd-seconds', $scope.countdown);
    });

  $scope.$on('auto-update-stop', function (event, data) {
      $scope.$broadcast('timer-reset');
    });

  $scope.$on('auto-update-timer-restart', function (event, data) {
      $scope.countdown = $scope.updateInterval * 60
      $scope.$broadcast('timer-reset');
      $scope.$broadcast('timer-add-cd-seconds', $scope.countdown);
    });
}]);