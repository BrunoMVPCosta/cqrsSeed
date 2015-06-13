'use strict';

angular.module('app.controllers.inventoryItems', ['app.modules.inventoryItems.inventoryItemsListModel', 'app.modules.inventoryItems.inventoryItemsDetailsModel'])
  .controller('inventoryItemsCtrl', ['inventoryItemsListModel', 'inventoryItemsDetailsModel', '$scope', '$routeParams', '$location', 'inventoryDomain',
    function(inventoryItemsListModel, inventoryItemsDetailsModel, $scope, $routeParams, $location, inventoryDomain) {
      inventoryItemsListModel.init();
      $scope.listModel = inventoryItemsListModel;

      inventoryItemsDetailsModel.init($routeParams.id);
      $scope.detailsModel = inventoryItemsDetailsModel;

      var handlers = [
        inventoryDomain.onEvent(inventoryDomain.events.CREATED, function(item) { $location.url('/inventoryItems/' + item.id); }),
        inventoryDomain.onEvent(inventoryDomain.events.REMOVED, function (id) { if (id == $routeParams.id) $location.url('/inventoryItems/'); })
      ];
      
      $scope.$on("$destroy", function () {
        for (var i in handlers) { handlers[i].unsubscribe(); }
      });
    }]);