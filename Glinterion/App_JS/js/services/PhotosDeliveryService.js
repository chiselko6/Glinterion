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

PhotosDeliveryService.$inject = ["$resource"];

function PhotosDeliveryService($resource) {
    return function(pageNumber, photosPerPage) {
    	return $resource("../../api/photos", {pageNumber: 1, photosPerPage: 1}, {
        	query: { method: "GET", params: {pageNumber: pageNumber, photosPerPage: photosPerPage}, isArray: true }
    	});
    }
}