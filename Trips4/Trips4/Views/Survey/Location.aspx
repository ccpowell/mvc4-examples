<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.LocationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    DRCOG :: Project Location</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">
    Transportation Improvement Survey
    <%= Model.Current.Name %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
    <link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskedinput-1.2.2.min.js")%>"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <%Html.RenderPartial("~/Views/Survey/Partials/ProjectTabPartial.ascx", Model); %>
        <div class="tab-form-container">
            <form method="put" action="/api/SurveyLocation" id="dataForm">
            <%Html.RenderPartial("~/Views/Survey/Partials/ManagerRibbonPartial.ascx", Model); %>
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("Year", Model.Current.Name)%>
            <%=Html.Hidden("ProjectVersionId", Model.Project.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.Project.ProjectId)%>
            <p>
                <label>
                    Facility Name:</label>
                <%= Html.DrcogTextBox("FacilityName", 
                    Model.Project.IsEditable(), 
                    Model.Location.FacilityName, 
                    new { @class = "longInputElement required", title="Please enter a primary road or facility name." })%>
            </p>
            <p>
                <label>
                    Limits or Area of Project Description</label></p>
            <p>
                <textarea class="longInputElement" name="Limits" id="Limits"><%=Model.Location.Limits%></textarea></p>
            <p>
                <label>
                    Route # (if applicable)</label>
                <%= Html.DropDownList("RouteId",
                                        Model.Project.IsEditable(),
                    new SelectList(Model.AvailableRoutes, "key", "value", Model.Location.RouteId), 
                    "--Select a Route--",
                    new { @class = "mediumInputElement", title = "Please select a route #.", @id="RouteId" })%>
            </p>
            <%if (Model.Project.IsEditable())
              { %>
            <div style="position: relative;">
                <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                    Save Changes</button>
                <div id="submit-result">
                </div>
            </div>
            <br />
            <%} %>
            <div>
                <h2>
                    Affected Counties</h2>
                <p>
                    Note: This is not versioned. Editing these values will effect all versions of this
                    project</p>
                <table id="county-shares">
                    <tr>
                        <td>
                            Primary
                        </td>
                        <td>
                            Share
                        </td>
                        <td>
                            County
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%foreach (DRCOG.Domain.Models.CountyShareModel cty in Model.CountyShares)
                      { %>
                    <tr id="county_row_<%=cty.CountyId.ToString() %>">
                        <td>
                            <%= Html.SimpleCheckBox("cty_" + cty.CountyId.ToString() + "_IsPrimary", cty.Primary.Value, "cshare_isPrimary_" + cty.CountyId.ToString(), Model.Project.IsEditable())%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("cty_" + cty.CountyId.ToString() + "_share", Model.Project.IsEditable(), (int)cty.Share.Value, new { style = "width:30px;", @id = "cshare_" + cty.CountyId.ToString() })%>%
                        </td>
                        <td>
                            <%=cty.CountyName %>
                        </td>
                        <%if (Model.Project.IsEditable())
                          { %>
                        <td>
                            <button class="delete-county fg-button ui-state-default ui-priority-primary ui-corner-all"
                                name="<%=cty.CountyId.ToString() %>_<%=cty.CountyName %>" id='delete_<%=cty.CountyId.ToString() %>'>
                                Delete</button>
                        </td>
                        <%} %>
                    </tr>
                    <%} %>
                    <%if (Model.Project.IsEditable())
                      { %>
                    <tr id="county-editor">
                        <td>
                            <%= Html.CheckBox("IsPrimary", Model.Project.IsEditable(), false, new { @id = "new_primary" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("Share", Model.Project.IsEditable(), 0, new { style = "width:30px;", @id = "cshare_new" })%>%
                        </td>
                        <td>
                            <%= Html.DropDownList("County",
                                Model.Project.IsEditable(),
                                new SelectList(Model.AvailableCounties, "key", "value", ""), 
                                "-- Select a County--",
                                new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_county" })%>
                        </td>
                        <td>
                            <button id="add-county" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                                Add</button>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td>
                            Share Total:
                        </td>
                        <td>
                            <div id="county-share-sum">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <h2>
                    Affected Municipalities</h2>
                <p>
                    Note: This is not versioned. Editing these values will effect all versions of this
                    project</p>
                <table id="muni-shares">
                    <tr>
                        <td>
                            Primary
                        </td>
                        <td>
                            Share
                        </td>
                        <td>
                            Municipality
                        </td>
                        <td>
                        </td>
                    </tr>
                    <%foreach (DRCOG.Domain.Models.MunicipalityShareModel muni in Model.MuniShares)
                      { %>
                    <tr id="muni_row_<%=muni.MunicipalityId.ToString() %>">
                        <td>
                            <%= Html.SimpleCheckBox("muni_" + muni.MunicipalityId.ToString() + "_IsPrimary", muni.Primary.Value, "mshare_isPrimary_" + muni.MunicipalityId.ToString(), Model.Project.IsEditable())%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("muni_" + muni.MunicipalityId.ToString() + "_share", Model.Project.IsEditable(), (int)muni.Share.Value, new { style = "width:30px;", @id = "mshare_" + muni.MunicipalityId.ToString() })%>%
                        </td>
                        <td>
                            <%=muni.MunicipalityName%>
                        </td>
                        <%if (Model.Project.IsEditable())
                          { %>
                        <td>
                            <button class="delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all"
                                name="<%=muni.MunicipalityId.ToString() %>_<%=muni.MunicipalityName%>" id='delete_<%=muni.MunicipalityId.ToString() %>'>
                                Delete</button>
                        </td>
                        <%} %>
                    </tr>
                    <%} %>
                    <%if (Model.Project.IsEditable())
                      { %>
                    <tr id="muni-editor">
                        <td>
                            <%= Html.CheckBox("IsPrimary", Model.Project.IsEditable(), false, new { @id = "new_primary" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("Share", Model.Project.IsEditable(), 0, new { style = "width:30px;", @id = "mshare_new" })%>%
                        </td>
                        <td>
                            <%= Html.DropDownList("Municipality",
                                Model.Project.IsEditable(),
                                new SelectList(Model.AvailableMunicipalities, "key", "value", ""), 
                                "-- Select a Municipality--",
                                new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_muni" })%>
                        </td>
                        <td>
                            <button disabled='disabled' id="add-muni" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                                Add</button>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td>
                            Share Total:
                        </td>
                        <td>
                            <div id="muni-share-sum">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            </form>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            "use strict";

            // Prevent accidental navigation away
            App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
            $('#submitForm').button({ disabled: true });

            var county_share_total;
            var AddCountyUrl = '<%=Url.Action("AddCountyShare" ) %>';
            var UpdateCountyUrl = '<%=Url.Action("UpdateCountyShare" ) %>';
            var DropCountyUrl = '<%=Url.Action("RemoveCountyShare")%>';
            var AddMuniUrl = '<%=Url.Action("AddMuniShare") %>';
            var UpdateMuniUrl = '<%=Url.Action("UpdateMuniShare") %>';
            var DropMuniUrl = '<%=Url.Action("RemoveMuniShare") %>';

            $('.delete-county').live("click", function () {
                //get the countyId and project id
                var pid = $('#ProjectId').val();
                var ctyId = this.id.replace('delete_', '');
                var ctyName = this.name.replace(ctyId + "_", "");
                var pvid = $("#ProjectVersionId").val();

                $.ajax({
                    type: "POST",
                    url: DropCountyUrl,
                    data: "projectId=" + pid + "&countyId=" + ctyId + "&projectVersionId=" + pvid,
                    dataType: "json",
                    success: function (response) {
                        $('#result').html(response.message);
                        $('#county_row_' + ctyId).empty();
                        $("#new_county").addOption(ctyId, ctyName);
                        window.onbeforeunload = null;
                    }
                });
                return false;
            });

            $('.delete-muni').live("click", function () {
                //get the countyId and project id
                var pid = $('#ProjectId').val();
                var muniId = this.id.replace('delete_', '');
                var muniName = this.name.replace(muniId + "_", "");
                var pvid = $("#ProjectVersionId").val();

                $.ajax({
                    type: "POST",
                    url: DropMuniUrl,
                    data: "projectId=" + pid + "&muniId=" + muniId + "&projectVersionId=" + pvid,
                    dataType: "json",
                    success: function (response) {
                        $('#result').html(response.message);
                        $('#muni_row_' + muniId).empty();
                        $("#new_muni").addOption(muniId, muniName);
                        window.onbeforeunload = null;
                    }
                });
                return false;
            });

            UpdateCountyShareTotal();
            UpdateMuniShareTotal();
        });

        function UpdateCountyShareTotal() {
            county_share_total = 0;
            //Sum the shares cshare_new
            $('input[id^=cshare]').each(function () {
                if ($(this).attr("id").indexOf("isPrimary") == -1) {
                    county_share_total += parseInt(this.value);
                }
            });
            $('#county-share-sum').html(county_share_total);

            //Enable Add button...
            var addButton = $('#add-county');
            if (county_share_total <= 100 && parseInt($('#cshare_new').val()) > 0 && $('#new_county option:selected').val() != '') {
                addButton.removeClass('ui-state-disabled').removeAttr('disabled');
            } else {
                //add disabled class if it does not exist
                if (!addButton.hasClass('ui-state-disabled')) {
                    addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
                }
            }
        }

        function UpdateMuniShareTotal() {
            var muni_share_total = 0;
            //Sum the shares
            $('input[id^=mshare]').each(function () {
                if ($(this).attr("id").indexOf("isPrimary") == -1) {
                    muni_share_total += parseInt(this.value);
                }
            });
            $('#muni-share-sum').html(muni_share_total);

            //Enable Add button...
            var addButton = $('#add-muni');
            if (muni_share_total <= 100 && parseInt($('#mshare_new').val()) > 0 && $('#new_muni option:selected').val() != '') {
                addButton.removeClass('ui-state-disabled').removeAttr('disabled');
            } else {
                //add disabled class if it does not exist
                if (!addButton.hasClass('ui-state-disabled')) {
                    addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
                }
            }
        }

        //Hook in the keyup event so we can keep track of changes to the shares
        $('input[id^=cshare]').live('keyup', function () { UpdateCountyShareTotal(); });
        $('input[id^=mshare]').live('keyup', function () { UpdateMuniShareTotal(); });

        //Hook in the keyup event so we can keep track of changes to the shares
        $('input[id^=cshare_]').blur(function () {
            var rowId = $(this).parents("tr").attr('id').replace('county_row_', '');
            var share = $("#cshare_" + rowId).val();
            var primary = $("#cshare_isPrimary_" + rowId).attr('checked');
            if (primary) {
                var checks = $('input[id^=cshare_isPrimary_]').attr('checked', false);
                $("#cshare_isPrimary_" + rowId).attr('checked', true);
            }
            var element = $(this);
            setConfirmUnload(true, false);
            var county_share_total = parseInt($("#county-share-sum").html());
            if (county_share_total <= 100) {
                updateCountyShare(rowId, share, primary);
                element.removeClass("input-validation-error").addClass("valid");
            } else {
                element.removeClass("valid").addClass("input-validation-error");
                element.val(0);
                UpdateCountyShareTotal();
            }
        });

        $('input[id^=mshare_]').blur(function () {
            var rowId = $(this).parents("tr").attr('id').replace('muni_row_', '');
            var share = $("#mshare_" + rowId).val();
            var primary = $("#mshare_isPrimary_" + rowId).attr('checked');
            var element = $(this);
            setConfirmUnload(true, false);
            var county_share_total = parseInt($("#muni-share-sum").html());
            if (county_share_total <= 100) {
                updateMuniShare(rowId, share, primary);
                element.removeClass("input-validation-error").addClass("valid");
            } else {
                element.removeClass("valid").addClass("input-validation-error");
                element.val(0);
                UpdateMuniShareTotal();
            }
        });

        $('#new_muni').bind('change', function () { UpdateMuniShareTotal(); });
        $('#new_county').bind('change', function () { UpdateCountyShareTotal(); });

        //Add a county to the list
        $('#add-county').click(function () {
            //grab the values from the active form
            var share = $('#cshare_new').val();
            var primary = $('#new_primary').attr('checked');
            var countyId = $('#new_county option:selected').val();
            var countyName = $('#new_county option:selected').text();
            var pid = $('#ProjectId').val();
            var pvid = $("#ProjectVersionId").val();

            //Remove selected option to ensure that we can't re-add it.
            //this leaves a hole, in that you could add, then remove, 
            //then want to re-add, and it would not be in the list.
            $('#new_county option:selected').remove();
            //reset the new share value to 0
            $('#cshare_new').val(0);
            //if primary is checked, see if primary is checked elsewhere, and warn user

            //Do we try to see if the county is already listed?

            //Add into the DOM
            var content = "<tr id='county_row_" + countyId + "'>";
            if (primary == true) {
                content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' checked='checked'/></td>";
            } else {
                content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' /></td>";
            }
            content += "<td><input id='cshare_" + countyId + "' type='text' value='" + share + "' style='width: 30px;' name='cty_" + countyId + "_share'/>%</td>";
            content += "<td>" + countyName + "</td>";
            content += "<td><button class='delete-county fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + countyId + "_" + countyName + "' id='delete_" + countyId + "'>Delete</button></td></tr>";
            $('#county-editor').before(content);


            //alert('Need XHR to add ' + countyName + ' with share ' + share + ' primary ' + primary + ' and CountyId:' + countyId);
            //Add to database via XHR

            $.ajax({
                type: "POST",
                url: AddCountyUrl,
                data: "projectId=" + pid + "&countyId=" + countyId + "&share=" + share + "&isPrimary=" + primary + "&projectVersionId=" + pvid,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    //Disable the add button
                    $('#add-county').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    window.onbeforeunload = null;
                }
            });


            return false;
        });

        //Update county
        function updateCountyShare(id, share, primary) {
            //grab the values from the active form
            var pid = $('#ProjectId').val();
            var pvid = $("#ProjectVersionId").val();
            $.ajax({
                type: "POST",
                url: UpdateCountyUrl,
                data: "projectId=" + pid + "&countyId=" + id + "&share=" + share + "&isPrimary=" + primary + "&projectVersionId=" + pvid,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    window.onbeforeunload = null;
                }
            });

            return false;
        };

        //Update county
        function updateMuniShare(id, share, primary) {
            //grab the values from the active form
            var pid = $('#ProjectId').val();
            var pvid = $("#ProjectVersionId").val();
            $.ajax({
                type: "POST",
                url: UpdateMuniUrl,
                data: "projectId=" + pid + "&muniId=" + id + "&share=" + share + "&isPrimary=" + primary + "&projectVersionId=" + pvid,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    window.onbeforeunload = null;
                }
            });

            return false;
        };

        //Add a county to the list
        $('#add-muni').click(function () {
            //grab the values from the active form
            var share = $('#mshare_new').val();
            var primary = $('#new_primary').attr('checked');
            var muniId = $('#new_muni option:selected').val();
            var muniName = $('#new_muni option:selected').text();
            var pid = $('#ProjectId').val();
            var pvid = $("#ProjectVersionId").val();
            //Remove selected option to ensure that we can't re-add it.
            //this leaves a hole, in that you could add, then remove, 
            //then want to re-add, and it would not be in the list.
            $('#new_muni option:selected').remove();
            //reset the new share value to 0
            $('#mshare_new').val(0);
            //if primary is checked, see if primary is checked elsewhere, and warn user

            //Do we try to see if the county is already listed?

            //Add into the DOM
            var content = "<tr id='muni_row_" + muniId + "'>";
            if (primary == true) {
                content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' checked='checked'/></td>";
            } else {
                content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' /></td>";
            }
            content += "<td><input id='mshare_" + muniId + "' type='text' value='" + share + "' style='width: 30px;' name='muni_" + muniId + "_share'/>%</td>";
            content += "<td>" + muniName + "</td>";
            content += "<td><button class='delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + muniId + "_" + muniName + "' id='delete_" + muniId + "'>Delete</button></td></tr>";
            $('#muni-editor').before(content);

            //alert('Need XHR to add muni ' + muniName + ' with share ' + share + ' primary ' + primary + ' and muniId:' + muniId);
            //Add to database via XHR
            $.ajax({
                type: "POST",
                url: AddMuniUrl,
                data: "projectId=" + pid + "&muniId=" + muniId + "&share=" + share + "&isPrimary=" + primary + "&projectVersionId=" + pvid,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    $('#add-muni').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    window.onbeforeunload = null;
                }
            });

            return false;
        });
    

    </script>
</asp:Content>
