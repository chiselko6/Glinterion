angular.module("glinterionServices").service("CouponCheckingService", couponCheckingService);

couponCheckingService.$inject = [
	"$http"
];

function couponCheckingService($http) {

	this.getAccount = function(serial) {
		return $http.get("../../api/accounts/check", {params: {serial: serial}});
	}

	this.updateAccount = function(account) {
		// return $http({
		// 	url: "../../api/accounts/update",
		// 	method: "post",
		// 	data: { account: _account }
		// });
		return $http.post("../../api/accounts/update", {params: {account: account}});
	}
}