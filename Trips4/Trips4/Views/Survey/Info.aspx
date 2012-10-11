<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project General Information</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server"><%= Model.Current.Name %> Survey Projects</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript" ></script>
<link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.sort.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskedinput-1.2.2.min.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskMoney.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript" ></script>

<script type="text/javascript">
    var isDirty = false, formSubmittion = false;
    var add1url = '<%=Url.Action("AddCurrent1Agency","RtpProject", new {year=Model.Current.Name, projectVersionId=Model.Project.ProjectVersionId}) %>';
    var remove1url = '<%=Url.Action("DropCurrent1Agency","RtpProject", new {year=Model.Current.Name, projectVersionId=Model.Project.ProjectVersionId}) %>';
    var add2url = '<%=Url.Action("AddCurrent2Agency","RtpProject", new {year=Model.Current.Name, projectVersionId=Model.Project.ProjectVersionId}) %>';
    var remove2url = '<%=Url.Action("DropCurrent2Agency","RtpProject", new {year=Model.Current.Name, projectVersionId=Model.Project.ProjectVersionId}) %>';
    var deleteFundingSource = '<%= Url.Action("DeleteFundingSource")%>'
    var addFundingSource = '<%= Url.Action("AddFundingSource")%>'


    $(document).ready(function () {
        $(".growable").growing({ buffer: 10 });

        var projectVersionId = <%= Model.Project.ProjectVersionId %>;


        var tabinforight = $("#tab-info-right");
        var tabinforightheight = $(tabinforight).height();

        $(tabinforight).css('top', 100);

        var pageheight = $(".page").height();
        var pagesizeleft = (pageheight - (tabinforightheight + $(tabinforight).position().top + 68));

        if (pagesizeleft < 0) {
            // need to grow the size of the page
            $(".page").height(pageheight + 40);
        }

        $(".money").maskMoney({ symbol: "$", thousands: ",", precision: 0, allowNegative: false, showSymbol: true, symbolStay: true, thousandsStay: true });

        function removeMask() {
            $(".money").unmaskMoney();
        }
        // Prevent accidental navigation away
        $(':input', document.dataForm).bind("change", function () { setConfirmUnload(true); });
        $(':input', document.dataForm).bind("keyup", function () { setConfirmUnload(true); });
        $(':input.nobind', document.dataForm).unbind("change");
        $(':input.nobind', document.dataForm).unbind("keyup");

        var $scrollingDiv = $("#update-inview");

        $(window).scroll(function () {
            $scrollingDiv
			.stop()
			.animate({ "marginTop": ($(window).scrollTop() + 80) + "px" }, "slow");
        });

        if ($('#submitForm')) {
            $('#submitForm').click(function () { 
                window.onbeforeunload = null; 
                
                return true; 
            });
        }

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    beforeSubmit: cleanRequest,
                    dataType: 'json',
                    success: function (response) {
                        //alert(response.error == 'undefined');
                        if (typeof response.error == 'undefined' || response.error == '') {
                            $('#site-message').text(response.message).removeClass('error').addClass('success').show().delay(5000).slideUp(300);
                            $('#submitForm').addClass('ui-state-disabled');
                            $('#submitForm').removeClass('bg-green');
                            $("#actionbar").show();
                        } else {
                            $('#site-message').html(response.error).addClass('error').show().delay(5000).slideUp(300);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $('#site-message').text(textStatus);
                        $('#site-message').addClass('error').show().delay(5000).slideUp(300);
                        $('#submitForm').addClass('ui-state-disabled');
                        $('#submitForm').removeClass('bg-green');
                    }
                });
            }
        });

        function cleanRequest(formData, jqForm, options) {
            formData[9].value = formData[9].value.replace('$', '').replace(',', ''); // clean money (TotalCost)
        }

        // pre-submit callback 
        function showRequest(formData, jqForm, options) {
            // formData is an array; here we use $.param to convert it to a string to display it
            // but the form plugin does this for you automatically when it submits the data

            var queryString = $.param(formData);

            // jqForm is a jQuery object encapsulating the form element.  To access the 
            // DOM element for the form do this: 
            // var formElement = jqForm[0]; 

            alert('About to submit: \n\n' + queryString);

            // here we could return false to prevent the form from being submitted; 
            // returning anything other than false will allow the form submit to continue 
            return true;
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
            $('#submitForm').addClass('bg-green');
            $('#site-message').html("").hide();
            //$("#actionbar").hide();
            window.onbeforeunload = (on) ? unloadMessage : null;
        }

        function unloadMessage() {
            return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
        }

        $('#ProjectSponsorsModel_PrimarySponsor_OrganizationId').change(function () {
            var sponsorId = $('#ProjectSponsorsModel_PrimarySponsor_OrganizationId').val();

            $.ajax({
                type: "POST",
                url: '<%= Url.Action("UpdateAvailableSponsorContacts")%>',
                data: "id=" + sponsorId,
                dataType: "json",
                success: function (response) {
                    //alert(response.id + ' ' + response.value);
                    $('#Project_SponsorContactId').fillSelect(response);
                },
                error: function (response) {
                    //$('#result').html(response.error);
                }
            });
        });

        $('#InfoModel_ImprovementTypeId').bind('change', function () {
            var improvementtypeid = $('#InfoModel_ImprovementTypeId :selected').val();
            improvementtypeid = parseInt(improvementtypeid);

            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetImprovementTypeMatch")%>',
                data: "id=" + improvementtypeid,
                dataType: "json",
                success: function (response) {
                    //alert(response.id + ' ' + response.value);
                    $('#InfoModel_ProjectTypeId').val(response.id);
                    $('#InfoModel_ProjectType').text(response.value);
                },
                error: function (response) {
                    $('#site-message').html(response.error);
                }
            });
        });

        $('select[id^=fundingsource_new]').change(function () {
            if ($("#fundingsource_new_name :selected").val() > 0) {
                if ($('#btn-fundingsource-new').hasClass('ui-state-disabled')) {
                    $('#btn-fundingsource-new').removeClass('ui-state-disabled').addClass("bg-green").removeAttr('disabled');
                }
            } else {
                $('#btn-fundingsource-new').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('#btn-fundingsource-new').removeClass("bg-green");
            }
        });

        $('.fundingsource-delete').live("click", function () {
            var resourceId = this.id.replace('fundingsource_delete_', '');
            $.ajax({
                type: "POST",
                url: deleteFundingSource,
                data: 'projectVersionId=' + projectVersionId
                + '&fundingResourceId=' + resourceId,
                dataType: "json",
                success: function (response) {
                    //$('#result').html(response.message);
                    var obj = { resourceId: resourceId, value: $("#fundingsource_" + resourceId + "_name").text() };
                    copyFundingOption(obj);
                    $('#fundingsource_row_' + resourceId).empty();
                    window.onbeforeunload = null;
                }
            });
            return false;
        });

        //Add a funding resource
        $("#btn-fundingsource-new").live("click", function() {
            if ($('#btn-fundingsource-new').hasClass('ui-state-disabled')) { return false; }
            var fundingResource = $("#fundingsource_new_name :selected");
            var fundingResourceId = fundingResource.val();

            $.ajax({
                type: "POST",
                url: addFundingSource,
                data: 'projectVersionId=' + projectVersionId
                    + '&fundingResourceId=' + fundingResourceId,
                dataType: "json",
                success: function(response) {
                    //$('#result').html(response.message);
                    var resource = {
                        'r1_fundingResourceId': fundingResourceId,
                        'r1_fundingResourceName': fundingResource.text()
                    };

                    var content = '<tr id="fundingsource_row_r1_fundingResourceId">';
                    content += '<td><span class="fakeinput w250" id="fundingsource_r1_fundingResourceId_name">r1_fundingResourceName</span>'; //<select name="fundingsource_r1_fundingResourceId_name" title="Please select a funding source" class="longInputElement not-required" id="fundingsource_r1_fundingResourceId_name"></td>';
                    content += '<td>';
                    content += '<button class="fundingsource-delete fg-button ui-state-default ui-priority-primary ui-corner-all" id="fundingsource_delete_r1_fundingResourceId" type="submit">Delete</button>';
                    content += "</td>";
                    content += '</tr>';
                    content = replaceFromArray(content, resource);
                    $('#fundingsource-editor').before(content);

                    $("#fundingsource_new_name").copyOptions("#fundingsource_" + fundingResourceId + "_name", "all");
                    $("#fundingsource_" + fundingResourceId + "_name").val(fundingResourceId);

                    removeFundingOption(fundingResourceId);
                    $("#fundingsource_new_name").removeOption(fundingResourceId);

                    $('#btn-fundingsource-new').addClass('ui-state-disabled').attr('disabled', 'disabled');
                    $('#btn-fundingsource-new').removeClass("bg-green");

                    window.onbeforeunload = null;
                }
            });
            return false;
        });
    });

    function copyFundingOption(object) {
        var id, name;
        id = object
        $("#fundingsource_new_name").addOption(object.resourceId, object.value, false);
    }

    function replaceFromArray(string, object) {
        var value = string;
        var intIndexOfMatch;
        for (var index in object) {
            //alert(index + ':' + object[index]);
            intIndexOfMatch = value.indexOf(index);
            while (intIndexOfMatch != -1) {
                value = value.replace(index, object[index]);
            
                intIndexOfMatch = value.indexOf(index);
            }
            //alert(value);
        }
        return value;
    }

    function removeFundingOption(id) {
        $("select[id^=fundingsource_]").each(function() {
            var value = $(this).val();
            if (!(value == id)) {
                $(this).removeOption(id);
            }
        });
    }
/*
    $.fn.clearSelect = function() {
        alert("hello");
        return this.each(function() {
            if (this.tagName == 'SELECT')
                this.options.length = 0;
        });
    }

    $.fn.fillSelect = function(data, options) {
        var defaults = {
            defaultOptionText: '-- Select --',
            defaultOptionValue: 0
        };
        var options = $.extend(defaults, options);

        return this.clearSelect().each(function() {
            if (this.tagName == 'SELECT') {
                var o = options;
                var defaultOption = new Option(o.defaultOptionText, o.defaultOptionValue);
                var dropdownList = this;
                if ($.browser.msie) {
                    dropdownList.add(defaultOption);
                }
                else {
                    dropdownList.add(defaultOption, null);
                }
                alert(data);
                $.each(data, function(index, optionData) {
                    var option = new Option(optionData.Text, optionData.Value);
                    alert(optionData.Text + ' ' + optionData.Value);
                    if ($.browser.msie) {
                        dropdownList.add(option);
                    }
                    else {
                        dropdownList.add(option, null);
                    }
                });
            }
        });
    }
    */
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% string value = String.Empty; %>
<div class="tab-content-container">
    <%Html.RenderPartial("~/Views/Survey/Partials/ProjectTabPartial.ascx", Model); %>
    <div class="tab-form-container">
    
    <% if ( !Model.Project.UpdateStatusId.Equals(default(int)) ) { %>
        
        <h2 id="status">Current Status: <%= Model.Project.UpdateStatus %></h2> 
    <% } %>
    <% using (Html.BeginForm("UpdateInfo", "Survey", FormMethod.Post, new { @id = "dataForm" })) %>
    <%{ %>
        <% if (Model.Current.IsEditable()) { Html.RenderPartial("~/Views/Survey/Partials/ManagerRibbonPartial.ascx", Model); } %>
        <fieldset>
        
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("Current.Name", Model.Current.Name)%>         
        <%= Html.Hidden("Project.ProjectVersionId", Model.Project.ProjectVersionId)%>
        <%= Html.Hidden("Project.ProjectId", Model.Project.ProjectId)%>
        <div id="tab-info-left">
        <p>
            <label>Project Name/Location:</label>
            <%= Html.DrcogTextBox("Project.ProjectName", 
                    (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), 
                    Model.Project.ProjectName.ToString(), 
                    new { @class = "longInputElement required", title="Please enter a project title.", @MAXLENGTH = 100 })%>
            <br />       
        </p>
        <p>
            <label>Primary Sponsor:</label>
            <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
              { %>
            <%= Html.DropDownList("ProjectSponsorsModel.PrimarySponsor.OrganizationId",
                                Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin),
                new SelectList(Model.ProjectSponsorsModel.GetAvailableAgenciesList(), "key", "value", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId),
                "-- Select a Primary Sponsor --", 
                new { @class = "mediumInputElement required", title="Please select a Primary Sponsor" })%>
            <%}
              else
              {%> 
            <% value = "None Selected"; if (Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId != null) { Model.ProjectSponsorsModel.GetAvailableAgenciesList().TryGetValue((int)Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId, out value); } %>
            <span class="fakeinput medium"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("ProjectSponsorsModel.PrimarySponsor.OrganizationId", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId)%>
            <br />
            <% } %>
        </p>
        <p>
            <label>Agency Contact:</label>
            <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
              { %>
                <%= Html.DropDownList("Project.SponsorContactId",
                    Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin),
                    new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.Project.SponsorContactId),
                    "-- Select a Sponsor Contact --",
                    new { @class = "mediumInputElement not-required", title = "Please select a project sponsor" })%>
            <%}
              else
              {%> 
            <% value = "None Selected"; if (Model.Project.SponsorContactId != null) { Model.AvailableSponsorContacts.TryGetValue((int)Model.Project.SponsorContactId, out value); } %>
            <span class="fakeinput medium"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("Project.SponsorContactId", Model.Project.SponsorContactId)%>
            <br />
            <% } %>
        </p>
        <%--<p>
            <label>Admin Level:</label>
            <%if (Model.Project.IsEditable())
              { %>
                <%= Html.DropDownList("Project.AdministrativeLevelId",
                        Model.Project.IsEditable(),
                        new SelectList(Model.AvailableAdminLevels, "key", "value", Model.Project.AdministrativeLevelId),
                        "-- Select Admin Level --",
                        new { @class = "mediumInputElement required", title = "Please select an Admin Level" })%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.Project.AdministrativeLevelId != null) { Model.AvailableAdminLevels.TryGetValue((int)Model.Project.AdministrativeLevelId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("Project.AdministrativeLevelId", Model.Project.AdministrativeLevelId)%>
                <br />
            <% } %>
        </p>--%>
       
        <%--<p>
            <label>Project Type:</label>
            <% value = "None Selected"; if (Model.Project.ProjectTypeId != null) 
               {
                   Model.AvailableProjectTypes.TryGetValue((int)Model.Project.ProjectTypeId, out value); 
               } %>
            <span class="fakeinput medium" id="InfoModel_ProjectType"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("InfoModel.ProjectTypeId", Model.Project.ProjectTypeId)%>
            <br />
        </p>--%>
         <p>
            <label>Improvement Type:</label>
            <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
              { %>
                <%= Html.DropDownList("Project.ImprovementTypeId",
                    (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), 
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.Project.ImprovementTypeId), 
                    "-- Select Improvment Type--", 
                    new { @class = "mediumInputElement required", title="Please select an improvment type." })%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.Project.ImprovementTypeId != null) { Model.AvailableImprovementTypes.TryGetValue((int)Model.Project.ImprovementTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("Project.ImprovementTypeId", Model.Project.ImprovementTypeId)%>
                <br />
            <% } %>
        </p>
        <% 
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencyGroupSeparator = ",";
            nfi.CurrencySymbol = "$";
            nfi.CurrencyDecimalDigits = 0; 
        %>
        <p>
            <label>Total Cost:</label>
            
            <%= Html.DrcogTextBox("Project.Funding.TotalCost", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), Model.Project.Funding.TotalCost.ToString("C", nfi), new { @class = "money w100" })%> &nbsp;($1,000s)
        </p>
        
        <br />
        <br />
        
        <h2>Funding Source(s)</h2>
            <table id="fundingsources">
            <%foreach (DRCOG.Domain.Models.Survey.FundingSource source in Model.Project.FundingSources)
             { 
                  Model.AvailableFundingResources.Remove(source.Id);
                  %>
                <tr id="fundingsource_row_<%=source.Id %>">
                    <td>
                        <span class="fakeinput w250" id="fundingsource_<%= source.Id %>_name"><%= source.Name %></span>
                    </td>
                <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
                  { %>
                    <td><button class="fundingsource-delete fg-button ui-state-default ui-priority-primary ui-corner-all" id='fundingsource_delete_<%=source.Id.ToString() %>'>Delete</button></td>                                
                <%} %> 
                </tr>
            <%} %>
            <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
              { %>
                <tr id="fundingsource-editor">
                <td><%= Html.DropDownList("fundingsource_new_name",
                    (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin),
                    new SelectList(Model.AvailableFundingResources, "key", "value"),
                    "-- Select --",
                    new { @class = "longInputElement not-required nobind", title = "Please select a project sponsor" })%>
                </td>
                <td><button id="btn-fundingsource-new" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all add">Add</button></td>                                
                </tr>                      
            <%} %>
            </table>
        </div>
        <%--<p>
            <label>Road or Transit:</label>
            <%if (Model.Project.IsEditable())
              { %>
                <%= Html.DropDownList("Project.TransportationTypeId",
                    Model.Project.IsEditable(), 
                    new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.Project.TransportationTypeId), 
                    "-- Select --", 
                    new { @class = "mediumInputElement required", title="Please specify if this is a Road or transit project" })%>
             <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.Project.TransportationTypeId != null) { Model.AvailableRoadOrTransitTypes.TryGetValue((int)Model.Project.TransportationTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("Project.TransportationTypeId", Model.Project.TransportationTypeId)%>
                <br />
            <% } %>   
        </p>--%>
        <%--<p>
            <label>Selection Agency:</label>
            <%if (Model.Project.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.SelectionAgencyId",
                        Model.Project.IsEditable(),
                        new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.Project.SelectionAgencyId),
                        "-- Select a Selection Agency --", 
                        new { @class = "mediumInputElement not-required", title="Please select an Agency" })%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.Project.SelectionAgencyId != null) { Model.AvailableSelectionAgencies.TryGetValue((int)Model.Project.SelectionAgencyId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.SelectionAgencyId", Model.Project.SelectionAgencyId)%>
                <br />
            <% } %> 
        </p>--%>
        <%--<p>
           <label>Regionally Significant?:</label>
           <%if (Model.Project.IsEditable())
              { %>
               <%=Html.CheckBox("InfoModel.IsRegionallySignificant",
                    Model.Project.IsEditable(), 
                    Model.InfoModel.IsRegionallySignificant.Value, 
                    new { @class = "smallInputElement not-required", title = "Specify Regional Signifigance!" })%>
            <%}
              else
              {%> 
                <% value = "No"; if (Model.InfoModel.IsRegionallySignificant != null) { value =  (bool)Model.InfoModel.IsRegionallySignificant ? "Yes" : "No"; } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.IsRegionallySignificant", Model.InfoModel.IsRegionallySignificant)%>
                <br />
            <% } %> 
        </p>--%>
        </div>
        <div id="tab-info-right">
        <p><label>Sponsor Notes:</label></p>
        <p>
        <%= Html.TextArea2("Project.SponsorNotes", 
                            (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin),
                            Model.Project.SponsorNotes,
                            0,
                            0,
                            new { @name = "Project.SponsorNotes", @class = "mediumInputElement not-required growable", title = "Please add the sponsor comments." })%>
        </p>
        <% if(Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>
        <p><label>DRCOG Notes:</label></p>
        <p>
        <%= Html.TextArea2("Project.DRCOGNotes",
                            Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin),
                            Model.Project.DRCOGNotes,
                            0,
                            0,
                            new { @name = "Project.DRCOGNotes", @class = "mediumInputElement growable" })%>
        </p>
        <% } %>
        </div>
        <div class="clear"></div> 
        
        <%if((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)){ %>
            <div class="relative" style="top: 80px;" id="update-inview">
            <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
            
            </div>
        <%} %>
        
        
        </fieldset>
    <%} %>
</div>
   
<div class="clear"></div>
</div>

</asp:Content>



