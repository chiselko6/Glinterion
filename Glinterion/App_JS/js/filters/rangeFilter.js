angular.module("glinterionFilters").filter("range", range);

range.$inject = [

];

function range() {
	return function(input, total) {
		total = parseInt(total);
		for (var i = 1; i <= total; i++)
			input.push(i);
		return input;
	};
}