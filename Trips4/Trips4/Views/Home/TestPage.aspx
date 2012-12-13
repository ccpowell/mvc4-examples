<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    TestPage
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        TestPage</h2>
    <div>
        <button id='amend-projects'>
            Amend Projects</button>
        <button id='include-projects'>
            Include More Projects</button>
        <button id='restore-projects'>
            Restore Projects</button>
    </div>
    <div id="dialog-amend-projects">
        <div class="info" id="amend-info">
            Select the projects you wish to amend in the next cycle</div>
        <table id="amendProjects">
            <tr>
                <td>
                    Available Projects:
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    Selected Projects: <span id="amend-countReady"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <select id="amend-availableProjects" class="w400 nobind" size="25" multiple="multiple">
                    </select>
                </td>
                <td>
                    <a href="#" id="amend-addProject" title="Add Project">
                        <img src="<%=Url.Content("~/content/images/24-arrow-next.png")%>" alt="add" /></a><br />
                    <a href="#" id="amend-removeProject" title="Remove Project">
                        <img src="<%=Url.Content("~/content/images/24-arrow-previous.png")%>" alt="remove" /></a><br />
                </td>
                <td>
                    <select id="amend-selectedProjects" class="w400 nobind" size="25" multiple="multiple">
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        var App = App || {};
        App.pp = {
            CurrentCycleId: 22,
            PreviousCycleId: 19,
            NextCycleId: 0,
            RtpPlanYear: '2035-S',
            RtpPlanYearId: 78,
            CurrentCycleName: '2011-1',
            NextCycleName: ''
        };
    </script>
    <script type="text/javascript" src='<%= Url.Content("~/Scripts/RtpProjectList.js") %>'></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
