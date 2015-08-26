angular.module("glinterionServices").service("PhotosPopupService", PhotosPopupService);

PhotosPopupService.$inject = [
	"$timeout"
];

function PhotosPopupService($timeout) {
	return function($scope, photos) {
		$scope.$watch(photos, function (newValue, oldValue) {
			// $timeout(function() {
				// Initialize popup as usual
				$(".gallery-photo-link").magnificPopup({ 
					type: 'image',
					mainClass: 'mfp-with-zoom', // this class is for CSS animation below

					// delegate: 'a',

					gallery:{
						enabled:true,
						preload: [0,2], // read about this option in next Lazy-loading section

						navigateByImgClick: true,

						arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>', // markup of an arrow button

						tPrev: 'Previous (Left arrow key)', // title for left button
						tNext: 'Next (Right arrow key)' // title for right button
					},

					image: {
						cursor: 'mfp-zoom-out-cur',
						titleSrc: "title"
					},
					// This will create a single gallery from all elements that have class "gallery-item"

					zoom: {
						enabled: true, // By default it's false, so don't forget to enable it

						duration: 300, // duration of the effect, in milliseconds
						easing: 'ease-in-out', // CSS transition easing function 

						// // The "opener" function should return the element from which popup will be zoomed in
						// // and to which popup will be scaled down
						// // By defailt it looks for an image tag:
						// opener: function(openerElement) {
						//   // openerElement is the element on which popup was initialized, in this case its <a> tag
						//   // you don't need to add "opener" option if this code matches your needs, it's defailt one.
						//   return openerElement.is('img') ? openerElement : openerElement.find('img');
						// }
					}

				});
			// });
		});
		
	}
}