(function(head) {	
	var bootstrap = [
		// Dependencies 
		"//knockoutjs.com/downloads/knockout-3.2.0.js",
		"//code.jquery.com/jquery-2.1.1.min.js",
		"/js/jquery.cookies.min.js",
		"/js/dust-core.min.js",
		"/js/ui-templates.js",
		"/js/SpaApp.js",
		"/js/SpaScreen.js",
		"/js/SpaControl.js",

		// App entry point
		function() {
			var app = new window.app.SpaApp();
			app.startAsync();
		}
	];

	head.load.apply(window, bootstrap);
})(head);