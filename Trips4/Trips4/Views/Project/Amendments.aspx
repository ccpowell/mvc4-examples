<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.AmendmentsViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project General Information
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.dataTables.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/slide.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/TipProjectAmendments.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var UpdateAmendmentDetailsUrl = '<%=Url.Action("UpdateAmendmentDetails","Project") %>';
        $(document).ready(function () {
            $('#projectListGrid').dataTable({
                "iDisplayLength": 10,
                "bSort": false,
                //"aaSorting": [[1, "desc"]],
                "aoColumns": [{ "bSortable": false, "sWidth": "5%" }, { "sWidth": "15%" }, { "sWidth": "15%" }, { "sWidth": "15%" }, { "sWidth": "50%"}]
            });
            $('#projectListGrid_length').attr("style", "display:none");

            $('#button-UpdateAmendmentDetails').live("click", function () {
                var amendmentReason = $("#ProjectAmendments_AmendmentReason").text();
                var amendmentCharacter = $("#ProjectAmendments_AmendmentCharacter").text();
                $.ajax({
                    type: "POST",
                    url: UpdateAmendmentDetailsUrl,
                    data: "ProjectVersionId=<%= Model.ProjectAmendments.ProjectVersionId %>"
                    + "&AmendmentReason=" + amendmentReason
                    + "&AmendmentCharacter=" + amendmentCharacter,
                    dataType: "json",
                    success: function (response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            $('#result').html(response.message).addClass('success').autoHide();
                        } else {
                            $('.dialog-result').html(response.message + " Details: " + response.exceptionMessage).addClass('error').autoHide({ wait: 10000, removeClass: "error" });
                        }
                        window.onbeforeunload = null;
                        charReasonCheck();
                    },
                    error: function (response, textStatus, AjaxException) {
                        //alert("error");
                        //$('').html(response.statusText);
                        //$('').addClass('error');
                        //autoHide(10000);
                    }
                });
            });

            // run check right away
            charReasonCheck();
            // run check after leaving input
            //$("#ProjectAmendments_AmendmentReason, #ProjectAmendments_AmendmentCharacter").blur(function() {
            //    charReasonCheck();
            //});

            function charReasonCheck() {
                var reason = $("#ProjectAmendments_AmendmentReason").val();
                var character = $("#ProjectAmendments_AmendmentCharacter").val();
                var isPending = "<%= Model.ProjectSummary.IsPending.ToString() %>";
                if ((isPending === "False" && reason === "") || character === "") {
                    $("#btn-restore").addClass("ui-state-disabled");
                } else { $("#btn-restore").removeClass("ui-state-disabled"); }
            };



        });

        function confirmDelete() {
            if (confirm('Are you sure you want to delete this amendment?'))
                return true;
            else
                return false;
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-content-container" style="height: 750px">
            <div id="Div1" class="rightColumn" style="margin-left: 20px; margin-bottom: 10px;
                <% if (new List<String> { "Submitted", "Proposed" }.Contains(Model.ProjectSummary.AmendmentStatus)) { %> position: absolute;
                right: 0px; margin-top: 10px; <% } %>">
                <%if (Model.ProjectSummary.IsTopStatus && Model.ProjectSummary.IsEditable())
                  { %>
                <% if (new List<String> { "Approved", "Amended", "Adopted" }.Contains(Model.ProjectSummary.AmendmentStatus))
                   { %>
                <a id="A2" class="confirmAmend fg-button w380 ui-state-default ui-corner-all" href="#">
                    Create Amendment </a>
                <% }
                   else if (Model.ProjectSummary.AmendmentStatus.Equals("Submitted"))
                   { %><%= Html.ActionLink("Move to Proposed", "Amend", "Project", new { projectVersionId = Model.ProjectSummary.ProjectVersionId, previousVersionId = Model.ProjectSummary.PreviousVersionId }, new { @id = "btn-restore", @class = "fg-button w380 ui-state-default ui-corner-all" })%>
                <% }
                   else if (Model.ProjectSummary.AmendmentStatus.Equals("Proposed"))
                   { %><%= Html.ActionLink("Amend Project", "Amend", "Project", new { projectVersionId = Model.ProjectSummary.ProjectVersionId, previousVersionId = Model.ProjectSummary.PreviousVersionId }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>
                <% } %>
                <% // Can only delete if IsEditable and the VersionModel says it is eligible for delete
                   if (Model.ProjectSummary.IsEditable() && Model.ProjectSummary.CanDelete(Model.ProjectSummary.AmendmentStatus))
                   { %>
                <%=Html.ActionLink("Delete Amendment", "DeleteAmendment", new { controller = "Project", projectVersionId = Model.ProjectSummary.ProjectVersionId, previousProjectVersionId = Model.ProjectSummary.PreviousVersionId, year = Model.ProjectSummary.TipYear }, new { @id = "confirm-delete", @class = "fg-button w380 ui-state-default ui-corner-all deleteAmendment", onclick = "return confirmDelete();" })%>
                <% } %>
                <% } %>
            </div>
            <% if (new List<String> { "Submitted", "Proposed" }.Contains(Model.ProjectSummary.AmendmentStatus))
               { %>
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
                    <span id="button-UpdateAmendmentDetails" class="fg-button w200 ui-state-default ui-corner-all"
                        style="position: relative; top: -8px;">Update</span>
                    <div id="result" style="position: absolute; margin: 0px; top: -50px; left: 500px;">
                    </div>
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
                }).Attributes(id => "projectListGrid")%>
        </div>
    </div>
    <!-- This contains the hidden content for inline calls -->
    <div style='display: none'>
        <div id='confirmDeleteContent' style='padding: 10px; background: #fff;'>
            <% Html.RenderPartial("~/Views/Project/Partials/AmendProjectPartial.ascx", Model); %>
        </div>
    </div>
    <script type="text/javascript">
        $(".confirmAmend").colorbox({ width: "800px", height: "330px", inline: true, href: "#confirmDeleteContent" });
    </script>
</asp:Content>
