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
