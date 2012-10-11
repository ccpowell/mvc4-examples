dojo.provide('dts.master.MasterController');

dojo.require('dijit.Toolbar');
dojo.require('dijit.Menu');
dojo.require('dijit.form.Button');
dojo.require('dojox.widget.Toaster');

dojo.declare("dts.master.MasterController", null,
    {
        //===========================
        //Constructor
        //===========================
        constructor: function(params) {
            //Set the private variables
            dojo.mixin(this, params);

            //extend nodelist to have a removeAttr function
            dojo.extend(dojo.NodeList, {
                removeAttr: function(attribute) {
                    return this.forEach(function(item) { dojo.removeAttr(item, attribute); });
                }
            });

            //extend nodelist to have a widgets function that filters the list and returns a nodelist of dijit widgets
            dojo.extend(dojo.NodeList, {
                widgets: function() {
                    return this.filter(function(item) {
                        var widget = dijit.byNode(item);
                        if (widget) { return true; }
                    }).map(function(widget) { return dijit.byNode(widget); });
                }
            });
            console.log('MasterController Initialized...');
        },

        //===========================
        //Public methods
        //===========================



        //===========================
        //UI Methods
        //===========================




        hideFormLoader: function() {
            //get the loader
            var dijitContainer = dojo.byId('dijitContainer');
            dojo.style(dijitContainer, { visibility: 'visible', height: 'auto' });
            var loader = dojo.byId('loader');
            //
            if (loader) {
                //fade it out
                dojo.fadeOut({ node: loader, duration: 150,
                    onEnd: function() {
                        //remove the node from the dom
                        if (loader) { dojo.style(loader, { display: 'none' }); }
                    }
                }).play();
            }
        },


        showConfirmDialog: function(title, message, confirmFunction) {// confirmationFunction needs to be submitted in quotes like so 'functionName()'
            //first check if it's there so we don't create a duplicate
            var dialogId = 'confirmDialog';
            var dialog = dijit.byId(dialogId);
            if (dialog) {   //it already exists, reset a few properties
                dialog.setTitle(title);
                dialog.href = this.confirmDialogUrl;
                dialog.onDownloadEnd = dojo.hitch(this, this.showConfirmDialogDownloadEnd, dialogId, message, confirmFunction);
            }
            else {   //it doesn't exist yet, so create it
                dialog = new dijit.Dialog({
                    refreshOnShow: true,
                    href: this.confirmDialogUrl,
                    id: dialogId,
                    title: title,
                    onDownloadEnd: dojo.hitch(this, this.showConfirmDialogDownloadEnd, dialogId, message, confirmFunction)
                });
            }

            //show it    
            dialog.show();
        },

        showConfirmDialogDownloadEnd: function(dialogId, message, confirmFunction) {
            //cancel button
            var cancelFunc = dojo.hitch(this, function() { dijit.byId(dialogId).hide(); });
            dojo.query('.dialogCancelBtn', dialogId).connect('onclick', cancelFunc);

            //message
            dojo.byId('confirmMessageDiv').innerHTML = message;

            //ok button
            var clickFunc = function() { confirmFunction(); cancelFunc(); };

            dojo.query('#okConfirmBtn', dialogId).removeAttr('disabled').connect('onclick', clickFunc);
        },

        displayUserMessage: function(message, level/*info warning error*/) {
            if (!message || message == '') { return; }

            //position it - this isn't working... not sure why...
            //            var containerCoords = dojo.coords(dojo.byId('main'));
            //            var toaster = dijit.byId('errorToaster');
            //            //dojo.style(toaster, { top: containerCoords.t + 'px', left: (containerCoords.l + containerCoords.w) - dojo.coords(toaster).w + 'px' });
            //            dojo.style(toaster, 'top', containerCoords.t + 'px');
            alert('NO TOASTER: ' + message);            
            },

            //===========================
            //Dojo stuff
            //===========================
            //destroys all widgets in domNode
            destroyChildWidgets: function(domNode /*string || domNode*/) {
                dojo.query('[widgetid], [widgetId]', domNode).forEach(function(item) {   //not using NodeList.widgets here because it causes an error under some circumstances
                    var widget = dijit.byNode(item);
                    if (widget) { widget.destroyRecursive(); }
                });
            },

            

            //===========================
            //Event handlers/ callbacks
            //===========================
            _dialogDownloadError: function(dialogId, err) {
                dijit.byId(dialogId).hide();

                //if (err.status == 401)
                if (err.responseText.indexOf('Invalid session') > -1) {   //session expired                    
                    this.displayUserMessage('Session has expired. Redirecting to login page.', 'error');
                    this.RedirectToLoginPage();
                }
                else {   //something unexpected happened...
                    this.displayUserMessage('An unexpected error ocurred downloading dialog content.', 'error');
                    return null;
                }
            },

            callbackError: function(error, ioArgs) {
                masterController.displayUserMessage('An error ocurred getting menu schema.', 'error');

                return error;
            }
        });
