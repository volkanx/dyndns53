

app.factory('GetExternalIP', function ($http) {
  return $http.get('https://8x6c5h83d9.execute-api.eu-west-2.amazonaws.com/prod', { cache: false });
});