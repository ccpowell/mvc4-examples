dojo.provide('dts.account');

dojo.require('dijit.Dialog');
dojo.require("dijit.form.TextBox");
dojo.require("dijit.form.ValidationTextBox");
dojo.require('dojox.validate.us');
dojo.require("dojox.grid.DataGrid");
dojo.require("dojo.data.ItemFileReadStore");
dojo.require("dojo.data.ItemFileWriteStore");

dojo.declare("dts.account.AccountDetailController", null,
    {
        //===========================
        //Constructor
        //===========================
        constructor: function(params) {
            //Set the private variables
            dojo.mixin(this, params);

            dojo.subscribe('onCurrentAccountChanged', this, 'ConfirmCurrentAccountChanged');
            //In DRCOG we can't create new users - just promote to a Login from a Contact, or demote back to Contact
            //dojo.subscribe('onCreateNewUser', this, 'ConfirmCreateNewUser');

            var onUnloadFunction = dojo.hitch(this, function(evt) { if (this.isDirty) { evt.returnValue = 'This Account has unsaved changes. If you continue, the changes will be lost. Are you sure you want to continue?'; } });
            dojo.connect(window, 'onbeforeunload', onUnloadFunction);

            this.isDirty = false;

            var that = this;
            dojo.addOnLoad(function() {
                dojo.parser.parse('mainPane');
                that.initializeUI(); //need to wait until the dojo stuff is there
            });
            console.log('AccountDetailController Initialized...');
        },

        //===========================
        //Private methods
        //===========================
        _formatPhoneNumber: function(value) {
            return masterController.FormatPhoneNumber(value);
        },

        _validatePhoneNumber: function(value) {
            return (value.length === 0) ? true : dojox.validate.us.isPhoneNumber(value);
        },

        _handleError: function(message) {
            masterController.hideProgressIndicator();
            this.hideProgressIndicator();
            masterController.displayUserMessage(message, 'error');
            console.error(message);
        },

        _formatDeleteGridCell: function() {
            return '<div class="gridRowDeleteCell" />';
        },

        _packUpUser: function() {
            var user = {};

            //FIELDS
            user.Id = dojo.byId('detail_Id').value;
            user.FirstName = dojo.byId('detail_FirstName').value;
            user.LastName = dojo.byId('detail_LastName').value;
            user.Email = dojo.byId('detail_Email').value;
            user.IsActive = dojo.byId('detail_IsActive').checked;

            //ROLES
            user.RoleIds = [];
            //get the rolesGrid
            var rolesGrid = dijit.byId('detail_RolesGrid');
            //push all the roleid's in the store to user.roleIds
            rolesGrid.store.fetch({ onItem: function(item) { user.RoleIds.push(item.RoleId[0]); } });

            return user;
        },

        //===========================
        //Public properties
        //===========================
        canEdit: function() {
            var canEdit = dojo.byId('detail_CanEdit');

            var result = (canEdit && canEdit.value && canEdit.value === 'True') ? true : false;
            return result;
        },

        //===========================
        //UI Methods
        //===========================
        initializeUI: function() {
            //dojo.connect any necessary handlers (what about disconnecting these first above - or maybe that happens when we destroy them
            if (this.isAdmin) {   //if they are an administrator
                //event listeners for action button clicks
                var addRoleButton = dojo.byId('addRoleButton');
                //console.info("Can Edit:" + this.canEdit());
                if (addRoleButton) {
                    dojo.connect(addRoleButton, 'onclick', dojo.hitch(this, this.addRoleButtonClick));
                    //addRoleButton.disabled = false;
                }
                var roledGrid = dijit.byId('detail_RolesGrid');
                if (roledGrid) {
                    dojo.connect(roledGrid, 'onRowClick', dojo.hitch(this, this.rolesGridRowClick));
                }


                //event listeners for control on change
                var isActiveCkBox = dojo.byId('detail_IsActive');
                if (isActiveCkBox) {
                    dojo.connect(isActiveCkBox, 'onclick', dojo.hitch(this, this.valueChanged));
                }
            }

            if (this.canEdit()) {   //if they are an administrator or currentuser == this user
                //event listeners for save button click
                dojo.query('.detailSaveButton').connect('onclick', this, this.save);

                //event listeners for control onchange
                dojo.query('input[type="text"]', 'mainPane').connect('onkeyup', this, this.valueChanged); //gotta be keyup - validation is out of sync if you use keydown or keypress
            }

            var accountId = dojo.byId('detail_AccountId')
            if (this.canEdit() && accountId && accountId.value != 0) {   //if they are an administrator or currentuser == this user AND this is not a new one
                //enable the reset password link
                var resetPwdDlgButton = dijit.byId('resetPasswordButton');
                if (resetPwdDlgButton) {
                    dojo.connect(resetPwdDlgButton, 'onClick', dojo.hitch(this, this.resetPassword));
                }
                var resetPwdDlgCancelBtn = dijit.byId('resetPasswordCancelButton');
                if (resetPwdDlgCancelBtn) {
                    dojo.connect(resetPwdDlgCancelBtn, 'onClick', dojo.hitch(this, this.resetPasswordCancel));
                }
                //make it visible and give the container an id so we can style it with css
                var resetPasswordNode = dijit.byId('resetPasswordLink');
                if (resetPasswordNode) {
                    dojo.style(resetPasswordNode.domNode, 'display', 'inline');
                    dojo.attr(resetPasswordNode.domNode, 'id', 'resetPasswordDropDown');
                }
                //Enable the Add role button
                console.info("Account Id: " + accountId.value)
                addRoleButton.disabled = false;

            }
            else {
                var resetPasswordLink = dijit.byId('resetPasswordLink');
                if (resetPasswordLink) {
                    dojo.style(resetPasswordLink.domNode, 'display', 'none');
                }
            }
        },

        initializeRoleDialog: function() {
            //dojo.connect any necessary handlers
            //event listeners for action button clicks
            var saveBtn = dojo.byId('saveRoleBtn');
            if (saveBtn) {
                dojo.connect(saveBtn, 'onclick', dojo.hitch(this, this.addRole));
            }
            dojo.query('.dialogCancelBtn', 'roleDialog').connect('onclick', function() { dijit.byId('roleDialog').hide() });
            dojo.removeAttr('saveRoleBtn', 'disabled');
        },

        initializeOrgDialog: function() {
            //dojo.connect any necessary handlers
            //event listeners for action button clicks
            var saveBtn = dojo.byId('saveOrgBtn');
            if (saveBtn) {
                dojo.connect(saveBtn, 'onclick', dojo.hitch(this, this.addOrg));
            }
            dojo.connect(dojo.byId('orgDialog_Org'), 'onchange', dojo.hitch(this, this.organizationChanged));
            dojo.query('.dialogCancelBtn', 'orgDialog').connect('onclick', function() { dijit.byId('orgDialog').hide() });
            dojo.removeAttr('saveOrgBtn', 'disabled');
            var orgsSelect = dojo.byId('orgDialog_Org');
            this.getBranchOfficesForOrg(orgsSelect.options[orgsSelect.selectedIndex].value)
        },

        //Progress indicator that hides mainPane to avoid the flash of unstyled content when we replace the html and run the dojo parser
        showProgressIndicator: function() {
            //console.info('in show...');
            //first remove it if it's there so we don't create a duplicate
            var loader = dojo.byId('userDetailsLoading');
            if (loader) { dojo.body().removeChild(loader); }

            //create it on the fly and append it to main because we are about to replace the innerhtml of the mainPane
            loader = document.createElement('div');
            dojo.attr(loader, 'id', 'userDetailsLoading');
            dojo.body().appendChild(loader);

            var main = dojo.byId('mainPane');

            if (!loader || !main) { return; }

            var coords = dojo.coords(main);
            var w = coords.w - 2;
            var h = 945; //(coords.h < dijit.getViewport().h) ? dijit.getViewport().h : coords.h; //when it's the first one, mainPane may only be a few pixels tall
            var t = (dojo.isIE) ? coords.y : coords.t;
            var l = (dojo.isIE) ? coords.x : coords.l;

            //console.dir(coords);

            dojo.style(loader, { width: w + 'px', height: h + 'px', top: t + 'px', left: l + 'px' });
        },

        hideProgressIndicator: function() {
            //get the loader
            var loader = dojo.byId('userDetailsLoading');

            if (loader) {
                //fade it out
                dojo.fadeOut({ node: loader, duration: 250,
                    onEnd: function() {
                        //remove the node from the dom
                        if (loader) { dojo.body().removeChild(loader); }
                    }
                }).play();
            }
        },

        resetPasswordCancel: function() {
            dijit.byId('resetPwdDlg').onCancel();
        },

        //===========================
        //Event handlers/ callbacks
        //===========================
        ConfirmCurrentAccountChanged: function(eventData) {
            if (this.isDirty) {
                //You have unsaved changes, these changes will be lost unless you save first do you want to...
                var confirmFunction = dojo.hitch(this, this.HandleCurrentAccountChanged, eventData);
                masterController.showConfirmDialog('Unsaved changes', 'This user has unsaved changes. If you continue, the changes will be lost. Are you sure you want to continue?', confirmFunction);
            }
            else {
                this.HandleCurrentAccountChanged(eventData);
            }
        },

        HandleCurrentAccountChanged: function(eventData) {
            this.isDirty = false;

            this.showProgressIndicator();

            eventData.f = 'partial';

            dojo.xhrGet(
            {
                url: this.accountDetailUrl,
                content: eventData,
                load: dojo.hitch(this, this.accountDetailCallback),
                error: dojo.hitch(this, this.accountDetailCallbackError),
                handleAs: 'text'
            });
        },

        //        ConfirmCreateNewUser: function()
        //        {
        //            if (this.isDirty)
        //            {
        //                //You have unsaved changes, these changes will be lost unless you save first do you want to...
        //                var confirmFunction = dojo.hitch(this, this.HandleCreateNewUser);
        //                masterController.showConfirmDialog('Unsaved changes', 'This user has unsaved changes. If you continue, the changes will be lost. Are you sure you want to continue?', confirmFunction);
        //            }
        //            else
        //            {
        //                this.HandleCreateNewUser();
        //            }
        //        },

        //        HandleCreateNewUser: function()
        //        {
        //            this.isDirty = false;

        //            this.showProgressIndicator();

        //            var content = {};
        //            content.f = 'partial';

        //            dojo.xhrGet(
        //            {
        //                url: this.newCustomerUrl,
        //                content: content,
        //                load: dojo.hitch(this, this.accountDetailCallback),
        //                error: dojo.hitch(this, this.accountDetailCallbackError),
        //                handleAs: 'text'
        //            });
        //        },

        accountDetailCallback: function(response, ioArgs) {
            //Check for errors
            if (response.Error && response.Error !== '') { this._handleError(response.Error); return response; }

            try {
                //Destroy any widgets whose dom elements we're about to get rid of
                masterController.destroyChildWidgets('mainPane');

                //Load the data into the ui
                dojo.byId('mainPane').innerHTML = response;
                //console.info(response);

                //parse the mainPane to instantiate any dijits we just injected
                dojo.parser.parse(dojo.byId('mainPane'));

                this.initializeUI();
            }
            catch (error) {
                this._handleError('An error ocurred displaying user details.')
            }

            //masterController.hideProgressIndicator();
            this.hideProgressIndicator();

            //Always return the response
            return response;
        },

        accountDetailCallbackError: function(error) {
            this._handleError('An error ocurred getting user details.')

            //Always return the response
            return error;
        },

        resetPassword: function() {
            masterController.showProgressIndicator();

            dojo.xhrGet(
            {
                url: this.resetPasswordUrl,
                load: dojo.hitch(this, this.resetPasswordCallback),
                error: dojo.hitch(this, this.resetPasswordCallbackError),
                handleAs: 'json'
            });

            dijit.byId('resetPwdDlg').onCancel();
        },

        resetPasswordCallback: function(response) {
            //Check for errors
            if (response.Error && response.Error !== '') { this._handleError(response.Error); return response; }

            try {
                masterController.displayUserMessage('The password has been reset. The new password has been emailed to ' + response.Data.email + '.', 'info');
            }
            catch (error) {
                this._handleError('An error ocurred resetting the password.')
            }

            masterController.hideProgressIndicator();

            //Always return the response
            return response;
        },

        resetPasswordCallbackError: function(err) {
            this._handleError('An error ocurred resetting the password.')

            //Always return the response
            return err;
        },

        rolesGridRowClick: function(evt) {
            if (evt && evt.cell && evt.cell.name && evt.cell.name === 'Delete' && evt.grid) {
                evt.grid.store.deleteItem(evt.grid.getItem(evt.rowIndex));

                this.valueChanged();
            }
        },

        addRoleButtonClick: function(evt) {
            this.showAddRoleDialog();
        },

        showAddRoleDialog: function() {
            //first check if it's there so we don't create a duplicate
            var dialog = dijit.byId('roleDialog');
            if (dialog) { dialog.destroyRecursive(); } //easier to just destroy it - not sure how to reset the title

            dialog = new dijit.Dialog({
                refreshOnShow: true,
                id: 'roleDialog',
                title: 'Add Role',
                onDownloadEnd: dojo.hitch(this, this.initializeRoleDialog),
                onDownloadError: dojo.hitch(masterController, masterController._dialogDownloadError, 'roleDialog')
            });

            dialog.setHref(this.roleDialogUrl);

            dialog.show();
        },

        addRole: function() {
            try {
                //disable save button to prevent attempting to add duplicates if user dbl clicks on save button
                dojo.attr(dojo.byId('saveRoleBtn'), 'disabled', 'disabled');

                var roleSelect = dojo.byId('roleDialog_Role');

                //create a new item
                var grid = dijit.byId('detail_RolesGrid');

                //check if it already exists in the store
                var exists;
                grid.store.fetch({ onComplete: function(items) {
                    exists = dojo.some(items, function(item) { return item.roleId[0] == roleSelect.options[roleSelect.selectedIndex].value; });
                }
                });
                //If it already exists in the store...
                if (exists) {
                    masterController.displayUserMessage('That role is already assigned to this user.', 'warning');
                    return;
                }

                grid.store.newItem({ roleId: roleSelect.options[roleSelect.selectedIndex].value, name: roleSelect.options[roleSelect.selectedIndex].text });

                this.valueChanged();

                dijit.byId('roleDialog').hide();
            }
            catch (error) {
                this._handleError('An error ocurred displaying the add role dialog.')
                console.error(error.message);
            }
        },

        orgsGridRowClick: function(evt) {
            if (evt && evt.cell && evt.cell.name && evt.cell.name === 'Delete' && evt.grid) {
                evt.grid.store.deleteItem(evt.grid.getItem(evt.rowIndex));

                this.valueChanged();
            }
        },

        addOrgButtonClick: function(evt) {
            this.showAddOrgDialog();
        },

        showAddOrgDialog: function() {
            //first check if it's there so we don't create a duplicate
            var dialog = dijit.byId('orgDialog');
            if (dialog) { dialog.destroyRecursive(); } //easier to just destroy it - not sure how to reset the title

            dialog = new dijit.Dialog({
                refreshOnShow: true,
                id: 'orgDialog',
                title: 'Add Organization',
                onDownloadEnd: dojo.hitch(this, this.initializeOrgDialog),
                onDownloadError: dojo.hitch(masterController, masterController._dialogDownloadError, 'orgDialog')
            });

            dialog.setHref(this.orgDialogUrl);

            dialog.show();
        },

        addOrg: function() {
            try {
                //disable save button to prevent attempting to add duplicates if user dbl clicks on save button
                dojo.attr(dojo.byId('saveOrgBtn'), 'disabled', 'disabled');

                var orgSelect = dojo.byId('orgDialog_Org');
                var branchSelect = dojo.byId('orgDialog_Branch');

                //create a new item
                var grid = dijit.byId('detail_OrgsGrid');

                //Check if it already exists in the store
                var exists;
                grid.store.fetch({ onComplete: function(items) {
                    exists = dojo.some(items, function(item) { return item.OrganizationId[0] == orgSelect.options[orgSelect.selectedIndex].value; });
                }
                });
                //If it already exists in the store...
                if (exists) {
                    masterController.displayUserMessage('That organization is already assigned to this user.', 'warning');
                    return;
                }

                if (branchSelect.selectedIndex === -1) {
                    grid.store.newItem({ OrganizationId: orgSelect.options[orgSelect.selectedIndex].value, OrganizationName: orgSelect.options[orgSelect.selectedIndex].text, BranchOfficeId: 0, BranchName: null });
                }
                else {
                    grid.store.newItem({ OrganizationId: orgSelect.options[orgSelect.selectedIndex].value, OrganizationName: orgSelect.options[orgSelect.selectedIndex].text, BranchOfficeId: branchSelect.options[branchSelect.selectedIndex].value, BranchName: branchSelect.options[branchSelect.selectedIndex].text });
                }

                this.valueChanged();

                dijit.byId('orgDialog').hide();
            }
            catch (error) {
                this._handleError('An error ocurred displaying the add role dialog.')
                console.error(error.message);
            }
        },

        organizationChanged: function(evt) {
            this.getBranchOfficesForOrg(evt.target.value);
        },

        getBranchOfficesForOrg: function(orgId) {
            //TODO: need a progress indicator for the dialog

            dojo.xhrGet(
            {
                url: this.getBranchOfficesUrl,
                content: { orgId: orgId },
                load: dojo.hitch(this, this.getBranchOfficesForOrgCallback),
                error: dojo.hitch(this, this.getBranchOfficesForOrgCallbackError),
                handleAs: 'json'
            });
        },

        getBranchOfficesForOrgCallback: function(response) {
            //Check for errors
            if (response.Error && response.Error !== '') { this._handleError(response.Error); return response; }

            var select = dojo.byId('orgDialog_Branch');
            for (i = select.length - 1; i >= 0; i--) {
                select.remove(i);
            }

            dojo.forEach(response.Data, function(item) {
                var opt = document.createElement('option');
                opt.text = item.BranchName;
                opt.value = item.BranchOfficeId;
                try {
                    select.add(opt, null);
                }
                catch (err) {
                    select.add(opt);
                }
            });

            return response;
        },

        getBranchOfficesForOrgCallbackError: function(error) {
            return error;
        },

        valueChanged: function(evt) {
            this.isDirty = true;
            try {
                //check if everything is valid
                var valid = dojo.query('[widgetid], [widgetId]', 'mainPane').widgets().every(function(item) {
                    if (item.isValid) { return item.isValid(); }
                    //if it is not a widget or does not have an isValid method, return true
                    return true;
                });

                //also check if the user is assigned to at least one role and org
                if (valid) {
                    var rolesGrid = dijit.byId('detail_RolesGrid');
                    if (rolesGrid) {
                        valid = rolesGrid.rowCount > 0;
                    }
                }
                if (valid) {
                    var orgsGrid = dijit.byId('detail_OrgsGrid');
                    if (orgsGrid) {
                        valid = orgsGrid.rowCount > 0;
                    }
                }

                if (valid) {
                    //enable the save buttons by removing the disabled attribute
                    dojo.removeAttr('detail_SaveButton', 'disabled');
                    dojo.removeAttr('detail_SaveButton2', 'disabled');
                }
                else {
                    //disable the save buttons
                    dojo.attr('detail_SaveButton', 'disabled', 'disabled');
                    dojo.attr('detail_SaveButton2', 'disabled', 'disabled');
                }
            }
            catch (error) {
                //swallow it
            }
        },

        save: function(evt) {
            this.isDirty = false;

            //disable the save buttons
            dojo.attr('detail_SaveButton', 'disabled', 'disabled');
            dojo.attr('detail_SaveButton2', 'disabled', 'disabled');

            //show the progress indicator
            masterController.showProgressIndicator();

            try {
                //Create the user object
                var user = this._packUpUser();

                dojo.xhrPost(
                {
                    url: this.saveUserUrl,
                    content: { custJson: dojo.toJson(user) },
                    load: dojo.hitch(this, this.saveCallback),
                    error: dojo.hitch(this, this.saveCallbackError),
                    handleAs: 'json'
                });
            }
            catch (err) {
                this._handleError('An error ocurred saving user details.');
            }
        },

        saveCallback: function(response) {
            //Check for errors
            if (response.Error && response.Error !== '') { this._handleError(response.Error); return response; }

            masterController.hideProgressIndicator();
            masterController.displayUserMessage('User saved!', 'info');

            //Always return the response
            return response;
        },

        saveCallbackError: function(error) {
            this._handleError('An error ocurred saving the user.')

            //Always return the response
            return error;
        }

    });
