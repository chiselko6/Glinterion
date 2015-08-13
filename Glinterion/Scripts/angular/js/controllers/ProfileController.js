angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$routeParams", 
	"$scope",
	"PhotosDeliveryService"
];

function ProfileController($routeParams, $scope, PhotosDelivery) {
	var profile = this;

	profile.userId = $routeParams.userId;
}