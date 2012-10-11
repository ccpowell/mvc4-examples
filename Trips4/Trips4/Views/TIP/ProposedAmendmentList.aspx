<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ProjectListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Test</title>
    <%-- jQuery 1.3.2 --%>
    
    <script src="../../scripts/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery-1.3.2.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery-ui-1.8.5.custom.min.js")%>" type="text/javascript"></script>
    
    <link href="<%= ResolveUrl("~/Content/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/Site.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/flick/jquery-ui-1.8.5.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/buttonframework.css") %>" rel="stylesheet" type="text/css" />
    
    <link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $().ready(function() {
            InitButtons();

            $('#projectListGrid').dataTable({
                "iDisplayLength": 25,
                "aaSorting": [[1, "desc"]],
                "aoColumns": [{ "bSortable": false }, null, null, null, null, null]

            });
        });

        function InitButtons() {
            //all hover and click logic for buttons
            $(".fg-button:not(.ui-state-disabled)")
		        .hover(
			        function() {
			            $(this).addClass("ui-state-hover");
			        },
			        function() {
			            $(this).removeClass("ui-state-hover");
			        }
		        )
		    .mousedown(function() {
		        $(this).parents('.fg-buttonset-single:first').find(".fg-button.ui-state-active").removeClass("ui-state-active");
		        if ($(this).is('.ui-state-active.fg-button-toggleable, .fg-buttonset-multi .ui-state-active')) { $(this).removeClass("ui-state-active"); }
		        else { $(this).addClass("ui-state-active"); }
		    })
		    .mouseup(function() {
		        if (!$(this).is('.fg-button-toggleable, .fg-buttonset-single .fg-button,  .fg-buttonset-multi .fg-button')) {
		            $(this).removeClass("ui-state-active");
		        }
		    });
        }
    </script>
    
</head>

<body >
    
    <div class="view-content-container">
        <div class="tab-content-container">
        <h2>Projects with Proposed Amendments</h2>

        <%= Html.Grid(Model.ProjectList).Columns(column => {
            column.For(x => Html.ActionLink("Edit", "Info", new { controller="Project", year = Model.TipSummary.TipYear, id = x.ProjectVersionId })).Encode(false);
            column.For(x => x.TipId).Named("TIPID");            
            column.For(x => x.Title).Named("Title");
            column.For(x => x.SponsorAgency).Named("Sponsor");
            column.For(x => x.ProjectType).Named("Project Type");
            column.For(x => x.AmendmentStatus).Named("Amendment Status");
        }).Attributes(id=> "projectListGrid") %>


        <div class="belowTable">
        </div>
    </div>
</body>
</html>
