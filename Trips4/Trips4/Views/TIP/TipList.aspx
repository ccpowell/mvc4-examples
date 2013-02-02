<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TipListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP List</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script type="text/javascript" charset="utf-8">
    $(document).ready(function () {
        <% if ( Model.IsAdmin ) { %>
            $('#tipListGrid').dataTable(
            {
                "aaSorting": [[2,'asc'],[1, 'desc']],
                "aoColumns": [{ "bSortable": false, 'sWidth':'50px' }, {'sWidth':'100px'}, null, null, null, null, null, null, null],
                "bLengthChange": false,
                "bPaginate": false
            });
        <% } %>
    });
</script>
   
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.TipYear = '<%=  Model.TipSummary.TipYear %>';
        $(document).ready(App.tabs.initializeTipTabs);
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
<div class="view-content-container">
    <h2>Transportation Improvement Program</h2>    
    <% if ( Model.IsAdmin ) { %>
        <%= Html.Grid(Model.TIPs).Columns(column => {
            column.For(x => Html.ActionLink("Details", "Dashboard", new {controller="Tip", year = x.TipYear })).Encode(false);
	        column.For(x => x.TipYear).Named("TIP Year");
            column.For(x => x.GetStatus()).Named("Status");
            column.For(x => x.PublicHearing).Named("Public Hearing").Format("{0:M/d/yyyy}");
            column.For(x => x.Adoption).Named("Adoption").Format("{0:M/d/yyyy}");
            column.For(x => x.LastAmended).Named("Last Amended").Format("{0:M/d/yyyy}");
            column.For(x => x.GovernorApproval).Named("Gov. Approval").Format("{0:M/d/yyyy}");
            column.For(x => x.USDOTApproval).Named("USDOT Approval").Format("{0:M/d/yyyy}");
            column.For(x => x.EPAApproval).Named("EPA Approval").Format("{0:M/d/yyyy}");
        }).Attributes(id=> "tipListGrid") %>
    <% } %>

    <a href="<%= Url.Action("Dashboard", "Tip", new {year = Model.CurrentTip.TipYear }) %>" title="Search Current TIP <%= Model.CurrentTip.TipYear %>">Search Current TIP <%= Model.CurrentTip.TipYear %></a>

    <%= Html.DropDownListFor(model => model.TIPs, Model.NotInTipYear(Model.CurrentTip.TimePeriodId), new { @class = "mediumInputElement big" })%>

    <div class="clear"></div>
    <div class="belowTable">
        <% if (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator")) { %>
            <a id="create-tip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="#">
                <span class="ui-icon ui-icon-circle-plus"></span>Create New TIP
            </a>
        <% } %>

    </div>      
  
</div>

<div id="dialog" title="Create New TIP">
	<p id="validateTips">All fields are required.</p>
	<form>
	<fieldset>
		<label for="startYear">Start Year:</label>
		<input type="text" name="startYear" id="startYear" class="text ui-widget-content ui-corner-all" />
		<label for="endYear">End Year:</label>
		<input type="text" name="endYear" id="endYear" value="" class="text ui-widget-content ui-corner-all" />		
		<label for="offset">Years offset:</label>
	    <select name="offset" id="offset" class="ui-widget-content ui-corner-all">
	        <option value="3">Offset 3 ( 1,2,3,4-6 )</option>
	        <option value="4">Offset 4 ( 1,2,3,4,5-6 )</option>
	    </select>
	</fieldset>
	</form>
</div>

<script type="text/javascript">
    var createTipUrl = "<%=Url.Action("CreateTip","TIP")%>";
    
    $(document).ready(function() 
    {
        //wire up the button
        $('#create-tip').click(function() {
            $('#dialog').dialog('open');
        })
		.hover(
			function() {
			    $(this).addClass("ui-state-hover");
			},
			function() {
			    $(this).removeClass("ui-state-hover");
			}
		).mousedown(function() {
		    $(this).addClass("ui-state-active");
		})
		.mouseup(function() {
		    $(this).removeClass("ui-state-active");
		});
    });



    $(function() {

        var startYear = $("#startYear"),
			endYear = $("#endYear"),
			allFields = $([]).add(startYear).add(startYear),
			tips = $("#validateTips");
			offset = $("#offset");

        $("#dialog").dialog({
            bgiframe: true,
            autoOpen: false,
            height: 290,
            modal: true,
            buttons: {
                'Create New TIP': function() {
                    var bValid = true;
                    allFields.removeClass('ui-state-error');
                    bValid = bValid && validateYear(startYear, "Start Year");
                    bValid = bValid && validateYear(endYear, "End Year");
                    bValid = bValid && validateYearRange(startYear, endYear);
                    if (bValid) {
                        //xhrPost this back to the server and wait for
                        //success response before closing
                        $.post(createTipUrl,{startYear:startYear.val(),endYear:endYear.val(),offset:offset.val()},CreateTipCallback);
                        //$(this).dialog('close');
                    }
                },
                Cancel: function() {
                    $(this).dialog('close');
                }
            },
            close: function() {
                allFields.val('').removeClass('ui-state-error');
            }
        });

        function CreateTipCallback(){
            $("#dialog").dialog('close');
            location.reload();
        }
        
        function validateOffset(o) {
            if (isNaN(o.val())) {
                updateTips('Offset must be a number.');
                o.addClass('ui-state-error');
                return false;
            }
        }

        function validateYear(o, n) {

            if (isNaN(o.val())) {
                updateTips(n + ' must be a 4 digit year.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val().length > 4 || o.val().length < 4) {
                updateTips(n + ' must be a 4 digit year.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val() > 2100 || o.val() < 1990) {
                updateTips(n + ' must be between 1990 and 2100.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val() > 2100 || o.val() < 1990) {
                updateTips(n + ' must be between 1990 and 2100.');
                o.addClass('ui-state-error');
                return false;
            }
            return true;
        }

        function validateYearRange(s, e) {
            if (s.val() > e.val()) {
                updateTips('Start Year must be earlier than End Year.');
                s.addClass('ui-state-error');
                e.addClass('ui-state-error');
                return false;
            } else { return true; }

        }

        function updateTips(t) {
            tips.text(t).effect("highlight", {}, 1500);
        }


    });

</script>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

