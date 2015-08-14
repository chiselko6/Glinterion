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

	profile.user.photos.$promise.then(function(photos) {

		// in order to set full path to files
		photos.forEach(function(photo) {
			photo.SrcPreview = location.host + "/" + photo.SrcPreview;
		});
	});
	
}