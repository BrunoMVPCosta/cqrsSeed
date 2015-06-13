/*global $:false */
'use strict';

angular.module('app.services.signalrClient', [])
.service('signalrClient', [function() {
  var instance = {
    inventory: $.connection.inventory
  };
    
  $.connection.inventory.connection.logging = false; // enable for debugging
  $.connection.inventory.client.start = function () { }; // init callback (used for establishing sockets connection)

  $.connection.hub.start();
  $.connection.hub.error(function (error) { console.log('SignalR error: ' + error); });
    
  return instance;
}]);