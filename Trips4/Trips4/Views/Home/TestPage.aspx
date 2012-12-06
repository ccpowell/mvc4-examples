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
    </div>
    <div id="dialog-amend-project" title="Amend Plan Projects">
        <div class="info">
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
                        <img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" alt="add" /></a><br />
                    <a href="#" id="amend-removeProject" title="Remove Project">
                        <img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" alt="remove" /></a><br />
                </td>
                <td>
                    <select id="amend-selectedProjects" class="w400 nobind" size="25" multiple="multiple">
                    </select>
                </td>
            </tr>
        </table>
        <div class="dialog-result" style="display: none;">
        </div>
    </div>
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
