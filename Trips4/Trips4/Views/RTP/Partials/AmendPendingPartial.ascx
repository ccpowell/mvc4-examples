<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.RTP.ProjectListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<div style='display:none'>
    <div id="dialog-amendpending-project" class="dialog" title="Amend Plan Projects">
        <div class="dialog-result" style="display:none; position: absolute; top: -40px; z-index: 1200;">
          <span></span>.
        </div>
        <h2>Projects ready for amending:<span id="countReady"></span></h2>
        <div id="amend-list"><!-- jquery will add ul here --></div>
        
        
    </div>
    
</div>

<div style="position: absolute; top: -9999px;">
    <select id="amendmentList" multiple="multiple"></select>
</div>


<script type="text/javascript" charset="utf-8">
    var AmendPendingProjects = '<%=Url.Action("Amend","RTP") %>';
    var GetAmendablePendingProjects = '<%=Url.Action("GetAmendablePendingProjects","RTP") %>';
    var DeleteProjectVersion = '<%=Url.Action("DeleteProjectVersion","RTP") %>';
    var SetPendingActiveCycle = '<%=Url.Action("SetActiveCycle","RTP") %>';
    var ResetSessionVal = '<%=Url.Action("ResetSearchModel", "RTP", new { year = Model.RtpSummary.RtpYear }) %>';
    var InitialCycle = '<%= Model.RtpSummary.Cycle.Id %>';


    $(document).ready(function () {
        $("#btn-amendpendingprojects").colorbox({
            width: "900px",
            height: "475px",
            inline: true,
            href: "#dialog-amendpending-project",
            onLoad: function () {
                //getAvailableProjects();
                getAmendablePendingProjectsList(InitialCycle);
            },
            onOpen: function () {
                $(document).unbind("keydown.cbox_close");
            },
            onComplete: function () {
                var buttonAmendProjects = $('<span id="button-amend-projects" class="cboxBtn">Amend</span>').appendTo('#cboxContent');
                var searchForm = $("<form class='cboxSearchForm'><label for='amendmentSearch'>Search</label><input type='text' id='amendmentSearch' class='cboxInput' /></form>").appendTo('#cboxContent')
                var projectCount = $('<span id="cboxProjectCount" style="display: none;" class="cboxLabel">Project Count: <span></span> </span>').appendTo('#cboxContent');
                var timePeriodId = "<%= Model.RtpSummary.RTPYearTimePeriodID.ToString() %>";

                $('#button-amend-projects').live("click", function () {

                    var element = $("#amendablelist li");
                    var cycleId = '<%= Model.RtpSummary.Cycle.Id %>';
                    var queueStatus = $("#countReady");

                    //var queueStatus = $("#countReady");

                    //var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };
                    //$("#countReady").html(eachCount);


                    if (!$(this).hasClass("disabled")) {
                        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
                        $("#cboxClose").hide();

                        var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };

                        $.each(element, function (i, o) {
                            var id = $(this).attr("id");
                            var projectVersionId = id.replace("row-amendable-", "");

                            //alert(projectVersionId);
                            $.ajax({
                                type: "POST",
                                url: AmendPendingProjects,
                                data: "projectVersionId=" + projectVersionId
                                    + "&cycleId=" + cycleId,
                                dataType: "json",
                                success: function (response, textStatus, XMLHttpRequest) {
                                    if (response.error == "false") {
                                        obj.eachCount--;
                                        obj.countSuccess++;

                                        updateRestoreQueue(queueStatus, obj.eachCount);
                                        $("#" + id).remove();
                                        //element.remove();
                                        //$('').html(response.message);
                                        //$('').addClass('success');
                                        //autoHide(2500);
                                        //oProjectListGrid.fnAddData([
                                        //    "<a href=\"/RtpProject/" + response.data.RtpYear + "/Info/" + response.data.ProjectVersionId + "\">" + response.data.Title + "</a>",
                                        //    response.data.PlanType,
                                        //    response.data.COGID,
                                        //    response.data.TIPId,
                                        //    response.data.ImprovementType,
                                        //    response.data.SponsorAgency,
                                        //    response.data.AmendmentStatus,
                                        //    response.data.ProjectVersionId]);
                                        //obj.eachCount--;
                                        //obj.countSuccess++;
                                        //removeProject(id, false, null);
                                        //updateRestoreQueue(queueStatus, obj.eachCount);
                                    } else {
                                        obj.eachCount--;
                                        obj.countError++;
                                        //obj.countError++;
                                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                                        //$('.dialog-result').addClass('error');
                                        //autoHide(10000);
                                    }
                                    window.onbeforeunload = null;
                                },
                                error: function (response, textStatus, AjaxException) {
                                    obj.countError++;
                                    //alert("error");
                                    //obj.countError++;
                                    //$('').html(response.statusText);
                                    //$('').addClass('error');
                                    //autoHide(10000);
                                }
                            });

                        });
                        $(obj).watch('eachCount', function (propName, oldVal, newVal) {
                            //alert(newVal);
                            if (newVal == 0) {
                                if (obj.countSuccess > 0) {
                                    //setPlanCurrent(timePeriodId);
                                    setPendingActiveCycle(cycleId, timePeriodId);

                                }
                                showRestoreResponse(obj.countError, obj.countSuccess);

                                $('#button-amend-projects').html("Amend").removeClass("disabled");
                                $("#cboxClose").show();
                            }
                        });
                    }

                    return false;
                });

                $("input#amendmentSearch").quicksearch("ul#amendablelist li");


            },
            onClosed: function () {
                $("#button-amend-projects").remove();

                location = ResetSessionVal;
            }
        });

        $('#amendablelist li > div').live("click", function () {
            var id = $(this).attr('id');
            if (confirm("Exclude from Amendment?"/*"Are you sure you want to delete this project version?"*/) == true) {
                deleteAmendment(id);
            }

            return false;
        });

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

    function setPendingActiveCycle(cycleId, timePeriodId) {
        $.ajax({
            type: "POST",
            url: SetPendingActiveCycle,
            data: "cycleId=" + cycleId
                    + "&timePeriodId=" + timePeriodId,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    //location.reload();
                    //$('div#resultRecordDetail').html(response.message);
                    //$('div#resultRecordDetail').addClass('success');
                    //autoHide(2500);
                    window.onbeforeunload = null;
                }
                else {
                    //$('div.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                    //$('div.dialog-result').addClass('error').show();
                    //autoHide(10000);
                }

            },
            error: function(response, textStatus, AjaxException) {
                //$('div.dialog-result').html(response.statusText);
                //$('div.dialog-result').addClass('error').show();
                //autoHide(10000);
            }
        });
        return false;
    }

    function getAmendablePendingProjectsList(cycleId) {
        $("#amend-list").html("<h2>Loading...</h2>");
        $.ajax({
            type: "POST",
            url: GetAmendablePendingProjects,
            data: "plan=<%= Model.RtpSummary.RtpYear %>"
                + "&cycleId=" + cycleId,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                $("#amend-list").html("");
                $("#amend-list").buildSummaryList(response);
                $("#cboxProjectCount").countItems($("ul#amendablelist li")).show();
            },
            error: function(response, textStatus, AjaxException) {
                alert("error");
            }
        });
    }

    function deleteAmendment(id) {
        var result = $('.dialog-result');
        var countElement = $("#cboxProjectCount span");

        // All we want to do here is remove it from the option to amend.
        /*
        result.html("Project removed from this amendment");
        result.addClass('success').autoHide({ wait: 2500, removeClass: "success" });
        $('#row-amendable-' + id).remove();
        $("#amendmentList").removeOption(id);
        var count = parseInt(countElement.html()) - 1;
        countElement.html(count);
        */
        
        //  This would actually really delete the version
        $.ajax({
            type: "POST",
            url: DeleteProjectVersion,
            data: "projectVersionId=" + id,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    result.html(response.message);
                    result.addClass('success').autoHide({ wait: 2500, removeClass: "success" });
                    $('#row-amendable-' + id).remove();
                    $("#amendmentList").removeOption(id);
                    var count = parseInt(countElement.html()) - 1;
                    countElement.html(count);
                } else {
                    result.html(response.message + " Details: " + response.exceptionMessage);
                    result.addClass('error').autoHide({ wait: 10000 });
                }
            },
            error: function(response, textStatus, AjaxException) {
                //$('').html(response.statusText);
                //$('').addClass('error');
                //autoHide(10000);
            }
        });
    }

    function setPlanCurrent(timePeriodId) {
        $.ajax({
            type: "POST",
            url: SetPlanCurrent,
            data: "timePeriodId=" + timePeriodId,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    //result.html(response.message);
                    //result.addClass('success').autoHide({ wait: 2500, removeClass: "success" });
                    //$('#row-amendable-' + id).remove();
                    //$("#amendmentList").removeOption(id);
                    //var count = parseInt(countElement.html()) - 1;
                    //countElement.html(count);
                } else {
                    //result.html(response.message + " Details: " + response.exceptionMessage);
                    //result.addClass('error').autoHide({ wait: 10000 });
                }
                $('.dialog-result').append("<br />" + response.message);
            },
            error: function(response, textStatus, AjaxException) {
                //$('').html(response.statusText);
                //$('').addClass('error');
                //autoHide(10000);
            }
        });
    }

    $.fn.buildSummaryList = function(data) {
        return this.each(function() {
            var list = $(this);
            var obj = "<ul id='amendablelist'>";
            $.each(data, function(index, itemData) {
                obj += "<li class='relative' id='row-amendable-" + itemData.ProjectVersionId + "'>"
                + "<div class='iremove' id='" + itemData.ProjectVersionId + "' title='Remove " + itemData.ProjectName + "'></div>"
                + itemData.SponsorAgency + " " + itemData.ProjectVersionId
                + ": " + itemData.RtpYear
                + ", " + itemData.ProjectName
                + "</li>";
                $("#amendmentList").addOption(itemData.ProjectVersionId, itemData.ProjectVersionId, false);
            });
            obj += "</ul>";
            list.html(obj);

        });
    };

    $.fn.buildCycleList = function(data) {
        return this.each(function() {
            var list = $(this);
            var obj = "";
            $.each(data, function(index, itemData) {
                if (InitialCycle != itemData.Id) {
                    obj += " <a id='cycle-" + itemData.Id + "' class='cycle-changer'>" + itemData.Name + "</a>";
                }
            });
            list.html(obj);
        });
    };

    $.fn.countItems = function(data) {
        return this.each(function() {
            var element = $(this);
            var itemCount = 0;
            $.each(data, function(index, itemData) {
                itemCount++;
            });
            element.children("span").html(itemCount);
        });
    };

    jQuery.fn.watch = function(id, fn) {

        return this.each(function() {

            var self = this;

            var oldVal = self[id];
            $(self).data(
            'watch_timer',
            setInterval(function() {
                if (self[id] !== oldVal) {
                    fn.call(self, id, oldVal, self[id]);
                    oldVal = self[id];
                }
            }, 100)
        );

        });

        return self;
    };

    jQuery.fn.unwatch = function(id) {

        return this.each(function() {
            clearInterval($(this).data('watch_timer'));
        });

    };
</script>