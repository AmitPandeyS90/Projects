// .controller('LocateCtrl', function($scope, $ionicLoading, $http, $ionicPlatform, placeMarkers, mathFunction, $cordovaGeolocation, txtToSpeech) {
//     $ionicPlatform.ready(function() {
//       //rit coordinates 43.0841584,-77.6751706
//       var myLatlng = new google.maps.LatLng(43.0841584, -77.6751706);
//       var mapOptions = {
//         center: myLatlng,
//         zoom: 16,
//         mapTypeId: google.maps.MapTypeId.ROADMAP
//       };
//
//       var map = new google.maps.Map(document.getElementById("map"), mapOptions);
//
//       var watchOptions = {
//         timeout: 40000,
//         enableHighAccuracy: true,
//         maximumAge: 0
//       };
//
//       //create watch as id
//       //var watch = $cordovaGeolocation.watchPosition(watchOptions);
//       var watch = $cordovaGeolocation.watchPosition(watchOptions);
//
//       //promise status
//       watch.then(
//         null,
//
//         //in case of ERROR
//         function(err) {
//           console.warn('ERROR(' + err.code + '): ' + err.message);
//         },
//
//         //promise success
//         function success(pos) {
//
//           //map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
//           var myPos = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);
//
//           //place marker at the user's current position
//           var myLocation = new google.maps.Marker({
//             position: myPos,
//             map: map,
//             //animation: google.maps.Animation.DROP,
//             center: myPos,
//             title: "My Location",
//             icon: "YouAreHere.png"
//           }); //end Marker
//
//           //instead of creating a new marker, adjust the same marker. Gives the impression of marker moving
//           //myLocation.setCenter(myPos);
//
//           myLocation.setPosition(myPos);
//           map.panTo(myPos);
//
//           //call service and get target location to compare
//           placeMarkers.getMarkers().then(function(markers) {
//             var lat = pos.coords.latitude;
//             var long = pos.coords.longitude;
//
//             //console.log("in moveMarker");
//             //loop through array(target) and continously calculate distance between user's location and target
//             angular.forEach(markers.data, function(value, index) {
//               //console.log("Latitude: ", value.lat, "Longitude: ", value.long);
//               console.log("user location ", lat, long);
//               mathFunction.calcDistance(lat, long, value.lat, value.long)
//                 //promise fulfilles
//                 .then(function(d) {
//                   if (d <= 0.5) {
//                     console.log(value.desc);
//                     txtToSpeech.speakText(value.desc);
//                   }
//                 }, function(error) {
//                   //promise rejected
//                   console.log("Error speaking text", error);
//                 }); //end mathFunction
//             }); //end forEach
//           }); //end placeMarkers
//         }); //function success
//
//       //$cordovaGeolocation.clearWatch(watch);
//
//
//
//       //map markers for RIT hotspots
//       $scope.map = map;
//       $scope.markers = [];
//       var infoWindow = new google.maps.InfoWindow();
//
//       var createMarker = function(info) {
//           var marker = new google.maps.Marker({
//             position: new google.maps.LatLng(info.lat, info.long),
//             map: $scope.map,
//             icon: "listen.gif",
//             animation: google.maps.Animation.DROP
//           });
//
//           marker.content = '<div class="infoWindowContent">' + info.title + '</div>';
//
//           google.maps.event.addListener(marker, 'click', function() {
//             infoWindow.setContent('<h2>' + marker.title + '</h2>');
//             infoWindow.open($scope.map, marker);
//           });
//           $scope.markers.push(marker);
//         } //end of createMarker function
//
//       var locArr = [];
//       //call a service placeMarkers to read lat/long from json
//       // Then call createMarker function to place marker at the lat/long
//       placeMarkers.getMarkers().then(function(markers) {
//         //locArr = markers.data;
//         for (i = 0; i < markers.data.length; i++) {
//           createMarker(markers.data[i]);
//           locArr[i] = markers.data[i];
//         } //end for
//         //locArr = markers.data;
//
//       }); //end service call
//     }); //end ready function
//   }) //end LocateCtrl

              //
              //               //console.log(locArr);
              //               var watch_user_position_after_secs = 3000;
              //
              //               //setInterval(function(){
              //               // userPosition.FindUserPosition().then(function(lat){
              //               //   //console.log(locArr);
              //               //   console.log(lat);
              //               // }); //end userPosition service call
              //
              //               //}, watch_user_position_after_secs)
              //               //var locArr = [];
              //               //read data from json
              //               // $http.get("data-json.json").success(function(response) {
              //               //
              //               //   //load data in the array
              //               //   locArr = response;
              //               //   for (i = 0; i < response.length; i++) {
              //               //     createMarker(response[i]);
              //               //   }
              //               //
              //               // });
              //               //  console.log(locArr);
              //
              //
              //             } //end of moveMarker
              //         }); //end of ionicPlatform device ready function
              //         //google.maps.event.addDomListener(document.getElementById("map"), 'load', $scope.initialise());
              //         //$ionicPlatform.ready(function () {$scope.initialise()});
              //       }) //end of LocateCtrl





            // .controller('LocateCtrl', function($scope, $ionicLoading, $http, placeMarkers, userPosition, mathFunction, $cordovaGeolocation) {
            //
            //     //shows users current location and markers on RIT map
            //     $scope.initialise = function() {
            //
            //       //rit coordinates 43.0841584,-77.6751706
            //       var myLatlng = new google.maps.LatLng(43.0841584, -77.6751706);
            //       var mapOptions = {
            //         center: myLatlng,
            //         zoom: 16,
            //         mapTypeId: google.maps.MapTypeId.ROADMAP
            //       };
            //       var map = new google.maps.Map(document.getElementById("map"), mapOptions);
            //
            //       //sets the user location at the center of the map
            //       // navigator.geolocation.getCurrentPosition(function(pos) {
            //       //   map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
            //       // });
            //
            //       //watches the user every 3 secs for it's current position
            //
            //       //     //call service and get target location to compare
            //
            //       //       //loop through array(target) and continously calculate distance between user's location and target
            //
            //
            //       var watchOptions = {
            //         timeout: 10000,
            //         enableHighAccuracy: true,
            //         maximumAge: 0
            //       };
            //       var watch = $cordovaGeolocation.watchPosition(watchOptions);
            //
            //       watch.then(
            //         null,
            //
            //         function(err) {
            //           console.warn('ERROR(' + err.code + '): ' + err.message);
            //         },
            //
            //         function success(pos) {
            //
            //           //map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
            //
            //           var myPos = new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude);
            //
            //           var myLocation = new google.maps.Marker({
            //             position: myPos,
            //             map: map,
            //             animation: google.maps.Animation.DROP,
            //             center: myLocation.setPosition(myPos),
            //             title: "My Location",
            //             icon: "YouAreHere.png"
            //           }); //end Marker
            //
            //           //call service and get target location to compare
            //           placeMarkers.getMarkers().then(function(markers) {
            //             var lat = pos.coords.latitude;
            //             var long = pos.coords.longitude;
            //
            //             //loop through array(target) and continously calculate distance between user's location and target
            //             angular.forEach(markers.data, function(value, index) {
            //               //console.log("Latitude: ", value.lat, "Longitude: ", value.long);
            //               console.log("user location ", lat, long);
            //               // mathFunction.calcDistance(lat, long, value.lat, value.long).then(function(d) {
            //               //   console.log(d);
            //               // }); //end mathFunction
            //
            //             }); //end forEach
            //           }); //end placeMarkers
            //         }); //function success
            //
            //       $cordovaGeolocation.watch.clearWatch(watch);
            //
            //
            //
            //
            //
            //
            //
            //       // var options = {
            //       //   timeout: 10000,
            //       //   enableHighAccuracy: true,
            //       //   maximumAge: 0
            //       // };
            //       //
            //       // function success(pos) {
            //       //   //map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
            //       //
            //       //   var myLocation = new google.maps.Marker({
            //       //     position: new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude),
            //       //     map: map,
            //       //     //animation: google.maps.Animation.DROP,
            //       //     center: new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude),
            //       //     title: "My Location",
            //       //     icon: "YouAreHere.png"
            //       //   }); //end Marker
            //       //
            //       //   //call service and get target location to compare
            //       //   placeMarkers.getMarkers().then(function(markers) {
            //       //     var lat = pos.coords.latitude;
            //       //     var long = pos.coords.longitude;
            //       //
            //       //     //loop through array(target) and continously calculate distance between user's location and target
            //       //     angular.forEach(markers.data, function(value, index) {
            //       //       //console.log("Latitude: ", value.lat, "Longitude: ", value.long);
            //       //       console.log("user location ", lat, long);
            //       //        mathFunction.calcDistance(lat, long, value.lat, value.long).then(function(d){
            //       //          console.log(d);
            //       //       });
            //       //
            //       //     });
            //       //   });
            //       // }; //function success
            //       //
            //       // function error(err) {
            //       //   console.warn('ERROR(' + err.code + '): ' + err.message);
            //       // } //function error
            //       //
            //       // setInterval(function() {
            //       //   navigator.geolocation.getCurrentPosition(success, error, options)
            //       // }, 5000);
            //
            //
            //       $scope.map = map;
            //
            //       // Additional Markers //
            //       $scope.markers = [];
            //       var infoWindow = new google.maps.InfoWindow();
            //       var createMarker = function(info) {
            //           var marker = new google.maps.Marker({
            //             position: new google.maps.LatLng(info.lat, info.long),
            //             map: $scope.map,
            //             icon: "listen.gif",
            //             animation: google.maps.Animation.DROP
            //           });
            //           marker.content = '<div class="infoWindowContent">' + info.title + '</div>';
            //           google.maps.event.addListener(marker, 'click', function() {
            //             infoWindow.setContent('<h2>' + marker.title + '</h2>');
            //             infoWindow.open($scope.map, marker);
            //           });
            //           $scope.markers.push(marker);
            //         } //end of createMarker function
            //
            //       var locArr = [];
            //       //call a service placeMarkers to read lat/long from json
            //       // Then call createMarker function to place marker at the lat/long
            //       placeMarkers.getMarkers().then(function(markers) {
            //         //locArr = markers.data;
            //         for (i = 0; i < markers.data.length; i++) {
            //           createMarker(markers.data[i]);
            //           locArr[i] = markers.data[i];
            //         } //end for
            //         //locArr = markers.data;
            //       }); //end service call
            //
            //       //console.log(locArr);
            //       var watch_user_position_after_secs = 3000;
            //
            //       //setInterval(function(){
            //       // userPosition.FindUserPosition().then(function(lat){
            //       //   //console.log(locArr);
            //       //   console.log(lat);
            //       // }); //end userPosition service call
            //
            //       //}, watch_user_position_after_secs)
            //       //var locArr = [];
            //       //read data from json
            //       // $http.get("data-json.json").success(function(response) {
            //       //
            //       //   //load data in the array
            //       //   locArr = response;
            //       //   for (i = 0; i < response.length; i++) {
            //       //     createMarker(response[i]);
            //       //   }
            //       //
            //       // });
            //       //  console.log(locArr);
            //
            //
            //     }; //end of $scope.initialise
            //
            //     google.maps.event.addDomListener(document.getElementById("map"), 'load', $scope.initialise());
            //
            //   }) //end of LocateCtrl

            //               // var options = {
            //               //   timeout: 10000,
            //               //   enableHighAccuracy: true,
            //               //   maximumAge: 0
            //               // };
            //               //
            //               // function success(pos) {
            //               //   //map.setCenter(new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude));
            //               //
            //               //   var myLocation = new google.maps.Marker({
            //               //     position: new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude),
            //               //     map: map,
            //               //     //animation: google.maps.Animation.DROP,
            //               //     center: new google.maps.LatLng(pos.coords.latitude, pos.coords.longitude),
            //               //     title: "My Location",
            //               //     icon: "YouAreHere.png"
            //               //   }); //end Marker
            //               //
            //               //   //call service and get target location to compare
            //               //   placeMarkers.getMarkers().then(function(markers) {
            //               //     var lat = pos.coords.latitude;
            //               //     var long = pos.coords.longitude;
            //               //
            //               //     //loop through array(target) and continously calculate distance between user's location and target
            //               //     angular.forEach(markers.data, function(value, index) {
            //               //       //console.log("Latitude: ", value.lat, "Longitude: ", value.long);
            //               //       console.log("user location ", lat, long);
            //               //        mathFunction.calcDistance(lat, long, value.lat, value.long).then(function(d){
            //               //          console.log(d);
            //               //       });
            //               //
            //               //     });
            //               //   });
            //               // }; //function success
            //               //
            //               // function error(err) {
            //               //   console.warn('ERROR(' + err.code + '): ' + err.message);
            //               // } //function error
            //               //
            //               // setInterval(function() {
            //               //   navigator.geolocation.getCurrentPosition(success, error, options)
            //               // }, 5000);
            //
            //
