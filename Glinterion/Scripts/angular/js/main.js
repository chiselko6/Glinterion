angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "glinterionControllers",
        "glinterionServices"
    ])
    .config([
        "$routeProvider", "$locationProvider",
        config
    ]);

function config($routeProvider, $locationProvider) {
    $routeProvider.
        when("/home", {
            templateUrl: "partials/home.html",
            controller: "HomeController"
        }).
        when("/profile/:userId", {
            templateUrl: "partials/profile.html",
            controller: "ProfileController"
        }).
        otherwise({
            redirectTo: "/home"
        });
    // allows to use natural URLs instead of unnatural hashbang URLs
    //$locationProvider.html5Mode(true);
}

