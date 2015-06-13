'use strict';

angular.module('app.directives.activeTab', [])
  .directive('activeTab', ['$location', function ($location) {
    return {
      restrict: 'A',
      link: function (scope, elm, attrs) {
        var route = attrs.activeTab;
        var exactMatching = attrs.hasOwnProperty('isExact');
        scope.$on('$routeChangeSuccess', function () {
          if (exactMatching ? ($location.path() === route) : ($location.path().indexOf(route) === 0)) {
            $(elm.context).addClass('active');
          } else {
            $(elm.context).removeClass('active');
          }
        });
      }
    };
  } ]);
