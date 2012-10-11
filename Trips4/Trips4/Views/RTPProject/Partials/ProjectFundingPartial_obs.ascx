<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.FundingModel>" %>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <table>
            <tr>
                <td colspan="2">Project Funding:</td>
                <td colspan="2">TIP $ by Source:</td>
            </tr>
            <tr>
                <td><label for="Previous">Previous:</label></td>
                <td>
                    <%= Html.DrcogTextBox("Previous", String.Format("{0:F}", Model.Previous)) %>
                    <%= Html.ValidationMessage("Previous", "*") %>
                </td>
                <td><label for="FederalTotal">FederalTotal:</label></td>
                <td>           
                    <%= Html.DrcogTextBox("FederalTotal", String.Format("{0:F}", Model.FederalTotal)) %>
                    <%= Html.ValidationMessage("FederalTotal", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="TIPFunding">TIPFunding:</label></td>
                <td>       
                    <%= Html.DrcogTextBox("Funding", String.Format("{0:F}", Model.Funding)) %>
                    <%= Html.ValidationMessage("Funding", "*") %>
                </td>
                <td><label for="StateTotal">StateTotal:</label></td>
                <td>           
                    <%= Html.DrcogTextBox("StateTotal", String.Format("{0:F}", Model.StateTotal)) %>
                    <%= Html.ValidationMessage("StateTotal", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="Future">Future:</label></td>
                <td>
                    <%= Html.DrcogTextBox("Future", String.Format("{0:F}", Model.Future)) %>
                    <%= Html.ValidationMessage("Future", "*") %>
                </td>
                <td><label for="LocalTotal">LocalTotal:</label></td>
                <td>
                    <%= Html.DrcogTextBox("LocalTotal", String.Format("{0:F}", Model.LocalTotal)) %>
                    <%= Html.ValidationMessage("LocalTotal", "*") %>
                </td>
            </tr>
            <tr>
                <td><label for="TotalCost">TotalCost:</label></td>
                <td>
                    <%= Html.DrcogTextBox("TotalCost", String.Format("{0:F}", Model.TotalCost)) %>
                    <%= Html.ValidationMessage("TotalCost", "*") %>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            </table>
            
            <p><input type="submit" value="Save" /></p>
        </fieldset>

    <% } %>


