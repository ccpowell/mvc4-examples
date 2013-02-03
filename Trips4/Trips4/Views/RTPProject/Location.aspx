<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.LocationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Location</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.ProjectSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
<link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        "use strict";

        // Prevent accidental navigation away
        App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
        $('#submitForm').button({ disabled: true });
    });
</script>
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.RtpYear = '<%= Model.ProjectSummary.RtpYear %>';
        App.pp.ProjectVersionId = <%= Model.ProjectSummary.ProjectVersionId %>;
        $(document).ready(App.tabs.initializeRtpProjectTabs);
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/RtpProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    
    <div class="tab-form-container"> 
        <form method="put" action="/api/RtpProjectLocation" id="dataForm">
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("RtpYear", Model.ProjectSummary.RtpYear)%>
            <%=Html.Hidden("ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
            
            <p>
                <label>Facility Name:</label>                
                <%= Html.DrcogTextBox("FacilityName", 
                    Model.ProjectSummary.IsEditable(), 
                    Model.RtpProjectLocation.FacilityName, 
                    new { @class = "longInputElement required", title="Please enter a primary road or facility name." })%>
            </p>
                        
            <p><label>Limits or Area of Project Description</label></p>                
            <p><textarea class="longInputElement" name="Limits"><%=Model.RtpProjectLocation.Limits%></textarea></p>
            
            <p>
                <label>Region:</label>
                <%= Html.DrcogTextBox("Region", Model.ProjectSummary.IsEditable(), Model.RtpCdotData.Region, new { @style = "min-width: 100px;" })%>
            </p>
            <p>
                <label>TPR ID:</label>
                <%= Html.DrcogTextBox("TPRID", Model.ProjectSummary.IsEditable(), Model.RtpCdotData.TPRID, new { @style = "min-width: 100px;" })%>
            </p>
            <p>
                <label>Route # (if applicable)</label> 
                <%= Html.DropDownList("RouteId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailableRoutes, "key", "value", Model.RtpProjectLocation.RouteId), 
                    "--Select a Route--",
                    new { @class = "mediumInputElement", title = "Please select a route #.", @id="RouteId" })%>            
            </p>
            <%if(Model.ProjectSummary.IsEditable()){ %>
                <div class="relative">
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="submit-result">
                    </div>
                </div>
                <br />
            <%} %>
    
           <div >
           <h2>Affected Counties</h2>
           <p>Note: This is not versioned. Editing these values will effect all versions of this project</p>
           <table id="county-shares">
            <tr>
            <td>Primary</td>
            <td>Share</td>
            <td>County</td>
            <td></td>
            </tr>
           <%foreach (DRCOG.Domain.Models.CountyShareModel cty in Model.CountyShares)
             { %>                                
                <tr id="county_row_<%=cty.CountyId.ToString() %>">
                <td><%= Html.SimpleCheckBox("cty_" + cty.CountyId.ToString() + "_IsPrimary", cty.Primary.Value, cty.ProjectId.ToString() + "_isPrimary", Model.ProjectSummary.IsEditable()  )%></td>
                <td><%= Html.DrcogTextBox("cty_" + cty.CountyId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)cty.Share.Value, new { style = "width:30px;", @id = "cshare_" + cty.ProjectId.ToString() })%>%</td>
                <td><%=cty.CountyName %></td>                
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <td><button class="delete-county fg-button ui-state-default ui-priority-primary ui-corner-all" name="<%=cty.CountyId.ToString() %>_<%=cty.CountyName %>" id='delete_<%=cty.CountyId.ToString() %>'>Delete</button></td>                                
                    <%} %>            
                </tr>
           <%} %>
           <%if (Model.ProjectSummary.IsEditable())
              { %>
                <tr id="county-editor">
                <td><%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id ="new_primary" })%></td>
                <td><%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "cshare_new" })%>%</td>
                <td><%= Html.DropDownList("County",
                                Model.ProjectSummary.IsEditable(),
                                new SelectList(Model.AvailableCounties, "key", "value", ""), 
                                "-- Select a County--",
                                new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_county" })%></td>
                                <td><button id="add-county" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button></td>                                
                </tr>                      
           <%} %>
           <tr><td>Share Total:</td>
                <td><div id="county-share-sum"></div></td>
                <td>&nbsp;</td><td>&nbsp;</td></tr>
           </table>
           </div>
           
           
           <div >
           <h2>Affected Municipalities</h2>
           <p>Note: This is not versioned. Editing these values will effect all versions of this project</p>
           <table id="muni-shares">
            <tr>
            <td>Primary</td>
            <td>Share</td>
            <td>Municipality</td>
            <td></td>
            </tr>
           <%foreach (DRCOG.Domain.Models.MunicipalityShareModel muni in Model.MuniShares)
             { %>                                
                <tr id="muni_row_<%=muni.MunicipalityId.ToString() %>">
                <td><%= Html.SimpleCheckBox("muni_" + muni.MunicipalityId.ToString() + "_IsPrimary", muni.Primary.Value,   muni.ProjectId.ToString() + "_isPrimary" ,Model.ProjectSummary.IsEditable())%></td>
                <td><%= Html.DrcogTextBox("muni_" + muni.MunicipalityId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)muni.Share.Value, new { style = "width:30px;", @id = "mshare_" + muni.ProjectId.ToString() })%>%</td>
                <td><%=muni.MunicipalityName%></td>                
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <td><button class="delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all" name="<%=muni.MunicipalityId.ToString() %>_<%=muni.MunicipalityName%>" id='delete_<%=muni.MunicipalityId.ToString() %>'>Delete</button></td>                                
                    <%} %>            
                </tr>
           <%} %>
           <%if (Model.ProjectSummary.IsEditable())
              { %>
                <tr id="muni-editor">
                <td><%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id ="new_primary" })%></td>
                <td><%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "mshare_new" })%>%</td>
                <td><%= Html.DropDownList("Municipality",
                                Model.ProjectSummary.IsEditable(),
                                new SelectList(Model.AvailableMunicipalities, "key", "value", ""), 
                                "-- Select a Municipality--",
                                new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_muni" })%></td>
                                <td><button disabled='disabled' id="add-muni" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button></td>                                
                </tr>                      
           <%} %>
           <tr><td>Share Total:</td>
                <td><div id="muni-share-sum"></div></td>
                <td>&nbsp;</td><td>&nbsp;</td></tr>
           </table>
           </div>
        </form>
    </div>

<div class="clear"></div>
</div>



<script type="text/javascript">
    var county_share_total;
    var AddCountyUrl = '<%=Url.Action("AddCountyShare" ) %>';
    var DropCountyUrl = '<%=Url.Action("RemoveCountyShare")%>';
    var AddMuniUrl = '<%=Url.Action("AddMuniShare") %>';
    var DropMuniUrl = '<%=Url.Action("RemoveMuniShare") %>';

    $().ready(function() {

        $('.delete-county').live("click", function() {
            //get the countyId and project id
            var pid = $('#ProjectId').val();
            var ctyId = this.id.replace('delete_', '');
            var ctyName = this.name.replace(ctyId + "_", "");
            $.ajax({
                type: "POST",
                url: DropCountyUrl,
                data: "projectId=" + pid + "&countyId=" + ctyId,
                dataType: "json",
                success: function(response) {
                    $('#result').html(response.message);
                    $('#county_row_' + ctyId).empty();
                    $("#new_county").addOption(ctyId, ctyName);
                    window.onbeforeunload = null;
                }
            });
            //alert('Need XHR to Remove: county:' + ctyId + ' projectId:' + pid);
            return false;
        });

        $('.delete-muni').live("click", function() {
            //get the countyId and project id
            var pid = $('#ProjectId').val();
            var muniId = this.id.replace('delete_', '');
            var muniName = this.name.replace(muniId + "_", "");
            
            $.ajax({
                type: "POST",
                url: DropMuniUrl,
                data: "projectId=" + pid + "&muniId=" + muniId,
                dataType: "json",
                success: function(response) {
                    $('#result').html(response.message);
                    $('#muni_row_' + muniId).empty();
                    $("#new_muni").addOption(muniId, muniName);
                    window.onbeforeunload = null;
                }
            });
            //alert('Need XHR to Remove: muni:' + muniId + ' projectId:' + pid);
            return false;
        });

        UpdateCountyShareTotal();
        UpdateMuniShareTotal();

    });

    

    function UpdateCountyShareTotal() {
        county_share_total = 0;
        //Sum the shares cshare_new
        $('input[id^=cshare]').each(function() {
            county_share_total += parseInt(this.value);            
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
        $('input[id^=mshare]').each(function() {
            muni_share_total += parseInt(this.value);
        });
        $('#muni-share-sum').html(muni_share_total);

        //Enable Add button...
        var addButton = $('#add-muni');
        if (muni_share_total <= 100 && parseInt($('#mshare_new').val()) > 0 && $('#new_muni option:selected').val() !='') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }
    }
    
    //Hook in the keyup event so we can keep track of changes to the shares
    $('input[id^=cshare]').live('keyup', function() { UpdateCountyShareTotal(); });
    $('input[id^=mshare]').live('keyup', function() { UpdateMuniShareTotal(); });
    $('#new_muni').bind('change', function() { UpdateMuniShareTotal(); });
    $('#new_county').bind('change', function() { UpdateCountyShareTotal(); });
       
    //Add a county to the list
    $('#add-county').click(function() {
        //grab the values from the active form
        var share = $('#cshare_new').val();
        var primary = $('#new_primary').attr('checked');
        var countyId = $('#new_county option:selected').val();
        var countyName = $('#new_county option:selected').text();
        var pid = $('#ProjectId').val();

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
            data: "projectId=" + pid + "&countyId=" + countyId + "&share=" + share + "&isPrimary=" + primary,
            dataType: "json",
            success: function(response) {
                $('#result').html(response.message);
                //Disable the add button
                $('#add-county').addClass('ui-state-disabled').attr('disabled', 'disabled');
                window.onbeforeunload = null;
            }
        });

        
        return false;
    });

    //Add a county to the list
    $('#add-muni').click(function() {
        //grab the values from the active form
        var share = $('#mshare_new').val();
        var primary = $('#new_primary').attr('checked');
        var muniId = $('#new_muni option:selected').val();
        var muniName = $('#new_muni option:selected').text();
        var pid = $('#ProjectId').val();
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
        content += "<td><input id='mshare_" + muniId + "' type='text' value='" + share + "' style='width: 30px;' name='muni_" + muniId +"_share'/>%</td>";
        content += "<td>" + muniName + "</td>";
        content += "<td><button class='delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + muniId + "_" + muniName + "' id='delete_" + muniId + "'>Delete</button></td></tr>";
        $('#muni-editor').before(content);

        //alert('Need XHR to add muni ' + muniName + ' with share ' + share + ' primary ' + primary + ' and muniId:' + muniId);
        //Add to database via XHR
        $.ajax({
            type: "POST",
            url: AddMuniUrl,
            data: "projectId=" + pid + "&muniId=" + muniId + "&share=" + share + "&isPrimary=" + primary,
            dataType: "json",
            success: function(response) {
                $('#result').html(response.message);
                $('#add-muni').addClass('ui-state-disabled').attr('disabled', 'disabled');
                window.onbeforeunload = null;
            }
        });
        
        return false;
    });
    

</script>
</asp:Content>



