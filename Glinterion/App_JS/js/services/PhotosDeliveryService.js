angular.module("glinterionServices").service("PhotosDeliveryService", PhotosDeliveryService);

angular.module("glinterionServices").factory("formDataObject", function() {
	return function(data) {
		var fd = new FormData();
			angular.forEach(data, function(value, key) {
				fd.append(key, value);
			});
		return fd;
	};
});

PhotosDeliveryService.$inject = [
	"$resource",
	"$http"
];

function PhotosDeliveryService($resource, $http) {
    	var that = this;

    	this.getPhotos = function(pageNumber, photosPerPage) {
	    	return $resource("../../api/photos", {pageNumber: 1, photosPerPage: 1}, {
	        	query: { method: "GET", params: {pageNumber: pageNumber, photosPerPage: photosPerPage}, isArray: true }
	    	});
    	}

    	// here we don't use $resource because it's supposed to be used for rest services
    	this.getTotalSize = function() {
    		return $http.get("../../api/photos/TotalSize");
    		// return $resource("../../api/photos/TotalSize", {}, {
    		// 	query: {method: "GET", params: {}, isArray: false}
    		// });
    	}

    	this.getTotalNumber = function() {
    		return $http.get("../../api/photos/TotalNumber");
    		// return $resource("../../api/photos/TotalNumber", {}, {
    		// 	query: {method: "GET", params: {}, isArray: false}
    		// });
    	}
}