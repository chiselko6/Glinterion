angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$scope",
	"$routeParams", 
	"ObjectDeliveryService",
	"PhotosPopupService",
	"formDataObject",
	"FileUploader",
	"$timeout",
	"auth"
];

function ProfileController($scope, $routeParams, ObjectDelivery, PhotosPopupService, formDataObject, FileUploader, $timeout, auth) {
	var profile = this;

	profile.photos = {};
	profile.user = {};

	// auth.profilePromise.then(function(profile) {
	//     profile.profile = this.profile;
	// });
	//   // Or using the object
	// profile.profile = auth.profile;


	profile.photos.photosPage = 1;
	profile.photos.photosPerPage = 5;
	profile.photos.pages = 1;

	profile.user.login = $routeParams.userLogin;
	ObjectDelivery.getUser(profile.user.login).success(function(data) {
		profile.user.account = data;
	});
	// .$promise.then(function(data) {
	// 	 = data;
	// });
	//profile.user.Id = $routeParams.userId;

	ObjectDelivery.getTotalNumber().success(function(data) {
		profile.user.photosNumber = data;
		profile.photos.pages = getTotalPages();
	});
	ObjectDelivery.getTotalSize().success(function(data) {
		profile.user.totalSize = +(+data.toFixed(2));
	});

	function getTotalPages() {
		var pages = Math.floor(profile.user.photosNumber / profile.photos.photosPerPage);
		if (profile.user.photosNumber % profile.photos.photosPerPage != 0) {
			pages++;
		}
		return pages;
	}

	(profile.selectPage = function() {
		profile.user.photos = ObjectDelivery.getPhotos(profile.photos.photosPage, profile.photos.photosPerPage).query();

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
	})();

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

    profile.uploader.onBeforeUploadItem = function (item) {
    	var rating = (item.rating ? item.rating : null);
	    if (item.description) {
		    item.headers.description = item.description;
	    }
	    item.headers.rating = rating;
	    var getFileExtension = function(item) {
	    	var name = item._file.name;
        	var parts = name.split('.');
        	return parts[parts.length - 1];
	    }
        item.headers.fileExtension = getFileExtension(item);
    };

	profile.uploader.onSuccessItem = function(item, response, status, headers) {
		profile.user.totalSize += +((+headers["size"] / 1024 / 1024).toFixed(2));
		profile.user.totalSize = +(profile.user.totalSize.toFixed(2));
		profile.user.photosNumber++;
		profile.photos.pages = getTotalPages();
		profile.selectPage();
	}

    profile.abortUpload = function (index) {
        profile.upload[index].abort();
    }

    profile.uploadFile = function(){
        var file = profile.myFile;
        // console.log('file is ' );
        // console.dir(file);
        // var uploadUrl = "/";
        fileUpload.uploadFileToUrl(file);
    };
}