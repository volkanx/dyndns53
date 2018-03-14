app.directive('domainListChecks', function() {
  return {
    require: 'ngModel',
    link: function(scope, elm, attrs, ctrl) {
      ctrl.$validators.emptyDomainList = function(modelValue, viewValue) {
        if (modelValue.domains.length > 0) {
          return true;
        }
        return false;
      };

      /*
      When enabled, this validation causes a weird bug (all domains disappear on 2nd addDomain call) so disabled for the time being
      ctrl.$validators.nameAndZoneIdRequired = function(modelValue, viewValue) {
        // var nonEmptyEntries = modelValue.domains.filter(function(el){ return el.name && el.zoneId; });
        // return (nonEmptyEntries.length == modelValue.domains.length);
        return true;
      };
      */
    }
  };
});