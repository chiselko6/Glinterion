angular.module("glinterionServices").service("ChartService", chartService);

chartService.$inject = [
	
];

function chartService() {
	var that = this;

	this.apply = function(id, used, remain) {
		var ctx = document.getElementById(id).getContext("2d");
		var data = [
			{
				value: used.toFixed(2),
				color: "#ff0000",
				highlight: "#ff0055",
				label: "used"
			},
			{
				value: remain.toFixed(2),
				color: "#00ff00",
				highlight: "#00ff55",
				label: "remain"
			}
		];
		var myNewChart = new Chart(ctx).Pie(data);
	}

}