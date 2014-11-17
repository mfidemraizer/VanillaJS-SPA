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
					dust.render("app_views_" + that.id, { resources: resources }, function(err, text) {
						if(err != null)
							throw Error(err);

						var screenElement = $(text);

						if(typeof args == "object" && args.hasOwnProperty("viewModel"))
							ko.applyBindings(args.viewModel, screenElement[0]);

						deferred.resolve({ element: screenElement });
					});
				});

			return deferred.promise();
		}
	};
})(app, dust, jQuery);