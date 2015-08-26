angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$scope",
	"$routeParams", 
	"SharingService",
	"ChartService",
	"ColorConverterService",
	"ObjectDeliveryService",
	"PhotosPopupService",
	"formDataObject",
	"FileUploader",
	"$timeout",
	"$q"
];

function ProfileController($scope, $routeParams, SharingService, Chart, ColorConverter, ObjectDelivery, PhotosPopupService, formDataObject, FileUploader, $timeout, $q) {
	var profile = this;

	profile.photos = {};
	profile.user = {};

	function update() {
		ObjectDelivery.getUser(profile.user.login).success(function(data) {
			profile.user.account = data;
		});
		ObjectDelivery.getTotalNumber().success(function(data) {
			profile.user.photosNumber = data;
			profile.photos.pages = getTotalPages();
		});
		ObjectDelivery.getTotalSize().success(function(data) {
			profile.user.totalSize = +(+data.toFixed(2));
		});
		ObjectDelivery.getPhotos(profile.photos.photosPage, profile.photos.photosPerPage).query().$promise.then(function(photos) {
			var prefix = "http://" + location.host;
			// in order to set full path to files
			photos.forEach(function(photo) {
				// just here: absolute urls are not allowed for file system
				photo.SrcPreview = prefix + photo.SrcPreview.slice(1);
				photo.SrcOriginal = prefix + photo.SrcOriginal.slice(1);
			});
			// we don't pass additional parameter as 'gallery-photo-link' to <a> because ng-class doesn't have binding to property
			PhotosPopupService($scope, photos);
		});
	}

	// auth.profilePromise.then(function(profile) {
	//     profile.profile = this.profile;
	// });
	//   // Or using the object
	// profile.profile = auth.profile;


	profile.photos.photosPage = 1;
	profile.photos.photosPerPage = 5;
	profile.photos.pages = 1;

	profile.user.login = $routeParams.userLogin;
	SharingService.userName = profile.user.login;
	var getUser = ObjectDelivery.getUser(profile.user.login);
	getUser.success(function(data) {
		profile.user.account = data;
		
		profile.user.account.color = ColorConverter.strToHex(data.Color);
	});

	// very bad

	ObjectDelivery.getTotalNumber().success(function(data) {
		profile.user.photosNumber = data;
		profile.photos.pages = getTotalPages();
	});
	var totalSize = ObjectDelivery.getTotalSize();
	totalSize.success(function(data) {
		profile.user.totalSize = +(+data.toFixed(2));
	});

	$q.all([totalSize, getUser]).then(function(data) {
		Chart.apply("size", profile.user.totalSize, profile.user.account.MaxSize);
	})

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
			var prefix = "http://" + location.host;
			// in order to set full path to files
			photos.forEach(function(photo) {
				// just here: absolute urls are not allowed for file system
				photo.SrcPreview = prefix + photo.SrcPreview.slice(1);
				photo.SrcOriginal = prefix + photo.SrcOriginal.slice(1);
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
		ObjectDelivery.getTotalSize().success(function(data) {
			profile.user.totalSize = +(+data.toFixed(2));
			Chart.apply("size", profile.user.totalSize, profile.user.account.MaxSize);
		});
		ObjectDelivery.getTotalNumber().success(function(data) {
			profile.user.photosNumber = data;
			profile.photos.pages = getTotalPages();
		});
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