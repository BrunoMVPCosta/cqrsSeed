'use strict';

angular.module('app.controllers.inventoryItemListCtrl', [])
  .controller('inventoryItemListCtrl', ['inventoryItemsListModel', '$scope',
    function(inventoryItemsListModel, $scope) {
      inventoryItemsListModel.init();
      $scope.listModel = inventoryItemsListModel;
    }]);