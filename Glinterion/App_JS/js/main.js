angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "auth0",
        "glinterionControllers",
        "glinterionServices",
        "glinterionDirectives",
        "glinterionFilters",
        "angularFileUpload"
    ])
    .config([
        "$routeProvider", 
        "$compileProvider",
        config
    ]);

function config($routeProvider, $compileProvider) {
    $routeProvider.
        when("/home", {
            templateUrl: "partials/home.html",
            controller: "HomeController"
        }).
        when("/profile/:userLogin", {
            templateUrl: "partials/profile.html",
            controller: "ProfileController"
        }).
        when("/profile/:userLogin/account", {
            templateUrl: "partials/account.html",
            controller: "AccountController"
        }).
        otherwise({
            redirectTo: "/home"
        });
    $compileProvider.imgSrcSanitizationWhitelist = /^\s*(https?|ftp|file|blob):|data:image\//;

    // allows to use natural URLs instead of unnatural hashbang URLs
    // $locationProvider.html5Mode(true);
}