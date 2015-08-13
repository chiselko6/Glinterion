angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$routeParams", 
	"PhotosDeliveryService"
];

function ProfileController($routeParams, PhotosDelivery) {
	var profile = this;

	profile.user = {};
	profile.user.login = "chiselko6";
	profile.user.accountType = "Simple";
	profile.user.Id = $routeParams.userId;
	profile.user.photos = PhotosDelivery.query();
	
}