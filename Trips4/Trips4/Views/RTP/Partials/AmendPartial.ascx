<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.RTP.ProjectListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<div style='display:none'>
    <div id="dialog-amend-project" class="dialog" title="Amend Plan Projects">
        <fieldset>
            <div class="info">Select the projects you wish to amend in the next cycle</div>
            <table id="amendProjects">
                <tr>
                    <td>
                        Available Projects: <%--<div>From Cycle: <span id="cycle-list"></span></div>--%>
                        <%--<form id="availableProjectSearchForm"><label for="availableProjectSearch">Search</label><input type="text" id="availableProjectSearch" /></form>--%>
                    </td>
                    <td>&nbsp;</td>
                    <td>Selected Projects: <span id="amend-countReady"></span></td>
                </tr>
                <tr>
                    <td>
                        <select id="amend-availableProjects" class="w400 nobind" size="25" multiple="multiple">.</select>
                    </td>
                    <td>
                        <a href="#" id="amend-addProject" title="Add Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                        <a href="#" id="amend-removeProject" title="Remove Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                    </td>
                    <td>
                      <select id="amend-selectedProjects" name="CycleAmendment.SelectedProjects" class="w400 nobind" size="25" multiple="multiple"></select>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="dialog-result" style="display:none;">
          <span></span>.
        </div>
    </div>
</div>



<script type="text/javascript" charset="utf-8">
    var GetAmendableProjects = '<%=Url.Action("GetAmendableProjects","RTP") %>'; //GetAvailableRestoreProjects
    var AmendProjectsUrl = '<%=Url.Action("Amend","RTP") %>';
    //var GetPlanCycles = '<%=Url.Action("GetPlanCycles","RTP") %>';
    var CurrentCycle = '<%= Model.RtpSummary.Cycle.Id %>';
    var SetActiveCycle = '<%=Url.Action("SetActiveCycle","RTP") %>';
    var PriorCycle = '<%= Model.RtpSummary.Cycle.PriorCycleId %>';
    var IsCycleActive = '<%= Model.RtpSummary.Cycle.StatusId.Equals((int)DRCOG.Domain.Enums.RTPCycleStatus.Active) %>';

    var cycleWatcherObj = { counter: 1, originalCycleId: 0, newCycleId: 0 };

    //var RestoreProject = '<%=Url.Action("Restore","RTP") %>';
    $(document).ready(function () {
        $("#btn-amendprojects, #btn-includemore").colorbox({
            width: "900px",
            height: "475px",
            inline: true,
            href: "#dialog-amend-project",
            escKey: false,
            overlayClose: false,
            onLoad: function () {
                //alert(PriorCycle + " " + CurrentCycle);
                getAmendableProjectsList(PriorCycle == 0 || IsCycleActive ? CurrentCycle : PriorCycle);
                //getPlanCycles();
            },
            onOpen: function () {
                $(document).unbind("keydown.cbox_close");
            },
            onComplete: function () {
                var buttonAmendProjects = $('<span id="button-amend-projects" class="cboxBtn disabled">Amend</span>').appendTo('#cboxContent');
                var timePeriodId = "<%= Model.RtpSummary.RTPYearTimePeriodID.ToString() %>";
                var ChangeStatus = !$(this).hasClass("includemore");
                //$("input#availableProjectSearch").search("select#availableProjects option");

                $('#button-amend-projects').live("click", function () {
                    var element = $("#amend-selectedProjects option");
                    var queueStatus = $("#amend-countReady");
                    var cycleId = '<%= Model.RtpSummary.Cycle.Id %>';

                    //alert(cycleId);
                    if (!$(this).hasClass("disabled") && element.length > 0) {
                        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
                        $("#cboxClose").hide();

                        var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };

                        cycleWatcherObj.originalCycleId = cycleId;

                        if (ChangeStatus) {
                            setActiveCycle(cycleId, timePeriodId);

                            $(cycleWatcherObj).watch('newCycleId', function (propName, oldVal, newVal) {
                                if (cycleWatcherObj.newCycleId == 0) {
                                    //cycleWatcherObj.counter++;
                                } else {
                                    $(cycleWatcherObj).unwatch();
                                    cycleId = cycleWatcherObj.newCycleId;
                                    amendProjects(element, cycleWatcherObj.newCycleId, obj);
                                }
                            });

                        } else { amendProjects(element, cycleId, obj); }

                    }

                    return false;
                });

                $.fn.colorbox.resize();

                $('.cycle-changer').live("click", function () {
                    var id = $(this).attr("id").replace("cycle-", "");
                    getAmendableProjectsList(id);
                });

            },
            onClosed: function () {
                $("#button-amend-projects").remove();
                resetRestoreSelects();
                $(".dialog-result").removeClass("success error");
                location.reload();
            }
        });

        $('#amend-addProject').click(function () {
            $('#amend-availableProjects option:selected').each(function (i) {
                $("#amend-availableProjects option[value='" + $(this).val() + "']").remove().prependTo('#amend-selectedProjects').attr('selected', false);
            });
            $("#button-amend-projects").removeClass("disabled");
            return false;
        });

        $('#amend-removeProject').click(function () {
            $('#amend-selectedProjects option:selected').each(function (i) {
                //$("#selectedProjects option[value='" + $(this).val() + "']").remove().prependTo('#availableProjects').attr('selected', false);
                removeProject($(this).val(), true, "amend-#availableProjects");
            });
            return false;
        });

    });

    function amendProjects(element, cycleId, watchObj) {
        $.each(element, function (i, o) {
            var id = $(this).val();
            $.ajax({
                type: "POST",
                url: AmendProjectsUrl,
                data: "projectVersionId=" + id
                    + "&cycleId=" + cycleId,
                dataType: "json",
                success: function (response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$('').html(response.message);
                        //$('').addClass('success');
                        //autoHide(2500);
                        watchObj.eachCount--;
                        watchObj.countSuccess++;
                        removeProject(id, false, null);
                        updateRestoreQueue(queueStatus, watchObj.eachCount);
                    } else {
                        watchObj.countError++;
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                    window.onbeforeunload = null;
                },
                error: function (response, textStatus, AjaxException) {
                    //alert("error");
                    watchObj.countError++;
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            //$("#countReady").text(eachCount + ", " + countError + ", " + countSuccess);
        });

        $(watchObj).watch('eachCount', function (propName, oldVal, newVal) {
            if (newVal == 0) {
                //                                if (obj.countSuccess > 0 && ChangeStatus) {
                //                                    //setPlanCurrent(timePeriodId);
                //                                    setActiveCycle(cycleId, timePeriodId);

                //                                }
                showRestoreResponse(watchObj.countError, watchObj.countSuccess);
                $('#button-amend-projects').html("Amend").removeClass("disabled");
                $("#cboxClose").show();
            }
        });
    }

    function showRestoreResponse(error, success) {
        //if (error > 0 || success > 0) {
            var message = "Process Summary: <br/>-----------------------------------";
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
    
    // move = true/false
    function removeProject(id, move, toElementName) {
        var element = $("#amend-selectedProjects option[value='" + id + "']");
        element.remove();
        if (move) {
            element.prependTo(toElementName).attr('selected', false);
        }
        if ($("#amend-selectedProjects option").length == 0) {
            $("#button-amend-projects").addClass("disabled");
        }
    };

    function addProject(id) {
        $("#amend-availableProjects option[value='" + id + "']").remove().prependTo('#amend-selectedProjects').attr('selected', false);
    };

    function getAmendableProjectsList(cycleId) {
        $.ajax({
            type: "POST",
            url: GetAmendableProjects,
            data: "plan=<%= Model.RtpSummary.RtpYear %>"
                + "&cycleId=" + cycleId,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                resetRestoreSelects();
                $('#amend-availableProjects').addItems(response);
            },
            error: function(response, textStatus, AjaxException) {
                alert("Your session has expired.");
                location.reload();
            }
        });
    }

//    function getPlanCycles() {
//        $.ajax({
//            type: "POST",
//            url: GetPlanCycles,
//            data: "plan=<%= Model.RtpSummary.RtpYear %>",
//            dataType: "json",
//            success: function(response, textStatus, XMLHttpRequest) {
//                $("#cycle-list").buildCycleList(response);
//            },
//            error: function(response, textStatus, AjaxException) {
//                alert("error");
//            }
//        });
//    }

    function resetRestoreSelects() {
        $('#amend-availableProjects').clearSelect();
        $('#amend-selectedProjects').clearSelect();
    }

    function setActiveCycle(cycleId, timePeriodId) {
        
        $.ajax({
            type: "POST",
            url: SetActiveCycle,
            data: "cycleId=" + cycleId
                    + "&timePeriodId=" + timePeriodId,
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    //location.reload();
                    //$('div#resultRecordDetail').html(response.message);
                    //$('div#resultRecordDetail').addClass('success');
                    //autoHide(2500);
                    cycleWatcherObj.newCycleId = response.data;
                    window.onbeforeunload = null;
                }
                else {
                    //$('div.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                    //$('div.dialog-result').addClass('error').show();
                    //autoHide(10000);
                }

            },
            error: function (response, textStatus, AjaxException) {
                //$('div.dialog-result').html(response.statusText);
                //$('div.dialog-result').addClass('error').show();
                //autoHide(10000);
            }
        });
        //return false;
    }

    $.fn.clearSelect = function() {
        return this.each(function() {
            if (this.tagName == 'SELECT')
                this.options.length = 0;
        });
    };
    $.fn.addItems = function(data) {
        return this.each(function() {
            var list = this;
            $.each(data, function(index, itemData) {
                var option = new Option(itemData.Text, itemData.Value);
                // stupid browser differences. IE does one thing where
                // All other browsers need the null thingy
                if ($.browser.msie) {
                    list.add(option);
                }
                else {
                    list.add(option, null);
                }
            });
            list.remove(''); // removed empty values
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

//    $.fn.buildCycleList = function(data) {
//        return this.each(function() {
//            var list = $(this);
//            var obj = "";
//            $.each(data, function(index, itemData) {
//                //if (InitialCycle != itemData.Id) {
//                    obj += " <a id='cycle-" + itemData.Id + "' class='cycle-changer'>" + itemData.Name + "</a>";
//                //}
//            });
//            list.html(obj);
//        });
//    };

    
</script>