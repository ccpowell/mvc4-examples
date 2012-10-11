<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TipDashboardViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>


<fieldset>
    <table id="restoreProjects">
        <tr>
            <td>
                Available Projects:
                <%--<form id="availableProjectSearchForm"><label for="availableProjectSearch">Search</label><input type="text" id="availableProjectSearch" /></form>--%>
            </td>
            <td>&nbsp;</td>
            <td>Selected Projects: <span id="countReady"></span></td>
        </tr>
        <tr>
            <td>
                <select id="availableProjects" class="w400 nobind" size="25" multiple="multiple">.</select>
            </td>
            <td>
                <a href="#" id="addProject" title="Add Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                <a href="#" id="removeProject" title="Remove Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
            </td>
            <td>
              <select id="selectedProjects" name="CycleAmendment.SelectedProjects" class="w400 nobind" size="25" multiple="multiple"></select>
            </td>
        </tr>
    </table>
</fieldset>

<div class="dialog-result" style="display:none;">
  <span></span>.
</div>


<script type="text/javascript" charset="utf-8">
    var GetAvailableRestoreProjects = '<%=Url.Action("GetAvailableRestoreProjects","TIP") %>';
    var RestoreProject = '<%=Url.Action("Restore","TIP") %>';

    $(document).ready(function() {

        $("#btn-restoreproject").colorbox({
            width: "900px",
            height: "475px",
            inline: true,
            href: "#dialog-restore-project",
            escKey: false,
            overlayClose: false,
            onLoad: function() {
                getAvailableProjects();
            },
            onOpen: function() {
                $(document).unbind("keydown.cbox_close");
            },
            onComplete: function() {
                var $buttonRestoreProjects = $('<span id="button-restore-projects" class="cboxBtn">Import</span>').appendTo('#cboxContent');
                $('#button-restore-projects').live("click", function() {

                    var element = $("#selectedProjects option");
                    var queueStatus = $("#countReady");

                    if (!$(this).hasClass("disabled")) {
                        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
                        $("#cboxClose").hide();
                        //$().unbind("keydown.cbox_close");

                        var obj = { eachCount: element.length, countSuccess: 0, countError: 0 };
                        //$("#countReady").html(eachCount);

                        $.each(element, function(i, o) {
                            var id = $(this).val();
                            //alert(i);
                            $.ajax({
                                type: "POST",
                                url: RestoreProject,
                                data: "timePeriod=<%= Model.TipSummary.TipYear.ToString() %>"
                                    + "&id=" + id,
                                dataType: "json",
                                success: function(response, textStatus, XMLHttpRequest) {
                                    if (response.error == "false") {
                                        //$('').html(response.message);
                                        //$('').addClass('success');
                                        //autoHide(2500);
                                        obj.eachCount--;
                                        obj.countSuccess++;
                                        removeProject(id, false, null);
                                        updateRestoreQueue(queueStatus, obj.eachCount);
                                    } else {
                                        obj.countError++;
                                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                                        //$('.dialog-result').addClass('error');
                                        //autoHide(10000);
                                    }
                                    window.onbeforeunload = null;
                                },
                                error: function(response, textStatus, AjaxException) {
                                    //alert("error");
                                    obj.countError++;
                                    //$('').html(response.statusText);
                                    //$('').addClass('error');
                                    //autoHide(10000);
                                }
                            });
                            //$("#countReady").text(eachCount + ", " + countError + ", " + countSuccess);
                        });

                        $(obj).watch('eachCount', function(propName, oldVal, newVal) {
                            //alert(newVal);
                            if (newVal == 0) {
                                showRestoreResponse(obj.countError, obj.countSuccess);
                                $('#button-restore-projects').html("Restore").removeClass("disabled");
                                $("#cboxClose").show();
                            }
                        });
                    }

                    return false;
                });


            },
            onClosed: function() {
                $("#button-restore-projects").remove();
                resetRestoreSelects();
                $(".dialog-result").removeClass("success error");
                location.reload();
            }
        });

        $('#addProject').click(function() {
            $('#availableProjects option:selected').each(function(i) {
                $("#availableProjects option[value='" + $(this).val() + "']").remove().prependTo('#selectedProjects').attr('selected', false);
            });
            return false;
        });

        $('#removeProject').click(function() {
            $('#selectedProjects option:selected').each(function(i) {
                //$("#selectedProjects option[value='" + $(this).val() + "']").remove().prependTo('#availableProjects').attr('selected', false);
                removeProject($(this).val(), true, "#availableProjects");
            });
            return false;
        });

    });

    function showRestoreResponse(error, success) {
        //if (error > 0 || success > 0) {
            var message = "Restoration Summary: <br/>-----------------------------------";
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
        var element = $("#selectedProjects option[value='" + id + "']");
        element.remove();
        if (move) {
            element.prependTo(toElementName).attr('selected', false);
        }
    };

    function addProject(id) {
        $("#availableProjects option[value='" + id + "']").remove().prependTo('#selectedProjects').attr('selected', false);
    };

    function getAvailableProjects() {
        $.ajax({
            type: "POST",
            url: GetAvailableRestoreProjects,
            data: "timePeriodId=<%= Model.TipSummary.TipYearTimePeriodID.ToString() %>",
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                $('#availableProjects').clearSelect().addItems(response);
                //qs.cache();
            },
            error: function(response, textStatus, AjaxException) {
                alert("Your session has expired.");
                location.reload();
            }
        });
    }

    function resetRestoreSelects() {
        $('#availableProjects').clearSelect();
        $('#selectedProjects').clearSelect();
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