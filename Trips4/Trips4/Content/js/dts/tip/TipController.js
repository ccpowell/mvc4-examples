dojo.provide('dts.tip.TipController');
dojo.require('dijit.Dialog');
dojo.require('dojo.data.ItemFileReadStore');
dojo.require('dts.widgets.DateTextBox');
dojo.require('dijit.form.Textarea');
dojo.require('dojox.form.DropDownSelect');
dojo.require('dijit.form.MultiSelect');
dojo.require('dijit.form.ComboBox');
dojo.require('dijit.form.Button');
dojo.require('dijit.form.Button');
dojo.require('dojox.grid.DataGrid');
dojo.require('dojo.data.ItemFileWriteStore');

dojo.declare("dts.tip.TipController", null,
    {
        //===========================
        //Constructor
        //===========================
        constructor: function(params/**/) {
            //Set the private variables
            dojo.mixin(this, params);

            //Hide the spinner
            masterController.hideFormLoader();

            //event handlers for dashboard Project filter
            dojo.connect(dojo.byId('dashboard_projectFilter'), 'onchange', dojo.hitch(this, this.loadDashboardList));

            var initializeUIDelegate = dojo.hitch(this, this.initializeUI);
            dojo.addOnLoad(function() {
                initializeUIDelegate(); //need to wait until the dojo stuff is there
            });
        },

        //===========================
        //Public properties
        //===========================


        //===========================
        //UI Methods
        //===========================
        initializeUI: function() {
            //unhide the loader
            console.log('Starting TipController.initializeUI...');

            //hook up the grid row click - they will all use the same handler

            var gridRowClickDelegate = dojo.hitch(this, this.gridRowClick);
            var gridRowDblClickDelegate = dojo.hitch(this, this.gridRowDblClick);
            var gridCellMouseOverDelegate = dojo.hitch(this, this.showGridToolTip);
            var gridCellMouseOutDelegate = dojo.hitch(this, this.hideGridToolTip);

            dojo.query('.dojoxGrid').widgets()
                .connect('onRowClick', gridRowClickDelegate)
                .connect('onRowDblClick', gridRowDblClickDelegate)
                .connect('onCellMouseOver', gridCellMouseOverDelegate)
                .connect('onCellMouseOut', gridCellMouseOutDelegate)
                .forEach(function(widget) {
                    widget.canSort = function(col) { if (Math.abs(col) === 1) { return false; } else { return true; } }; //disable sorting on details/delete columns                                            
                }, this);
        },
        //===========================
        //Event handlers/ callbacks
        //===========================
        gridRowClick: function(evt) {

            //if it's the details button...
            if (evt && evt.cell && evt.cell.name
                    && evt.cell.name === 'Details'
                    && evt.grid && evt.grid.store) {
                var item = evt.grid.getItem(evt.rowIndex);
                window.location = this.projectBaseUrl + '/' + item.ProjectId;
            }
        },

        gridRowDblClick: function(evt) {


        },

        //-----------------------------------------
        //xhr to load a table from server
        //-----------------------------------------
        loadDashboardList: function() {
            var filterValue = dojo.byId('dashboard_projectFilter').value;

            var content = {};
            content.f = 'partial';
            content.tipid = this.currentTipId;
            content.page = 1;
            content.listtype = filterValue;
            dojo.xhrGet(
            {
                url: this.dashboardListUrl,
                content: content,
                load: dojo.hitch(this, this.dashboardListCallback),
                error: dojo.hitch(this, this.dashboardListCallbackError),
                handleAs: 'text'
            });
        },


        dashboardListCallback: function(response, ioArgs)
        {
            if (response.Error && response.Error !== '') { this._handleError(response.Error); return response; }

            try
            {
                //Destroy any widgets whose dom elements we're about to get rid of
                masterController.destroyChildWidgets('tipDashboardListContainer');

                //Load the data into the ui
                dojo.byId('tipDashboardListContainer').innerHTML = response;
            }
            catch (error) 
            {
                this._handleError('An error ocurred displaying project details.')
            }
            //Always return the response
            return response;
        },

        showGridToolTip: function(evt) {
            var msg = '';
            if (evt && evt.cell && evt.cell.name && evt.grid && evt.grid.store && evt.grid.store.declaredClass === 'dojo.data.ItemFileWriteStore') {
                switch (evt.cell.name) {
                    case 'Details':
                        msg = 'View Details';
                        break;
                    case 'Move Up':
                        msg = 'Move up';
                        break;
                    case 'Move Down':
                        msg = 'Move down';
                        break;
                    default:
                        //msg = 'Double click to edit';
                        //break;
                        this.hideGridToolTip(evt);
                        return;
                }
                dijit.showTooltip(msg, evt.cellNode);
            }
        },

        hideGridToolTip: function(evt) {
            dijit.hideTooltip(evt.cellNode);
            // FIXME: make sure that pesky tooltip doesn't reappear!
            // would be nice if there were a way to hide tooltip without regard to aroundNode.
            //dijit._masterTT._onDeck=null;
        },



        //===========================
        //Private methods
        //===========================
        _handleError: function(message)
        {            
            masterController.displayUserMessage(message, 'error');
        }

        //Wireup the jump to project details from the grid        

    });
