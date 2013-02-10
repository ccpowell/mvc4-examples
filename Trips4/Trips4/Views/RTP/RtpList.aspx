<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RtpListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RTP List</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $('#rtpListGrid').dataTable(
    {
        "aaSorting": [[1, 'desc']],
        "aoColumns": [{ "bSortable": false }, { 'sWidth': '100px' }, null, null, null, null, null, null],
        "bLengthChange": false,
        "bPaginate": false
    });
        });

    </script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
    <div class="view-content-container">
        <h2>
            Regional Transportation Plans</h2>
        <%= Html.Grid(Model.RTPs).Columns(column => {
        column.For(x => Html.ActionLink("Details", "Dashboard", new {controller="Rtp", year = x.Plan })).Encode(false);
	    column.For(x => x.Plan).Named("Plan Year");
        column.For(x => x.RtpSummary.GetStatus()).Named("Status");
        column.For(x => x.PublicHearing).Named("Public Hearing").Format("{0:M/d/yyyy}");
        column.For(x => x.Adoption).Named("Adoption").Format("{0:M/d/yyyy}");
        column.For(x => x.LastAmended).Named("Last Amended").Format("{0:M/d/yyyy}");
        column.For(x => x.USDOTApproval).Named("USDOT Approval").Format("{0:M/d/yyyy}");
        column.For(x => x.CDOTAction).Named("CDOT Action").Format("{0:M/d/yyyy}");  
    }).Attributes(id=> "rtpListGrid") %>
        <div class="clear">
        </div>
        <div class="belowTable">
            <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
               { %>
            <button id="create-rtp">
                Create New Plan</button>
            <% } %>
        </div>
    </div>
    <div id="dialog" title="Create New Plan">
        <form action="">
        <div>
            <label for="planName">
                Plan Name:</label>
            <input type="text" name="planName" id="planName" class="required"  />
            </div>
        <div>
            <label for="cycle-name">
                Cycle Name:</label>
            <input type="text" name="cycle-name" id="cycle-name" class="required"  />
            </div>
        <div>
            <label for="cycle-description">
                Cycle Description:</label>
            <input type="text" name="cycle-description" id="cycle-description" class="w380" />
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            function CreateRtpCallback() {
                $("#dialog").dialog('close');
                location.reload();
            }

            //wire up the button
            $('#create-rtp').button().click(function () {
                $('#dialog').dialog('open')
                    .find("form").validate().resetForm();
            });

            $("#dialog").dialog({
                bgiframe: true,
                autoOpen: false,
                width: 650,
                modal: true,
                buttons: {
                    'Create New RTP': function () {
                        var stuff = {
                            PlanName: $("#planName").val(),
                            CycleName: $("#cycle-name").val(),
                            CycleDescription: $("#cycle-description").val()
                        };
                        if (!$('#dialog form').valid()) {
                            alert('invalid form');
                            return;
                        }
                        App.postit("/Operation/RtpOperation/CreatePlan", {
                            data: JSON.stringify(stuff),
                            success: CreateRtpCallback
                        });
                    },
                    Cancel: function () {
                        $(this).dialog('close');
                    }
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
