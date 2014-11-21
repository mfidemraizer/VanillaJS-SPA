(function(app, dust, $) {
	if(!app.hasOwnProperty("screens"))
		app.screens = {};

	app.screens.SpaScreen = function(args) {
		if(!args.hasOwnProperty("app"))
			throw Error("'app' is required");

		if(!args.hasOwnProperty("id")) 
			throw Error("'id' is required");

		this._app = args.app;
		this._id = args.id;
	};

	app.screens.SpaScreen.prototype = {
		get app() {
			return this._app;
		},

		get id() {
			return this._id;
		},

		loadAsync: function(args) {
			var deferred = new $.Deferred();
			var that = this;

			$.getJSON("/resources/" + this.id + "Screen." + $.cookies.get("app.culture") + ".json")
				.done(function(resources) {
					dust.render("app_views_" + that.id.toLowerCase(), { resources: resources }, function(err, text) {
						if(err != null)
							throw Error(err);

						var screenElement = $("<div />").addClass("spa-screen-content").append(text);

						that.loadControlsAsync(screenElement).done(function() {
							deferred.resolve({ element: screenElement });
						});
					});
				});

			return deferred.promise();
		},

		loadControlsAsync: function(screenElement) {
			var deferred = new $.Deferred();
			var controlElements = screenElement.find("[data-spa-control]");
			var controlSources = [];
			var controlNames = [];
			var that = this;

			controlElements.each(function(controlIndex, controlElement) {
				controlNames.push(controlElement.getAttribute("data-spa-control"));
				controlSources.push("/js/controls/" + controlElement.getAttribute("data-spa-control") + ".js");
			});

			controlSources.push(function() {
				var controlLoadPromises = [];

				controlNames.forEach(function(controlName) {
					var control = new app.controls[controlName]({ screen: that, name: controlName });
					controlLoadPromises.push(control.loadAsync());
				});

				$.when.apply($, controlLoadPromises).done(function() {
					var controlIndex = 0;

					for(var controlPromiseArgIndex in arguments) {
						$(controlElements[controlIndex]).replaceWith(arguments[controlPromiseArgIndex].element);

						controlIndex++;
					}

					deferred.resolve();
				});
			});

			head.load.apply(window, controlSources);

			return deferred.promise();
		}
	};
})(app, dust, jQuery);