angular.module("glinterionServices").service("PhotosDeliveryService", PhotosDeliveryService);

PhotosDeliveryService.$inject = ["$resource"];

function PhotosDeliveryService($resource) {
    return $resource("api/photos", {}, {
        query: { method: "GET", params: {}, isArray: true }
    });
}