(function(head) {	
	var bootstrap = [
		// Dependencies 
		"//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout-min.js",
		"//code.jquery.com/jquery-2.1.1.min.js",
		"/js/jquery.cookies.min.js",
		"/js/dust-core.min.js",
		"/js/ui-templates.js",
		"/js/SpaApp.js",
		"/js/SpaScreen.js",
		"/js/SpaControl.js",

		// SignalR
		"//ajax.aspnetcdn.com/ajax/signalr/jquery.signalr-2.1.0.min.js",
		"//localhost:9555/signalr/hubs",

		// App entry point
		function() {
			var app = new window.app.SpaApp();
			app.startAsync();
		}
	];

	head.load.apply(window, bootstrap);
})(head);