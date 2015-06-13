'use strict';

angular.module('app.domains.inventoryDomain', [])
  .service('inventoryDomain', ['Restangular', 'signalrClient', function (restangular, signalrClient) {

    function wrapObject(obj) {
      if (obj.remove === undefined) {
        return restangular.restangularizeElement(null, obj, 'inventory');
      } else {
        return obj;
      }
    }
    
    function versionParam(inventoryItem) {
      return { version: inventoryItem.version };
    }

    var inventoryDomain = {
      getList: restangular.all('inventory').getList,

      get: function(id) { return restangular.one('inventory', id).get(); },

      save: function(inventoryItem) { restangular.all('inventory').post({ name: inventoryItem.name }); },

      deactivate: function(inventoryItem) {
        wrapObject(inventoryItem).remove(versionParam(inventoryItem));
      },

      rename: function(inventoryItem) {
        wrapObject(inventoryItem).put();
      },

      removeItems: function(inventoryItem, count) {
        wrapObject(inventoryItem).customPOST(count, 'removeItems', versionParam(inventoryItem));
      },
      
      checkIn: function(inventoryItem, count) {
        wrapObject(inventoryItem).customPOST(count, 'checkIn', versionParam(inventoryItem));
      },
      
      onEvent: function (name, fun) {
        signalrClient.inventory.on(name, fun);

        return {
          unsubscribe: function() { signalrClient.inventory.off(name, fun); }
        };
      },
      
      events: {
        CREATED: 'inventoryItemCreated',
        CHANGED: 'inventoryItemChanged',
        REMOVED: 'inventoryItemRemoved',
        LIST_UPDATED: 'inventoryItemsListUpdated'
      }
    };
    
    return inventoryDomain;
  }]);