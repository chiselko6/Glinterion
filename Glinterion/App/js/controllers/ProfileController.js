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

	// profile.photos = {};
	profile.user = {};
	profile.user.gallery = {};
	
	// function update() {
	// 	ObjectDelivery.getUser(profile.user.login).success(function(data) {
	// 		profile.user.account = data;
	// 	});
	// 	ObjectDelivery.getTotalNumber().success(function(data) {
	// 		profile.user.photosNumber = data;
	// 		profile.photos.pages = getTotalPages();
	// 	});
	// 	ObjectDelivery.getTotalSize().success(function(data) {
	// 		profile.user.totalSize = +(+data.toFixed(2));
	// 	});
	// 	ObjectDelivery.getPhotos(profile.photos.photosPage, profile.photos.photosPerPage).query().$promise.then(function(photos) {
	// 		var prefix = "http://" + location.host;
	// 		// in order to set full path to files
	// 		photos.forEach(function(photo) {
	// 			// just here: absolute urls are not allowed for file system
	// 			photo.SrcPreview = prefix + photo.SrcPreview.slice(1);
	// 			photo.SrcOriginal = prefix + photo.SrcOriginal.slice(1);
	// 		});
	// 		// we don't pass additional parameter as 'gallery-photo-link' to <a> because ng-class doesn't have binding to property
	// 		PhotosPopupService($scope, photos);
	// 	});
	// }

	profile.user.gallery.photosPage = 1;
	profile.user.gallery.photosPerPage = 5;
	profile.user.gallery.pages = 1;

	profile.users = [];

	// SharingService.user.userName = profile.user.login;

	var getUser = ObjectDelivery.getUser();
	getUser.success(function(user) {
		profile.user.gallery.photos = user.Photos;
		profile.user.login = user.Login;
		profile.user.account = user.Account;
		profile.user.email = user.Email;
		profile.user.lastName = user.LastName;
		profile.user.firstName = user.FirstName;
		profile.user.role = user.Role;

		profile.user.account.color = ColorConverter.strToHex(user.Account.Color);
	});

	var totalNumber = ObjectDelivery.getTotalNumber();
	totalNumber.success(function(data) {
		profile.user.gallery.photosNumber = data;
		profile.user.gallery.pages = getTotalPages();
	});

	var totalSize = ObjectDelivery.getTotalSize();
	totalSize.success(function(data) {
		profile.user.gallery.totalSize = +(+data.toFixed(2));
		profile.selectPage();
	});

	$q.all([totalSize, getUser]).then(function(data) {
		Chart.apply("size", profile.user.gallery.totalSize, profile.user.account.MaxSize - profile.user.gallery.totalSize);
	})

	function getTotalPages() {
		var pages = Math.floor(profile.user.gallery.photosNumber / profile.user.gallery.photosPerPage);
		if (profile.user.gallery.photosNumber % profile.user.gallery.photosPerPage != 0) {
			pages++;
		}
		return pages;
	}

	profile.selectPage = function() {
		profile.user.gallery.photos = ObjectDelivery.getPhotos(profile.user.gallery.photosPage, profile.user.gallery.photosPerPage).query();

		profile.user.gallery.photos.$promise.then(function(photos) {
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
	};

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
			profile.user.gallery.totalSize = +(+data.toFixed(2));
			Chart.apply("size", profile.user.gallery.totalSize, profile.user.account.MaxSize - profile.user.gallery.totalSize);
		});
		ObjectDelivery.getTotalNumber().success(function(data) {
			profile.user.gallery.photosNumber = data;
			profile.user.gallery.pages = getTotalPages();
		});
		profile.selectPage();
	}

	profile.uploader.onErrorItem = function(item, response, status, headers) {
		item.errorMessage = response;
	}

    profile.abortUpload = function (index) {
        profile.upload[index].abort();
    }

    profile.uploadFile = function(){
        var file = profile.myFile;
        fileUpload.uploadFileToUrl(file);
    };

    profile.getUsers = function() {
    	ObjectDelivery.getUsers().success(function(data) {
    		profile.users = [];
    		data.forEach(function(_user) {
    			var user = {};
    			user.login = _user.Login;
    			user.account = _user.Account;
    			user.email = _user.Email;
    			user.role = _user.Role;
    			user.lastName = _user.LastName;
				user.firstName = _user.FirstName;
				user.account.color = ColorConverter.strToHex(_user.Account.Color);
				profile.users.push(user);
    		})
    	});
    }
}