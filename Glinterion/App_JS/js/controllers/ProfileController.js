angular.module("glinterionControllers").controller("ProfileController", ProfileController);

ProfileController.$inject = [
	"$scope",
	"$routeParams", 
	"PhotosDeliveryService",
	"PhotosPopupService",
	"FileUploader"
];

function ProfileController($scope, $routeParams, PhotosDelivery, PhotosPopupService, FileUploader, $http, $timeout, $upload) {
	var profile = this;

	profile.user = {};
	profile.user.login = "chiselko6";
	profile.user.accountType = "Simple";
	profile.user.Id = $routeParams.userId;
	profile.user.photos = PhotosDelivery(1, 9).query();
	profile.uploader = new FileUploader({
		url: "../../api/photos/upload",
		queueLimit: 5,
		formData: [1, 2, 3]
	});

	profile.uploader.filters.push({
        name: 'imageFilter',
        fn: function(item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });

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


	// profile.upload = [];
 //    profile.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

 //    profile.onFileSelect = function ($files) {
 //    	console.log(52);
 //        //$files: an array of files selected, each file has name, size, and type.
 //        for (var i = 0; i < $files.length; i++) {
 //            var $file = $files[i];
 //            (function (index) {
 //                profile.upload[index] = $upload.upload({
 //                    url: "../../api/photos/upload", // webapi url
 //                    method: "POST",
 //                    data: { fileUploadObj: profile.fileUploadObj },
 //                    file: $file
 //                }).progress(function (evt) {
 //                    // get upload percentage
 //                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
 //                }).success(function (data, status, headers, config) {
 //                    // file is uploaded successfully
 //                    console.log(data);
 //                }).error(function (data, status, headers, config) {
 //                    // file failed to upload
 //                    console.log(data);
 //                });
 //            })(i);
 //        }
 //    }

    profile.abortUpload = function (index) {
        profile.upload[index].abort();
    }
}