<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ScopeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project General Information</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>"
        type="text/javascript"></script>
    <script type="text/javascript">
        var county_share_total;
        var isEditable = <%= Model.ProjectSummary.IsEditable().ToString().ToLower() %>;
        var AddPoolUrl = '<%=Url.Action("AddPoolProject" ) %>';
        var DropPoolUrl = '<%=Url.Action("DeletePoolProject")%>';
        var EditPoolUrl = '<%=Url.Action("UpdatePoolProject")%>';

        //Wireup the change handlers to enable the save button...
        $().ready(function () {
            // Prevent accidental navigation away
            if (isEditable) {
                App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
                $('#submitForm').button({ disabled: true });
            }

            $(".growable").growing({ buffer: 25 });

            $('.delete-pool').live("click", function () {
                //get the poolprojectid : pool_row_5022
                if (confirm("Would you like to proceed with the deletion of this pool project?")) {
                    var poolid = this.id.replace('delete_', '');
                    var row = $('#pool_row_' + poolid);
                    //$('#pool_row_' + poolid).empty();
                    $.ajax({
                        type: "POST",
                        url: DropPoolUrl,
                        data: "poolProjectId=" + poolid,
                        dataType: "json",
                        success: function (response) {
                            //var div = $('#pool_div_' + poolid);
                            $(row).empty();
                            $('#submit-result').html(response.message).addClass('success');
                            autoHide();
                        }
                    });
                }
                return false;
            });

            //Update a pool in the list
            $('.update-pool').live("click", function () {
                var poolprojectid = this.id.replace('delete_', '');
                //grab the values from the active form
                var poolname = $('#poolproject_' + poolprojectid).val();
                var pooldesc = $('#poolproject_' + poolprojectid + '_Description').val();
                var poolbeginat = $('#poolproject_' + poolprojectid + '_BeginAt').val();
                var poolendat = $('#poolproject_' + poolprojectid + '_EndAt').val();
                var poolcost = $('#poolproject_' + poolprojectid + '_Cost').val();

                //Do we try to see if the pool name is already listed?

                //Add to database via XHR
                //alert('Need XHR Big Test to Add: poolProjectId=' + poolprojectid + '&projectName=' + poolname + '&description=' + pooldesc + '&beginAt=' + poolbeginat + '&endAt=' + poolendat + '&cost=' + poolcost);
                $.ajax({
                    type: "POST",
                    url: EditPoolUrl,
                    data: "poolProjectId=" + poolprojectid + "&projectName=" + poolname + "&description=" + pooldesc + "&beginAt=" + poolbeginat + "&endAt=" + poolendat + "&cost=" + poolcost,
                    dataType: "json",
                    success: function (response) {
                        $('#submit-result').html(response.message).addClass('success');
                        autoHide();

                        //Disable the add button
                        $('.update-pool').html("Delete").addClass('delete-pool').removeClass('update-pool');
                    }
                });

                return false;
            });

            $.mask.masks.money = { mask: '9', type: 'repeat' };
            $('input:text').setMask();

            UpdatePoolProjectTotal();

            //Hook in the keyup event so we can keep track of changes to the shares
            //    $('input[id^=cshare]').live('keyup', function() { UpdatePoolProjectTotal(); });
            //$('#new_pool').bind('change', function() { UpdatePoolProjectTotal(); });
            $('input[id^=poolproject]').live('keyup', function () {
                var poolid = $(this).parents("tr").attr('id').replace('pool_row_', '');
                ChangePoolProjectToUpdate(poolid);
            });
            $('input[id^=new_pool]').live('keyup', function () { EnablePoolProjectAddButton(); });

            //Add a county to the list
            $('#add-pool').live("click", function () {
                //grab the values from the active form
                var poolname = $('#new_poolname').val();
                var pooldesc = $('#new_pooldesc').val();
                var poolbeginat = $('#new_poolbeginat').val();
                var poolendat = $('#new_poolendat').val();
                var poolcost = $('#new_poolcost').val();
                var poolprojectid;
                var poolmasterid = $('#TipProjectScope_ProjectVersionId').val();

                //reset the new share value to 0
                $('#new_poolname').val("");
                $('#new_pooldesc').val("");
                $('#new_poolbeginat').val("");
                $('#new_poolendat').val("");
                $('#new_poolcost').val("");

                //Do we try to see if the pool name is already listed?

                //Add to database via XHR

                //alert('Need XHR Big Test to Add: poolMasterVersionId=' + poolmasterid + '&projectName=' + poolname + '&description=' + pooldesc + '&beginAt=' + poolbeginat + '&endAt=' + poolendat + '&cost=' + poolcost);

                $.ajax({
                    type: "POST",
                    url: AddPoolUrl,
                    data: "poolMasterVersionId=" + poolmasterid + "&projectName=" + poolname + "&description=" + pooldesc + "&beginAt=" + poolbeginat + "&endAt=" + poolendat + "&cost=" + poolcost,
                    dataType: "json",
                    success: function (response) {
                        $('#submit-result').html(response.message).addClass('success');
                        autoHide();

                        //Add into the DOM
                        poolprojectid = response.poolprojectid;
                        var content = "<tr id='pool_row_" + poolprojectid + "'>";
                        content += "<td><input id='poolproject_" + poolprojectid + "' type='text' value='" + poolname + "' style='width: 200px;' name='poolproject_" + poolprojectid + "_ProjectName' maxlength = 255/></td>";
                        content += "<td><input type='text' value='" + pooldesc + "' style='width: 150px;' name='poolproject_" + poolprojectid + "_Description' maxlength = 75/></td>";
                        content += "<td><input type='text' value='" + poolbeginat + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_BeginAt' maxlength = 75/></td>";
                        content += "<td><input type='text' value='" + poolendat + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_EndAt' maxlength = 75/></td>";
                        content += "<td><input type='text' value='" + poolcost + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_Cost' maxlength = 75 alt='money'/></td>";
                        content += "<td><button class='delete-pool fg-button ui-state-default ui-priority-primary ui-corner-all' id='delete_" + poolprojectid + "'>Delete</button></td></tr>";
                        $('#pool-editor').before(content);

                        //Disable the add button
                        $('#add-pool').addClass('ui-state-disabled').attr('disabled', 'disabled');
                        UpdatePoolProjectTotal();
                    }
                });
                return false;
            });

            function autoHide() {
                //$('div#submit-result:contains("error")').addClass('error');
                setTimeout(function () {
                    $("div#submit-result").fadeOut("slow", function () {
                        $("div#submit-result").empty().removeClass();
                    });
                }, 5000);
            }

            function ChangePoolProjectToUpdate(id) {
                //$('.delete-pool').html("Update").removeClass('delete-pool').addClass('update-pool');
                $('#delete_' + id).html("Update").removeClass('delete-pool').addClass('update-pool');
            }
            function EnablePoolProjectAddButton() {
                //Enable Add button...
                var addButton = $('#add-pool');
                //if ($('new_poolname').val() != '') {
                addButton.removeClass('ui-state-disabled').removeAttr('disabled');
            }

            function UpdatePoolProjectTotal() {
                total = 0;

                //project-costs-sum
                //Sum the shares

                $('input[alt=money]').each(function () {
                    if (this.value) {
                        total += parseInt(this.value);
                    }
                });
                $('#project-costs-sum').html(total);
            }
        });
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-form-container tab-scope">
            <form method="put" action="/api/TipProjectScope" id="dataForm">
            <fieldset>
                <legend></legend>
                <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
                <%=Html.Hidden("TipProjectScope.TipYear", Model.ProjectSummary.TipYear)%>
                <%=Html.Hidden("TipProjectScope.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
                <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
                <p>
                    <label>
                        Project Scope:</label></p>
                <p>
                    <%--<textarea id="TipProjectScope_ProjectDescription" class="required growable"
                    title="Please provide a description for the project." 
                    name="TipProjectScope.ProjectDescription" cols="0" rows="0"><%= Model.TipProjectScope.ProjectDescription %></textarea>--%>
                    <%= Html.TextArea2("TipProjectScope.ProjectDescription", Model.ProjectSummary.IsEditable(), Model.TipProjectScope.ProjectDescription, 0, 0, 
                    new { @style = "width: 500px", @class = "required growable", @title = "Please provide a description for the project.", @name = "TipProjectScope.ProjectDescription" })%>
                </p>
                <p>
                    <label>
                        Begin Construction:</label>
                    <%= Html.DrcogTextBox("TipProjectScope.BeginConstructionYear", 
                    Model.ProjectSummary.IsEditable(),
                    Model.TipProjectScope.BeginConstructionYear, 
                    new { @class = "mediumInputElement digits", })%>
                </p>
                <p>
                    <label>
                        Open to Public:</label>
                    <%= Html.DrcogTextBox("TipProjectScope.OpenToPublicYear", 
                    Model.ProjectSummary.IsEditable(), 
                    Model.TipProjectScope.OpenToPublicYear,
                    new { @class = "mediumInputElement digits" })%>
                </p>
                <%if (Model.ProjectSummary.IsEditable())
                  { %>
                <div class="relative">
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="submit-result">
                    </div>
                </div>
                <%} %>
            </fieldset>
            </form>
        </div>
        <div class="clear">
        </div>
        <%--<% Html.RenderPartial("~/Views/Project/Partials/PoolProjectPartial.ascx", Model.PoolProjects); %>--%>
        <div>
            <h2>
                Pool Projects<span> | Total Costs:<span id="project-costs-sum"></span></span></h2>
            <span style="float: right; text-align: right;"><span style="font-size: 1.2em; font-weight: bold;">
                { <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span>
                }</span></span>
            <% if (Model.PoolProjects.Count == 0)
               { %>
            There are no pool projects for
            <%= Model.ProjectSummary.ProjectName %>
            <% }
               else
               { %>
            All pool project funding depicts federal and/or state funding only
            <% } %>
            <br />
            <br />
            <% if (Model.ProjectSummary.IsEditable() || Model.PoolProjects.Count > 0)
               { %>
            <%--<span>Project Costs Total:<span id="project-costs-sum"></span></span>--%>
            <table id="pool-project">
                <tr>
                    <th>
                        Project Name
                    </th>
                    <th>
                        Description
                    </th>
                    <th>
                        Begin
                    </th>
                    <th>
                        End
                    </th>
                    <th>
                        Cost
                    </th>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %><th>
                      </th>
                    <% } %>
                </tr>
                <% foreach (var item in Model.PoolProjects.ToList<DRCOG.Domain.Models.TIPProject.PoolProject>())
                   { %>
                <tr id="pool_row_<%=item.PoolProjectID.ToString() %>">
                    <td>
                        <%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_ProjectName", Model.ProjectSummary.IsEditable(), item.ProjectName.ToString(), new { style = "width:200px;", @id = "poolproject_" + item.PoolProjectID.ToString(), @maxlength = "255" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_Description", Model.ProjectSummary.IsEditable(), item.Description.ToString(), new { style = "width:150px;", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_BeginAt", Model.ProjectSummary.IsEditable(), item.BeginAt.ToString(), new { style = "width:100px;", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_EndAt", Model.ProjectSummary.IsEditable(), item.EndAt.ToString(), new { style = "width:100px;", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_Cost", Model.ProjectSummary.IsEditable(), (int)item.Cost, new { style = "width:100px;", @maxlength = "75", @alt = "money" })%>
                    </td>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <td>
                        <button class="delete-pool fg-button ui-state-default ui-priority-primary ui-corner-all"
                            id='delete_<%=item.PoolProjectID.ToString() %>'>
                            Delete</button>
                    </td>
                    <% } %>
                </tr>
                <% } %>
                <%if (Model.ProjectSummary.IsEditable())
                  { %>
                <tr id="pool-editor">
                    <td>
                        <%= Html.DrcogTextBox("ProjectName", true, "", new { style = "width: 200px;", @id = "new_poolname", @maxlength = "255" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("Description", true, "", new { style = "width: 150px;", @id = "new_pooldesc", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("BeginAt", true, "", new { style = "width: 100px;", @id = "new_poolbeginat", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("EndAt", true, "", new { style = "width: 100px;", @id = "new_poolendat", @maxlength = "75" })%>
                    </td>
                    <td>
                        <%= Html.DrcogTextBox("Cost", true, "", new { style = "width: 100px;", @id = "new_poolcost", @maxlength = "75", @alt = "money" })%>
                    </td>
                    <td>
                        <button id="add-pool" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                            Add</button>
                    </td>
                </tr>
                <% } %>
            </table>
            <% } %>
        </div>
        <%--<h2>Segments</h2>
    <% Html.RenderPartial("~/Views/Project/Partials/SegmentSummaryPartial.ascx", Model.Segments); %>--%>
        <div class="clear">
        </div>
    </div>
</asp:Content>
