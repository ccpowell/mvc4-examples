<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<div style='display:none'>
    <div id="dialog-amend-project" class="dialog" title="New Survey Project Creation">
        <fieldset>
            <div class="info">Select the projects you wish to include in the next Survey</div>
            <table id="amendProjects">
                <tr>
                    <td>
                        Available Projects:
                    </td>
                    <td>&nbsp;</td>
                    <td>Selected Projects: <span id="amend-countReady"></span></td>
                </tr>
                <tr>
                    <td>
                        <select id="amend-availableProjects" class="w400 nobind" size="25" multiple="multiple"></select>
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

    $(document).ready(function () {


        $('#amend-addProject').click(function () {
            $('#amend-availableProjects option:selected').each(function (i) {
                $("#amend-availableProjects option[value='" + $(this).val() + "']").remove().prependTo('#amend-selectedProjects').attr('selected', false);
            });
            return false;
        });

        $('#amend-removeProject').click(function () {
            $('#amend-selectedProjects option:selected').each(function (i) {
                removeProject($(this).val(), true, "amend-#availableProjects");
            });
            return false;
        });

        $('#btn_createsurvey').click(function () {
            var surveyname = $('#new_survey #surveyname').val();
            if (surveyname != '' && timePeriodId != '') {
                createSurvey(timePeriodId, surveyname);
            }
            return false;
        });
        $('#btn-includemore').click(function () {
            if (timePeriodId != '') {
                openSurveySelector(timePeriodId);
            }
            return false;
        });


    });
    function createSurvey(planId, surveyName) {
        $.ajax({
            type: "POST",
            url: CreateSurveyUrl,
            data: "planId=" + planId
                + "&surveyName=" + surveyName.toString(),
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    var surveyId = response.data;
                    if (surveyId != '') {
                        openSurveySelector(surveyId);
                    }
                } else {
                    alert(response.exceptionMessage);
                }
            },
            error: function (response, textStatus, AjaxException) {
                alert(response.exceptionMessage);
            }
        });
        return false;
    }

    function openSurveySelector(surveyId) {
        $.fn.colorbox({
            width: "900px",
            height: "475px",
            inline: true,
            href: "#dialog-amend-project",
            escKey: false,
            overlayClose: false,
            open: true,
            onLoad: function () {
                getAmendableProjectsList();
            },
            onOpen: function () {
                $(document).unbind("keydown.cbox_close");
            },
            onComplete: function () {
                var buttonAmendProjects = $('<span id="button-amend-projects" class="cboxBtn">Amend</span>').appendTo('#cboxContent');
                //var ChangeStatus = !$(this).hasClass("includemore");

                $('#button-amend-projects').live("click", function () {
                    var element = $("#amend-selectedProjects option");
                    var queueStatus = $("#amend-countReady");

                    if (!$(this).hasClass("disabled")) {
                        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
                        $("#cboxClose").hide();

                        var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };

                        $.each(element, function (i, o) {
                            var id = $(this).val();
                            $.ajax({
                                type: "POST",
                                url: AmendProjects,
                                data: "projectVersionId=" + id
                                    + "&surveyId=" + surveyId,
                                dataType: "json",
                                success: function (response, textStatus, XMLHttpRequest) {
                                    if (response.error == "false") {
                                        obj.eachCount--;
                                        obj.countSuccess++;
                                        removeProject(id, false, null);
                                        updateRestoreQueue(queueStatus, obj.eachCount);
                                    } else {
                                        obj.countError++;
                                    }
                                    window.onbeforeunload = null;
                                },
                                error: function (response, textStatus, AjaxException) {
                                    obj.countError++;
                                }
                            });
                        });

                        $(obj).watch('eachCount', function (propName, oldVal, newVal) {
                            if (newVal == 0) {
                                $(obj).unwatch('eachCount');
                                
                                showRestoreResponse(obj.countError, obj.countSuccess);
                                $('#button-amend-projects').html("Amend").removeClass("disabled");
                                $("#cboxClose").show();
                            }
                        });
                    }
                    return false;
                });

                $.fn.colorbox.resize();
            },
            onClosed: function () {
                $("#button-amend-projects").remove();
                resetRestoreSelects();
                $(".dialog-result").removeClass("success error");
                location.reload();
            }
        });
    }
    function showRestoreResponse(error, success) {
        var message = "Survey creation summary: <br/>-----------------------------------";
        if (success > 0) { message += "<br/>Successful: " + success; }
        if (error > 0) { message += "<br/>Errored: " + error; }
        $('.dialog-result').html(message);
        $('.dialog-result').addClass(error > 0 ? 'error' : 'success').show();
        $.fn.colorbox.resize();
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
    };

    function addProject(id) {
        $("#amend-availableProjects option[value='" + id + "']").remove().prependTo('#amend-selectedProjects').attr('selected', false);
    };

    function getAmendableProjectsList() {
        $.ajax({
            type: "POST",
            url: GetAmendableProjects,
            data: "timePeriodId=" + timePeriodId,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                resetRestoreSelects();
                $('#amend-availableProjects').addItems(response);
            }
        });
    }

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
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    window.onbeforeunload = null;
                }
                else {
                }

            },
            error: function(response, textStatus, AjaxException) {
            }
        });
        return false;
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