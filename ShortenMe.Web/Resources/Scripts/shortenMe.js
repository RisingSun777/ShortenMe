(function () {
    var apiHostName = "https://microsoft-apiappb806d628f7474cc297f3b2d8c31d22d7.azurewebsites.net/";
    var hostName = "http://shortenme.azurewebsites.net/";
    //var apiHostName = "http://localhost:3531/";
    //var hostName = "http://localhost:4447/";

	window.Constants = {
	    AppName: "shortenMe",
	    ApiLocation: apiHostName + "api/LinkInfo/",
	    AnalyticsApiLocation: apiHostName + "api/Analytics/",
        WebAppLocation: hostName + "#/"
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
            .when('/analytics/:shortenedLink', {
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
	
	app.controller("linkAnalyticsController", ["$scope", "$routeParams", "$http", function ($scope, $routeParams, $http) {
	    var shortenedLink = $routeParams.shortenedLink;
	    $scope.message = "Running analytics. Please wait for a moment...";

	    $http({
	        method: "GET",
	        url: window.Constants.AnalyticsApiLocation,
	        params: {
	            shortenedLink: shortenedLink
	        }
	    })
	    .then(function (data) {
	        $scope.message = "Analytics result for " + shortenedLink;

	        $scope.analyticsModel = {
	            totalHits: data.data.TotalHits,
	            totalHitsByBrowsers: data.data.TotalHitsByBrowsers,
	            totalHitsInLast7Days: data.data.TotalHitsInLast7Days
	        };
	    }, function (error) {
	        $scope.message = "Analytics failed. Please re-verify your input.";
	        $scope.analyticsModel = null;
	    });
	}]);
})();

(function() {
	var app = angular.module(window.Constants.AppName);
	
	app.controller("linkInfoController", ["$scope", "$http", function($scope, $http) {
	    $scope.fullLinkInfo = "";

	    $scope.submitForm = function () {
	        $scope.shortenedLink = "Please wait while we are shortening your link...";

	        $http({
	            method: "POST",
	            url: window.Constants.ApiLocation,
	            data: {
	                FullLink: $scope.fullLinkInfo
	            }
	        })
            .then(function (data) {
                $scope.shortenedLink = "Your shortened link (will be available for the next 24 hours) is: " + window.Constants.WebAppLocation + data.data;
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