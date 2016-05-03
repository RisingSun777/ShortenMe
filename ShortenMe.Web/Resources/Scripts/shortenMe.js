(function() {
	window.Constants = {
	    AppName: "shortenMe",
	    ApiLocation: "http://localhost:3531/api/LinkInfo/"
	};
	var app = angular.module(window.Constants.AppName, ['ngRoute']);

	app.config(['$routeProvider', function ($routeProvider) {
	    $routeProvider
            .when('/', {
                templateUrl: 'Resources/Scripts/App/partials/linkInfo.html',
                controller: 'linkInfoController'
            })
            .when('/:shortenedLink', {
                templateUrl: 'Resources/Scripts/App/partials/redirectFromShortenedLink.html',
                controller: 'redirectFromShortenedLinkController'
            })
            .when('/analytics', {
                templateUrl: 'Resources/Scripts/App/partials/analytics.html',
                controller: 'linkAnalyticsController'
            })
            .otherwise({
                redirectTo: '/'
            });
	}]);
})();

(function() {
	var app = angular.module(window.Constants.AppName);
	
	app.controller("linkInfoController", ["$scope", "$http", function($scope, $http) {
	    $scope.fullLinkInfo = "";

	    $scope.submitForm = function () {
	        $scope.shortenedLink = "";
	        $scope.errorMessage = "";

	        $http({
	            method: "POST",
	            url: window.Constants.ApiLocation,
	            data: {
	                FullLink: $scope.fullLinkInfo
	            }
	        })
            .then(function (data) {
                $scope.shortenedLink = "Your shortened link (will be available for the next 24 hours) is: " + data.data;
                $scope.errorMessage = "";
            }, function (error) {
                $scope.errorMessage = "Shortening failed. Please re-verify your link input.";
                $scope.shortenedLink = "";
            });
	    };
	}]);
})();
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