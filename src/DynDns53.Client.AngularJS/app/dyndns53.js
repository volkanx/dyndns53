var app = angular.module('dyndns53App', ['timer', 'ngMessages']);

app.run(function($rootScope) {
    
    $rootScope.logData = []
    $rootScope.logDataString = ""

    $rootScope.clearLog = function() {
      $rootScope.logData = []
      $rootScope.logDataString = ""
    }

    $rootScope.$on('rootScope:log', function (event, data) {
      var logLine = moment().format('D/MM/YYYY HH:mm:ss') + '\t' + data; //+ '\n';
      $rootScope.logData.unshift(logLine);
      $rootScope.logData = $rootScope.logData.slice(0, $rootScope.maxLogRowCount);
      $rootScope.logDataString = $rootScope.logData.join('\n');
    });
})
