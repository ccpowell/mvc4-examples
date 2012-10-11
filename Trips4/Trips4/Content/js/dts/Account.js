dojo.provide('dts.account');

dojo.require('dijit.Dialog');
dojo.require('dojo.data.ItemFileReadStore');

dojo.declare("dts.account.AccountController", null,
    {
        //===========================
        //Constructor
        //===========================
        constructor: function(params/*isAuthenticated, menuSchemaJSON, authenticateUserUrl*/)
        {
            //Set the private variables
            dojo.mixin(this, params);

            //Hide the spinner
            masterController.hideProgressIndicator();
        }

        //===========================
        //Public properties
        //===========================


        //===========================
        //UI Methods
        //===========================

        //===========================
        //Event handlers/ callbacks
        //===========================

    });
