<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.SponsorsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Survey Eligible Agencies</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server"><%= Model.Current.Name %> Survey Projects</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />


<script type="text/javascript">
    var addurl = '<%=Url.Action("AddEligibleAgency","Survey", new {timePeriod=Model.Current.Name}) %>';
    var removeurl = '<%=Url.Action("DropEligibleAgency","Survey", new {timePeriod=Model.Current.Name}) %>';
    var addImprovementTypeurl = '<%=Url.Action("AddEligibleImprovementType","Survey", new {timePeriod=Model.Current.Name}) %>';
    var removeImprovementTypeurl = '<%=Url.Action("DropEligibleImprovementType","Survey", new {timePeriod=Model.Current.Name}) %>';
    var addFundingResourceurl = '<%=Url.Action("AddEligibleFundingResource","Survey", new {timePeriod=Model.Current.Name}) %>';
    var removeFundingResourceurl = '<%=Url.Action("DropEligibleFundingResource","Survey", new {timePeriod=Model.Current.Name}) %>';

    $().ready(function () {

        $('#add').click(function () {
            $('#AvailableAgencies option:selected').each(function (i) {
                addAgency($(this).val());
            });
            return false;
        });

        $('#remove').click(function () {
            $('#EligibleAgencies option:selected').each(function (i) {
                removeAgency($(this).val());
            });
            return false;
        });

        $('#addImprovementType').click(function () {
            $('#AvailableImprovementTypes option:selected').each(function (i) {
                addImprovementType($(this).val());
            });
            return false;
        });

        $('#removeImprovementType').click(function () {
            $('#EligibleImprovementTypes option:selected').each(function (i) {
                removeImprovementType($(this).val());
            });
            return false;
        });

        $('#addFundingResource').click(function () {
            $('#AvailableFundingResources option:selected').each(function (i) {
                addFundingResource($(this).val());
            });
            return false;
        });

        $('#removeFundingResource').click(function () {
            $('#EligibleFundingResources option:selected').each(function (i) {
                removeFundingResource($(this).val());
            });
            return false;
        });

        function addAgency(id) {
            $.ajax({
                type: "POST",
                url: addurl,
                dataType: "json",
                data: { agencyId: id },
                success: function (response) {
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
                success: function (response) {
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

        function addImprovementType(id) {
            $.ajax({
                type: "POST",
                url: addImprovementTypeurl,
                dataType: "json",
                data: { improvementTypeId: id },
                success: function (response) {
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        var selector = "#AvailableImprovementTypes option[value='" + id + "']";
                        $(selector).remove().prependTo('#EligibleImprovementTypes');
                    } else {
                        ShowMessageDialog('Error adding ImprovementType', response.Error);
                    }
                }
            });
        }

        function removeImprovementType(id) {
            $.ajax({
                type: "POST",
                url: removeImprovementTypeurl,
                dataType: "json",
                data: { improvementTypeId: id },
                success: function (response) {
                    //alert("success");
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        $("#EligibleImprovementTypes option[value='" + id + "']").remove().prependTo('#AvailableImprovementTypes');
                    } else {
                        //alert(response.Error);
                        ShowMessageDialog('ImprovementType was not Removed', response.Error);
                    }
                }
            });
        }

        function addFundingResource(id) {
            $.ajax({
                type: "POST",
                url: addFundingResourceurl,
                dataType: "json",
                data: { fundingResourceId: id },
                success: function (response) {
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        var selector = "#AvailableFundingResources option[value='" + id + "']";
                        $(selector).remove().prependTo('#EligibleFundingResources');
                    } else {
                        ShowMessageDialog('Error adding FundingResourceId', response.Error);
                    }
                }
            });
        }

        function removeFundingResource(id) {
            $.ajax({
                type: "POST",
                url: removeFundingResourceurl,
                dataType: "json",
                data: { fundingResourceId: id },
                success: function (response) {
                    //alert("success");
                    if ((response.Error == null) || (response.Error == "")) {
                        //success
                        $("#EligibleFundingResources option[value='" + id + "']").remove().prependTo('#AvailableFundingResources');
                    } else {
                        //alert(response.Error);
                        ShowMessageDialog('FundingResourceId was not Removed', response.Error);
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
                Ok: function () {
                    $(this).dialog('close');
                }
            }
        });


    });          //end $().ready
    
    
</script>

    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.SurveyName = '<%= Model.Current.Name %>';
        $(document).ready(App.tabs.initializeSurveyTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<div class="view-content-container">
<div class="clear"></div>

<%Html.RenderPartial("~/Views/Survey/Partials/TabPartial.ascx", Model.Current); %>

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
        <%if (Model.Current.IsEditable())
          { %>
        <a href="#" id="add"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
        <a href="#" id="remove"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
        <%} %>
        </td>
        <td>
          <%= Html.ListBox("EligibleAgencies", new MultiSelectList(Model.GetEligibleAgencySelectList().Items, "OrganizationId", "OrganizationName"), new {  @class = "mediumInputElement", size = 10 })%><br/>
        </td>
        </tr>
        </table>
            
      <p>Note: Changes are stored to the database as they are made in the interface.</p>
    </div>

    <div id="eligibleImprovementTypeForm" class="tab-form-container">
    <h2>Eligible ImprovementTypes List</h2>
       
        <table>
        <tr>
        <td>Available ImprovementTypes:</td>
        <td>&nbsp;</td>
        <td>Eligible ImprovementTypes:</td>
        </tr>
        <tr>
        <td>
        <%= Html.ListBox("AvailableImprovementTypes", new MultiSelectList(Model.GetAvailableImprovementTypesSelectList().Items, "Id", "Description"), new {  @class = "mediumInputElement", size = 10 })%><br/>        
        </td>
        <td>
        <%if (Model.Current.IsEditable())
          { %>
        <a href="#"  id="addImprovementType"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
        <a href="#"  id="removeImprovementType"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
        <%} %>
        </td>
        <td>
          <%= Html.ListBox("EligibleImprovementTypes", new MultiSelectList(Model.GetEligibleImprovementTypesSelectList().Items, "Id", "Description"), new { @class = "mediumInputElement", size = 10 })%><br/>
        </td>
        </tr>
        </table>
      <p>Note: Changes are stored to the database as they are made in the interface.</p>
    
    </div>

    <div id="eligibleFundingResourceForm" class="tab-form-container">
    <h2>Eligible Funding Resources List</h2>
       
        <table>
        <tr>
        <td>Available Funding Resources:</td>
        <td>&nbsp;</td>
        <td>Eligible Funding Resources:</td>
        </tr>
        <tr>
        <td>
        <%= Html.ListBox("AvailableFundingResources", new MultiSelectList(Model.GetAvailableFundingResourcesSelectList().Items, "FundingResourceId", "FundingType"), new { @class = "mediumInputElement", size = 10 })%><br/>        
        </td>
        <td>
        <%if (Model.Current.IsEditable())
          { %>
        <a href="#"  id="addFundingResource"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
        <a href="#"  id="removeFundingResource"  ><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
        <%} %>
        </td>
        <td>
          <%= Html.ListBox("EligibleFundingResources", new MultiSelectList(Model.GetEligibleFundingResourcesSelectList().Items, "FundingResourceId", "FundingType"), new { @class = "mediumInputElement", size = 10 })%><br/>
        </td>
        </tr>
        </table>
      <p>Note: Changes are stored to the database as they are made in the interface.</p>
    
    </div>
</div>
<div class="clear"></div>


<div id="dialog" title="">
	<p id="dialogMessage"></p>
</div>


</asp:Content>



