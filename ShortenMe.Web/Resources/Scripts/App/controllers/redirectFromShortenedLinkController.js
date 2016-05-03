(function() {
	var app = angular.module(window.Constants.AppName);
	
	app.controller("redirectFromShortenedLinkController", ["$scope", "$http", "$window", "$location", "$routeParams", function ($scope, $http, $window, $location, $routeParams) {
	    $scope.shortenedLink = $routeParams.shortenedLink;

	    $http({
	        method: "GET",
	        url: window.Constants.ApiLocation,
	        params: {
	            shortenedLink: $scope.shortenedLink
	        }
	    })
        .then(function (data) {
            $window.location.href = data.data;
        }, function (error) {
            $location.path("/");
        });
	}]);
})();