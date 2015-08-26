angular.module("glinterionServices").service("UploadPhotoService", UploadPhotoService);

UploadPhotoService.$inject = [
	$http
];

function UploadPhotoService($http) {
    this.uploadFileToUrl = function(file, uploadUrl){
        var uploadUrl = "../../api/photos/uploadAvatar";
        var fd = new FormData();
        fd.append('file', file);
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: {'Content-Type': undefined}
        })
        .success(function(){

        })
        .error(function(){

        });
    }
}]);

    //---------------------------------------------
    //End of Image Upload and Insert record
 
    // This Method IS TO save image name
 

}