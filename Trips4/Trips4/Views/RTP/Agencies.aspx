<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.SponsorsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Eligible Agencies</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />


<script type="text/javascript">

    var addurl = '<%=Url.Action("AddEligibleAgency","RTP", new {plan=Model.RtpSummary.RtpYear}) %>';
    var removeurl = '<%=Url.Action("DropEligibleAgency","RTP", new {plan=Model.RtpSummary.RtpYear}) %>';
    $().ready(function() {

        $('#add').click(function() {
            $('#AvailableAgencies option:selected').each(function(i) {
                //make callback add to Eligible Agency list
                addAgency($(this).val());
            });
            return false;
        });

        $('#remove').click(function() {
            $('#EligibleAgencies option:selected').each(function(i) {
                //alert($(this).val());
                removeAgency($(this).val());
            });
            return false;
        });

        function addAgency(id) {
            $.ajax({
                type: "POST",
                url: addurl,
                dataType: "json",
                data: { agencyId: id },
                success: function(response) {
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        var selector = "#AvailableAgencies option[value='" + id + "']";
                        $(selector).remove().prependTo('#EligibleAgencies');
                    } else {
                        ShowMessageDialog('Error adding Sponsor', response.Error);
                    }
                }
            });
        }

        function removeAgency(id) {
            $.ajax({
                type: "POST",
                url: removeurl,
                dataType: "json",
                data: { agencyId: id },
                success: function(response) {
                    //alert("success");
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        $("#EligibleAgencies option[value='" + id + "']").remove().prependTo('#AvailableAgencies');
                    } else {
                        //alert(response.Error);
                        ShowMessageDialog('Sponsor not Removed', response.Error);
                    }
                }
            });
        }

        function ShowMessageDialog(title, message) {
            $('#dialog').dialog('option', 'title', title);
            $('#dialogMessage').html(message);
            $('#dialog').dialog('open');
        }

        //Initialize the dialog
        $("#dialog").dialog({
            autoOpen: false,
            draggable: false,
            bgiframe: true,
            modal: true,
            buttons: {
                Ok: function() {
                    $(this).dialog('close');
                }
            }
        });


    });        //end $().ready
    
    
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="view-content-container">
<div class="clear"></div>

<%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>

    <div id="eligibleAgenciesForm" class="tab-form-container">
    <h2>Eligible Agencies List</h2>
       
        <table>
        <tr>
        <td>Available Agencies:</td>
        <td>&nbsp;</td>
        <td>Eligible Agencies:</td>
        </tr>
        <tr>
        <td>
        <%= Html.ListBox("AvailableAgencies", new MultiSelectList(Model.GetAvailableAgencySelectList().Items, "OrganizationId", "OrganizationName"), new {  @class = "mediumInputElement", size = 10 })%><br/>        
        </td>
        <td>
        <%if (Model.RtpSummary.IsEditable())
          { %>
        <a href="#"  id="add"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
        <a href="#"  id="remove"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
        <%} %>
        </td>
        <td>
          <%= Html.ListBox("EligibleAgencies", new MultiSelectList(Model.GetEligibleAgencySelectList().Items, "OrganizationId", "OrganizationName"), new {  @class = "mediumInputElement", size = 10 })%><br/>
        </td>
        </tr>
        </table>
            
      <%--  </fieldset>--%>
      <p>Note: Changes are stored to the database as they are made in the interface.</p>
    
<%--    <%} %>--%>
    </div>
</div>
<div class="clear"></div>


<div id="dialog" title="">
	<p id="dialogMessage"></p>
</div>


</asp:Content>



