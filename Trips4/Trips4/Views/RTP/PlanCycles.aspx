<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.PlanCyclesViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RTP Plan Cycles</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.RtpYear = '<%=  Model.RtpSummary.RtpYear %>';
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
    <script type="text/javascript" src='<%= Url.Content("~/Scripts/RtpPlanCycles.js") %>'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.Hidden("RtpYearId", Model.RtpSummary.RTPYearTimePeriodID)%>
    <div class="view-content-container">
        <div class="clear">
        </div>
        <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
        <div class="tab-content-container">
            <h2>
                Plan Cycles</h2>
            <%= Html.Grid(Model.Cycles).Columns(column => {
            column.For(x => x.Id);
            column.For(x => x.Name);
            column.For(x => x.Status);
            column.For(x => x.Description);            
            }).Attributes(id => "cyclesGrid", @class => "ui-helper-hidden data-table")%>
            <div class="clear">
            </div>
            <div class="belowTable">
                <% if (!Model.ExistsNewPlanCycle)
                   { %>
                <button id="addCycle">
                    Add New Cycle</button>
                <% } %>
            </div>
        </div>
    </div>
    <div id="cycle-dialog" title="New Cycle">
        <form class="cycle-form" action="">
        <p>
            <label>
                Name:
                <input id="cycle-Name" type="text" class="required" />
            </label>
        </p>
        <p>
            <label>
                Description:
                <input id="cycle-Description" type="text" class="w600" />
            </label>
        </p>
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
