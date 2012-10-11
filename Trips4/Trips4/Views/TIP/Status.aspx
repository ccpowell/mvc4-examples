<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.StatusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP Status</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>


<script type="text/javascript">
    var isDirty = false, formSubmittion = false;

    $(document).ready(function () {
        //setup the date pickers
        $(".datepicker").datepicker();
        $(':input', document.statusForm).bind("change", function () { setConfirmUnload(true); }); // Prevent accidental navigation away
        $(':input', document.statusForm).bind("keyup", function () { setConfirmUnload(true); });
        if ($('#submitForm')) {
            $('#submitForm').click(function () { window.onbeforeunload = null; return true; });
        }
        $("#Notes").growing();

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            //Keep this in $().ready or add a $("#form").ajaxForm(); in $().ready
            submitHandler: function (form) {
                $(form).ajaxSubmit({
                    success: function (response) {
                        $('#result').html(response.message).addClass("success");
                        $('#submitForm').addClass('ui-state-disabled');
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $('#result').text(data.message);
                        $('#result').addClass('error');
                    },
                    dataType: 'json'
                });
            }
        });
    });

    function setConfirmUnload(on) {

        $('#submitForm').removeClass('ui-state-disabled');
        $('#result').html("");        
        window.onbeforeunload = (on) ? unloadMessage : null;
    }

    function unloadMessage() {
        return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
    }


   

   
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="view-content-container">
<%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
<div class="clear"></div>

<%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>

    <div id="tipStatusForm" class="tab-form-container">
    <% using (Html.BeginForm("UpdateStatus", "Tip", FormMethod.Post, new { @id = "dataForm"})) %>
    <%{ %>
        <fieldset>
            <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>

            <h2>Program Start and End Dates</h2>
            <%=Html.Hidden("TimePeriodId", Model.TipStatus.TimePeriodId)%>
            <%=Html.Hidden("ProgramId", Model.TipStatus.ProgramId)%>
            <p>
                <label>TIP Year: </label>
                <%=Html.DrcogTextBox("TipYear",Model.TipSummary.IsCurrent,Model.TipSummary.TipYear, new{@class="required", @style="width: 100px;", title="Please specify a tip year (i.e. 2008-2013)"}) %>
            </p>
            <p>
                <label for="CurrentStatus">Current:</label>
                <%= Html.CheckBox("IsCurrent", Model.IsEditable(), Model.TipStatus.IsCurrent, new { @id = "CurrentStatus" })%>
            </p>
            <p>
                <label for="PendingStatus">Pending:</label>
                <%= Html.CheckBox("IsPending", Model.IsEditable(), Model.TipStatus.IsPending, new { @id = "PendingStatus" })%>
            </p>
            <p>
                <label for="PreviousStatus">Previous:</label>
                <%= Html.CheckBox("IsPrevious", Model.IsEditable(), Model.TipStatus.IsPrevious, new { @id = "PreviousStatus" })%>
            </p>
            
            <h2>TIP Meeting and Approval Dates</h2>
            <p>
            <label for="summary_PublicHearing" class="beside" >Public Hearing:</label>
            <%= Html.DrcogTextBox("PublicHearing", Model.IsEditable(), Model.TipStatus.PublicHearing.HasValue ? Model.TipStatus.PublicHearing.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
            </p>
            <p>
            <label for="summary_BoardApproval">Board Adoption:</label>
            <%= Html.DrcogTextBox("Adoption", Model.IsEditable(), Model.TipStatus.Adoption.HasValue ? Model.TipStatus.Adoption.Value.ToShortDateString() : "", new { @class = "shortInputElement  datepicker" })%>
            </p>
            <p>
            <label for="summary_GovernorApproval">Governor's Approval:</label>
            <%= Html.DrcogTextBox("GovernorApproval", Model.IsEditable(), Model.TipStatus.GovernorApproval.HasValue ? Model.TipStatus.GovernorApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement  datepicker" })%>
            </p>
            <p>
            <label for="summary_USDOTApproval">U.S. DOT Approval:</label>
            <%= Html.DrcogTextBox("USDOTApproval", Model.IsEditable(), Model.TipStatus.USDOTApproval.HasValue ? Model.TipStatus.USDOTApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>            
            </p>
            <p>
            <label for="summary_EPAApproval">U.S. EPA Approval:</label>
            <%= Html.DrcogTextBox("EPAApproval", Model.IsEditable(), Model.TipStatus.EPAApproval.HasValue ? Model.TipStatus.EPAApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement  datepicker" })%>
            </p>
            <p>
            <label for="summary_ShowDelayDate">Delay visibility Date:</label>
            <%= Html.DrcogTextBox("ShowDelayDate", Model.IsEditable(), !Model.TipStatus.ShowDelayDate.Equals(default(DateTime)) ? Model.TipStatus.ShowDelayDate.ToShortDateString() : "", new { @class = "shortInputElement  datepicker" })%>
            </p>
            <p>
            <label for="summary_Notes">Notes:</label>
            <%= Html.TextArea2("Notes", Model.IsEditable(), Model.TipStatus.Notes, 10, 1)%>
            </p>
        </fieldset>
        <br />
        
        <%if (Model.IsEditable())
          { %>        
            <p>
            <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
            <div id="result" style="position: relative; left: 0;"></div>
            </p>    
       <%} %>       
    <%} %>
    </div>
</div>
<div class="clear"></div>


</asp:Content>



