/// <reference path="_references.js" />

window.Ombudsman = function (initialData) {
    var self = this;
    upshot.map(initialData, upshot.type(Ombudsman), self);
}

window.OmbudsmenViewModel = function () {
    // Private data
    var self = this;
    var dataSource = upshot.dataSources.Ombudsmen.refresh();

    // Public data
    self.ombudsmen = dataSource.getEntities();
    self.editingOmbudsman = ko.observable();
    self.validationRules = $.extend({}, dataSource.getEntityValidationRules(), {
        submitHandler: function () { self.acceptEdit() },
        resetFormOnChange: self.editingOmbudsman
    });

    // Navigation
    self.nav = new NavHistory({
        params: { editItem: null },
        onNavigate: function (navEntry) {
            dataSource.findById(navEntry.params.editItem, self.editingOmbudsman);
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
        var currentItem = self.editingOmbudsman();
        dataSource.revertChanges(currentItem);
        self.closeEditor();
    }

    self.createOmbudsman = function () {
        var newItem = new Ombudsman({});
        self.ombudsmen.push(newItem);
        self.openEditor(newItem);
    }

    self.deleteCurrentOmbudsman = function () {
        var currentItem = self.editingOmbudsman();
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

