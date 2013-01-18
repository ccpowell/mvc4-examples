<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div style='display: none'>
    <div id="dialog-amend-projects">
        <div class="info" id="amend-info">
            Select the projects you wish to include</div>
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
</div>
