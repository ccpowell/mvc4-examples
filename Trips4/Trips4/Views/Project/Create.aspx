<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<InfoViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Test</title>
    <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
    <link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        var isDirty = false, formSubmittion = false;

        var add1url = '<%=Url.Action("AddCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        var remove1url = '<%=Url.Action("DropCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        var add2url = '<%=Url.Action("AddCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        var remove2url = '<%=Url.Action("DropCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';

        $(document).ready(function() {
            // Prevent accidental navigation away
            $(':input', document.dataForm).bind("change", function() { setConfirmUnload(true); });
            $(':input', document.dataForm).bind("keyup", function() { setConfirmUnload(true); });
            //$(':button', document.dataForm).unbind("keyup", function() { setConfirmUnload(true); }); // Want to not do this for my hyperlink buttons. -DBD
            //disable the onbeforeunload message if we are using the submitform button
            if ($('#submitForm')) {
                $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
            }

            //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
            $("#dataForm").validate({
                submitHandler: function(form) {
                    $(form).ajaxSubmit({
                        dataType: 'json',
                        success: function(response) {
                            $('#result').html(response.message);
                            $('#submitForm').addClass('ui-state-disabled');
                        },
                        error: function(XMLHttpRequest, textStatus, errorThrown) {
                            $('#result').text(data.message);
                            $('#result').addClass('error');
                        }
                    });
                }
            });

            $('#add1').click(function() {
                $('#AvailableAgencies option:selected').each(function(i) {
                    //make callback add to Eligible Agency list
                    add1Agency($(this).val());
                });
                return false;
            });

            $('#remove1').click(function() {
                $('#Current1Agencies option:selected').each(function(i) {
                    remove1Agency($(this).val());
                });
                return false;
            });

            $('#add2').click(function() {
                $('#AvailableAgencies option:selected').each(function(i) {
                    //make callback add to Eligible Agency list
                    add2Agency($(this).val());
                });
                return false;
            });

            $('#remove2').click(function() {
                $('#Current2Agencies option:selected').each(function(i) {
                    remove2Agency($(this).val());
                });
                return false;
            });

            function add1Agency(id) {
                $.ajax({
                    type: "POST",
                    url: add1url,
                    dataType: "json",
                    data: { agencyId: id },
                    success: function(response) {
                        if ((response.Error == null) || (response.Error == "")) {
                            //success
                            var selector = "#AvailableAgencies option[value='" + id + "']";
                            $(selector).remove().appendTo('#Current1Agencies');
                        } else {
                            ShowMessageDialog('Error adding Primary Sponsor', response.Error);
                        }
                    }
                });
            }

            function add2Agency(id) {
                $.ajax({
                    type: "POST",
                    url: add2url,
                    dataType: "json",
                    data: { agencyId: id },
                    success: function(response) {
                        if ((response.Error == null) || (response.Error == "")) {
                            //success
                            var selector = "#AvailableAgencies option[value='" + id + "']";
                            $(selector).remove().appendTo('#Current2Agencies');
                        } else {
                            ShowMessageDialog('Error adding Secondary Sponsor', response.Error);
                        }
                    }
                });
            }

            function remove1Agency(id) {
                $.ajax({
                    type: "POST",
                    url: remove1url,
                    dataType: "json",
                    data: { agencyId: id },
                    success: function(response) {
                        if ((response.Error == null) || (response.Error == "")) {
                            //success
                            var selector = "#Current1Agencies option[value='" + id + "']";
                            var selector2 = "#AvailableAgencies";
                            //$(selector).remove().appendTo('#AvailableAgencies'); 
                            $(selector).remove().prependTo('#AvailableAgencies');
                            $(selector2).sort(); // Can not figure out how to sort this. Will prepend instead. -DBD
                        } else {
                            ShowMessageDialog('Primary Sponsor not Removed', response.Error);
                        }
                    }
                });
            }

            function remove2Agency(id) {
                $.ajax({
                    type: "POST",
                    url: remove2url,
                    dataType: "json",
                    data: { agencyId: id },
                    success: function(response) {
                        if ((response.Error == null) || (response.Error == "")) {
                            //success
                            var selector = "#Current2Agencies option[value='" + id + "']";
                            var selector2 = "#AvailableAgencies";
                            //$(selector).remove().appendTo('#AvailableAgencies'); 
                            $(selector).remove().prependTo('#AvailableAgencies');
                            $(selector2).sort(); // Can not figure out how to sort this. Will prepend instead. -DBD
                        } else {
                            ShowMessageDialog('Secondary Sponsor not Removed', response.Error);
                        }
                    }
                });
            }

            function ShowMessageDialog(title, message) {
                $('#dialog').dialog('option', 'title', title);
                $('#dialogMessage').html(message);
                $('#dialog').dialog('open');
            }

            //Initialize the dialog
            $("#dialog").dialog({
                autoOpen: false,
                draggable: false,
                bgiframe: true,
                modal: true,
                buttons: {
                    Ok: function() {
                        $(this).dialog('close');
                    }
                }
            });

            function setConfirmUnload(on) {
                $('#submitForm').removeClass('ui-state-disabled');
                $('#result').html("");
                window.onbeforeunload = (on) ? unloadMessage : null;
            }

            function unloadMessage() {
                return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
            }

            $('#SponsorId').change(function() {
                var sponsorId = $('#SponsorId').val();
                $.getJSON('<%= Url.Action("UpdateAvailableSponsorContacts")%>/' + sponsorId, null, function(data) {
                    $('#SponsorContactId').fillSelect(data);
                });
            });
        });
    </script>
</head>

<body >
<div class="tab-content-container">
    <div class="tab-form-container">    
    <% using (Html.BeginForm("Create", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
    <%{ %>
        <fieldset>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%--<%= Html.Hidden("InfoModel.TipYear", Model.InfoModel.TipYear)%>         
        <%= Html.Hidden("InfoModel.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
        <%= Html.Hidden("InfoModel.ProjectId", Model.ProjectSummary.ProjectId)%>--%>
        
        <p>
            <label>Project Name:</label>
            <%= Html.DrcogTextBox("InfoModel.ProjectName", 
                    Model.ProjectSummary.IsEditable(), 
                    null, 
                    new { @class = "longInputElement required", title="Please enter a project title." })%>
        </p>
        <p>
        <div id="currentSponsorsForm" class="tab-form-container">
            <label>Sponsor(s):</label>
            <table border="1" rules="none">
            <tr>
                <td>Primary Sponsor:</td>
                <td>&nbsp;</td>
                <td>Available Agencies:</td>
            </tr>
            <tr>
                <td>
                    <%= Html.ListBox("Current1Agencies", 
                        null,
                        //new MultiSelectList(Model.ProjectSponsorsModel.GetCurrent1AgencySelectList().Items, "OrganizationId", "OrganizationName"), 
                        new {  @class = "mediumInputElement", size = 1 })%><br/>        
                </td>
                <td>
                    <%if (1 == 1){ %> <!-- ToDo: Figure out if you ever need this greyed out. -DBD -->
                        <a href="#" id="remove1" ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                        <a href="#" id="add1" ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                    <%} %>
                </td>
                <td rowspan="3" >
                    <%= Html.ListBox("AvailableAgencies", 
                        new MultiSelectList(Model.ProjectSponsorsModel.GetAvailableAgencySelectList().Items, "OrganizationId", "OrganizationName"), 
                        new { @class = "mediumInputElement", size = 10 })%><br/>
                </td>
            </tr>
            <tr>
                <td>Secondary Sponsor(s):</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td rowspan="3">
                    <%= Html.ListBox("Current2Agencies", 
                        new MultiSelectList(Model.ProjectSponsorsModel.GetCurrent2AgencySelectList().Items, "OrganizationId", "OrganizationName"), 
                        new {  @class = "mediumInputElement", size = 5 })%><br/>        
                </td>
                <td>
                    <a href="#" id="remove2" ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                    <a href="#" id="add2" ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                </td>
                <td>&nbsp;</td>
            </tr>
            </table>
            <span>Note: Changes are stored to the database as they are made in the interface.</span>
        </div>
        </p>
        <p>
            <label>Sponsor Contact:</label>             
            <%= Html.DropDownList("InfoModel.SponsorContactId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.InfoModel.SponsorContactId),
                    "-- Select a Sponsor Contact --", 
                    new { @class = "mediumInputElement not-required", title="Please select a project sponsor" })%>
 
        </p>
        <p>
            <label>Admin Level:</label>
            <%= Html.DropDownList("InfoModel.AdministrativeLevelId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId),
                    "-- Select Admin Level --", 
                    new { @class = "mediumInputElement required", title="Please select an Admin Level" })%>
            <span>Facility Responsibility Level</span>
        </p>
       
        <p>
            <label>Project Type:</label> 
            <%= Html.DropDownList("InfoModel.ProjectTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableProjectTypes, "key", "value", Model.InfoModel.ProjectTypeId), 
                    "-- Select Project Type--", 
                    new { @class = "mediumInputElement required", title="Please select a project type" })%>
        </p>
         <p>
            <label>Improvement Type:</label>
            <%= Html.DropDownList("InfoModel.ImprovementTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.InfoModel.ImprovementTypeId), 
                    "-- Select Improvment Type--", 
                    new { @class = "mediumInputElement required", title="Please select an improvment type." })%>
        </p>
        <p>
            <label>Road or Transit:</label>
            <%= Html.DropDownList("InfoModel.TransportationTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.InfoModel.TransportationTypeId), 
                    "-- Select --", 
                    new { @class = "mediumInputElement required", title="Please specify if this is a Road or transit project" })%>
        </p>
        <p>
            <label>Pool Name:</label>
              <%= Html.DropDownList("InfoModel.ProjectPoolId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailablePools, "key", "value", Model.InfoModel.ProjectPoolId), 
                    "-- Select Pool --",
                    new { @class = "mediumInputElement not-required", title = "Please specify a pool for this project (if applicable)" })%>
        </p>
        <br />
        <p>
            <label>Pool Master:</label>                               
            <br />
        </p>
        <p>
            <label>Selection Agency:</label>
                <%= Html.DropDownList("InfoModel.SelectionAgencyId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.InfoModel.SelectionAgencyId),
                        "-- Select a Selection Agency --", 
                        new { @class = "mediumInputElement not-required", title="Please select an Agency" })%>
        </p>
        <p>
           <label>Regionally Significant?:</label>
           <%=Html.CheckBox("InfoModel.IsRegionallySignificant",
                Model.ProjectSummary.IsEditable(), 
                Model.InfoModel.IsRegionallySignificant.Value, 
                new { @class = "smallInputElement required", title = "Specify Regional Signifigance!" })%>
        </p>  
        <p><label>Sponsor Notes:</label></p>
        <p>
            <textarea class="longInputElement required" 
                title="Please add the sponsor comments" 
                name="InfoModel.SponsorNotes" 
                rows="10" cols="50"></textarea>
        </p>
        
        <p><label>DRCOG Notes:</label></p>
        <p>
            <textarea class="longInputElement" 
                name="InfoModel.DRCOGNotes" 
                rows="10" cols="50"></textarea>
        </p>
                                   
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <p>
            <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
            <div id="result"></div>
            </p>
        <%} %>
        
        </fieldset>
    <%} %>
</div>
   
<div class="clear"></div>
</div>
</body>
</html>
