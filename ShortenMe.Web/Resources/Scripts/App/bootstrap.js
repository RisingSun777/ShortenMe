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
