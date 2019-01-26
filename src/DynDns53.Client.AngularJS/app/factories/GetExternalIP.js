

app.factory('GetExternalIP', function ($http) {
  return $http.get('https://46ebv5trsk.execute-api.eu-west-2.amazonaws.com/prod', { cache: false });
});