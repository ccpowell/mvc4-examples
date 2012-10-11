dojo.provide('dts.account.AccountSearchController');

dojo.require('dijit.Dialog');
dojo.require('dojo.data.ItemFileReadStore');
dojo.require('dojox.grid.DataGrid');

dojo.declare("dts.account.AccountSearchController", null,
    {
        //===========================
        //Constructor
        //===========================
        constructor: function(params) {
            //Set the private variables
            dojo.mixin(this, params);

            //event handlers to disable the paging controls when any of the search fields are changed
            dojo.connect(dojo.byId('search_status'), 'onchange', dojo.hitch(this, this.disablePagingControls));
            dojo.connect(dojo.byId('search_FirstName'), 'onchange', dojo.hitch(this, this.disablePagingControls));
            dojo.connect(dojo.byId('search_LastName'), 'onchange', dojo.hitch(this, this.disablePagingControls));
            dojo.connect(dojo.byId('search_email'), 'onchange', dojo.hitch(this, this.disablePagingControls));

            //event handlers for search and paging buttons
            dojo.connect(dojo.byId('searchButton'), 'onclick', dojo.hitch(this, this.searchButtonClick));
            dojo.connect(dojo.byId('previousPageButton'), 'onclick', dojo.hitch(this, this.pagingButtonClick));
            dojo.connect(dojo.byId('nextPageButton'), 'onclick', dojo.hitch(this, this.pagingButtonClick));

            //Hide the spinner
            masterController.hideProgressIndicator();
            console.log('AccountSearchController Initialized...');
        },

        //===========================
        //Public properties
        //===========================


        //===========================
        //UI Methods
        //===========================
        accountSearch: function(page) {

            masterController.showProgressIndicator();

            var requestedPage = {};
            requestedPage.page = (!page) ? 1 : page;

            var form = dojo.byId('accountSearchForm');

            dojo.xhrGet(
                {
                    url: this.accountSearchUrl,
                    content: requestedPage,
                    form: form,
                    load: dojo.hitch(this, this.accountSearchCallback),
                    error: dojo.hitch(this, this.accountSearchCallbackError),
                    handleAs: 'json'
                });
        },

        createButtonClick: function() {
            dojo.publish('onCreateNewUser');
        },

        searchButtonClick: function() {

            this.accountSearch(1);
        },

        pagingButtonClick: function(evt) {
            //If we don't have the expected object, return
            if (!evt || !evt.target) { return; }

            //If paging info is not set, return
            if (!this._currentPage || !this._totalPages) { return; }

            //Figure out the desired page
            var newPage = (evt.target.id === 'nextPageButton') ? this._currentPage + 1 : this._currentPage - 1;

            //Check if we can go in the requested direction
            if (newPage < 1 || newPage > this._totalPages) { return; }

            //Call accountSearch, passing the desired page
            this.accountSearch(newPage);
        },

        //Called when a row in the results grid id clicked
        searchGridRowClick: function(event) {
            var grid = dijit.byId('searchResultsGrid');
            var content = {};
            content.AccountId = grid.store.getValue(grid.getItem(event.rowIndex), 'AccountId');

            dojo.publish('onCurrentAccountChanged', [content]);
        },

        updatePagingControls: function(currentPage, totalPages) {
            if (totalPages > 1) {
                //set state of the prev/ next buttons toggle the css class and set/ remove an event handler
                var prev = dojo.byId('previousPageButton');
                var action = (currentPage === 1) ? 'removeClass' : 'addClass';
                dojo[action](prev, 'active');

                action = (currentPage >= totalPages) ? 'removeClass' : 'addClass';
                var next = dojo.byId('nextPageButton');
                dojo[action](next, 'active');

                //set the state of the pageStatus
                dojo.byId('pageStatus').innerHTML = ' &nbsp --&nbsp page ' + currentPage + ' of ' + totalPages + '&nbsp -- &nbsp ';

                //display the paging controls
                dojo.style('pagingControls', 'display', 'block');
            }
            else {
                //hide the paging controls
                dojo.style('pagingControls', 'display', 'none');
            }
        },

        disablePagingControls: function() {
            //make them visibly disabled
            dojo.removeClass(dojo.byId('previousPageButton'), 'active');
            dojo.removeClass(dojo.byId('nextPageButton'), 'active');

            //make sure they don't do anything
            this._currentPage = null;
            this._totalPages = null;
        },

        //===========================
        //Event handlers/ callbacks
        //===========================
        accountSearchCallback: function(response, ioArgs) {
            //clear the results
            var grid = dijit.byId('searchResultsGrid');
            if (grid) { grid.setStore(null); }

            //Check for errors
            if (response.Error && response.Error !== '') {
                if (response.Error === 'No users matched the search criteria.') { masterController.displayUserMessage(response.Error, 'warning'); }
                else { masterController.displayUserMessage(response.Error, 'error'); }
                masterController.hideProgressIndicator();
                this.updatePagingControls(0, 0);
                return response;
            }

            try {
                //handle the paging controls
                if (response.Data.TotalPages && response.Data.CurrentPage) {
                    this.updatePagingControls(response.Data.CurrentPage, response.Data.TotalPages);
                    //Need to keep track of currentpage and totalpages as members of this class
                    this._currentPage = response.Data.CurrentPage;
                    this._totalPages = response.Data.TotalPages;
                }

                //Load the data into the ui
                //Create the itemfilereadstore
                var storeData = {};
                //storeData.identifier = 'AccountId';
                storeData.items = dojo.map(dojo.clone(response.Data.Accounts), function(item) {   //itemfilereadstore seems to need a name property on each item even if you set the label property of storeJSON
                    if (!item.name) item.name = item.LastName + ', ' + item.FirstName;
                    return item;
                });

                var store = new dojo.data.ItemFileReadStore({ data: storeData });                
                if (grid) {
                    grid.setStore(store);
                }
                else {
                    var layout = [[
                  { field: 'name', name: 'name', width: 'auto' },
                  { field: 'edit', formatter: function() { return '<div class="userSearchResults"></div>'; }, width: '52px', rowSpan: 2 }
                  ], [
                  { field: 'Agency', name: 'agency', width: 'auto' }
                  ]];
                  
                    grid = new dojox.grid.DataGrid({
                        store: store,
                        structure: layout,
                        autoHeight: 10
                    }, 'searchResultsGrid');

                    dojo.connect(grid, 'onRowClick', dojo.hitch(accountSearchController, accountSearchController.searchGridRowClick));
                }

                //Render the grid
                grid.startup();

                masterController.hideProgressIndicator();
            }
            catch (error) {
                masterController.hideProgressIndicator();
                masterController.displayUserMessage('An error ocurred displaying user search results.', 'error');
            }

            //Always return the response
            return response;
        },

        accountSearchCallbackError: function(error) {
            masterController.hideProgressIndicator();
            masterController.displayUserMessage('An error ocurred searching for users.', 'error');

            //Always return the response
            return error;
        }

    });
