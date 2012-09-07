// Create a Namespace
var prodapp = prodapp || {};

prodapp.viewModel = function () {
    var self = this;

    // The boolean here determines if we are buffering changes (can be overridden on each dataSet)
    self.data = new spa.dataContext(false);

    // The boolean here can override the default for the context
    self.data.AddSet("Values", "ID");

    // Create a View from the DataSet we just created
    self.view = self.data["Values"].CreateView();

    // Somewhere to hold the item we have Selected
    self.selectedItem = ko.observable();

    // function to Add an add function with default values
    self.addItem = function () {
        var newItem = new spa.dataEntity({
            ID: 0,
            Name: "New-Item-Name",
            Price: 0
        });
        self.view.AddItem(newItem);
        self.selectedItem(newItem);
    };

    // function to Delete an item in the view
    self.deleteItem = function (item) {
        self.view.DeleteItem(item);
        if (item == self.selectedItem())
            self.selectedItem(null);
    };

    // function to Refresh our View
    self.refresh = function () {
        self.view.Refresh(true);
    };

    // Start a refresh immeidately on load
    self.view.Refresh(true);
    // Public Return Items
    return {
        Items: self.view.ViewItems,
        SelectedItem: self.selectedItem,
        Refresh: self.refresh,
        AddItem: self.addItem,
        Delete: self.deleteItem,
    }
};

$(document).ready(function () {
    // Bind Once the page is loaded
    ko.applyBindings(new prodapp.viewModel());
});