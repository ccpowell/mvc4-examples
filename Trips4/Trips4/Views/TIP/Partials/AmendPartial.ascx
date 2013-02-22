<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.TIP.AmendmentsViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid" %>
<div style='display: none'>
    <div id="dialog-amend-project" class="dialog" title="Amend Plan Projects">
        <h2>
            Projects ready for amending:<span id="countReady"></span></h2>
        <div id="amend-list">
            <!-- jquery will add ul here -->
        </div>
        <div class="dialog-result" style="display: none;">
            <span></span>.
        </div>
    </div>
</div>
<div style="position: absolute; top: -9999px;">
    <select id="amendmentList" multiple="multiple">
    </select>
</div>
<script type="text/javascript" charset="utf-8">
    var AmendProjects = '<%=Url.Action("Amend","TIP") %>';
    var GetAmendableProjects = '<%=Url.Action("GetProjectsByAmendmentStatusId","TIP") %>';
    //var DeleteProjectVersion = '<%=Url.Action("DeleteProjectVersion","RTP") %>'; SetPlanCurrent
    //var SetPlanCurrent = '<%=Url.Action("SetPlanCurrent","RTP") %>';
    //var SetActiveCycle = '<%=Url.Action("SetActiveCycle","RTP") %>';
    var ResetSessionVal = '<%=Url.Action("ResetSearchModel", "TIP", new { year = Model.TipSummary.TipYear }) %>';

    $(document).ready(function () {

        $("#btn-amendprojects").colorbox({
            width: "900px",
            height: "475px",
            inline: true,
            href: "#dialog-amend-project",
            onLoad: function () {
                getAmendableProjectsList();
            },
            onOpen: function () {
                $(document).unbind("keydown.cbox_close");
            },
            onComplete: function () {
                var buttonAmendProjects = $('<span id="button-amend-projects" class="cboxBtn">Amend</span>').appendTo('#cboxContent');
                var searchForm = $("<form class='cboxSearchForm'><label for='amendmentSearch'>Search</label><input type='text' id='amendmentSearch' class='cboxInput' /></form>").appendTo('#cboxContent')
                var projectCount = $('<span id="cboxProjectCount" style="display: none;" class="cboxLabel">Project Count: <span></span> </span>').appendTo('#cboxContent');
                var timePeriodId = "<%= Model.TipSummary.TipYearTimePeriodID.ToString() %>";

                $('#button-amend-projects').live("click", function () {

                    var element = $("#amendablelist li");
                    var queueStatus = $("#countReady");

                    if (!$(this).hasClass("disabled")) {
                        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
                        $("#cboxClose").hide();

                        var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };

                        $.each(element, function (i, o) {
                            var id = $(this).attr("id");
                            var projectVersionId = id.replace("row-amendable-", "");

                            App.postit("/api/TipProjectAmendment", {
                                data: JSON.stringify({ ProjectVersionId: projectVersionId, AmendmentStatusId: 1767 }),
                                success: function (response) {
                                    obj.eachCount--;
                                    obj.countSuccess++;

                                    updateRestoreQueue(queueStatus, obj.eachCount);
                                    $("#" + id).remove();
                                },
                                error: function (response, textStatus, AjaxException) {
                                    obj.countError++;
                                }
                            });

                        });
                        $(obj).watch('eachCount', function (propName, oldVal, newVal) {
                            if (newVal == 0) {
                                if (obj.countSuccess > 0) {
                                    //setPlanCurrent(timePeriodId);
                                }
                                showRestoreResponse(obj.countError, obj.countSuccess);

                                $('#button-amend-projects').html("Amend").removeClass("disabled");
                                $("#cboxClose").show();
                            }
                        });
                    }

                    return false;
                });
                //$("input#amendmentSearch").quicksearch("ul#amendablelist li");


            },
            onClosed: function () {
                $("#button-amend-projects").remove();

                location = ResetSessionVal;
            }
        });

        //        $('#amendablelist li > div').live("click", function() {
        //            var id = $(this).attr('id');
        //            if (confirm("Are you sure you want to delete this project version?") == true) {
        //                deleteAmendment(id);
        //            }
        //            return false;
        //        });

    });

    function showRestoreResponse(error, success) {
        //if (error > 0 || success > 0) {
        var message = "Amend Summary: <br/>-----------------------------------";
        if (success > 0) { message += "<br/>Successful: " + success; }
        if (error > 0) { message += "<br/>Errored: " + error; }
        $('.dialog-result').html(message);
        $('.dialog-result').addClass(error > 0 ? 'error' : 'success').show();
        $.fn.colorbox.resize();
        //};
    };
    function updateRestoreQueue(element, count) {
        element.text(count + " left to process");
    }

    function getAmendableProjectsList() {
        $.ajax({
            type: "POST",
            url: GetAmendableProjects,
            data: "timePeriod=<%= Model.TipSummary.TipYear %>"
                + "&amendmentStatusId=<%= (int)DRCOG.Domain.Enums.TIPAmendmentStatus.Proposed %>",
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                $("#amend-list").buildSummaryList(response);
                $("#cboxProjectCount").countItems($("ul#amendablelist li")).show();
            },
            error: function (response, textStatus, AjaxException) {
                alert("error");
            }
        });
    }

    //    function deleteAmendment(id) {
    //        var result = $('.dialog-result');
    //        var countElement = $("#cboxProjectCount span");
    //        
    //        $.ajax({
    //            type: "POST",
    //            url: DeleteProjectVersion,
    //            data: "projectVersionId=" + id,
    //            dataType: "json",
    //            success: function(response, textStatus, XMLHttpRequest) {
    //                if (response.error == "false") {
    //                    result.html(response.message);
    //                    result.addClass('success').autoHide({ wait: 2500, removeClass: "success" });
    //                    $('#row-amendable-' + id).remove();
    //                    $("#amendmentList").removeOption(id);
    //                    var count = parseInt(countElement.html()) - 1;
    //                    countElement.html(count);
    //                } else {
    //                    result.html(response.message + " Details: " + response.exceptionMessage);
    //                    result.addClass('error').autoHide({ wait: 10000 });
    //                }
    //            },
    //            error: function(response, textStatus, AjaxException) {
    //                //$('').html(response.statusText);
    //                //$('').addClass('error');
    //                //autoHide(10000);
    //            }
    //        });
    //    }

    //    function setPlanCurrent(timePeriodId) {
    //        $.ajax({
    //            type: "POST",
    //            url: SetPlanCurrent,
    //            data: "timePeriodId=" + timePeriodId,
    //            dataType: "json",
    //            success: function(response, textStatus, XMLHttpRequest) {
    //                if (response.error == "false") {
    //                    //result.html(response.message);
    //                    //result.addClass('success').autoHide({ wait: 2500, removeClass: "success" });
    //                    //$('#row-amendable-' + id).remove();
    //                    //$("#amendmentList").removeOption(id);
    //                    //var count = parseInt(countElement.html()) - 1;
    //                    //countElement.html(count);
    //                } else {
    //                    //result.html(response.message + " Details: " + response.exceptionMessage);
    //                    //result.addClass('error').autoHide({ wait: 10000 });
    //                }
    //                $('.dialog-result').append("<br />" + response.message);
    //            },
    //            error: function(response, textStatus, AjaxException) {
    //                //$('').html(response.statusText);
    //                //$('').addClass('error');
    //                //autoHide(10000);
    //            }
    //        });
    //    }

    $.fn.buildSummaryList = function (data) {
        return this.each(function () {
            var list = $(this);
            var obj = "<ul id='amendablelist'>";
            $.each(data, function (index, itemData) {
                obj += "<li class='relative' id='row-amendable-" + itemData.ProjectVersionId + "'>"
                + itemData.TipId + ": " + itemData.SponsorAgency
                + ", " + itemData.ProjectName
                + "</li>";

                $("#amendmentList").addOption(itemData.ProjectVersionId, itemData.ProjectVersionId, false);
            });
            obj += "</ul>";
            list.html(obj);

        });
    };

    $.fn.countItems = function (data) {
        return this.each(function () {
            var element = $(this);
            var itemCount = 0;
            $.each(data, function (index, itemData) {
                itemCount++;
            });
            element.children("span").html(itemCount);
        });
    };

    jQuery.fn.watch = function (id, fn) {

        return this.each(function () {

            var self = this;

            var oldVal = self[id];
            $(self).data(
            'watch_timer',
            setInterval(function () {
                if (self[id] !== oldVal) {
                    fn.call(self, id, oldVal, self[id]);
                    oldVal = self[id];
                }
            }, 100)
        );

        });

        return self;
    };

    jQuery.fn.unwatch = function (id) {

        return this.each(function () {
            clearInterval($(this).data('watch_timer'));
        });

    };
</script>
