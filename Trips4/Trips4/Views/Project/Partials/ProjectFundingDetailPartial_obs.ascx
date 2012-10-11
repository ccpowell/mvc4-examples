<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.FundingDetailPivotModel>" %>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Project Financial Details</legend>
            
            <table>
            <% foreach (System.Data.DataRow dr in Model.FundingDetailTable.Rows)
               { %>
            <tr>
            <td><%= dr.ItemArray[2].ToString() %></td>
            <td><%= dr.ItemArray[3].ToString() %></td>
            <td><%= dr.ItemArray[5].ToString() %></td>
            <td><%= dr.ItemArray[6].ToString() %></td>
            <td><%= dr.ItemArray[7].ToString() %></td>
            <td><%= dr.ItemArray[8].ToString() %></td>
            </tr>
            <% } %>
            </table>
    
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>



