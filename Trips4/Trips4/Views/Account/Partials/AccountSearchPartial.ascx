<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AccountSearchModel>" %>


<div id="searchPane">
    <form id="accountSearchForm">
        <div class="panelTitle">
        <h2 class="searchTitle">Search for Accounts</h2>
        </div>
        <fieldset>
            <label class="label_beside" for="search_AccountType">Type</label>
            <select id="search_status" name="search_status" >
                <option selected value=true>Active</option>
                <option value=false>In-Active</option>
            </select>
            <br />
            <label class="label_top" for="search_FirstName">First Name:</label>
            <input id="search_FirstName" name="search_FirstName" />
            <br />
            
            <label class="label_top" for="search_LastName">Last Name:</label>
            <input id="search_LastName" name="search_LastName" />
            <br />
            
            <label class="label_top" for="search_email">Email Address:</label>
            <input id="search_email" name="search_email" />
            <br />
            <p><strong>Note:</strong>Wild cards are automatically applied.</p>
        </fieldset>
        <div class="formControls">
            <%= Html.Button("searchButton", "Search", true, new { @class = "actionButton", type = "button" })%>            
        </div>
    </form>
</div>
<div id="searchResultsPane">
    <div class="panelTitle">
        <h2 class="resultsTitle">Search Results</h2>
    </div>
    <div id="searchResults">
        <div id="pagingControls">
            <span id="previousPageButton" class="pagingButton">previous</span><span id="pageStatus"></span><span id="nextPageButton" class="pagingButton">next</span>
        </div>
        <div id="searchResultsGrid"><p>Enter Search Criteria and click Search.</p></div>
    </div>
</div>

<script type="text/javascript">
    dojo.addOnLoad(function() {
        //alert("AccountSearchPartial addOnLoad");
        var params = {};
        params.accountSearchUrl = '<%= Url.Action("Search", "Account") %>';
        accountSearchController = new dts.account.AccountSearchController(params);
    });
</script>
