<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project General Information</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript" ></script>
<link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.sort.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.popeye-2.0.4.min.js")%>" type="text/javascript"></script>
<link href="<%= Page.ResolveUrl("~/Content/jquery.popeye.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Page.ResolveUrl("~/Content/jquery.popeye.style.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>


<script type="text/javascript">
    var isDirty = false, formSubmittion = false;

    var add1url = '<%=Url.Action("AddCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var remove1url = '<%=Url.Action("DropCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var add2url = '<%=Url.Action("AddCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var remove2url = '<%=Url.Action("DropCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';

    $(document).ready(function () {
        //$('#AvailableAgencies').removeAttr('multiple');
        //$('#Current2Agencies').removeAttr('multiple');

        $(".growable").growing({ buffer: 5 });
//        var ppy1options = {
//            caption: true,
//            navigation: 'hover',
//            direction: 'left'
//        }
//        $('#ppy1').popeye(ppy1options);

        // Prevent accidental navigation away
        $(':input', document.dataForm).bind("change", function () { setConfirmUnload(true); });
        $(':input', document.dataForm).bind("keyup", function () { setConfirmUnload(true); });
        $(':input.nobind', document.dataForm).unbind("change");
        $(':input.nobind', document.dataForm).unbind("keyup");



        //$(':button', document.dataForm).unbind("keyup", function() { setConfirmUnload(true); }); // Want to not do this for my hyperlink buttons. -DBD
        //disable the onbeforeunload message if we are using the submitform button
        if ($('#submitForm')) {
            $('#submitForm').click(function () { window.onbeforeunload = null; return true; });
        }

        if ($('#submitImageForm')) {
            $('#submitImageForm').click(function () { window.onbeforeunload = null; return true; });
        }

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    dataType: 'json',
                    success: function (response) {
                        //$('#result').html(response.message).addClass('success').show();
                        //$('#submitForm').addClass('ui-state-disabled');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("error");
                        //$('#result').text(data.message);
                        //$('#result').addClass('error');
                    }
                });
            }
        });

        $('#add1').click(function () {
            $('#AvailableAgencies option:selected').each(function (i) {
                //make callback add to Eligible Agency list
                add1Agency($(this).val());
            });
            return false;
        });

        $('#remove1').click(function () {
            $('#Current1Agencies option:selected').each(function (i) {
                remove1Agency($(this).val());
            });
            return false;
        });

        $('#add2').click(function () {
            $('#AvailableAgencies option:selected').each(function (i) {
                //make callback add to Eligible Agency list
                add2Agency($(this).val());
            });
            return false;
        });

        $('#remove2').click(function () {
            $('#Current2Agencies option:selected').each(function (i) {
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
                success: function (response) {
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        var selector = "#AvailableAgencies option[value='" + id + "']";
                        var sponsorId = $('#PrimarySponsorId');
                        var oldPrimarySponsor = $('#PrimarySponsor').html();

                        $('#AvailableAgencies').prepend('<option value="' + sponsorId.val() + '">' + oldPrimarySponsor + '</option>');
                        $('#PrimarySponsor').html($(selector).text());
                        $(selector).remove();
                        sponsorId.val(id);
                        $('#AvailableAgencies option').sort(function (a, b) {
                            return $(a).text() > $(b).text() ? 1 : -1;
                        });
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
                success: function (response) {
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

        function remove2Agency(id) {
            $.ajax({
                type: "POST",
                url: remove2url,
                dataType: "json",
                data: { agencyId: id },
                success: function (response) {
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        var selector = "#Current2Agencies option[value='" + id + "']";
                        var selector2 = "#AvailableAgencies";
                        $(selector).remove().prependTo('#AvailableAgencies');
                        $('#AvailableAgencies option').sort(function (a, b) {
                            return $(a).text() > $(b).text() ? 1 : -1;
                        });
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
                Ok: function () {
                    $(this).dialog('close');
                }
            }
        });

        function setConfirmUnload(on) {
            $('#submitForm').removeClass('ui-state-disabled');
            $('#result').html("").hide();
            window.onbeforeunload = (on) ? unloadMessage : null;
        }

        function unloadMessage() {
            return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
        }

        $('#SponsorId').change(function () {
            var sponsorId = $('#SponsorId').val();
            $.getJSON('<%= Url.Action("UpdateAvailableSponsorContacts")%>/' + sponsorId, null, function (data) {
                $('#SponsorContactId').fillSelect(data);
            });
        });

        var tabinforightheight = $("#tab-info-right").height();
        var uploadwrapperheight = $("#uploadwrapper").height();



        $("#uploadwrapper").css('position', 'absolute');
        $("#uploadwrapper").css('top', tabinforightheight + 40);
        $("#uploadwrapper").css('right', '0px');

        var pageheight = $(".page").height();
        var pagesizeleft = (pageheight - (tabinforightheight + uploadwrapperheight + 40 + 193));

        if (pagesizeleft < 0) {
            // need to grow the size of the page
            $(".page").height(pageheight + 40);
        }

        $('#InfoModel_ImprovementTypeId').bind('change', function () {
            var improvementtypeid = $('#InfoModel_ImprovementTypeId :selected').val();
            improvementtypeid = parseInt(improvementtypeid);

            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetImprovementTypeMatch")%>',
                data: "id=" + improvementtypeid,
                dataType: "json",
                success: function (response) {
                    $('#InfoModel_ProjectTypeId').val(response.id);
                },
                error: function (response) {
                    $('#result').html(response.error);
                }
            });
        });

        $('#InfoModel_ProjectTypeId').bind('change', function () {
            var projecttypeid = $('#InfoModel_ProjectTypeId :selected').val();
            var parsed = parseInt(projecttypeid);
            projecttypeid = !isNaN(parsed) ? parsed : 0;
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetProjectTypeMatch")%>',
                data: "id=" + projecttypeid,
                dataType: "json",
                success: function (response) {
                    $('#InfoModel_ImprovementTypeId').removeOption(/./);
                    $('#InfoModel_ImprovementTypeId')
                        .fillSelect(response.data, { 'defaultOptionText': '-- Select Improvement Type --' })
                        .sortOptions();
                },
                error: function (response) {
                    $('#result').html(response.data);
                }
            });
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/Project/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    
    <div class="tab-form-container">    
    
    <% if (ViewData["message"] != null) { %>
        <div id="message" class="info"><%= ViewData["message"].ToString() %></div>
    <% } %>
        
    <% using (Html.BeginForm("UpdateInfo", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
    <%{ %>
        <fieldset>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("InfoModel.TipYear", Model.InfoModel.TipYear)%>         
        <%= Html.Hidden("InfoModel.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
        <%= Html.Hidden("InfoModel.ProjectId", Model.ProjectSummary.ProjectId)%>
        <div id="tab-info-left">
        <p>
            <label>Project Name:</label>
            <%= Html.DrcogTextBox("InfoModel.ProjectName", 
                    (Model.ProjectSummary.IsEditable()), 
                    Model.InfoModel.ProjectName, 
                    new { @class = "longInputElement required", title="Please enter a project title.", @MAXLENGTH = 100 })%>
        </p>
        <p>
        <div id="currentSponsorsForm" class="tab-info-sponsor">
            <label>Sponsor(s):</label>
            <br />
            <table border="1" rules="none">
            <tr>
                <td>Primary Sponsor:</td>
                <% if(Model.ProjectSummary.IsEditable()) { %>
                    <td>&nbsp;</td>
                    <td>Available Agencies:</td>
                <% } %>
            </tr>
            <tr>
                <td>
                    <span id="PrimarySponsor" class="fakeinput medium" style="margin: 0 0 0 3px;"><%= Html.Encode(Model.ProjectSponsorsModel.PrimarySponsor.OrganizationName) %></span>
                    <%= Html.Hidden("PrimarySponsorId", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId) %>
                </td>
                <% if(Model.ProjectSummary.IsEditable()) { %>
                    <td>
                            <%--<a href="#" id="remove1" ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />--%>
                            <a href="#" id="add1" ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                    </td>
                
                    <td rowspan="3" >
                        <%= Html.ListBox("AvailableAgencies", 
                            new SelectList(Model.ProjectSponsorsModel.GetAvailableAgencySelectList().Items, "OrganizationId", "OrganizationName"),
                                                    new { @class = "mediumInputElement nobind", size = 10 })%><br/>
                    </td>
                <% } %>
            </tr>
            <tr>
                <td>Secondary Sponsor(s):</td>
                <% if(Model.ProjectSummary.IsEditable()) { %>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                <% } %>
            </tr>
            <tr>
                <td>
                    <%= Html.ListBox("Current2Agencies", 
                        new SelectList(Model.ProjectSponsorsModel.GetCurrent2AgencySelectList().Items, "OrganizationId", "OrganizationName"),
                                                new { @class = "mediumInputElement nobind", size = 5 })%><br/>        
                </td>
                <%if (Model.ProjectSummary.IsActive && Model.ProjectSummary.IsEditable()) { %>
                    <td>
                        <a href="#" id="remove2" ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                        <a href="#" id="add2" ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                    </td>
                    <td>&nbsp;</td>
                <% } %>
            </tr>
            </table>
            <% if(Model.ProjectSummary.IsEditable()) { %>
                <span>Note: Changes are stored to the database as they are made in the interface.</span>
            <% } %>
        </div>
        </p>
        <% if(Model.ProjectSummary.IsEditable()) { %>
        <p>
            <label>Sponsor Contact:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.SponsorContactId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.InfoModel.SponsorContactId),
                    "-- Select a Sponsor Contact --",
                    new { @class = "mediumInputElement not-required", title = "Please select a project sponsor" })%>
            <%}
              else
              {%> 
            <% string value = "None Selected"; if (Model.InfoModel.SponsorContactId != null) { Model.AvailableSponsorContacts.TryGetValue((int)Model.InfoModel.SponsorContactId, out value); } %>
            <span class="fakeinput medium"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("InfoModel.SponsorContactId", Model.InfoModel.SponsorContactId)%>
            <br />
            <% } %>
        </p>
        <% } %>
        <% if(Model.ProjectSummary.IsEditable()) { %>
        <p>
            <label>Admin Level:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.AdministrativeLevelId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId),
                        "-- Select Admin Level --",
                        new { @class = "mediumInputElement required", title = "Please select an Admin Level" })%>
            <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.AdministrativeLevelId != null) { Model.AvailableAdminLevels.TryGetValue((int)Model.InfoModel.AdministrativeLevelId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.AdministrativeLevelId", Model.InfoModel.AdministrativeLevelId)%>
                <br />
            <% } %>
        </p>
        <% } %>
       
        <p>
            <label>TIP ID:</label>
            <span class="fakeinput medium"><%= Html.Encode(Model.ProjectSummary.TipId)%></span>
        </p>
        <p>
            <label>STIP ID:</label>
            <%= Html.DrcogTextBox("InfoModel.STIPID", Model.ProjectSummary.IsEditable(), Model.InfoModel.STIPID, null)%>
            
        </p>

        <p>
            <label>Project Type:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.ProjectTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableProjectTypes, "key", "value", Model.InfoModel.ProjectTypeId), 
                    "-- Select Project Type--", 
                    new { @class = "mediumInputElement required", title="Please select a project type" })%>
            <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.ProjectTypeId != null) { Model.AvailableProjectTypes.TryGetValue((int)Model.InfoModel.ProjectTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.ProjectTypeId", Model.InfoModel.ProjectTypeId)%>
                <br />
            <% } %>
        </p>
         <p>
            <label>Improvement Type:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.ImprovementTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.InfoModel.ImprovementTypeId), 
                    "-- Select Improvment Type--", 
                    new { @class = "mediumInputElement required", title="Please select an improvment type." })%>
            <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.ImprovementTypeId != null) { Model.AvailableImprovementTypes.TryGetValue((int)Model.InfoModel.ImprovementTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.ImprovementTypeId", Model.InfoModel.ImprovementTypeId)%>
                <br />
            <% } %>
        </p>
        <p>
            <label>Road or Transit:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.TransportationTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.InfoModel.TransportationTypeId), 
                    "-- Select --", 
                    new { @class = "mediumInputElement required", title="Please specify if this is a Road or transit project" })%>
             <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.TransportationTypeId != null) { Model.AvailableRoadOrTransitTypes.TryGetValue((int)Model.InfoModel.TransportationTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.TransportationTypeId", Model.InfoModel.TransportationTypeId)%>
                <br />
            <% } %>   
        </p>
        <p>
            <label>Pool Name:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.ProjectPoolId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailablePools, "key", "value", Model.InfoModel.ProjectPoolId), 
                    "-- Select Pool --",
                    new { @class = "mediumInputElement not-required", title = "Please specify a pool for this project (if applicable)" })%>
            <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.ProjectPoolId != null) { Model.AvailablePools.TryGetValue((int)Model.InfoModel.ProjectPoolId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.ProjectPoolId", Model.InfoModel.ProjectPoolId)%>
                <br />
            <% } %> 
        </p>
        <%--<br />--%>
        <%--<p>
            <label>Pool Master:</label>                               
            <br />
        </p>--%>
        <p>
            <label>Selection Agency:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.SelectionAgencyId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.InfoModel.SelectionAgencyId),
                        "-- Select a Selection Agency --", 
                        new { @class = "mediumInputElement not-required", title="Please select an Agency" })%>
            <%}
              else
              {%> 
                <% string value = "None Selected"; if (Model.InfoModel.SelectionAgencyId != null) { Model.AvailableSelectionAgencies.TryGetValue((int)Model.InfoModel.SelectionAgencyId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.SelectionAgencyId", Model.InfoModel.SelectionAgencyId)%>
                <br />
            <% } %> 
        </p>
        <p>
           <label>Regionally Significant?:</label>
           <%if (Model.ProjectSummary.IsEditable())
              { %>
               <%=Html.CheckBox("InfoModel.IsRegionallySignificant",
                    Model.ProjectSummary.IsEditable(), 
                    Model.InfoModel.IsRegionallySignificant.Value, 
                    new { @class = "smallInputElement not-required", title = "Specify Regional Signifigance!" })%>
            <%}
              else
              {%> 
                <% string value = "No"; if (Model.InfoModel.IsRegionallySignificant != null) { value =  (bool)Model.InfoModel.IsRegionallySignificant ? "Yes" : "No"; } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.IsRegionallySignificant", Model.InfoModel.IsRegionallySignificant)%>
                <br />
            <% } %> 
        </p>
        </div>
        <div id="tab-info-right">
            <% if(Request.IsAuthenticated) { %>
            <p><label>Sponsor Notes:</label></p>
            <p>
                <%= Html.TextArea2("InfoModel_SponsorNotes", 
                    Model.ProjectSummary.IsEditable(),
                    Model.InfoModel.SponsorNotes,
                    0,
                    0,
                    new { @name = "InfoModel.SponsorNotes", @class = "not-required mediumInputElement growable", @rows = "0", title = "Please add the sponsor comments." })%>
            </p>
        
            <p><label>DRCOG Notes:</label></p>
            <p>
                <%= Html.TextArea2("InfoModel_DRCOGNotes",
                    Model.ProjectSummary.IsEditable(),
                    Model.InfoModel.DRCOGNotes,
                    0,
                    0,
                    new { @name = "InfoModel.DRCOGNotes", @class = "mediumInputElement growable" })%>
            </p>
            <% } %>
            <span id="uploadplaceholder"></span>
        </div>
        <div class="clear"></div> 
        
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <div class="relative">
            <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
            <div id="result" class="relative"></div>
            </div>
        <%} %>
        
        
        </fieldset>
    <%} %>
</div>
   
<div class="clear"></div>
</div>

</asp:Content>



