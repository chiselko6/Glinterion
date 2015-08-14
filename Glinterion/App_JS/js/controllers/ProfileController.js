angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$scope",
	"$routeParams", 
	"PhotosDeliveryService",
	"PhotosPopupService"
];

function ProfileController($scope, $routeParams, PhotosDelivery, PhotosPopupService) {
	var profile = this;

	profile.user = {};
	profile.user.login = "chiselko6";
	profile.user.accountType = "Simple";
	profile.user.Id = $routeParams.userId;
	profile.user.photos = PhotosDelivery(1, 9).query();

	// profile.galleryPhotoLinkClass = "gallery-photo-link";

	profile.user.photos.$promise.then(function(photos) {
		var prefix = "http://" + location.host + "/";
		// in order to set full path to files
		photos.forEach(function(photo) {
			photo.SrcPreview = prefix + photo.SrcPreview;
			photo.SrcOriginal = prefix + photo.SrcOriginal;
		});
		// we don't pass additional parameter as 'gallery-photo-link' to <a> because ng-class doesn't have binding to property
		PhotosPopupService($scope, photos);
	});

	
}