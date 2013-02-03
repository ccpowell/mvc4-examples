<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.AmendmentsViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project General Information
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/slide.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/TipProjectAmendments.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.TipYear = '<%= Model.ProjectSummary.TipYear %>';
        App.pp.UpdateAmendmentDetailsUrl = '<%=Url.Action("UpdateAmendmentDetails","Project") %>';
        App.pp.ProjectVersionId = parseInt('<%= Model.ProjectSummary.ProjectVersionId %>');
        App.pp.PreviousVersionId = parseInt('<%= Model.ProjectSummary.PreviousVersionId %>');
        App.pp.AmendmentIsPending = App.utility.parseBoolean('<%= Model.ProjectSummary.IsPending.ToString() %>');
        App.pp.AmendmentStatusId = parseInt('<%= Model.ProjectAmendments.AmendmentStatusId %>');

        $(document).ready(App.tabs.initializeTipProjectTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        // we can only modify the top Amendment if it the very latest.
        // this page specified the ID of the latest amendment to include. 
        // if it is not the latest, we cannot modify the Amendment.
        // why look at an earlier Amendment? I dunno...
        bool isTopStatus = Model.ProjectSummary.IsTopStatus;
        bool isEditable = Model.ProjectSummary.IsEditable() && isTopStatus;
        string amendmentStatus = Model.ProjectSummary.AmendmentStatus;
        bool isDeletable = Model.ProjectSummary.CanDelete(amendmentStatus);
        bool isSubmitted = (amendmentStatus == "Submitted");
        bool isProposed = (amendmentStatus == "Proposed");
        bool isAdopted = (amendmentStatus == "Adopted");
        bool isAmended = (amendmentStatus == "Amended");
        bool isApproved = (amendmentStatus == "Approved");
        string rightColumnStyle = "margin-left: 20px; margin-bottom: 10px;";
        if (isSubmitted || isProposed)
        {
            rightColumnStyle += " position: absolute; right: 0px; margin-top: 10px;";
        }
   
    %>
    <div class="view-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/TipProjectTabPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-content-container" style="height: 750px">
            <div id="Div1" class="rightColumn" style='<%= rightColumnStyle %>'>
                <% if (isEditable)
                   { 
                %>
                <% if (isAdopted || isAmended || isApproved)
                   { 
                %>
                <button id="create-amendment">
                    Create Amendment</button>
                <% }
                   else if (isSubmitted)
                   { %>
                   <button id="move-to-proposed">Move To Proposed</button>
                <% }
                   else if (isProposed)
                   { %>
                   <button id="amend-project">Amend Project</button>
                <% } %>
                <% // Can only delete if IsEditable and the VersionModel says it is eligible for delete
                   if (isDeletable)
                   { %>
                   <br />
                   <button id="delete-amendment">Delete Amendment</button>
                <% } %>
                <% } %>
            </div>
            <% if (isSubmitted || isProposed)
               { 
            %>
            <div id="amendmentCharacter" class="box">
                <p>
                    <label for="ProjectAmendments_AmendmentReason">
                        Reason:</label></p>
                <p>
                    <%= Html.TextAreaFor(x => x.ProjectAmendments.AmendmentReason, new { @class = "w400" })%></p>
                <p>
                    <label for="ProjectAmendments_AmendmentCharacter">
                        Amendment Character:</label></p>
                <p>
                    <%= Html.TextAreaFor(x => x.ProjectAmendments.AmendmentCharacter, new { @class = "w400" })%></p>
                <div style="position: relative;">
                   <button id="update-amendment">Update</button>
                </div>
            </div>
            <% } %>
            <%= Html.Grid(Model.AmendmentList).Columns(column =>
                {
                    //column.For(x => Html.ActionLink("View", "Details", new { controller = "Project", year = Model.ProjectSummary.TipYear, id = x.ProjectVersionId })).DoNotEncode();
                   
                    column.For(
                        x => x.ProjectVersionId.Equals(Model.ProjectSummary.ProjectVersionId) ? MvcHtmlString.Create("Current") : Html.ActionLink("View", "Funding", new { controller = "Project", year = x.Year, id = x.ProjectVersionId })
                    ).Named("View").Encode(false);
                    //column.For(x => x.ProjectName).Named("Title");
                    column.For(x => x.AmendmentDate.Equals(DateTime.MinValue) ? String.Empty : x.AmendmentDate.ToShortDateString()).Named("Amendment<br>Date");
                    column.For(x => x.AmendmentStatus).Named("Amendment<br>Status");
                    column.For(x => x.VersionStatus).Named("Version<br>Status");
                    column.For(x => x.AmendmentCharacter).Named("Amendment<br>Character");
                }).Attributes(id => "projectListGrid") %>
        </div>
    </div>
    <!-- This contains the hidden content for inline calls -->
    <div style='display: none'>
        <div id="create-amendment-dialog" title="Amend Project">
            <% using (Html.BeginForm("Amend", "Project", FormMethod.Post, new { @id = "create-amendment-form" }))
               { 
            %>
            <fieldset>
                <legend>New Amendment</legend>
                <p>
                    <label for="NewAmendment_AmendmentReason">
                        Reason:</label>
                    <%= Html.TextArea("NewAmendment.AmendmentReason")%>
                </p>
                <p>
                    <label for="NewAmendment_AmendmentCharacter">
                        Amendment Character:</label>
                    <%= Html.TextArea("NewAmendment.AmendmentCharacter")%>
                </p>
                <p>
                    <label for="AmendmentTypes">
                        Amendment Type:</label>
                    <%= Html.DropDownListFor(x => x.ProjectAmendments.AmendmentTypeId, new SelectList(Model.AmendmentTypes, "Key", "Value"), new { @class = "mediumInputElement big" })%>
                </p>
            </fieldset>
            <% } %>
        </div>
        <div id="delete-amendment-dialog" title="Confirm Delete">
        <div>
        Are you sure you want to delete this Amendment? This operation cannot be undone.
        </div>
        </div>
    </div>
</asp:Content>
