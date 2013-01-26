<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.LoginViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-form-container">
        <% using (Html.BeginForm("Index", "Login", FormMethod.Post, new { @id = "loginForm" }))
           { %>
        <%= Html.Hidden("returnUrl", Model.ReturnUrl)%>
        <input type="hidden" id="login-type" name="LogOnModel.LoginType" value="administrator" />
        <%--<%= Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.", new { @class="error"} )%>--%>
        <div id="loginWrapper">
            <div id="containers">
                <div id="loginContainer">
                    <h2>
                        Login</h2>
                    <div class="twothirds">
                        <div class="border box">
                            <button type="submit" id="login-guest">
                                Log In as Guest</button>
                            <br />
                            No user name or password required.
                        </div>
                        <br />
                        <div class="border box">
                            <p>
                                <%= Html.LabelFor(m => m.LogOnModel.UserName) %>
                                <%= Html.TextBoxFor(m => m.LogOnModel.UserName) %>
                            </p>
                            <p>
                                <%= Html.LabelFor(m => m.LogOnModel.Password) %>
                                <%= Html.PasswordFor(m => m.LogOnModel.Password)%>
                            </p>
                            <p>
                                <%= Html.LabelFor(m => m.LogOnModel.RememberMe) %>
                                <%= Html.CheckBoxFor(m => m.LogOnModel.RememberMe) %>
                            </p>
                            <p>
                                <button type="submit" id="login-administrator">
                                    Log In
                                </button>
                            </p>
                            <p>
                                <button id="forgot-password">
                                    Forgot Password?</button>
                            </p>
                            <br />
                        </div>
                        <asp:Label ID="LabelMessage" runat="server" Text="Message" ForeColor="Red" Font-Bold="true">
                        <%= Model.Message %>
                        </asp:Label>
                    </div>
                    <div class="onethird border help-font">
                        <div class="bx_help">
                            <h2>
                                Welcome...
                            </h2>
                            <p>
                                You are about to enter a site dedicated to planned and programmed transportation
                                projects in the nine-county Denver region. This application is a tool to review,
                                edit, and report on these projects in:
                            </p>
                            <ul>
                                <li>the Regional Transportation Plan</li>
                                <li>the Transportation Improvement Program</li>
                                <li>the Transportation Improvement Survey, and</li>
                                <li>other Denver Regional Council of Governments (DRCOG) transportation planning activities.</li>
                            </ul>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
            <div id="footer">
                <table style="border-style: solid; border-width: 1; border-color: Gray">
                    <tr>
                        <td colspan="2">
                            <strong>Disclaimer:</strong>The content is the sole responsibility of the Denver
                            Regional Council of Governments based on information provided by the individual
                            project sponsors. Preparation and maintenance of the web site and transportation
                            projects database has been financed in part through grants from the Federal Transit
                            Administration and Federal Highway Administration of the U.S. Department of Transportation.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>Copyright 2009-2012 Denver Regional Council of Governments</span>
                        </td>
                        <td>
                            <span>Version
                                <%= Model.AssemblyVersion.ToString() %></span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <% } %>
    </div>
    <div class="clear">
    </div>
    <!-- This contains the hidden content for inline calls -->
    <div style='display: none'>
        <div id="forgot-password-dialog" title="I Forgot My Password">
            <h3>
                Please enter your email address below and a recovery email will be sent to your
                account</h3>
            <br />
            <input id="forgot-password-email" type="text" />
        </div>
    </div>
    <script type="text/javascript" src='<%= Url.Content("~/Scripts/Login.js") %>'></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/site.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/login.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.ghosttext.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.maskedinput-1.2.2.min.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
