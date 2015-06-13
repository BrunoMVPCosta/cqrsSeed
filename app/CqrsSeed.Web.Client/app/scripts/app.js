'use strict';

angular.module('app', [
  'ngCookies',
  'ngResource',
  'ngRoute',

  'restangular',

  'app.controllers',
  'app.domains',
  'app.services',
  'app.directives'
])
  .config(['$routeProvider', '$locationProvider', 'RestangularProvider', function ($routeProvider, $locationProvider, RestangularProvider) {
    $routeProvider
      .when('/', {
        controller: 'mainCtrl',
        templateUrl: 'views/main/main.html'
      })
      .when('/inventoryItems', {
        templateUrl: 'views/inventoryItems/inventoryItems.html',
        controller: 'inventoryItemsCtrl'
      })
      .when('/inventoryItems/:id', {
        templateUrl: 'views/inventoryItems/inventoryItems.html',
        controller: 'inventoryItemsCtrl'
      })
    
      .when('/inventoryItemList', {
        templateUrl: 'views/inventoryItemList/inventoryItemList.html',
        controller: 'inventoryItemListCtrl'
      })
      .when('/inventoryItemDetail', {
        templateUrl: 'views/inventoryItemDetail/inventoryItemDetail.html',
        controller: 'inventoryItemDetailCtrl'
      })
      .when('/inventoryItemDetail/:id', {
        templateUrl: 'views/inventoryItemDetail/inventoryItemDetail.html',
        controller: 'inventoryItemDetailCtrl'
      })
    
      .otherwise({
        templateUrl: 'views/main/404.html'
      });

    $locationProvider.html5Mode(true);
    RestangularProvider.setBaseUrl('/api');
  }]);


