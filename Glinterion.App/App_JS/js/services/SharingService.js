angular.module("glinterionServices").service("SharingService", sharingService);

sharingService.$inject = [

];

function sharingService() {
	var that = this;

	this.isChanged = false;
}