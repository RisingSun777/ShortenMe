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
