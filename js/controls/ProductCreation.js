(function (app, dust, $) {
    if (!app.hasOwnProperty("controls"))
        app.controls = {};

    app.controls.ProductCreation = function (args) {
        app.controls.SpaControl.call(this, args);

        this._viewModel = new app.controls.ProductCreationViewModel({ control: this });
    };

    app.controls.ProductCreation.prototype = new app.controls.SpaControl();

    Object.defineProperties(app.controls.ProductCreation.prototype, {
        viewModel: {
            get: function () {
                return this._viewModel;
            }
        }
    });

    app.controls.ProductCreationViewModel = function (args) {
        if (typeof args != "object")
            throw Error("This constructor requires arguments");

        if (!args.hasOwnProperty("control"))
            throw Error("'control' argument is mandatory");

        this._control = args.control;

        this.productName = ko.observable("");
    };

    app.controls.ProductCreationViewModel.prototype = {
        get control() {
            return this._control;
        },

        createProductAsync: function () {
            var that = this;

            return $.ajax({
                url:"/v1/products",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ name: this.productName() })
            }).done(function() {
                that.control.screen.app.navigateToAsync("home");
            });
        }
    };
})(app, dust, jQuery);