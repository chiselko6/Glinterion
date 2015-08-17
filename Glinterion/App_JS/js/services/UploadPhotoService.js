angular.module("glinterionServices").service("UploadPhotoService", UploadPhotoService);

UploadPhotoService.$inject = [
	$http,
	$q
];

function UploadPhotoService() {

    var serv = {};
    serv.uploadFile = function (file) {
        var formData = new FormData();
        formData.append("file", file);
  
        var defer = $q.defer();
        $http.post("/Home/UploadFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function () {
            defer.reject("File Upload Failed!");
        });

        return defer.promise;

    }
    return serv;

    //---------------------------------------------
    //End of Image Upload and Insert record
 
    // This Method IS TO save image name
 

}