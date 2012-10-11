<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>DRCOG Transportation Projects Website</title>
    <link href="<%= ResolveUrl("~/Content/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Content/login.css") %>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <% using(Html.BeginForm()) { %>
        <div id="loginContainer">
                <div class="header">
                    <div id="loginLogo">
                    <img src="<%=ResolveUrl("~/Content/images/drcog-logox80.png") %>" /></div>
                    <div id="loginTitle"><h2>Transportation Projects Web Site</h2></div>
                    <div class="clear"></div>
                </div>
                <div class="twothirds">
                    <table>
                        <tr>
                            <td>
                                <label for="email">Email Address:</label>
                            </td>
                         </tr>
                        <tr>
                            <td>                                
                                <input id="email" name="email" size="35" value="admin@dtsagile.com" />
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <div id="loginErrorMessage" style="font-weight:bold">
                                 <%= Html.ValidationSummary() %>                                 
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>                        
                               
                                <input type="submit" value="Reset Password" class="login-submit" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="onethird border help-font">
                    <div class="bx_help">
                        <h2 class="bx_help_title">                            
                            Password Recovery...
                        </h2>
                        <p class="bx_help_text">
                            Please enter the email address associated with your account.  Your password will be reset and the new password will be sent to you
                            in an email.
                        </p>
                    </div>
                </div>
                <div class="clear"></div>
                <div id="footer">
                <p><strong>Disclaimer:</strong>The content is the sole responsibility of the Denver Regional Council 
                of Governments based on information provided by the individual project sponsors.
                Preparation and maintenance of the web site and transportation projects database has been 
                financed in part through grants from the Federal Transit Administration and Federal Highway 
                Administration of the U.S. Department of Transportation.</p>
                <p>Copyright 2009-2012 Denver Regional Council of Governments</p>
                </div>
            </div>
    <% } %>
</body>
</html>

