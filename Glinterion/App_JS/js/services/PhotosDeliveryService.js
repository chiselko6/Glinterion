angular.module("glinterionServices").service("PhotosDeliveryService", PhotosDeliveryService);

PhotosDeliveryService.$inject = ["$resource"];

function PhotosDeliveryService($resource) {
    return function(startId, endId) {
    	return $resource("../../api/photos", {startId: 1, endId: 1}, {
        	query: { method: "GET", params: {startId: startId, endId: endId}, isArray: true }
    	});
    }
}