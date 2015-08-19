angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$scope",
	"$routeParams", 
	"PhotosDeliveryService",
	"PhotosPopupService",
	"formDataObject",
	"FileUploader",
	"$timeout"
];

function ProfileController($scope, $routeParams, PhotosDelivery, PhotosPopupService, formDataObject, FileUploader, $timeout) {
	var profile = this;

	profile.user = {};
	profile.user.login = "chiselko6";
	profile.user.accountType = "Simple";
	profile.user.Id = $routeParams.userId;
	profile.user.photos = PhotosDelivery(1, 9).query();
	profile.uploader = new FileUploader({
		url: "../../api/photos/upload",
		queueLimit: 5
	});

	profile.uploader.filters.push({
        name: 'imageFilter',
        fn: function(item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });

    profile.uploader.onAfterAddingFile = function(item) {
    	$timeout(function() {
	    	$(".rating").rating({
				stars: 5,
				min: 0,
				max: 5,
				step: 1,
				"size": "sm"
			});
	    }, 100);
    }

    profile.uploader.onBeforeUploadItem = function (item) {
    	if (!item.description || !item.rating) {
			item.cancel();
			return;
		}
		if (isNaN(+item.rating)) {
			// profile.uploader.cancelItem(item);
			item.cancel();
			return;
		}
	    item.formData.push({description : item.description, rating: item.rating});
	};

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

    profile.abortUpload = function (index) {
        profile.upload[index].abort();
    }
}