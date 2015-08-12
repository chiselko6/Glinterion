angular.module("glinterion", [
        "ngRoute",
        "ngResource",
        "glinterionControllers",
        "glinterionServices"
    ])
    .config([
        "$routeProvider",
        function($routeProvider) {
            $routeProvider.
                when("/home", {
                    templateUrl: "partials/rapid-board.html",
                    controller: "HomeController"
                }).
                otherwise({
                    redirectTo: "/home"
                })
        }
    ]);

