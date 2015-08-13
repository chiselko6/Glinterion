angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "glinterionControllers",
        "glinterionServices"
    ])
    .config([
        "$routeProvider",
        config
    ]);

function config($routeProvider) {
    $routeProvider.
        when("/home", {
            templateUrl: "partials/home.html",
            controller: "HomeController"
        }).
        otherwise({
            redirectTo: "/home"
        });
}

