'use strict';

angular.module('app.controllers.inventoryItemDetailCtrl', [])
  .controller('inventoryItemDetailCtrl', ['inventoryItemsDetailsModel', '$scope', '$routeParams', '$location', 'inventoryDomain',
    function(inventoryItemsDetailsModel, $scope, $routeParams, $location, inventoryDomain) {
      inventoryItemsDetailsModel.init($routeParams.id);
      $scope.detailsModel = inventoryItemsDetailsModel;

      var handlers = [
        inventoryDomain.onEvent(inventoryDomain.events.CREATED, function(item) { $location.url('/inventoryItemDetail/' + item.id); }),
        inventoryDomain.onEvent(inventoryDomain.events.REMOVED, function(id) { if (id == $routeParams.id) $location.url('/inventoryItemList/'); })
      ];
      
      $scope.$on("$destroy", function () {
        for (var i in handlers) { handlers[i].unsubscribe(); }
      });
    }]);