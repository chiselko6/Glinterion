angular.module("glinterionControllers").controller("AccountController", accountController);

accountController.$inject = [
	"CouponCheckingService",
	"SharingService"
];

function accountController(CouponCheckingService, SharingService) {
	var account = this;

	account.account = {}; 
	account.serial = {};
	account.isRejected = false;
	account.isAccepted = false;
    account.userName = SharingService.user.userName;

	account.enter = function(accountNumber) {
		var serial = account.serial.part1 + account.serial.part2 + account.serial.part3;
    	CouponCheckingService.getAccount(serial)
    		.success(function( data ) {
    			if (data == null) {
    				account.isRejected = true;
    				account.isAccepted = false;
	    			account.failureReason = "You entered invalid coupon number!";
    			}
    			else {
    				// CouponCheckingService.updateAccount(data);
	    			account.account = data;
	    			account.isRejected = false;
	    			account.isAccepted = true;
    			}
    		})
    		.error(function(reason) {
    			account.isRejected = true;
    			account.isAccepted = false;
    			account.failureReason = reason;
    		});
	}
}