'use strict';

angular.module('app.modules.inventoryItems.inventoryItemsListModel', [])
  .service('inventoryItemsListModel', ['inventoryDomain', '$rootScope', function (inventoryDomain, $rootScope) {
    var model = {
      list: null,

      init: function () {
        var self = this;
        if (self.list === null) {
          inventoryDomain.getList().then(function(items) { self.list = items; });
        }
      },
    };

    inventoryDomain.onEvent(inventoryDomain.events.LIST_UPDATED, function (items) {
      model.list = items;
      $rootScope.$digest();
    });

    return model;
  }]);