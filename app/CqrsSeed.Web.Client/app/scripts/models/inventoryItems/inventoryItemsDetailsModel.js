'use strict';

angular.module('app.modules.inventoryItems.inventoryItemsDetailsModel', [])
  .service('inventoryItemsDetailsModel', ['inventoryDomain', function (inventoryDomain) {
    var model = {
      inventoryItem: null,
      changeCountValue: 0,
      changeNameValue: '',
      mode: null,

      init: function (id) {
        switch(id) {
          case undefined: // item not selected
            this.setMode('hide');
            this.inventoryItem = null;
            break;
          case 'create': // add new item 
            this.inventoryItem = { count: 0, name: '' };
            this.setMode('add');
            break;
          default: // view selected item
            if (this.inventoryItem == null || this.inventoryItem.id !== id) {
              this.get(id);
            }
            this.setMode('view');
            break;
        }
      },

      setMode: function (mode) {
        this.mode = mode;
        switch (mode) {
          case 'rename':
            this.changeNameValue = this.inventoryItem.name;
            break;
          case 'changeCount':
            this.changeCountValue = 0;
            break;
        }
      },

      get: function(id){
        var self = this;
        self.inventoryItem = inventoryDomain.get(id).then(function (item) { self.inventoryItem = item; });
      },
                
      save: function(){
        var self = this;
        inventoryDomain.save(self.inventoryItem);
        self.setMode('view');
      },

      deactivate: function () {
        var self = this;
        inventoryDomain.deactivate(self.inventoryItem);
      },
      
      rename: function () {
        var self = this;
        self.inventoryItem.name = self.changeNameValue;
        inventoryDomain.rename(self.inventoryItem);
        self.setMode('view');
      },
      
      removeItems: function (count) {
        var self = this;
        count = parseInt(count);
        if (self.inventoryItem.currentCount >= count && count > 0) {
          inventoryDomain.removeItems(self.inventoryItem, count);
          self.setMode('view');
        }
      },
      
      checkIn: function(count) {
        var self = this;
        count = parseInt(count);
        if (count > 0){
          inventoryDomain.checkIn(self.inventoryItem, count);
          self.setMode('view');
        }
      }
    };

    inventoryDomain.onEvent(inventoryDomain.events.CREATED, function(item) {
      model.inventoryItem = item;
      model.setMode('view');
    });

    inventoryDomain.onEvent(inventoryDomain.events.CHANGED, function (item) {
      if (model.inventoryItem && model.inventoryItem.id == item.id) {
        model.inventoryItem = item;
      }
    });

    return model;
  }]);