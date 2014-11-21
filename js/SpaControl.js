(function(app, dust, $) {
	if(!app.hasOwnProperty("controls"))
		app.controls = {};

	app.controls.SpaControl = function(args) {
		if(typeof args == "object") {
			this._name = args.name;
			this._screen = args.screen;
		}
	};

	app.controls.SpaControl.prototype = {
		get name() {
			return this._name;
		},

		get screen() {
			return this._screen;
		},

		get viewModel() {
			return null;
		},

		loadAsync: function() {
			var deferred = new $.Deferred();
			var that = this;

			$.getJSON("/js/controls/resources/" + that.name + "." + $.cookies.get("app.culture") + ".json")
				.done(function(resources) {
					dust.render("app_views_" + that.name.toLowerCase(), { resources: resources }, function(err, text) {
						if(err != null)
							throw Error(err);

						var controlElement = $(text);
						controlElement.addClass("spa-control");

						if("viewModel" in that)
							ko.applyBindings(that.viewModel, controlElement[0]);

						deferred.resolve({ element: controlElement });
					});
				});

			return deferred.promise();
		}
	};
})(app, dust, jQuery);