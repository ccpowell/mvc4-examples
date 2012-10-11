<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RecoveryViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PasswordRecovery</title>
    <link href="<%= ResolveUrl("~/Content/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/site.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/login.css") %>" rel="stylesheet" type="text/css" />
</head>
<body>
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
                <% using (Html.BeginForm("PasswordRecovery","Account",FormMethod.Post, new { @id = "passwordRecoveryForm" })) { %>           
                    <%= Html.HiddenFor(model => model.PasswordRecoveryModel.Email) %>
                    <%= Html.ValidationSummary(true, "Password change was unsuccessful. Please correct the errors and try again.") %>
                    <div>
                        <fieldset>
                            <legend>Account Information</legend>
                            <%= Html.HiddenFor(model => model.PasswordRecoveryModel.Question)%>
                            <h2 id="question"><%= Html.LabelFor(m => m.PasswordRecoveryModel.Question)%><span><%= Html.Encode(Model.PasswordRecoveryModel.Question)%></span></h2>
                            <div class="editor-label">
                                <%= Html.LabelFor(m => m.PasswordRecoveryModel.Answer)%>
                            </div>
                            <div class="editor-field">
                                <%= Html.PasswordFor(m => m.PasswordRecoveryModel.Answer)%>
                                <%= Html.ValidationMessageFor(m => m.PasswordRecoveryModel.Answer)%>
                            </div>
                            
                            <div class="editor-label">
                                <%= Html.LabelFor(m => m.PasswordRecoveryModel.Password)%>
                            </div>
                            <div class="editor-field">
                                <%= Html.PasswordFor(m => m.PasswordRecoveryModel.Password, new { size = "40", @class = "required" })%>
                            </div>
                            
                            <div class="editor-label">
                                <%= Html.LabelFor(m => m.PasswordRecoveryModel.ConfirmPassword)%>
                            </div>
                            <div class="editor-field">
                                <%= Html.PasswordFor(m => m.PasswordRecoveryModel.ConfirmPassword, new { size = "40", @class = "required", @equalTo = "#Password" })%>
                            </div>
                            
                            <p>
                                <input type="submit" value="Change Password" />
                            </p>
                        </fieldset>
                    </div>
                   <% } %>
                </div>
                <div class="onethird border help-font">
                    <div class="bx_help">
                        <h2 >                            
                            Welcome...
                        </h2>
                        <p >
                            You are about to enter into the world of transportation projects planned and programmed in the nine-county Denver region. 
                            This application is a tool to review, edit and report on these projects in:</p>
                            <ul>
                            <li>the Regional Transportation Plan,</li>
                            <li>the Transportation Improvement Program,</li>
                            <li>the Transportation Improvement Survey, and</li>
                            <li>used in other Denver Regional Council of Government (DRCOG) transportation planning activities.</li>
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
                    <td ><span>Copyright 2009-2012 Denver Regional Council of Governments</span></td>
                    <td ><span>Version <%= Model.AssemblyVersion.ToString() %></span></td>
                </tr>
            </table>
        </div>
    </div>
    
        
        <!-- This contains the hidden content for inline calls -->
	<div style='display:none'>
	</div>
	
	<script type="text/javascript" charset="utf-8">
    </script>
</body>
</html>
