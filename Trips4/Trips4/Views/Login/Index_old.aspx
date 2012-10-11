<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.LoginViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DRCOG Transportation Projects Website</title>
    <script src="<%= ResolveClientUrl("~/scripts/jquery.min.js")%>" type="text/javascript"></script>
    <script src="<%= ResolveClientUrl("~/scripts/jquery.colorbox-min.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskedinput-1.2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.ghosttext.js") %>" type="text/javascript"></script>
    <link href="<%= ResolveUrl("~/Content/colorbox.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/site.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/login.css") %>" rel="stylesheet" type="text/css" />
    
</head>
<body>
   <% using (Html.BeginForm("Index","Login",FormMethod.Post, new { @id = "loginForm" })) { %>           
       <%= Html.Hidden("returnUrl", Model.ReturnUrl)%>
       <%--<%= Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.", new { @class="error"} )%>--%>
        <div id="loginWrapper">
            <div class="header">
                <div id="loginLogo">
                <img src="<%=ResolveUrl("~/Content/images/drcog-logox80.png") %>" /></div>
                <div id="loginTitle"><h2>Transportation Improvement Programs</h2></div>
                <div class="clear"></div>
            </div>
            <div id="containers">
                <div id="loginContainer">
                    <div class="twothirds">
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
                            <button  type="submit" class="">
                                <img src="<%=ResolveUrl("~/content/images/16-security-lock.png") %>"/>
                                Log In
                                </button>
                                or
                                <a id="btn_forgotPassword" href="#">Forgot Password?</a>
                                <%--<%= Html.ActionLink("Forgot Password?", "RecoverPassword", "Account")%>--%>
                                
                        </p>
                    <%-- hidden temporarly
                        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="GrayText" Font-Italic="true">
                            Do you need an Account? 
                            <a id="btn_register" href="#">Register</a><br />                 
                        </asp:Label>--%>
                        <br />
                        <asp:Label ID="LabelMessage" runat="server" Text="Message" ForeColor="Red" Font-Bold="true">
                        <%= Model.Message %>
                        </asp:Label>
                    </div>
                    <div class="onethird border help-font">
                        <div class="bx_help">
                            <h2 >                            
                                Welcome...
                            </h2>
                            <p >
                                You are about to enter a site dedicated to planned and programmed transportation projects in the nine-county Denver region. This application is a tool to review, edit, and report on these projects in:
                            </p>
                            <ul>
                                <li>the Regional Transportation Plan</li>
                                <li>the Transportation Improvement Program</li>
                                <li>the Transportation Improvement Survey, and</li>
                                <li>other Denver Regional Council of Governments (DRCOG) transportation planning activities.</li>
                            </ul> 
                            
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            
            <div id="footer">
                <table style="border-style: solid; border-width: 1; border-color: Gray">
                    <tr >
                        <td colspan="2">
                            <strong>Disclaimer:</strong>The content is the sole responsibility of the Denver Regional Council 
                            of Governments based on information provided by the individual project sponsors.
                            Preparation and maintenance of the web site and transportation projects database has been 
                            financed in part through grants from the Federal Transit Administration and Federal Highway 
                            Administration of the U.S. Department of Transportation.
                        </td>
                    </tr>
                    <tr>
                        <td ><span>Copyright 2009-2010 Denver Regional Council of Governments</span></td>
                        <td ><span>Version <%= Model.AssemblyVersion.ToString() %></span></td>
                    </tr>
                </table>
            </div>
        </div>
    <% } %>
        
        <!-- This contains the hidden content for inline calls -->
	<div style='display:none'>
		<div id='registrationContainer' style='padding:10px; background:#fff;'>
		    <% Html.RenderPartial("~/Views/Login/Partials/RegisterPartial.ascx", new Person()); %>
		</div>
		<div id='forgotPasswordContainer' style='padding:10px; background:#fff;'>
		    <% Html.RenderPartial("~/Views/Login/Partials/ForgotPasswordPartial.ascx", new DRCOG.Domain.Security.PasswordRecoveryModel()); %>
		</div>
	</div>
	
	<script type="text/javascript" charset="utf-8">
	    $(document).ready(function() {
	        $(".ghost_text").ghostText();

	    });
    </script>
	
</body>
</html>
