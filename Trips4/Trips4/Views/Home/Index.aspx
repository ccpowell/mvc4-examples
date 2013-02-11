<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.HomeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Home</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <h2>DRCOG's Transportation Regional Improvement Projects and Survey (TRIPS)</h2>
    <div style="font-size: 0.6em; position : fixed; right: 0; top: 0;">Version: <%= Model.AssemblyVersion.ToString() %></div>
    <%--<p>Welcome <%= (ViewData["CurrentUser"] as Account).FirstName %> <%= (ViewData["CurrentUser"] as Account).LastName%> from <%= (ViewData["CurrentUser"] as Account).OrganizationName%> </p>--%>
    <%--<p><%= User.Identity.Name.ToString() %></p>--%>
    <p>
        This application allows you to view planned and programmed transportation projects within the nine-county Denver metro area and Southwest Weld County. The database includes projects contained in the Regional Transportation Plan, the Transportation Improvement Program, and the Transportation Improvement Survey.
    </p>
    <p>
        For more information, please contact the following:
    </p>

    <ul id="contact">
        <li>
            Todd Cottrell: <a href='&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#116;&#99;&#111;&#116;&#116;&#114;&#101;&#108;&#108;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;'>&#116;&#99;&#111;&#116;&#116;&#114;&#101;&#108;&#108;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;</a> | 303.480.6737 for TIP Projects
        </li>
        <li>
            Robert Spotts: <a href='&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#114;&#115;&#112;&#111;&#116;&#116;&#115;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;'>&#114;&#115;&#112;&#111;&#116;&#116;&#115;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;</a> | 303.480.5626 for TIP Projects
        </li>
        <li>
            Lawrence Tilong: <a href='&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#108;&#116;&#105;&#108;&#111;&#110;&#103;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;'>&#108;&#116;&#105;&#108;&#111;&#110;&#103;&#64;&#100;&#114;&#99;&#111;&#103;&#46;&#111;&#114;&#103;</a> | 303.480.6761 for Survey projects
        </li>
    </ul>
    
    <p>
        <div class="boldFont">Summary of Projects in the Transportation Improvement Program (TIP)</div><br />
        The Transportation Improvement Program (TIP) identifies all federally funded transportation projects in the Denver region over a six-year period.  It is prepared by DRCOG every four years, and must show it meets air quality requirements.  The most recently adopted TIP covers federal fiscal years 2012-2017.
    </p>
    <p>
        <div class="boldFont">Summary of Projects in the Regional Transportation Plan (RTP)</div><br />
        The Metro Vision Regional Transportation Plan ( MVRTP) addresses the challenges and guides the development of a multimodal transportation system out into the future years. The most recently adopted plan goes through the year 2035, and is called the 2035 MVRTP. It reflects a transportation system that closely interacts with the growth, development, and environmental elements of the region’s long-range plan, Metro Vision.
    </p>
    <p>
        <div class="boldFont">Summary of Projects in the Transportation Survey (Survey)</div><br />
        Each year, with assistance from local governments, DRCOG conducts a transportation improvement survey to identify the status of minor arterial and collector roadway projects. Shown are projects from the survey to be completed from 2011 through 2035. These projects are incorporated into the travel demand model network for DRCOG's air quality conformity process.
    </p>
    
    <%--
    <p>
        <p><b>USERNAME:</b>&nbsp;<%= HttpContext.Current.User.Identity.Name %></p>
        <p><b>ROLES:</b></p>
        <p>* Administrator:&nbsp;<%= HttpContext.Current.User.IsInRole("Administrator").ToString() %></p>
        <p>* Contact:&nbsp;<%= HttpContext.Current.User.IsInRole("Contact").ToString()%></p>
        <p>* Contact Manager:&nbsp;<%= HttpContext.Current.User.IsInRole("Contact Manager").ToString()%></p>
        <p>* RTP Administrator:&nbsp;<%= HttpContext.Current.User.IsInRole("RTP Administrator").ToString() %></p>
        <p>* SponsorRoleManager:&nbsp;<%= HttpContext.Current.User.IsInRole("SponsorRoleManager").ToString()%></p>
        <p>* Survey Administrator:&nbsp;<%= HttpContext.Current.User.IsInRole("Survey Administrator").ToString()%></p>
        <p>* Survey Contributor:&nbsp;<%= HttpContext.Current.User.IsInRole("Survey Contributor").ToString()%></p>
        <p>* TIP Administrator:&nbsp;<%= HttpContext.Current.User.IsInRole("TIP Administrator").ToString()%></p>
        <p>* Viewer:&nbsp;<%= HttpContext.Current.User.IsInRole("Viewer").ToString()%></p>
    </p>
    --%>
</div>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
