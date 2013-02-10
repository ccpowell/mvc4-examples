<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.StatusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RTP Status</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>"
        type="text/javascript"></script>
    <script type="text/javascript">
    var isDirty = false, formSubmittion = false;
    //var addurl = '<%=Url.Action("AddCycle","RTP", new {plan=Model.RtpSummary.RtpYear}) %>';
    //var removeurl = '<%=Url.Action("DropCycle","RTP") %>';
    //var createCycleUrl = '<%=Url.Action("CreateCycle","RTP") %>';
    var GetPlanAvailableProjects = '<%=Url.Action("GetPlanAvailableProjects","RTP") %>';
    //var SetActiveCycle = '<%=Url.Action("SetActiveCycle","RTP") %>';
    //var updateCycleSortUrl = '<%=Url.Action("UpdateCycleSort","RTP") %>';
    var updateTimePeriodStatusIdUrl = '<%=Url.Action("UpdateTimePeriodStatusId","RTP") %>';
    var SetSurveyDatesUrl = '<%=Url.Action("SetSurveyDates","RTP") %>';
    var GetSurveyDatesUrl = '<%=Url.Action("GetSurveyDates","RTP") %>';
    var OpenSurveyNowUrl = '<%=Url.Action("OpenSurveyNow","RTP") %>';
    var CloseSurveyNowUrl = '<%=Url.Action("CloseSurveyNow","RTP") %>';

    var GetAmendableProjects = '<%=Url.Action("GetAmendableSurveyProjects","RTP") %>'; //GetAvailableRestoreProjects
    var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
    var CreateSurveyUrl = '<%=Url.Action("CreateSurvey","RTP") %>';

    var isEditable = <%= Model.RtpSummary.IsEditable().ToString().ToLower() %>;
    
    $(document).ready(function() {
        $(".growable").growing({buffer:5});
        
        if (isEditable) {
            //setup the date pickers
            $(".datepicker").datepicker();
        
            // Prevent accidental navigation away
            App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
            $('#submitForm').button({disabled: true});
        }
        
        function SurveyAction(id, action) {
            switch (action) {
                case "1"://SetOpen
                case "2"://SetClose
                    OpenSurveyDateDialog(id);
                    break;
                case "3"://Open
                    //SetSurveyDates(id, "<%= DateTime.UtcNow %>", null);
                    OpenSurveyDateDialog(id);
                    //OpenSurveyNow(id);
                    break;
                case "4"://Close
                    //alert("close now");
                    OpenSurveyDateDialog(id);
                    //CloseSurveyNow(id);
                    break;
            }
            return false;
        }
        
        function SurveyChangeButtonAction(id, action) {
            id.removeClass();
            
            id.addClass("action_" + action + " fg-button w75 ui-state-default ui-corner-all");
        }
        
        function SetSurveyDates(id, open, close) {
            //alert("Values: " + id + " " + open + " " + " " + close);

            $.ajax({
                type: "POST",
                url: SetSurveyDatesUrl,
                data: "Id=" + id
                    + "&OpeningDate=" + open
                    + "&ClosingDate=" + close,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        //$('').html(response.message);
                        //$('').addClass('success');
                        //autoHide(2500);
                        location.reload();
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        
        function OpenSurveyNow(id) {
            var element = $("#btn_survey_" + id);
            $.ajax({
                type: "POST",
                url: OpenSurveyNowUrl,
                data: "Id=" + id,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        $("#survey-result").addClass("success").html(response.message).autoHide();
                        
                        SurveyChangeButtonAction(element, 4);
                        
                        var currentTime = new Date();
                        var month = currentTime.getMonth() + 1;
                        var day = currentTime.getDate();
                        var year = currentTime.getFullYear();
                        
                        $("#survey_status_" + id).text("Opened " + month + "/" + day + "/" + year);
                        $(element).text("Close Now");
                        
                        //$('').addClass('success');
                        //autoHide(2500);
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        function CloseSurveyNow(id) {
            var element = $("#btn_survey_" + id);
            $.ajax({
                type: "POST",
                url: CloseSurveyNowUrl,
                data: "Id=" + id,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        //$('').html(response.message);
                        //$('').addClass('success');
                        $("#survey-result").addClass("success").html(response.message).autoHide();
                        
                        SurveyChangeButtonAction(element, 3);
                        
                        var currentTime = new Date();
                        var month = currentTime.getMonth() + 1;
                        var day = currentTime.getDate();
                        var year = currentTime.getFullYear();
                        
                        $("#survey_status_" + id).text("Closed " + month + "/" + day + "/" + year);
                        $(element).text("Open");
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        
        function OpenSurveyDateDialog(surveyid) {
            var id = $("#dialog_SurveyId").val(surveyid);
            var open = $("#dialog_SurveyOpen");
            var close = $("#dialog_SurveyClose");
            var openraw, closeraw;
            
            $.fn.colorbox({
                    width: "560px",
                    height: "240px",
                    inline: true,
                    href: "#dialog-set-survey-date",
                    onLoad: function() {
                        $.ajax({
                            type: "POST",
                            url: GetSurveyDatesUrl,
                            data: "Id=" + surveyid,
                            dataType: "json",
                            success: function(response, textStatus, XMLHttpRequest) {
                                if (response.error == "false") {
                                    $("#survey-result").addClass("success").html(response.message).autoHide();

                                    openraw = new Date(response.data.OpeningDateForJavascript);
                                    closeraw = new Date(response.data.ClosingDateForJavascript);

                                    open.val(openraw.getMonth() + 1 + "/" + (openraw.getDate() + 1) + "/" + openraw.getFullYear());
                                    close.val(closeraw.getMonth() + 1 + "/" + (closeraw.getDate() + 1) + "/" + closeraw.getFullYear());
                                } else {
                                    alert(response.exceptionMessage);
                                }
                            },
                            error: function(response, textStatus, AjaxException) {
                                alert(response.exceptionMessage);
                            }
                        });
                    },
                    onComplete: function() {
                        var $buttonCreateCycle = $('<span id="button-Set-Survey-dates" class="cboxBtn">Set Dates</span>').appendTo('#cboxContent');
                        $("#button-Set-Survey-dates").click(function() {
                            SetSurveyDates(id.val(), open.val(), close.val());
                        });
                    },
                    onClosed: function() {
                        $('.cboxBtn').remove();
                        id.val("");
                        open.val("");
                        close.val("");
                    }
                });
        }
        
        $('span[id^=btn_survey_]').live("click",function() {
         
            var element = $(this);
            var id = $(element).attr("id").replace("btn_survey_","");
            // get first class name which describes the action to take
            var action = $(element).attr('class').split(' ').slice(0, 1).toString().replace("action_", "");
            SurveyAction(id,action);
            
            return false;
        });

        $('span[id=btn_createsurvey]').live("click",function() {
            var surveyname = $("#new_survey #surveyname").val(),
                data = {
                    planId: timePeriodId,
                    surveyName: surveyname
                };
            // create the survey
            // after survey is created, set the dates
            $.post(CreateSurveyUrl, data, function(result) {
                OpenSurveyDateDialog(result.data);
            });

            return false;
        });
             
        
        function checkable( element ) {
	        return /radio|checkbox/i.test(element.type);
        }
        
        function getLength(value, element) {
		    switch( element.nodeName.toLowerCase() ) {
		        case 'select':
			        return $("option:selected", element).length;
		        case 'input':
			        if( this.checkable( element) )
				        return this.findByName(element.name).filter(':checked').length;
		    }
		    return value.length;
        }

        
        $('#addProject').click(function() {
            $('#availableProjects option:selected').each(function(i) {
                $("#availableProjects option[value='" + $(this).val() + "']").remove().prependTo('#selectedProjects').attr('selected',false);
            });
            return false;
        });
        
        $('#removeProject').click(function() {
            $('#selectedProjects option:selected').each(function(i) {
                $("#selectedProjects option[value='" + $(this).val() + "']").remove().prependTo('#availableProjects').attr('selected',false);
            });
            return false;
        });
        
        
        $('#button-plan-unlock').live("click", function() {
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            var statusId = "<%= Model.RtpSummary.GetNextStatus() %>";
            //alert(statusId);
            var message = "Are you sure you want to unlock this plan?";

            if (confirm(message) == true) {
            
                if(updateTimePeriodStatusId(timePeriodId, statusId)) {
                    //$("#button-plan-unlock").hide();
                    location.reload();
                
                }

                location.reload();
            }
            return false;
        });

        $('#button-plan-close').live("click", function() {
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            var statusId = "<%= (int)DRCOG.Domain.Enums.RtpTimePeriodStatus.Inactive %>";
            
            var message = "Are you sure you want to close this plan?";

            if (confirm(message) == true) {
                if(updateTimePeriodStatusId(timePeriodId, statusId)) {
                    //$("#button-plan-unlock").hide();
                    location.reload();
                }
            }
            return false;
        });

    });

    // *** General functions

    function autoHide(timeout) {
        if (isNaN(timeout)) timeout = 5000;
        setTimeout(function() {
            $("div#result").fadeOut("slow", function() {
            $("div#result").empty().removeClass().removeAttr('style');
            });
        }, timeout);
    }
        
    function updateTimePeriodStatusId(timePeriodId, statusId) {
        $.ajax({
            type: "POST",
            url: updateTimePeriodStatusIdUrl,
            data: { timePeriodId: timePeriodId, statusId: statusId },
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    return true;
                    //$('div#resultRecordDetail').html(response.message);
                    //$('div#resultRecordDetail').addClass('success');
                    //autoHide(2500);
                }
                else {
                    //$('div.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                    //$('div.dialog-result').addClass('error').show();
                    //autoHide(10000);
                }

            },
            error: function(response, textStatus, AjaxException) {
                //$('div.dialog-result').html(response.statusText);
                //$('div.dialog-result').addClass('error').show();
                //autoHide(10000);
            }
        });
    }

   
    
    function getAvailableProjects(selected) {
        $.ajax({
            type: "POST",
            url: GetPlanAvailableProjects,
            data: "planId=" + parseInt(<%= Model.RtpStatus.TimePeriodId %>)
                + '&cycleId=' + parseInt(<%= Model.RtpSummary.Cycle.Id %>),
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                $('#availableProjects').clearSelect().addItems(response);
                // scenario sql broken in RTP.tfn_GetProjects 
                // $('#scenario-container').show();
            },
            error: function(response, textStatus, AjaxException) {
                alert("error");
            }
        });
    }
    
    function sortDropDownListByText(list) {
        // Loop for each select element on the page.
        $(list).each(function() {
            // Keep track of the selected option.
            var selectedValue = $(this).val();

            // Sort all the options by text. I could easily sort these by val.
            $(this).html($("option", $(this)).sort(function(a, b) {
                return a.text == b.text ? 0 : a.text < b.text ? -1 : 1
            }));

            // Select one option.
            $(this).val(selectedValue);
        });
    }
    
   $.fn.clearSelect = function() {
        return this.each(function() {
            if (this.tagName == 'SELECT')
                this.options.length = 0;
        });
    };
    $.fn.addItems = function(data) {
        return this.each(function() {
            var list = this;
            $.each(data, function(index, itemData) {
                var option = new Option(itemData.Text, itemData.Value);
                // stupid browser differences. IE does one thing where
                // All other browsers need the null thingy
                if ($.browser.msie) {
                    list.add(option);
                }
                else {
                    list.add(option, null);
                }
            });
            list.remove(''); // removed empty values
        });
    };

   
    </script>
    
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <%--<h2 ><%=Html.ActionLink("RTP List", "Index",new {controller="RTP"}) %> / RTP <%=Model.RtpSummary.RtpYear%></h2>--%>
        <div class="clear">
        </div>
        <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
        <div id="StatusForm" class="tab-form-container">
            <div id="StatusForm-wrapper">
            <form method="put" action="/api/RtpStatus" id="dataForm">
                <fieldset>
                <legend></legend>
                    <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
                    <h2>
                        Program Start and End Dates</h2>
                    <%=Html.Hidden("TimePeriodId", Model.RtpStatus.TimePeriodId)%>
                    <%=Html.Hidden("ProgramId", Model.RtpStatus.ProgramId)%>
                    <p>
                        <label>
                            Plan:</label>
                        <%--<%=Html.DrcogTextBox("Plan", Model.RtpSummary.IsCurrent, Model.RtpSummary.RtpYear, new { @id = "Plan", @class = "required", title = "Please specify a tip year (i.e. 2008-2013)" })%>--%>
                        <span class="fakeinput">
                            <%= Html.Encode(Model.RtpSummary.RtpYear) %></span>
                        <%= Html.Hidden("Plan", Model.RtpSummary.RtpYear, new { @id = "Plan" }) %>
                        <br />
                    </p>
                    <p>
                        <label>
                            Base Year:</label>
                        <%= Html.DropDownList("BaseYearId",
                        Model.RtpSummary.IsEditable(),
                        new SelectList(Model.AvailableYears, "key", "value", Model.RtpStatus.BaseYearId), 
                        "-- Select --",
                        new { title = "Please select a financial year.", @id = "BaseYearId", @class = "cycle-required" })%>
                        <%--<%=Html.DrcogTextBox("BaseYear", Model.RtpSummary.IsCurrent, Model.RtpStatus.BaseYear, new { @id = "BaseYear", @class = "required", title = "Please specify a base year" })%>--%>
                        <br />
                    </p>
                    <%--<p>
                    <label for="CurrentStatus">Current:</label>
                    <%= Html.CheckBox("IsCurrent", Model.IsEditable(), Model.RtpStatus.IsCurrent, new { @id = "CurrentStatus" })%>
                </p>
                <p>
                    <label for="PendingStatus">Pending:</label>
                    <%= Html.CheckBox("IsPending", Model.IsEditable(), Model.RtpStatus.IsPending, new { @id = "PendingStatus" })%>
                </p>
                <p>
                    <label for="PreviousStatus">Previous:</label>
                    <%= Html.CheckBox("IsPrevious", Model.IsEditable(), Model.RtpStatus.IsPrevious, new { @id = "PreviousStatus" })%>
                </p>
                    --%>
                    <h2>
                        RTP Meeting and Approval Dates</h2>
                    <p>
                        <label for="summary_PublicHearing" class="beside">
                            Public Hearing:</label>
                        <%= Html.DrcogTextBox("PublicHearing", Model.RtpSummary.IsEditable(), Model.RtpStatus.PublicHearing.HasValue ? Model.RtpStatus.PublicHearing.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                    </p>
                    <p>
                        <label for="summary_BoardApproval">
                            Board Adoption:</label>
                        <%= Html.DrcogTextBox("Adoption", Model.RtpSummary.IsEditable(), Model.RtpStatus.Adoption.HasValue ? Model.RtpStatus.Adoption.Value.ToShortDateString() : "", new { @class = "datepicker" })%>
                    </p>
                    <p>
                        <label for="summary_LastAmended">
                            Last Amended:</label>
                        <span class="fakeinput">
                            <%= Model.RtpStatus.LastAmended.HasValue ? Html.Encode(Model.RtpStatus.LastAmended.Value.ToShortDateString()) : String.Empty %></span>
                        <%--<%= Html.DrcogTextBox("LastAmended", Model.IsEditable(), Model.RtpStatus.LastAmended.HasValue ? Model.RtpStatus.LastAmended.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>--%>
                    </p>
                    <p>
                        <label for="CDOTAction">
                            CDOT Action:</label>
                        <%= Html.DrcogTextBox("CDOTAction", Model.RtpSummary.IsEditable(), Model.RtpStatus.CDOTAction.HasValue ? Model.RtpStatus.CDOTAction.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                    </p>
                    <p>
                    <p>
                        <label for="summary_USDOTApproval">
                            U.S. DOT Approval:</label>
                        <%= Html.DrcogTextBox("USDOTApproval", Model.RtpSummary.IsEditable(), Model.RtpStatus.USDOTApproval.HasValue ? Model.RtpStatus.USDOTApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                    </p>
                    <p>
                        <label for="summary_Notes">
                            Notes:</label>
                        <%= Html.TextArea2("Notes", Model.RtpSummary.IsEditable(), Model.RtpStatus.Notes,5,0, new { @name = "Notes", @class = "mediumInputElement growable" })%>
                    </p>
                    <p>
                        <label for="summary_Description">
                            Description:</label>
                        <%= Html.TextArea2("Description", Model.RtpSummary.IsEditable(), Model.RtpStatus.Description, 5, 0, new { @name = "Description", @class = "mediumInputElement growable" })%>
                    </p>
                </fieldset>
                <br />
                <%if (Model.RtpSummary.IsEditable())
                  { %>
                <div>
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="submit-result">
                    </div>
                </div>
                <%} %>
                </form>
            </div>
            <div id="plan-cycles">
                <div class="box">
                    <h2>
                        Plan Survey Administration</h2>
                    <table id="planSurveys">
                        <thead>
                            <tr>
                                <th>
                                    Survey
                                </th>
                                <th>
                                    Status
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <%foreach (DRCOG.Domain.Models.Survey.Survey item in Model.Surveys)
                          { %>
                        <tr id="survey_<%= item.Id %>">
                            <td>
                                <%= Html.ActionLink(item.Name, "Dashboard", new {controller="Survey", year = item.Name }) %>
                            </td>
                            <td id="survey_status_<%= item.Id %>">
                                <%= item.GetStatusText() %>
                            </td>
                            <td>
                                <span id="btn_survey_<%= item.Id %>" style="display: block;" class="action_<%= item.ButtonActionCode %> fg-button w100 ui-state-default ui-corner-all">
                                    <%= item.ButtonActionText %></span>
                            </td>
                        </tr>
                        <% } %>
                        <tr>
                            <td colspan="3">
                                New Survey Name
                            </td>
                        </tr>
                        <tr id="new_survey">
                            <td colspan="2">
                                <input type="text" id="surveyname" name="surveyname" maxlength="50" size="30" />
                            </td>
                            <td>
                                <span id="btn_createsurvey" style="display: block;" class="fg-button w100 ui-state-default ui-corner-all">
                                    Create New</span>
                            </td>
                        </tr>
                    </table>
                    <div id="survey-result">
                    </div>
                </div>
                <% if (!Model.RtpStatus.BaseYearId.Equals(0))
                   { %>
                <%--<table>
                    <tr>
                        <%if (!Model.RtpSummary.IsCurrent)
                          { %>
                            <td>Available Cycles:</td>
                            <td>&nbsp;</td>
                        <%} %>
                        <td>Current Cycles:</td>
                    </tr>
                    <tr>
                        <%if (!Model.RtpSummary.IsCurrent)
                          { %>
                        <td>
                            <%= Html.ListBox("AvailableCycles", new MultiSelectList(Model.GetAvailableCyclesSelectList().Items, "Key", "Value"), new { @class = "w150 nobind", size = 10 })%><br/>        
                        </td>
                        <td>
                            <a href="#" id="add"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                            <a href="#" id="remove"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                        </td>
                        <%} %>
                        <td>
                          <%= Html.ListBox("CurrentPlanCycles", new MultiSelectList(Model.GetCurrentPlanCyclesSelectList().Items, "Key", "Value"), new { @class = "w150 nobind", size = 10 })%><br/>
                        </td>
                    </tr>
                </table>--%>
                <div id="cycle-modifier" style="display: none;">
                </div>
                <div id="plan-current-cycle" class="box" style="display: none;">
                    <div id="process-cycle-step1">
                        <h2>
                            Process Cycle</h2>
                        <% if (!String.IsNullOrEmpty(Model.RtpSummary.Cycle.Name))
                           { %>
                        <p>
                            Current:
                            <%= Model.RtpSummary.Cycle.Name%></p>
                        <div id="plan-next-cycle">
                            <label for="NextCycleId" class="boldFont">
                                Next Cycle:</label>
                            <%= Html.DropDownList("NextCycleId",
                                    new SelectList(Model.PlanUnusedCycles, "key", "value"),
                                //new SelectList(Model.CurrentPlanCycles.Where(pair => pair.Value != Model.RtpSummary.Cycle.Name).ToDictionary(pair => pair.Key, pair => pair.Value), "key", "value"), 
                                    "-- Select --",
                                    new { title = "Please select a Cycle.", @id = "cycle-nextCycleId", @class = "nobind" })%>
                            <br />
                            <span id="plan-change-cycle" class="fg-button w100 ui-state-default ui-corner-all ui-state-disabled">
                                Change Cycles</span>
                        </div>
                        <% }
                           else
                           { %>
                        <% } %>
                    </div>
                    <div id="process-cycle-step2" class="dialog" title="Move Projects to new Cycle">
                        <% Html.RenderPartial("~/Views/RTP/Partials/AmendableProjects.ascx"); %>
                    </div>
                </div>
                <% } %>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <div style='display: none'>
        <div id="dialog-create-cycle" class="dialog" title="Create a new Cycle">
            <% Html.RenderPartial("~/Views/RTP/Partials/AddCycle.ascx"); %>
        </div>
    </div>
    <div style='display: none'>
        <div id="dialog-create-scenario" class="dialog" title="Create a new Scenario">
        </div>
        <div id="dialog-set-survey-date" class="dialog">
            <h2>
                Survey Management: Set Open Period</h2>
            <%= Html.Hidden("dialog_SurveyId") %>
            <fieldset>
                <p>
                    <label for="dialog_SurveyOpen" class="big">
                        Open Date:</label>
                    <%= Html.DrcogTextBox("dialog_SurveyOpen", true, DateTime.Now.ToShortDateString(), new { @class = "datepicker big nobind" })%>
                </p>
                <p>
                    <label for="dialog_SurveyClose" class="big">
                        Close Date:</label>
                    <%= Html.DrcogTextBox("dialog_SurveyClose", true, DateTime.Now.ToShortDateString(), new { @class = "datepicker big nobind" })%>
                </p>
            </fieldset>
        </div>
    </div>
    <div style='display: none'>
        <div id="dialog-cycle-projects" class="dialog" title="Move Projects to new Cycle">
            <% Html.RenderPartial("~/Views/RTP/Partials/AmendableProjects.ascx"); %>
        </div>
    </div>
    <% Html.RenderPartial("~/Views/Survey/Partials/AmendPartial.ascx"); %>
</asp:Content>
