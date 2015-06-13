'use strict';

angular.module('app.controllers.main', [])
  .controller('mainCtrl', ['$scope', function ($scope) {
  
    $scope.awesomeThings = [
      'CQRS + Event Sourcing',
      'ASP.NET MVC Web API',
      'SignalR'
    ];
  }]);
