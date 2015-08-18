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
    return function(startId, endId) {
    	return $resource("../../api/photos", {startId: 1, endId: 1}, {
        	query: { method: "GET", params: {startId: startId, endId: endId}, isArray: true }
    	});
    }
}