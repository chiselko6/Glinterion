angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "glinterionControllers",
        "glinterionServices",
        "glinterionDirectives",
        "glinterionFilters",
        "angularFileUpload"
    ])
    .config([
        "$routeProvider", 
        "$compileProvider",
        "$locationProvider",
        config
    ]);

function config($routeProvider, $compileProvider, $locationProvider) {
    $routeProvider.
        when("/home", {
            templateUrl: "/app/partials/home.html",
            controller: "HomeController"
        }).
        when("/profile", {
            templateUrl: "/app/partials/profile.html",
            controller: "ProfileController"
        }).
        when("/account", {
            templateUrl: "/app/partials/account.html",
            controller: "AccountController"
        }).
        otherwise({
            redirectTo: "/home"
        });
    $compileProvider.imgSrcSanitizationWhitelist = /^\s*(https?|ftp|file|blob):|data:image\//;

    // allows to use natural URLs instead of unnatural hashbang URLs
    $locationProvider.html5Mode(false);
}