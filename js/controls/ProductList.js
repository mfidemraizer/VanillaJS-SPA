(function(app, dust, $) {
	if(!app.hasOwnProperty("controls"))
		app.controls = {};

	app.controls.ProductList = function(args) {
		app.controls.SpaControl.call(this, args);

		this._viewModel = new app.controls.ProductListViewModel({ control: this });
	};

	app.controls.ProductList.prototype = new app.controls.SpaControl();

	Object.defineProperties(app.controls.ProductList.prototype, {
		viewModel: { 
			get: function() { 
				return this._viewModel;
			}	
		}
	});

	app.controls.ProductListViewModel = function(args) {
		if(typeof args != "object") 
			throw Error("This constructor requires arguments");

		if(!args.hasOwnProperty("control")) 
			throw Error("'control' argument is mandatory");

		this._control = args.control;

		this.someText = ko.observable("hola");
		this.items = ko.observableArray(["uno", "dos", "tres"]);
	};

	app.controls.ProductListViewModel.prototype = {
		get control() {
			return this._control;
		}
	};
})(app, dust, jQuery);