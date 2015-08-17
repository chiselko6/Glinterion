angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "glinterionControllers",
        "glinterionServices",
        "glinterionDirectives",
        "angularFileUpload"
    ])
    .config([
        "$routeProvider", "$compileProvider",
        config
    ]);

function config($routeProvider, $compileProvider) {
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
    $compileProvider.imgSrcSanitizationWhitelist = /^\s*(https?|ftp|file|blob):|data:image\//;

    // allows to use natural URLs instead of unnatural hashbang URLs
    //$locationProvider.html5Mode(true);
}

