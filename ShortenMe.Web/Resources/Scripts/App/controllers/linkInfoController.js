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