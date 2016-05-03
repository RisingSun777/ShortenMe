(function() {
	var app = angular.module(window.Constants.AppName);
	
	app.controller("linkInfoController", function($scope, $http) {
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
	});
})();