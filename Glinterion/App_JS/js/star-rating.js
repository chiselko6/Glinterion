(function ( $ ) {
	
	// let's save all stars created
	var stars = [];
	var parentIndex = 0;

	jQuery.fn.starRating = function(settings) {

		// parametres needed
		var configs;


		// merge user settings with default values
		configs = $.extend({}, {
			grades: 5,
			initTotal: 5,
			isEnabled: [],
			position: "right",
			dataField: "rating",
			url: "..",
			submitObj: {},
			dataReceived:
			 function(data) { alert(data);}
		}, settings);

		if (settings.isEnabled == undefined) {
			for (var i = 0; i < configs.grades; i++) {
				configs.isEnabled[i] = true;
			}
		}

		// if configs.grades is real
		configs.grades = Math.floor(configs.grades);
		// if configs.grades < 0
		configs.grades = Math.max(configs.grades, 0);

		// if configs.initTotal < 0
		configs.initTotal = Math.max(configs.initTotal, 0);
		// if configs.initTotal > configs.grades
		configs.initTotal = Math.min(configs.initTotal, configs.grades);

		return $(this).each(function() {
			if ($(this).attr("star-wrapper-index") != undefined) {
				var wrapperIndex = $(this).attr("star-wrapper-index");
				update(configs.initTotal, wrapperIndex);
				return;
			}
			var wrapper = getStarWrapper(parentIndex);
			$(this).append(wrapper);
			var tempStars = [];
			// draw certain number of stars in div specified as a wrapper
			for (var i = 0; i < configs.grades; i++) {
				tempStars.push(starFactory($(this), i));
				$(wrapper).append(tempStars[tempStars.length - 1]);
			}
			stars.push(tempStars);
			setAttributes(parentIndex);
			setOptions(wrapper, parentIndex);
			parentIndex++;
		});

		function getStarWrapper(wrapperIndex) {
			var wrapper = $("<div>");
			wrapper.addClass("star-wrapper");
			wrapper.attr("star-wrapper-index", wrapperIndex);
			return wrapper;
		}

		function update(newRating, wrapperIndex) {
			setPercentage(newRating, wrapperIndex);
		}

		function setAttributes(wrapperIndex) {

			function setHover(star) {
				star.hover(
					// hover in
					function() {
						if (configs.isEnabled[star.parent().attr("star-wrapper-index")]) {
							// color changes
							for (var i = 0; i < configs.grades; i++) {
								stars[wrapperIndex][i].removeClass("star-selected");
								stars[wrapperIndex][i].removeClass("star-half-selected");
							}
							//set class .star-active for all previous items
							var index = +($(this).attr("data-index"));
							for (var i = 0; i < index; i++) {
								$(this).siblings("[data-index=\"" + i + "\"]").addClass("star-active");
							}
							$(this).addClass("star-active");
						}
					},	
					// hover out					
					function() {
						if (configs.isEnabled[star.parent().attr("star-wrapper-index")]) {
							// remove class .star-active for all previous items
							var index = +($(this).attr("data-index"));
							for (var i = 0; i < index; i++) {
								$(this).siblings("[data-index=\"" + i + "\"]").removeClass("star-active");
							}
							$(this).removeClass("star-active");
							// have to color stars back
							setPercentage(configs.initTotal, wrapperIndex);
						}
					})
				
			}

			function setClick(star) {
				star.click(function() {
					if (configs.isEnabled[star.parent().attr("star-wrapper-index")]) {
						// to prevent side-effects
						configs.isEnabled[star.parent().attr("star-wrapper-index")] = false;
						// then it will update and set for static value
						disable($(this).parent(0));
						// get index == rating for current element
						var index = $(this).attr("data-index");
						index++;
						// write rating to the object property provided
						configs.submitObj[configs.submitObj.dataField] = index;
						// post results
						$.post({
							url: configs.url,
							data: configs.submitObj							
						}).always(function(data) {
						    //raise event on parent and pass data to it
						    configs.dataReceived(1, star.parent());
						});
						// now user can't vote again
					}
				})

				function disable(parent) {
					// first get index of the star parent 
					var wrapperIndex = $(parent).attr("star-wrapper-index");
					for (var i = 0; i < configs.grades; i++) {
						with(stars[wrapperIndex][i]) {
							removeClass("star-active");
							removeClass("star-selected");
							removeClass("star-half-selected");
							//addClass("star-disabled");
							// clear hover and click events
							hover(function(){});
							click(function(){});
						}
					}
				}
			}
			for (var i = 0; i < configs.grades; i++) {
				setHover(stars[wrapperIndex][i]);
				setClick(stars[wrapperIndex][i]);
			}

		}

		// create 1 new star
		function starFactory(star, index) {
			var star = $("<div>");
			addAttributes(star, index);

			function addAttributes(star, index) {
				star.addClass("star-item");
				star.attr("data-index", index);
			}

			return star;
		}

		function setPercentage(total, wrapperIndex) {
			var step = 1;
			// count stars full colored
			var count_selected = Math.floor(total / step);
			for (var i = 0; i < count_selected; i++) {
				stars[wrapperIndex][i].addClass("star-selected");
			}
			// and one star may be half colored, if rest rating is over than half of it
			if (configs.initTotal - count_selected * step + 0.0000001 >= step / 2) {
				stars[wrapperIndex][count_selected].addClass("star-half-selected");
			}
		}

		function setOptions(wrapper, wrapperIndex) {
			// first color stars according to the total rating
			setPercentage(configs.initTotal, wrapperIndex);
			// in order avoid <style> in markup, let's use default style
			if (configs.position !== "right") {
				$(wrapper).css("text-align", configs.position);
			}
		}
	};
})(jQuery);