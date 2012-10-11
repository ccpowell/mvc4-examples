<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.DelaysViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"><%= Model.TipSummary.TipYear %> - <%= Model.DelayYear %> Delays</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.autoresize.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.tzCheckbox.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/FixedHeader.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/TableTools.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/ZeroClipboard.js")%>" type="text/javascript"></script>
<script type="text/javascript" charset="utf-8">
    var asInitVals = new Array();

    $(document).ready(function () {
        //    $('#projectListGrid').dataTable({
        //        "iDisplayLength": 50,
        //        "aaSorting": [[1, "asc"], [2, "asc"]],
        //        "aoColumns": [{ "bSortable": false }, { "sWidth": "100px" }, null, null, null, null, null, null]
        //    });

        $.ajaxSetup({
            // Disable caching of AJAX responses
            cache: false
        });


        var oTable = $("#delaysGrid").dataTable({
            "oLanguage": {
                "sSearch": "Search all columns:"
            },
            "bStateSave": true,
            "aaSorting": [[2, "asc"], [0, 'desc']],
            "iDisplayLength": 25,
            "sDom": 'T<"clear">lfrtip',
            "oTableTools": {
                "sSwfPath": "/Scripts/swf/copy_cvs_xls_pdf.swf"
            },
            "aoColumns": [
                    { "sName": "TIP ID" },
                    { "sName": "Project Name" },
                    { "sName": "Sponsor" },
                    { "sName": "Phase" },
                    { "sName": "Affected Location" },
                    { "sName": "", "bVisible": true, "bSortable": false, "bSearchable": false }
                ],
            "fnInitComplete": function (oSettings, json) {
                $('#delaysGrid_wrapper #delaysGrid_filter').append(constructDelaysLocationfilter());
                $('#delaysGrid_wrapper #delaysGrid_length').append('<label style="margin-left: 10px;" id="dt-legend" class="bold">Legend: <span class="legend delay">Previous Year Delay</span> <span class="legend checked">Status Updated</span></label>');
                // RETAIN FILTER VALUES ON PAGE RELOAD
                $("#delaysGrid_wrapper input#delays_location").val(oSettings.aoPreSearchCols[4].sSearch);
            },
            "fnDrawCallback": function (oSettings, json) {
                $('.openDialog').colorbox({
                    scrolling: false,
                    width: "600px",
                    onComplete: function () {
                        var $detailwrapper = $("#contactdetailwrapper");
                        var $cboxLoadedContent = $("#cboxLoadedContent");
                        var $cboxContent = $("#cboxContent");
                        var editurl = "";
                        var cboxedit = '';

                        var isDelay = $(this).attr("delay") !== '' ? true : false;

                        $("#colorbox").draggable({ handle: "#cboxBottomCenter", opacity: 0.35, cursor: 'move' });

                        $('textarea.resizable').autoResize({
                            // On resize:
                            onResize: function () {
                                $(this).css({ opacity: 0.8 });
                                $.colorbox.resize();
                            },
                            // After resize:
                            animateCallback: function () {
                                $(this).css({ opacity: 1 });
                            },
                            // Quite slow animation:
                            animateDuration: 300,
                            // Less extra space:
                            extraSpace: 10,
                            limit: 300
                        });

                        $('input[type=checkbox]').tzCheckbox({ labels: ['Yes', 'No'] });

                        var cboxsubmit = $('<span id="cbox-submit" class="cboxBtn">Save</span>').appendTo('#cboxContent');

                        var $management = $("#managementContainer");

                        $management.hide();


                        var $save = $("#cbox-submit");

                        $("form#form-delayupdate :submit").hide();

                        var $form = $("form#form-delayupdate");

                        //$.validator.unobtrusive.parseDynamicContent($form);

                        $form.find('input[data-val-required="Required"]').addClass("required");

                        bindSave();

                        $('<span class="cboxLbl">|</span>').appendTo('#cboxContent');

                        var cboxcheckstate_flagged = $('<span id="cbox-checkstate" class="cboxBtn flag">Save as Checked</span>');
                        var cboxcheckstate_unflagged = $('<span id="cbox-checkstate" class="cboxBtn unflag">Save as Unchecked</span>');

                        var $Entity_IsChecked = $management.find('#IsChecked');
                        if ($Entity_IsChecked.attr("CHECKED") === 'checked') {
                            cboxcheckstate_unflagged.appendTo('#cboxContent');
                        } else cboxcheckstate_flagged.appendTo('#cboxContent');

                        var $cbox_checkstate = $("#cbox-checkstate");

                        $cbox_checkstate.unbind("click").bind("click", function () {
                            if ($(this).hasClass("flag")) {
                                $Entity_IsChecked.attr("CHECKED", "checked");
                                //$(this).removeClass("flag").addClass("unflag").text("Unflag as Checked");
                            } else {
                                $Entity_IsChecked.removeAttr("CHECKED");
                                //$(this).removeClass("unflag").addClass("flag").text("Flag as Checked");
                            }

                            $save.trigger('click');
                        });

                        $.fn.colorbox.resize();



                        function bindSave() {
                            $save.bind("click", function () {
                                $save.unbind("click").addClass("disabled").text("Saving .....");
                                // remove masking for normalized saving
                                //$("input[id*=Phone]").unmask().mask("9999999999");
                                //$("#Entity_ZipCode").unmask().mask("99999?9999");

                                $.post($form.attr('action'), $form.serialize(), function (json) {
                                    if (json.error === 'false') {
                                        $save.hide();
                                        $(".cboxBtn").remove();
                                        $(".cboxLbl").remove();
                                        var cboxsubmit = $('<span class="cboxLbl">Successfully Saved</span>')
                                            .appendTo('#cboxContent')
                                            .delay(1000)
                                            .hide("fast", function () {
                                                $('<span class="cboxLbl">Saved, Preparing to close...</span>').appendTo('#cboxContent')
                                                    .delay(1000)
                                                    .hide("fast", function () {
                                                        setTimeout(function () {
                                                            location.reload(true);
                                                        }, 0);
                                                    });
                                            });
                                    } else {
                                        $cboxLoadedContent.append('<div class="savedetails-result error"><span>' + json.message + '</span>.</div>');
                                        $.fn.colorbox.resize();
                                        $(".savedetails-result").delay(5000).hide("fast", function () {
                                            $.fn.colorbox.resize();
                                        });

                                        $save.unbind("click").bind("click").removeClass("disabled").text("Save");

                                        $form.find('input.required').each(function (index) {
                                            if ($(this).val() === "") {
                                                $(this).removeClass('valid').addClass('input-validation-error');
                                            }
                                        });
                                    }
                                }, 'json');
                            });
                        };
                    },
                    onClosed: function () {
                        $("#cbox-submit").unbind("click");
                        $(".cboxLbl").remove();
                        $(".cboxBtn").remove();
                    }
                });
            }
        });

        if (oTable.length > 0) {
            new FixedHeader(oTable);

            // add download link
            var url = '<%= Url.Action("DownloadDelays", new { year = Model.TipSummary.TipYear, id = Model.DelayYear }) %>';

            var dttt_container = $('#delaysGrid_wrapper div.DTTT_container');
            $('<button class="DTTT_button DTTT_button_xls" id="button_print_formatted" title="Download Formatted Report"><span>Download Report</span></button>').appendTo(dttt_container);

            $('#button_print_formatted').click(function (e) {
                e.preventDefault();
                location.href = url;
            });

        }

        $("#delaysGrid_wrapper input").keyup(function (e) {
            /* Filter on the column (the index) of this element */

            var attr = $(this).attr("index");
            if (typeof attr !== 'undefined' && attr !== false) {
                oTable.fnFilter(this.value, attr);
            };
        });


        /*
        * Support functions to provide a little bit of 'user friendlyness' to the textboxes in 
        * the footer
        */
        $("#delaysGrid_wrapper input").each(function (i) {
            asInitVals[i] = this.value;
        });

        $("#delaysGrid_wrapper input").focus(function () {
            if (this.className == "search_init") {
                this.className = "";
                this.value = "";
            }
        });

        $("#delaysGrid_wrapper input").blur(function (i) {
            if (this.value == "") {
                this.className = "search_init";
                this.value = asInitVals[$("#delaysGrid_wrapper input").index(this)];
            }
        });


    });

    function constructDelaysLocationfilter() {
        var html = '<input type="text" name="delays_location" id="delays_location" index="4" value="Search Delay Location" class="search_init" />';
        return html;
    }
</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>

    <div class="tab-content-container">
    
    <% if (Model.Delays.Count() > 0) { %>
    <h2><%= Model.DelayYear %> Project Delays Management</h2>
    <div style="position: absolute; text-align: right; top: -7px; left: 300px; width: 500px;">
        <ul>
            <% foreach (string year in Model.DelayYears)
                { if(!year.Equals(Model.DelayYear)) {%><li><%= Html.ActionLink(year, "Delays", new { year = Model.TipSummary.TipYear, id = year }, new { @class = "fg-button ui-state-default ui-corner-all", @style = "text-decoration: none" })%></li><%} } %>
        </ul>
    </div>
   <%= Html.Grid(Model.Delays).Columns(column =>
        {
            column.For(x => Html.ActionLink(x.TipId, "Funding", new { controller = "Project", year = Model.TipSummary.TipYear, id = x.ProjectVersionId })).Encode(false).Named("TIPID").Attributes(data => new MvcContrib.Hash(@style => "width:75px"));
            column.For(x => x.ProjectName).Named("Project Name");
            column.For(x => x.Sponsor).Named("Sponsor");
            column.For(x => x.Phase).Named("Phase");
            column.For(x => x.AffectedProjectDelaysLocation).Named("Affected Location");
            column.For(x =>
                Html.ActionLink("Update", "DelayUpdate", new
                {
                    controller = "TIP"
                    ,
                    phaseId = x.PhaseId
                    ,
                    projectFinancialRecordId = x.ProjectFinancialRecordId
                    ,
                    fundingIncrementId = x.FundingIncrementId
                    ,
                    fundingResourceId = x.FundingResourceId
                    ,
                    projectVersionId = x.ProjectVersionId
                }, new { @class = "fg-button ui-state-default ui-corner-all openDialog", @delay = x.IsDelay ? "delay" : "" })).Attributes(data => new MvcContrib.Hash(@style => "width:75px")); //style => "width:75px", delay => ( x.IsDelay) ? "delayed" : "");
        }).Attributes(id => "delaysGrid")
                   .RowAttributes(data => new MvcContrib.Hash(@class => ((data.Item.IsDelay) ? "delayed" : "" + (data.Item.IsChecked ? "checked" : ""))))
    %>
    <% } else { %>
        <h2>Select a Delay year in the <%= Model.TipSummary.TipYear %> TIP to Management:</h2>
        <ul>
            <% foreach (string year in Model.DelayYears)
               { %><li><%= Html.ActionLink(year, "Delays", new { year = Model.TipSummary.TipYear, id = year }, new { @class = "fg-button ui-state-default ui-corner-all", @style = "text-decoration: none" })%></li><% } %>
        </ul>
    <% }  %>
    <div class="belowTable">
    
    
    <%--<a id="button-create-project" class="fg-button w380 ui-state-default ui-corner-all" href="#">Create Completely New Project</a>--%>
    <%--<a id="createNewTip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Project", new {tipYear=Model.TipSummary.TipYear}) %>">--%>
    <%--<span class="ui-icon ui-icon-circle-plus"></span>Create New Project</a>--%>
    
    </div>
    </div>
</div>

</asp:Content>


