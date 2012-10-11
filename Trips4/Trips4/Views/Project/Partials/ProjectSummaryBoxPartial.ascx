<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>

  <div class="project-info-container">
       <h2>Project General Information</h2>
       <ul>
       <li>TIPID: <%=Model.TipId %></li>
       <li>COGID: <%=Model.COGID %></li>
       <li>Name: <%=Model.ProjectName %></li>
       <li>Sponsor: <%=Model.SponsorAgency %></li>
       <li>Type: <%=Model.ProjectType %></li>
       <li>TIP Year: <%=Model.TipYear %></li>
       <li>
       <%if (!Model.PreviousVersionId.Equals(default(int)))
         { %>
            <%=Html.ActionLink("Previous Version", this.ViewContext.RouteData.Values["action"].ToString(), new { id = Model.PreviousVersionId, tipyear = Model.PreviousVersionYear })%>
       <%}
         else
         { %>
            Previous Version
       <%} %>
       |
       <%if (Model.NextVersionId.HasValue)
         { %>
            <%=Html.ActionLink("Next Version", this.ViewContext.RouteData.Values["action"].ToString(), new { id = Model.NextVersionId, tipyear = Model.NextVersionYear })%>
       <%}
         else
         { %>
            Next Version
       <%} %>
       </li>
       <li><a href="#">Print Project Description</a></li>
       <li>ProjectVersionID: <%=Model.ProjectVersionId.ToString() %></li>
       </ul>
                      
    </div>