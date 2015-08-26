angular.module("glinterionServices").service("ColorConverterService", colorConverter);

colorConverter.$inject = [

];

function colorConverter() {
	var that = this;

	var componentToHex = function(c) {
	    var hex = c.toString(16);
	    return hex.length == 1 ? "0" + hex : hex;
	}

	this.rgbToHex = function(r, g, b) {
	    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
	}

	this.strToHex = function(str) {
		str = str.split(',');
		var r = +str[0].split(' ').join('');
		var g = +str[1].split(' ').join('');
		var b = +str[2].split(' ').join('');
		return that.rgbToHex(r, g, b);
	}
}