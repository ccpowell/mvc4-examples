<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RtpListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP List</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
        $('#rtpListGrid').dataTable(
    {
        "aaSorting": [[1, 'desc']],
        "aoColumns": [{ "bSortable": false }, {'sWidth':'100px'}, null, null, null, null, null, null],
        "bLengthChange": false,
        "bPaginate": false
    });
    });

</script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
<div class="view-content-container">
    <h2>Regional Transportation Plans</h2>    

 <%= Html.Grid(Model.RTPs).Columns(column => {
        column.For(x => Html.ActionLink("Details", "Dashboard", new {controller="Rtp", year = x.Plan })).Encode(false);
	    column.For(x => x.Plan).Named("Plan Year");
        column.For(x => x.RtpSummary.GetStatus()).Named("Status");
        column.For(x => x.PublicHearing).Named("Public Hearing").Format("{0:M/d/yyyy}");
        column.For(x => x.Adoption).Named("Adoption").Format("{0:M/d/yyyy}");
        column.For(x => x.LastAmended).Named("Last Amended").Format("{0:M/d/yyyy}");
        column.For(x => x.USDOTApproval).Named("USDOT Approval").Format("{0:M/d/yyyy}");
        column.For(x => x.CDOTAction).Named("CDOT Action").Format("{0:M/d/yyyy}");  
    }).Attributes(id=> "rtpListGrid") %>

    <div class="clear"></div>
    <div class="belowTable">
        
        <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator")) { %>
            <a id="create-rtp" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="#">
                <span class="ui-icon ui-icon-circle-plus"></span>Create New Plan
            </a>
	    <% } %>

    </div>      
  
</div>

<div id="dialog" title="Create New Plan">
	<p id="validateTips">All fields are required.</p>
	<form>
	<fieldset>
	    <label for="startYear">Plan Name:</label>
		<input type="text" name="planName" id="planName" class="text ui-widget-content ui-corner-all" />
		<%--<label for="startYear">Start Year:</label>
		<input type="text" name="startYear" id="startYear" class="text ui-widget-content ui-corner-all" />
		<label for="endYear">End Year:</label>
		<input type="text" name="endYear" id="endYear" value="" class="text ui-widget-content ui-corner-all" />	--%>	
		<%--<label for="offset">Years offset:</label>
		<input type="text" name="offset" id="offset" value="" class="text ui-widget-content ui-corner-all" />--%>
	</fieldset>
	</form>
</div>

<script type="text/javascript">
    var createRtpUrl = "<%=Url.Action("CreateRtp","RTP")%>";
    
    $(document).ready(function() 
    {
        //wire up the button
        $('#create-rtp').click(function() {
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

        var planName = $("#planName"),
            //startYear = $("#startYear"),
			//endYear = $("#endYear"),
			allFields = $([]).add(planName),//.add(startYear).add(startYear),
			rtps = $("#validateRtps");

        $("#dialog").dialog({
            bgiframe: true,
            autoOpen: false,
            height: 280,
            modal: true,
            buttons: {
                'Create New RTP': function() {
                    var bValid = true;
                    allFields.removeClass('ui-state-error');
                    //bValid = bValid && validateYear(startYear, "Start Year");
                    //bValid = bValid && validateYear(endYear, "End Year");
                    //bValid = bValid && validateYearRange(startYear, endYear);

                    if (bValid) {
                        //xhrPost this back to the server and wait for
                        //success response before closing
                        //$.post(createRtpUrl,{startYear:startYear.val(),endYear:endYear.val()},CreateRtpCallback);
                        $.post(createRtpUrl,{planName:planName.val()},CreateRtpCallback);
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

        function CreateRtpCallback(){
            $("#dialog").dialog('close');
            location.reload();
        }
        
        function validateOffset(o) {
            if (isNaN(o.val())) {
                updateRtps('Offset must be a number.');
                o.addClass('ui-state-error');
                return false;
            }
        }

        function validateYear(o, n) {

            if (isNaN(o.val())) {
                updateRtps(n + ' must be a 4 digit year.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val().length > 4 || o.val().length < 4) {
                updateRtps(n + ' must be a 4 digit year.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val() > 2100 || o.val() < 1990) {
                updateRtps(n + ' must be between 1990 and 2100.');
                o.addClass('ui-state-error');
                return false;
            }
            if (o.val() > 2100 || o.val() < 1990) {
                updateRtps(n + ' must be between 1990 and 2100.');
                o.addClass('ui-state-error');
                return false;
            }
            return true;
        }

        function validateYearRange(s, e) {
            if (s.val() > e.val()) {
                updateRtps('Start Year must be earlier than End Year.');
                s.addClass('ui-state-error');
                e.addClass('ui-state-error');
                return false;
            } else { return true; }

        }

        function updateRtps(t) {
            rtps.text(t).effect("highlight", {}, 1500);
        }


    });

</script>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>

