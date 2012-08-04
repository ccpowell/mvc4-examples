/// <reference path="_references.js" />

window.Facility = function (initialData) {
    var self = this;
    upshot.map(initialData, upshot.type(Facility), self);

    self.OmbudsmanText = ko.computed(function () {
        return self.Ombudsman() ? self.Ombudsman().Name() : "(None)";
    });
}

window.FacilitiesViewModel = function () {
    // Private data
    var self = this;
    var dataSource = upshot.dataSources.Facilities.refresh();
    var ombudsmanOptionsDataSource = upshot.dataSources.OmbudsmanOptionsForFacility.refresh();

    // Public data
    self.facilities = dataSource.getEntities();
    self.ombudsmanOptions = ombudsmanOptionsDataSource.getEntities();
    self.editingFacility = ko.observable();
    self.validationRules = $.extend({}, dataSource.getEntityValidationRules(), {
        submitHandler: function () { self.acceptEdit() },
        resetFormOnChange: self.editingFacility
    });

    // Navigation
    self.nav = new NavHistory({
        params: { editItem: null },
        onNavigate: function (navEntry) {
            dataSource.findById(navEntry.params.editItem, self.editingFacility);
        }
    }).initialize({ linkToUrl: true });

    // Public operations
    self.openEditor = function (item) {
        self.nav.navigate({ editItem: dataSource.getEntityId(item) });
    }

    self.closeEditor = function () {
        self.nav.navigate({ editItem: null });
    }

    self.saveAll = function () {
        dataSource.commitChanges(null, showFirstInvalidItem); // On error, display the affected item
    }

    self.revertAll = function () {
        dataSource.revertChanges();
    }

    self.acceptEdit = function () {
        self.closeEditor();
    }

    self.rejectEdit = function () {
        var currentItem = self.editingFacility();
        dataSource.revertChanges(currentItem);
        self.closeEditor();
    }

    self.createFacility = function () {
        var newItem = new Facility({});
        self.facilities.push(newItem);
        self.openEditor(newItem);
    }

    self.deleteCurrentFacility = function () {
        var currentItem = self.editingFacility();
        dataSource.deleteEntity(currentItem);
        self.closeEditor();
    }

    // Private functions
    function showFirstInvalidItem() {
        var firstError = dataSource.getEntityErrors()[0];
        if (firstError)
            self.openEditor(firstError.entity);
    }
}

