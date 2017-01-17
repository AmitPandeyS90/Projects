angular.module('starter.controllers', ['starter.services', 'ngCordova', 'ionic'])

.controller('HmCtrl', function($scope) {})


.controller('LocateCtrl', function($scope, $ionicLoading, $http, $ionicPlatform, placeMarkers, mathFunction, txtToSpeech, $cordovaGeolocation, storeCurrentlyVisitedPlace) {

    $ionicPlatform.ready(function() {
      //display RIT map
      //rit coordinates 43.0841584,-77.6751706
      var myLatlng = new google.maps.LatLng(43.0841584, -77.6751706);
      var mapOptions = {
        center: myLatlng,
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      }; //end of mapoptions

      var map = new google.maps.Map(document.getElementById("map"), mapOptions);

      //options for getCurrentPosition
      var options = {
        timeout: 10000,
        enableHighAccuracy: true,
        maximumAge: 0
      };

      //get current position of the user
      $cordovaGeolocation.getCurrentPosition(options).then(function(position) {

        var myPos = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

        //place marker at the user's current position
        var myLocation = new google.maps.Marker({
          position: myPos,
          map: map,
          animation: google.maps.Animation.DROP,
          center: myPos,
          title: "My Location",
          icon: "YouAreHere.png"
        }); //end Marker

        // Additional Markers //
        $scope.markers = [];
        $scope.map = map;
        var infoWindow = new google.maps.InfoWindow();

        var createMarker = function(info) {
            var marker = new google.maps.Marker({
              position: new google.maps.LatLng(info.lat, info.long),
              map: $scope.map,
              icon: "listen.gif",
              animation: google.maps.Animation.DROP
            });
            marker.content = '<div class="infoWindowContent">' + info.title + '</div>';
            google.maps.event.addListener(marker, 'click', function() {
              infoWindow.setContent('<h2>' + marker.title + '</h2>');
              infoWindow.open($scope.map, marker);
            });
            $scope.markers.push(marker);
          } //end of createMarker function

        var locArr = [];

        //call a service placeMarkers to read lat/long from json
        // Then call createMarker function to place marker at the lat/long
        placeMarkers.getMarkers().then(function(markers) {

          for (i = 0; i < markers.data.length; i++) {
            createMarker(markers.data[i]);
            locArr[i] = markers.data[i];
          } //end for

        }); //end service call


        //call function to make the marker moving as user moves
        //also places target(RIT hotspots) markers
        moveMarker(map, myLocation);

      }); //end getCurrentPosition function
    }); //end of ionic platform ready


    var moveMarker = function(map, myLocation) {

        document.addEventListener('deviceready', function() {

          //set options for watchPosition
          var watchOptions = {
            timeout: 10000,
            enableHighAccuracy: true,
            maximumAge: 0
          };

          //create watch to constantly monitor user movement
          var watch = $cordovaGeolocation.watchPosition(watchOptions);

          //variable that stores the nameof the visited place
          //stops program from invoking speak function if user remains in the same place
          var previouslyVisitedName = null;
          watch.then(
            null,
            //in case of ERROR
            function(err) {
              console.warn('ERROR(' + err.code + '): ' + err.message);
            },
            //promise kept!
            function success(pos) {

              var myPos = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);

              //instead of creating a new marker, adjust the same marker. Gives the impression of marker moving
              myLocation.setPosition(myPos);
              map.panTo(myPos);

              //call service and get target location to compare
              placeMarkers.getMarkers().then(function(markers) {
                var lat = pos.coords.latitude;
                var long = pos.coords.longitude;

                //loop through array(hotspots)
                angular.forEach(markers.data, function(value, index) {

                  console.log("user location ", lat, long);

                  //console.log(previouslyVisitedName, value.title);

                  //function that continously calculate distance between user's location and target
                  mathFunction.calcDistance(lat, long, value.lat, value.long)

                  //promise fulfilled
                  .then(function(d) {
                    if ((d <= 0.03) && (previouslyVisitedName != value.title)) {
                      previouslyVisitedName = value.title;
                      storeCurrentlyVisitedPlace.setInfo(value.title, value.desc);
                      txtToSpeech.speakText(value.desc);
                    }
                  }, function(error) {
                    //promise rejected
                    console.log("Error speaking text", error);
                  }); //end mathFunction
                }); //end forEach
              }); //end placeMarkers
            }); //function success
            
          //watch.clearWatch();
        }); //end device ready
      } //end of moveMarker

  }) //end of locate controller

.controller('ReadCtrl', function($scope,storeCurrentlyVisitedPlace, $state) {
  $scope.mydata = [];
    $scope.mydata = storeCurrentlyVisitedPlace.getInfo();
    //console.log(data);
    //(function(ttitle, descr){
        console.log("Title in Read Controller", $scope.mydata[0]);
        console.log("Description in Read Controller", $scope.mydata[1]);
        //document.getElementByTagName("p").innerHTML = mydata[0];
        //$state.go('tab.read', {result: mydata});
  //  })

    //console.log(name, "---", desc);

})
