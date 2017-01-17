angular.module('starter.services', [])

.service('placeMarkers', function($http) {
    var markers = [];
    return {
      getMarkers: function() {
        return $http.get("data-json.json").success(function(response) {
          markers = response;
          return markers;
        });
      }
    } //end return
  }) //end service


.service('mathFunction', function($q) {
    return {
      calcDistance: function(lat1, lon1, lat2, lon2) {
          var deferred = $q.defer();
          var R = 6371; // Radius of the earth in km
          // var dLat = deg2rad(lat2 - lat1); // deg2rad below
          // var dLon = deg2rad(lon2 - lon1);
          var dLat = (lat2 - lat1) * (Math.PI / 180); // deg2rad below
          var dLon = (lon2 - lon1) * (Math.PI / 180);

          var a =
            Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos((lat1) * (Math.PI / 180)) * Math.cos((lat2) * (Math.PI / 180)) *
            Math.sin(dLon / 2) * Math.sin(dLon / 2);
          var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
          var d = R * c; // Distance in km
          deferred.resolve(d);
          return deferred.promise;
        } //end calcDistance function
    }
  }) //end mathFunction

.service('txtToSpeech', function() {
    return {
      speakText: function(myText) {
          window.TTS.speak({
            text: myText,
            locale: 'en-US',
            rate: 1,
            volume: 50
          }, function() {
            // Do Something after success

          }, function(reason) {
            console.log("error: ", reason);
          }); // end speak
        } // speakText function


    } //end return
  }) //end of txtToSpeech

.factory('storeCurrentlyVisitedPlace', function() {
  var invokedName = {};
  var invokedDesc = {};
  var data = [];
  return {
    getInfo: function() {
      console.log("data in getInfo: ", data[0]);
      console.log("data in getInfo: ", data[1]);
      return (data);
    },

    setInfo: function(name, desc) {
      data[0] = name;
      data[1] = desc;

      //invokedName = name;
      //invokedDesc = desc;
      console.log("data in setInfo: ", data[0]);
      console.log("data in setInfo: ", data[1]);
    }
  };

}) //end factory
