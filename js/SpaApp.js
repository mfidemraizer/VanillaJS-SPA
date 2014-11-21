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

					that.initializeMasterViewLinks();

					that.navigateToAsync(that.config.ui.defaultScreenId).done(function() {
						deferred.resolve();

						that.listenRealTimeNotifications();
					});
				});

				return deferred.promise();
			},

			initializeMasterViewLinks: function() {
				var links = $("body > header > nav a");
				var that = this;

				links.each(function(index, element) {
					element = $(element);

					element.on("click", { viewId: element.attr("href").replace("#", "") }, function(e) {
						that.navigateToAsync(e.data.viewId);
					});

					element.removeAttr("href");
				});
			},

			listenRealTimeNotifications: function() {
				$.connection.hub.url = "http://localhost:9555/signalr";
				 var productHub = $.connection.productHub;
            
	            productHub.client.productAdded = function (name) {
	            	alert("There's a new product called '" + name + "'!");
	            };

	            $.connection.hub.start().done(function () {
                	productHub.server.notifyProductCreated("test");
	            });
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

					// History API
					history.pushState(history.state, id, that.config.ui.screens[id].uri);

					deferred.resolve();
				});

				return deferred.promise();
			},

			onNavigationChange: function(e) {
				// Do some stuff when user or code goes back or forward on current site's history
			}
		};

		return exports;
	})();
})(jQuery, window);