<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DRCOG.Domain.Models.TIPProject.SegmentModel>>" %>

    <table>
        <tr>
            <th>SegmentId</th>
            <th>FacilityName</th>
            <th>Network</th>
            <th>OpenYear</th>
            <th>StartAt</th>
            <th>EndAt</th>
        </tr>
        
    <% foreach (var item in Model.ToList<DRCOG.Domain.Models.TIPProject.SegmentModel>()) { %>
        <tr>
            <td><%= Html.Encode(item.SegmentId) %></td>
            <td><%= Html.Encode(item.FacilityName) %></td>
            <td><%= Html.Encode(item.Network) %></td>
            <td><%= Html.Encode(item.OpenYear) %></td>
            <td><%= Html.Encode(item.StartAt) %></td>
            <td><%= Html.Encode(item.EndAt) %></td>
        </tr>
    <% } %>
    </table>


