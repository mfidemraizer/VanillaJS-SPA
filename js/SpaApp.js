(function($, global) {
	global.app = (function() {
		var exports = {};

		exports.SpaApp = function() {
			this._config = null;
			this._contentElement = null;

			window.addEventListener("popstate", this.onNavigationChange);
		};

		exports.SpaApp.prototype = {
			get config() {
				return this._config;
			},

			get contentElement() {
				return this._contentElement;
			},

			startAsync: function() {
				var that = this;
				var deferred = new $.Deferred();

				var configPromise = this.loadAppConfigAsync();

				$.when(configPromise).done(function(config) {
					that._config = config;
					that._contentElement = $("#content");

					that.navigateToAsync(that.config.ui.defaultScreenId).done(function() {
						deferred.resolve();
					});
				});

				return deferred.promise();
			},

			loadAppConfigAsync: function() {
				return $.getJSON("/config/app.json");
			},

			clearContent: function() {
				this.contentElement.empty();
			},

			navigateToAsync: function(id) {
				var that = this;
				var deferred = new $.Deferred();

				var screen = new window.app.screens.SpaScreen({ app: this, id: id });

				screen.loadAsync().done(function(args) {
					that.clearContent();

					that.contentElement.append(args.element);

					history.pushState(history.state, id, that.config.ui.screens[id].uri);

					deferred.resolve();
				});

				return deferred.promise();
			},

			onNavigationChange: function(e) {
				debugger;
			}
		};

		return exports;
	})();
})(jQuery, window);