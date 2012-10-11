<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.StatusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Status</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= ResolveUrl("~/Content/jquery.contextMenu.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery-ui-1.8.5.custom.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.contextMenu.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    var isDirty = false, formSubmittion = false;
    var addurl = '<%=Url.Action("AddCycle","RTP", new {plan=Model.RtpSummary.RtpYear}) %>';
    var removeurl = '<%=Url.Action("DropCycle","RTP") %>';
    var createCycleUrl = '<%=Url.Action("CreateCycle","RTP") %>';
    var GetPlanAvailableProjects = '<%=Url.Action("GetPlanAvailableProjects","RTP") %>';
    var AmendProjects = '<%=Url.Action("Amend","RTP") %>';
    var SetActiveCycle = '<%=Url.Action("SetActiveCycle","RTP") %>';
    var updateCycleSortUrl = '<%=Url.Action("UpdateCycleSort","RTP") %>';
    var updateTimePeriodStatusIdUrl = '<%=Url.Action("UpdateTimePeriodStatusId","RTP") %>';
    var SetSurveyDatesUrl = '<%=Url.Action("SetSurveyDates","RTP") %>';
    var GetSurveyDatesUrl = '<%=Url.Action("GetSurveyDates","RTP") %>';
    var OpenSurveyNowUrl = '<%=Url.Action("OpenSurveyNow","RTP") %>';
    var CloseSurveyNowUrl = '<%=Url.Action("CloseSurveyNow","RTP") %>';

    var GetAmendableProjects = '<%=Url.Action("GetAmendableSurveyProjects","RTP") %>'; //GetAvailableRestoreProjects
    var AmendProjects = '<%=Url.Action("AmendForNewSurvey","RTP") %>';
    var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
    var CreateSurveyUrl = '<%=Url.Action("CreateSurvey","RTP") %>';
    
    $(document).ready(function() {
        
        var AvailableCycles = $("#AvailableCycles");
        var CurrentPlanCycles = $("#CurrentPlanCycles");
        
            if("<%= Model.RtpSummary.IsEditable() %>" == "False") {
                $(".isdatepicker").removeClass("isdatepicker");
            } else {
                $(".isdatepicker").removeClass("isdatepicker").addClass("datepicker");
            }
            
        
        
        $(".growable").growing({buffer:5});
        // Context Menus
        $(AvailableCycles).contextMenu({
           menu: "myMenu"
        }, 
        function(action, el, pos) {
            switch (action)
            {
                case "add": AddAvailableCycles(); break;
                case "edit":
                    var cycleModifier = $("#cycle-modifier");
                    buildCycleModifier(AvailableCycles, cycleModifier);
                    break;
            }
        }, 
        function() {
            if(resetContextMenu(AvailableCycles)) {
                $('#myMenu').enableContextMenuItems('#add');
            }
        });
        
        $(CurrentPlanCycles).contextMenu({
           menu: "myMenu"
        }, 
        function(action, el, pos) {
            switch (action)
            {
                case "remove": RemoveCurrentCycles(); break;
                case "edit":
                    var cycleModifier = $("#cycle-modifier");
                    buildCycleModifier(CurrentPlanCycles, cycleModifier);
                    break;
            }
        },  
        function() {
            if(resetContextMenu(CurrentPlanCycles)) {
                $('#myMenu').enableContextMenuItems('#remove');
            }
        });
        
        function resetContextMenu(source) {
            $('#myMenu').disableContextMenuItems();
            if($(source).selectedOptions().size() > 0 ) { 
                $('#myMenu').enableContextMenuItems('#edit');
                return true;
            }
            return false;
        }
        // End Context Menus
        
        
        function buildCycleModifier(source, target) {
            var options = $(source).selectedOptions();
            var sourceid = source.attr("id");
            target.html('');
            var container = "<ul>";
            $.each(options, function() {
                var id = $(this).val();
                var value = $(this).text();
                
                var content = '<li id="update-cycle-' + id + '">';
                    content += '<input type="text" name="cycleName.' + sourceid + '" value="' + value + '" />';
                    content += '<span class="update-cycle fg-button ui-state-default ui-priority-primary ui-corner-all">Update</span>';
                    content += '</li>';
                container += content;
            });
            container += "</table>";
            target.append(container);
            target.show();
            update();
        }
        
        
        var update = function() {
            var UpdateCycleName = '<%=Url.Action("UpdateCycleName","RTP") %>';
            $('.update-cycle').click(function() {
                var parent = $(this).parent();
                var cycle = $(parent).children("input");
                var cycleName = cycle.val();
                var sourceName = $(cycle).attr("name").replace("cycleName.", "");
                var id = $(parent).attr("id").replace("update-cycle-", "");
                
                $.ajax({
                    type: "POST",
                    url: UpdateCycleName,
                    data: "cycleId=" + id
                        + "&cycle=" + cycleName,
                    dataType: "json",
                    success: function(response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            $("#update-cycle-" + id).remove();
                            $("#" + sourceName).removeOption(id);
                            $("#" + sourceName).addOption(id, cycleName);
                            //$('').html(response.message);
                            //$('').addClass('success');
                            //autoHide(2500);
                        } else {
                            alert(response.exceptionMessage);
                            //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                            //$('.dialog-result').addClass('error');
                            //autoHide(10000);
                        }
                    },
                    error: function(response, textStatus, AjaxException) {
                        alert(response.exceptionMessage);
                        //$('').html(response.statusText);
                        //$('').addClass('error');
                        //autoHide(10000);
                    }
                });
                return false;
            });
        };
        
        //setup the date pickers
        $(".datepicker").datepicker();
        $(':input', document.statusForm).bind("change", function() { setConfirmUnload(true); }); // Prevent accidental navigation away
        $(':input', document.statusForm).bind("keyup", function() { setConfirmUnload(true); });
        $(':input.nobind', document.dataForm).unbind("change");
        $(':input.nobind', document.dataForm).unbind("keyup");
        if ($('#submitForm')) {
            $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
        }
        

        $('#cycle-nextCycleId').bind("change", function() {
            if ($(this).val() == '') {
                $('#plan-change-cycle').addClass('ui-state-disabled');
            } else { $('#plan-change-cycle').removeClass('ui-state-disabled'); }
            $('#availableProjects').clearSelect();
            $('#selectedProjects').clearSelect();
        });
        
        $('#cycle-initialCycleId').bind("change", function() {
            if ($(this).val() == '') {
                $('#button-set-initial-cycle').addClass('ui-state-disabled');
            } else { $('#button-set-initial-cycle').removeClass('ui-state-disabled'); }
        });
        
        function SurveyAction(id, action) {
            switch (action) {
                case "1"://SetOpen
                case "2"://SetClose
                    OpenSurveyDateDialog(id);
                    break;
                case "3"://Open
                    //SetSurveyDates(id, "<%= DateTime.UtcNow %>", null);
                    OpenSurveyDateDialog(id);
                    //OpenSurveyNow(id);
                    break;
                case "4"://Close
                    //alert("close now");
                    OpenSurveyDateDialog(id);
                    //CloseSurveyNow(id);
                    break;
            }
            return false;
        }
        
        function SurveyChangeButtonAction(id, action) {
            id.removeClass();
            
            id.addClass("action_" + action + " fg-button w75 ui-state-default ui-corner-all");
        }
        
        function SetSurveyDates(id, open, close) {
            //alert("Values: " + id + " " + open + " " + " " + close);

            $.ajax({
                type: "POST",
                url: SetSurveyDatesUrl,
                data: "Id=" + id
                    + "&OpeningDate=" + open
                    + "&ClosingDate=" + close,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        //$('').html(response.message);
                        //$('').addClass('success');
                        //autoHide(2500);
                        location.reload();
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        
        function OpenSurveyNow(id) {
            var element = $("#btn_survey_" + id);
            $.ajax({
                type: "POST",
                url: OpenSurveyNowUrl,
                data: "Id=" + id,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        $("#survey-result").addClass("success").html(response.message).autoHide();
                        
                        SurveyChangeButtonAction(element, 4);
                        
                        var currentTime = new Date();
                        var month = currentTime.getMonth() + 1;
                        var day = currentTime.getDate();
                        var year = currentTime.getFullYear();
                        
                        $("#survey_status_" + id).text("Opened " + month + "/" + day + "/" + year);
                        $(element).text("Close Now");
                        
                        //$('').addClass('success');
                        //autoHide(2500);
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        function CloseSurveyNow(id) {
            var element = $("#btn_survey_" + id);
            $.ajax({
                type: "POST",
                url: CloseSurveyNowUrl,
                data: "Id=" + id,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$("#update-cycle-" + id).remove();
                        //$("#" + sourceName).removeOption(id);
                        //$("#" + sourceName).addOption(id, cycleName);
                        //$('').html(response.message);
                        //$('').addClass('success');
                        $("#survey-result").addClass("success").html(response.message).autoHide();
                        
                        SurveyChangeButtonAction(element, 3);
                        
                        var currentTime = new Date();
                        var month = currentTime.getMonth() + 1;
                        var day = currentTime.getDate();
                        var year = currentTime.getFullYear();
                        
                        $("#survey_status_" + id).text("Closed " + month + "/" + day + "/" + year);
                        $(element).text("Open");
                    } else {
                        alert(response.exceptionMessage);
                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('.dialog-result').addClass('error');
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    alert(response.exceptionMessage);
                    //$('').html(response.statusText);
                    //$('').addClass('error');
                    //autoHide(10000);
                }
            });
            return false;
        }
        
        function OpenSurveyDateDialog(surveyid) {
            var id = $("#dialog_SurveyId").val(surveyid);
            var open = $("#dialog_SurveyOpen");
            var close = $("#dialog_SurveyClose");
            var openraw, closeraw;
            
            $.fn.colorbox({
                    width: "560px",
                    height: "240px",
                    inline: true,
                    href: "#dialog-set-survey-date",
                    onLoad: function() {
                        $.ajax({
                            type: "POST",
                            url: GetSurveyDatesUrl,
                            data: "Id=" + surveyid,
                            dataType: "json",
                            success: function(response, textStatus, XMLHttpRequest) {
                                if (response.error == "false") {
                                    $("#survey-result").addClass("success").html(response.message).autoHide();

                                    openraw = new Date(response.data.OpeningDateForJavascript);
                                    closeraw = new Date(response.data.ClosingDateForJavascript);

                                    open.val(openraw.getMonth() + 1 + "/" + (openraw.getDate() + 1) + "/" + openraw.getFullYear());
                                    close.val(closeraw.getMonth() + 1 + "/" + (closeraw.getDate() + 1) + "/" + closeraw.getFullYear());
                                } else {
                                    alert(response.exceptionMessage);
                                }
                            },
                            error: function(response, textStatus, AjaxException) {
                                alert(response.exceptionMessage);
                            }
                        });
                    },
                    onComplete: function() {
                        var $buttonCreateCycle = $('<span id="button-Set-Survey-dates" class="cboxBtn">Set Dates</span>').appendTo('#cboxContent');
                        $("#button-Set-Survey-dates").click(function() {
                            SetSurveyDates(id.val(), open.val(), close.val());
                        });
                    },
                    onClosed: function() {
                        $('.cboxBtn').remove();
                        id.val("");
                        open.val("");
                        close.val("");
                    }
                });
        }
        
        $('span[id^=btn_survey_]').live("click",function() {
         
            var element = $(this);
            var id = $(element).attr("id").replace("btn_survey_","");
            // get first class name which describes the action to take
            var action = $(element).attr('class').split(' ').slice(0, 1).toString().replace("action_", "");
            SurveyAction(id,action);
            
            return false;
        });

        $('span[id=btn_createsurvey]').live("click",function() {
            var surveyname = $("#new_survey #surveyname").val();
            SurveyAction(id,action);
            
            return false;
        });
        
        $(function() {
            $("#sortable2").sortable({
                connectWith: '.connectedSortable'
                ,
                placeholder: 'ui-state-highlight'
                ,
                items: 'li:not(.ui-state-disabled)'
                ,
                receive: function(event, ui) {
                    addCycle($(ui.item).attr("id").replace("cycle_",""));
                },
                remove: function(event, ui) {
                    removeCycle($(ui.item).attr("id").replace("cycle_",""));
                },
                update: function() {
                    $("#sortable2").sortable("refresh");
                    updateCycleSort();
                }
            })
            
            $("#sortable1").sortable({
                connectWith: '.connectedSortable'
                ,
                placeholder: 'ui-state-highlight'
            })
            
            $("#sortable1 li, #sortable2 li").disableSelection();
        });
        
        function updateCycleSort() {
            var values = $("#sortable2").sortable({items: 'li'}).sortable('toArray');
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            if(values.length > 0) {
                $.ajax({
                    type: "POST",
                    url: updateCycleSortUrl,
                    data: "cycles=" + values,
                    dataType: "json",
                    success: function(response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            //$('div#resultRecordDetail').html(response.message);
                            //$('div#resultRecordDetail').addClass('success');
                            //autoHide(2500);
                        }
                        else {
                            //$("#sortable2").sortable('cancel');
                            alert("Cycle error");
                        }
                    },
                    error: function(response, textStatus, AjaxException) {
                        alert("BIG error " + response.statusText);
                        //$('div.dialog-result').html(response.statusText);
                        //$('div.dialog-result').addClass('error').show();
                        //autoHide(10000);
                    }
                });
            }
            return false;
        }
        
        $(function() {
            var container = $("#plan-before-cycle");
            var statusId = <%= Model.RtpSummary.Cycle.StatusId %>;
            var message = "";
            
            $(":input.cycle-required", "#dataForm").each(function() {
                var element = $(this);
                
                var title = "";
                switch( element.get(0).nodeName.toLowerCase() ) {
                    
		            case 'select':
			            // could be an array for select-multiple or a string, both are fine this way
			            var val = $(element).val();
			            if (val.length == 0) { 
			                title += element.attr("title");
			            }
			            break;
		            case 'input':
			            if ( checkable(element) )
				            alert(getLength(value, element) > 0);
		            default:
			            return $.trim(element.val()).length > 0;
		        }
                
                if(title.length > 0) {
                    message += "<h3>" + title + "</h3>";
                }
            });
            
            if( message.length == 0 && statusId == 0 ) {
                message += "<h3>Below please move cycles from available to current. Put the initial cycle at the top then click the <span style='font-weight: bold'>Set Initial</span> button</h3>";
                $("#plan-initial-cycle").show();
                $("#button-set-initial-cycle").show();
            }
            else if (statusId != null) {
                $("#button-set-initial-cycle").hide();
                $("#plan-initial-cycle").hide();
            }
            
            if(message.length > 0) {
                container.prepend("<h2>Before you can continue...</h2>" + message);
                container.show();
            } else container.hide();
            
        });
        
        function checkable( element ) {
	        return /radio|checkbox/i.test(element.type);
        }
        
        function getLength(value, element) {
		    switch( element.nodeName.toLowerCase() ) {
		        case 'select':
			        return $("option:selected", element).length;
		        case 'input':
			        if( this.checkable( element) )
				        return this.findByName(element.name).filter(':checked').length;
		    }
		    return value.length;
        }

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            //Keep this in $().ready or add a $("#form").ajaxForm(); in $().ready
            submitHandler: function(form) {
                $(form).ajaxSubmit({
                    success: function(response) {
                        $('#result').html(response.message);
                        $('div#result').addClass('success');
                        $('#submitForm').addClass('ui-state-disabled');
                        location.reload();
                        autoHide(2500);
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $('#result').text(data.message);
                        $('#result').addClass('error');

                    },
                    dataType: 'json'
                });
            }
        });

        $('#add').click(function() {
            updateCycleSort();
            //AddAvailableCycles();
            return false;
        });
        
        $('#remove').click(function() {
            RemoveCurrentCycles();
            return false;
        });
        
        function AddAvailableCycles() {
            $('#AvailableCycles option:selected').each(function(i) {
                addCycle($(this).val());
            });
        }
        
        function RemoveCurrentCycles() {
            $('#CurrentPlanCycles option:selected').each(function(i) {
                removeCycle($(this).val());
            });
        }
        
        $('#addProject').click(function() {
            $('#availableProjects option:selected').each(function(i) {
                $("#availableProjects option[value='" + $(this).val() + "']").remove().prependTo('#selectedProjects').attr('selected',false);
            });
            return false;
        });
        
        $('#removeProject').click(function() {
            $('#selectedProjects option:selected').each(function(i) {
                $("#selectedProjects option[value='" + $(this).val() + "']").remove().prependTo('#availableProjects').attr('selected',false);
            });
            return false;
        });
        
        $('#button-set-initial-cycle').live("click", function() {
            var cycleId = $("#sortable2 li").first().attr("id").replace("cycle_","");
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            
            setActiveCycle(cycleId, timePeriodId);
            return false;
        });
        
        $('#button-plan-unlock').live("click", function() {
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            var statusId = "<%= Model.RtpSummary.GetNextStatus() %>";
            //alert(statusId);
            var message = "Are you sure you want to unlock this plan?";

            if (confirm(message) == true) {
                
            
                if(updateTimePeriodStatusId(timePeriodId, statusId)) {
                    //$("#button-plan-unlock").hide();
                    location.reload();
                
                }

                location.reload();
            }
            return false;
        });
        $('#button-plan-close').live("click", function() {
            var timePeriodId = "<%= Model.RtpStatus.TimePeriodId %>";
            var statusId = "<%= (int)DRCOG.Domain.Enums.RtpTimePeriodStatus.Inactive %>";
            
            var message = "Are you sure you want to close this plan?";

            if (confirm(message) == true) {
                if(updateTimePeriodStatusId(timePeriodId, statusId)) {
                    //$("#button-plan-unlock").hide();
                    location.reload();
                }
            }
            return false;
        });
        
        $('#button-process-cycle').click(function() {
            var projectList = $("#selectedProjects")
            
            $.ajax({
                type: "POST",
                url: AmendProjects,
                data: "CycleAmendment.SelectedProjects=" + projectList,
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        location.reload();
                        //$('div#resultRecordDetail').html(response.message);
                        //$('div#resultRecordDetail').addClass('success');
                        //autoHide(2500);
                        window.onbeforeunload = null;
                    }
                    else {
                        //$.fn.colorbox.close();
                        //$('div.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //$('div.dialog-result').addClass('error').show();
                        //autoHide(10000);
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    //$.fn.colorbox.close();
                    //$('div.dialog-result').html(response.statusText);
                    //$('div.dialog-result').addClass('error').show();
                    //autoHide(10000);
                }
            });
            return false;
            
        });

        $('.button-process-cycle-close').click(function() {
            $('#process-cycle-step2').fadeOut("fast");
            
            $('#plan-current-cycle.box').animate({
                "top": "+=230px",
                "left": "+=425px",
                "height": "70px",
                "width": "340px"
            }, "slow", function() {
                $('#process-cycle-step1').show();


            });
        });

        $('#plan-change-cycle').click(function() {
            getAvailableProjects($('#cycle-nextCycleId option:selected'));
            $('#process-cycle-step1').fadeOut("fast");

            $('#plan-current-cycle.box').animate({
                "top": "-=230px",
                "left": "-=425px",
                "height": "300px",
                "width": "850px"
            }, "slow", function() {
                $('#process-cycle-step2').delay(100).fadeIn("fast");
            });

        });

        //        $("#plan-change-cycle").colorbox({
        //            width: "460px",
        //            height: "340px",
        //            inline: true,
        //            href: "#dialog-cycle-projects",
        //            onLoad: function() {
        //                var $buttonCreateCycle = $('<span id="button-amend-cycle" class="cboxBtn">Amend to Cycle</span>').appendTo('#cboxContent');
        //            }
        //        });

        
        
        $("#button-create-cycle").colorbox({
            width: "460px",
            height: "240px",
            inline: true,
            href: "#dialog-create-cycle",
            onLoad: function() {
                var $buttonCreateCycle = $('<span id="createCycle" class="cboxBtn">Create</span>').appendTo('#cboxContent');

                $('#createCycle').live("click", function() {
                    var cycle = $('#Cycle').val();
                    $.ajax({
                        type: "POST",
                        url: createCycleUrl,
                        data: "cycle=" + cycle,
                        dataType: "json",
                        success: function(response, textStatus, XMLHttpRequest) {
                            if (response.error == "false") {
                                location.reload();
                                $.fn.colorbox.close();
                                //$('div#resultRecordDetail').html(response.message);
                                //$('div#resultRecordDetail').addClass('success');
                                //autoHide(2500);
                                window.onbeforeunload = null;
                            }
                            else {
                                //$.fn.colorbox.close();
                                $('div.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                                $('div.dialog-result').addClass('error').show();
                                autoHide(10000);
                            }

                        },
                        error: function(response, textStatus, AjaxException) {
                            //$.fn.colorbox.close();
                            $('div.dialog-result').html(response.statusText);
                            $('div.dialog-result').addClass('error').show();
                            autoHide(10000);
                        }
                    });
                    return false;
                });
            },
            onClosed: function() {
                $('.cboxBtn').remove();
                $('div.dialog-result').html('').removeClass('error').hide();
            }
        });

    });

    // *** General functions

    function autoHide(timeout) {
        if (isNaN(timeout)) timeout = 5000;
        setTimeout(function() {
            $("div#result").fadeOut("slow", function() {
            $("div#result").empty().removeClass().removeAttr('style');
            });
        }, timeout);
    }
    
    function setConfirmUnload(on) {

        $('#submitForm').removeClass('ui-state-disabled');
        $('#result').html("");        
        window.onbeforeunload = (on) ? unloadMessage : null;
    }

    function unloadMessage() {
        return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
    }
    
    function updateTimePeriodStatusId(timePeriodId, statusId) {
        $.ajax({
            type: "POST",
            url: updateTimePeriodStatusIdUrl,
            data: { timePeriodId: timePeriodId, statusId: statusId },
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    return true;
                    //$('div#resultRecordDetail').html(response.message);
                    //$('div#resultRecordDetail').addClass('success');
                    //autoHide(2500);
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
    }

    function addCycle(id) {
        $.ajax({
            type: "POST",
            url: addurl,
            dataType: "json",
            data: { cycleid: id },
            success: function(response) {
                if ((response.Error == null) || (response.Error == "")) {
                    //success
                    //var selector = "#AvailableCycles option[value='" + id + "']";
                    //var clone = $(selector).clone();
                    //$(selector).remove().prependTo('#CurrentPlanCycles');
                    //$(clone).appendTo("#cycle-nextCycleId");
                    //sortDropDownListByText("#cycle-nextCycleId");
                    var source = $("#cycle_" + id);
                    var sourceText = source.text();
                    //$("#cycle-initialCycleId").addOption(id, sourceText);
                } else {
                    ShowMessageDialog('Error adding Cycle', response.Error);
                }
            }
        });
    }
    

    function removeCycle(id) {
        $.ajax({
            type: "POST",
            url: removeurl,
            dataType: "json",
            data: { cycleid: id },
            success: function(response) {
                //alert("success");
                if ((response.Error == null) || (response.Error == "")) {
                    //success
                    //$("#CurrentPlanCycles option[value='" + id + "']").remove().prependTo('#AvailableCycles');
                    //$("#cycle-nextCycleId option[value='" + id + "']").remove();
                    //sortDropDownListByText("#AvailableCycles");
                    //$("#cycle-initialCycleId").removeOption(id);
                } else {
                    //alert(response.Error);
                    //ShowMessageDialog('Cycle not Removed', response.Error);
                }
            }
        });
    }
    
    function getAvailableProjects(selected) {
        $.ajax({
            type: "POST",
            url: GetPlanAvailableProjects,
            data: "planId=" + parseInt(<%= Model.RtpStatus.TimePeriodId %>)
                + '&cycleId=' + parseInt(<%= Model.RtpSummary.Cycle.Id %>),
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                $('#availableProjects').clearSelect().addItems(response);
                $('#scenario-container').show();
            },
            error: function(response, textStatus, AjaxException) {
                alert("error");
            }
        });
    }
    
    function sortDropDownListByText(list) {
        // Loop for each select element on the page.
        $(list).each(function() {
            // Keep track of the selected option.
            var selectedValue = $(this).val();

            // Sort all the options by text. I could easily sort these by val.
            $(this).html($("option", $(this)).sort(function(a, b) {
                return a.text == b.text ? 0 : a.text < b.text ? -1 : 1
            }));

            // Select one option.
            $(this).val(selectedValue);
        });
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
                        location.reload();
                        //$('div#resultRecordDetail').html(response.message);
                        //$('div#resultRecordDetail').addClass('success');
                        //autoHide(2500);
                        window.onbeforeunload = null;
                        $("#plan-initial-cycle").hide();
                        $("#button-set-initial-cycle").hide();
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

   
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<ul id="myMenu" class="contextMenu">
    <li class="edit">
        <a href="#edit">Edit</a>
    </li>
    <li class="copy disabled separator">
        <a href="#add">Add</a>
    </li>
    <li class="delete disabled">
        <a href="#remove">Remove</a>
    </li>
</ul>
<div class="view-content-container">
<%--<h2 ><%=Html.ActionLink("RTP List", "Index",new {controller="RTP"}) %> / RTP <%=Model.RtpSummary.RtpYear%></h2>--%>
<div class="clear"></div>

<%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>

    <div id="StatusForm" class="tab-form-container">
        <div id="StatusForm-wrapper">
        <% using (Html.BeginForm("UpdateStatus", "RTP", FormMethod.Post, new { @id = "dataForm"})) %>
        <%{ %>
            <fieldset>
                <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>

                <h2>Program Start and End Dates</h2>
                <%=Html.Hidden("TimePeriodId", Model.RtpStatus.TimePeriodId)%>
                <%=Html.Hidden("ProgramId", Model.RtpStatus.ProgramId)%>
                <p>
                    <label>Plan:</label>
                    <%--<%=Html.DrcogTextBox("Plan", Model.RtpSummary.IsCurrent, Model.RtpSummary.RtpYear, new { @id = "Plan", @class = "required", title = "Please specify a tip year (i.e. 2008-2013)" })%>--%>
                    <span class="fakeinput"><%= Html.Encode(Model.RtpSummary.RtpYear) %></span>
                    <%= Html.Hidden("Plan", Model.RtpSummary.RtpYear, new { @id = "Plan" }) %>
                    <br />
                </p>
                <p>
                    <label>Base Year:</label>
                    <%= Html.DropDownList("BaseYearId",
                        Model.RtpSummary.IsEditable(),
                        new SelectList(Model.AvailableYears, "key", "value", Model.RtpStatus.BaseYearId), 
                        "-- Select --",
                        new { title = "Please select a financial year.", @id = "BaseYearId", @class = "cycle-required" })%>
                    <%--<%=Html.DrcogTextBox("BaseYear", Model.RtpSummary.IsCurrent, Model.RtpStatus.BaseYear, new { @id = "BaseYear", @class = "required", title = "Please specify a base year" })%>--%>
                    <br />
                </p>
                <%--<p>
                    <label for="CurrentStatus">Current:</label>
                    <%= Html.CheckBox("IsCurrent", Model.IsEditable(), Model.RtpStatus.IsCurrent, new { @id = "CurrentStatus" })%>
                </p>
                <p>
                    <label for="PendingStatus">Pending:</label>
                    <%= Html.CheckBox("IsPending", Model.IsEditable(), Model.RtpStatus.IsPending, new { @id = "PendingStatus" })%>
                </p>
                <p>
                    <label for="PreviousStatus">Previous:</label>
                    <%= Html.CheckBox("IsPrevious", Model.IsEditable(), Model.RtpStatus.IsPrevious, new { @id = "PreviousStatus" })%>
                </p>
                --%>
                
                <h2>RTP Meeting and Approval Dates</h2>
                <p>
                <label for="summary_PublicHearing" class="beside" >Public Hearing:</label>
                <%= Html.DrcogTextBox("PublicHearing", Model.RtpSummary.IsEditable(), Model.RtpStatus.PublicHearing.HasValue ? Model.RtpStatus.PublicHearing.Value.ToShortDateString() : "", new { @class = "shortInputElement isdatepicker" })%>
                </p>
                <p>
                <label for="summary_BoardApproval">Board Adoption:</label>
                <%= Html.DrcogTextBox("Adoption", Model.RtpSummary.IsEditable(), Model.RtpStatus.Adoption.HasValue ? Model.RtpStatus.Adoption.Value.ToShortDateString() : "", new { @class = "isdatepicker" })%>
                </p>
                <p>
                <label for="summary_LastAmended">Last Amended:</label>
                <span class="fakeinput"><%= Model.RtpStatus.LastAmended.HasValue ? Html.Encode(Model.RtpStatus.LastAmended.Value.ToShortDateString()) : String.Empty %></span>
                <%--<%= Html.DrcogTextBox("LastAmended", Model.IsEditable(), Model.RtpStatus.LastAmended.HasValue ? Model.RtpStatus.LastAmended.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>--%>
                </p>
                <p>
                <label for="CDOTAction">CDOT Action:</label>
                <%= Html.DrcogTextBox("CDOTAction", Model.RtpSummary.IsEditable(), Model.RtpStatus.CDOTAction.HasValue ? Model.RtpStatus.CDOTAction.Value.ToShortDateString() : "", new { @class = "shortInputElement isdatepicker" })%>            
                </p>
                <p>
                <p>
                <label for="summary_USDOTApproval">U.S. DOT Approval:</label>
                <%= Html.DrcogTextBox("USDOTApproval", Model.RtpSummary.IsEditable(), Model.RtpStatus.USDOTApproval.HasValue ? Model.RtpStatus.USDOTApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement isdatepicker" })%>            
                </p>
                <p>
                <label for="summary_Notes">Notes:</label>
                <%= Html.TextArea2("Notes", Model.RtpSummary.IsEditable(), Model.RtpStatus.Notes,5,0, new { @name = "Notes", @class = "mediumInputElement growable" })%>
                </p>
                <p>
                <label for="summary_Description">Description:</label>
                <%= Html.TextArea2("Description", Model.RtpSummary.IsEditable(), Model.RtpStatus.Description, 5, 0, new { @name = "Description", @class = "mediumInputElement growable" })%>
                </p>
                
            </fieldset>
            <br />
            
            <%if (Model.RtpSummary.IsEditable())
              { %>        
                <p>
                <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
                <div id="result"></div>
                </p>    
           <%} %>       
        <%} %>
        </div>
        <div id="plan-cycles">
            <div id="plan-before-cycle" style="display: none;" class="box">
                <div id="plan-initial-cycle" style="display: none;">
                    <% if (!Model.RtpStatus.BaseYearId.Equals(0))
                       { %>
                        <%--<h3>Please pick the first cycle for this plan</h3><br />--%>
                        <%--<label for="InitialCycleId" class="boldFont">Initial Cycle:</label>
                        <%= Html.DropDownList("InitialCycleId",
                            new SelectList(Model.PlanUnusedCycles, "key", "value"),
                            "-- Select --",
                            new { title = "Please select a Cycle.", @id = "cycle-initialCycleId", @class = "nobind" })%>
                        <span id="button-set-initial-cycle_old" class="fg-button w100 ui-state-default ui-corner-all ui-state-disabled">Set</span>--%>
                    <% }
                       else
                       { %>
                        <h2>Before you can continue...</h2>
                        <h3>Please select a Financial Year then Save.</h3>
                    <% } %>
                </div>
            </div>
            
            <div class="box" style="position: relative;">
            <h2>Plan Cycle Administration</h2>
            
            <p>Note: Changes are stored to the database as they are made in the interface.</p>
                <div id="availableCycles">
                    Available Cycles:
                    <ul id="sortable1" class="connectedSortable">
                    <%foreach (KeyValuePair<int,string> item in Model.AvailableCycles)
                    { %>
	                    <li id="cycle_<%= item.Key %>" class="ui-state-default"><%= item.Value %></li>
                    <% } %>
                    </ul>
                    <div>
                        <span id="button-create-cycle" class="fg-button w380 ui-state-default ui-corner-all">Create new Cycle</span>
                        <span id="button-create-scenario" style="display: none;" class="fg-button ui-state-default ui-corner-all">Create new Scenario</span>
                        
                            <span id="button-plan-unlock" class="fg-button ui-state-default ui-corner-all">
                                <%= Model.RtpSummary.NextStatusText %>
                            </span>
                            <% if (Model.RtpSummary.TimePeriodStatusId.Equals((int)DRCOG.Domain.Enums.RtpTimePeriodStatus.CurrentLocked)){ %>
                            <span id="button-plan-close" class="fg-button ui-state-default ui-corner-all">
                                Set Inactive
                            </span>
                            <% } %>
                    </div>
                    
                    <%--alert($("#sortable2 li").first().attr("id"));--%>
                </div>
                <div id="currentCycles">
                    Current Cycles:
                    <ul id="sortable2" class="connectedSortable">
                    <%foreach (KeyValuePair<int,string> item in Model.CurrentPlanCycles)
                    { %>
	                    <li id="cycle_<%= item.Key %>" class="ui-state-highlight <%= Model.IsUnusedCycle(item.Key) ? "" : "ui-state-disabled"%>"><%= item.Value %></li>
                    
                    <% } %>
                    </ul>
                    <span id="button-set-initial-cycle" style="display: none;" class="fg-button w75 ui-state-default ui-corner-all">Set Initial</span>
                </div>
                <br clear="both" /> 
            </div>
            
            
            <div class="box" style="position: relative;">
                <h2>Plan Survey Administration</h2>
                <table id="planSurveys">
                    <thead>
                        <tr>
                            <th>Survey</th>
                            <th>Status</th>
                            <th></th>
                        </tr>
                    </thead>
                    <%foreach (DRCOG.Domain.Models.Survey.Survey item in Model.Surveys) { %>
                        <tr id="survey_<%= item.Id %>">
                            <td><%= Html.ActionLink(item.Name, "Dashboard", new {controller="Survey", year = item.Name }) %></td>
                            <td id="survey_status_<%= item.Id %>">
                                <%= item.GetStatusText() %>
                            </td>
                            <td><span id="btn_survey_<%= item.Id %>" style="display: block;" class="action_<%= item.ButtonActionCode %> fg-button w100 ui-state-default ui-corner-all"><%= item.ButtonActionText %></span></td>
                        </tr>
                    <% } %>
                    <tr>
                        <td colspan="3">New Survey Name</td>
                    </tr>
                    <tr id="new_survey">
                        <td colspan="2"><input type="text" id="surveyname" name="surveyname" maxlength="50" size="30" /></td>
                        <td><span id="btn_createsurvey" style="display: block;" class="fg-button w100 ui-state-default ui-corner-all">Create New</span></td>
                    </tr>
                </table>
                <div id="survey-result"></div>
            </div>
            
            
            <% if (!Model.RtpStatus.BaseYearId.Equals(0))
               { %>
                <%--<table>
                    <tr>
                        <%if (!Model.RtpSummary.IsCurrent)
                          { %>
                            <td>Available Cycles:</td>
                            <td>&nbsp;</td>
                        <%} %>
                        <td>Current Cycles:</td>
                    </tr>
                    <tr>
                        <%if (!Model.RtpSummary.IsCurrent)
                          { %>
                        <td>
                            <%= Html.ListBox("AvailableCycles", new MultiSelectList(Model.GetAvailableCyclesSelectList().Items, "Key", "Value"), new { @class = "w150 nobind", size = 10 })%><br/>        
                        </td>
                        <td>
                            <a href="#" id="add"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                            <a href="#" id="remove"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                        </td>
                        <%} %>
                        <td>
                          <%= Html.ListBox("CurrentPlanCycles", new MultiSelectList(Model.GetCurrentPlanCyclesSelectList().Items, "Key", "Value"), new { @class = "w150 nobind", size = 10 })%><br/>
                        </td>
                    </tr>
                </table>--%>
                
                
                <div id="cycle-modifier" style="display: none;">
                    
                </div>
                
                
                
                <div id="plan-current-cycle" class="box" style="display: none;">
                    <div id="process-cycle-step1">
                        <h2>Process Cycle</h2>
                        <% if (!String.IsNullOrEmpty(Model.RtpSummary.Cycle.Name))
                               
                           { %>
                            <p>Current: <%= Model.RtpSummary.Cycle.Name%></p>
                            <div id="plan-next-cycle">
                                <label for="NextCycleId" class="boldFont">Next Cycle:</label>
                                <%= Html.DropDownList("NextCycleId",
                                    new SelectList(Model.PlanUnusedCycles, "key", "value"),
                                //new SelectList(Model.CurrentPlanCycles.Where(pair => pair.Value != Model.RtpSummary.Cycle.Name).ToDictionary(pair => pair.Key, pair => pair.Value), "key", "value"), 
                                    "-- Select --",
                                    new { title = "Please select a Cycle.", @id = "cycle-nextCycleId", @class = "nobind" })%>
                                    <br />
                                <span id="plan-change-cycle" class="fg-button w100 ui-state-default ui-corner-all ui-state-disabled">Change Cycles</span>
                            </div>
                        <% }
                           else
                           { %>
                            
                        <% } %>
                    </div>
                    
                    <div id="process-cycle-step2" class="dialog" title="Move Projects to new Cycle">
                        <% Html.RenderPartial("~/Views/RTP/Partials/AmendableProjects.ascx"); %>
                    </div>
                </div>
            <% } %>
        </div>
        <div class="clear"></div>
    </div>

</div>

<div class="clear"></div>

<div style='display:none'>
    <div id="dialog-create-cycle" class="dialog" title="Create a new Cycle">
        <% Html.RenderPartial("~/Views/RTP/Partials/AddCycle.ascx"); %>
    </div>
</div>

<div style='display:none'>
    <div id="dialog-create-scenario" class="dialog" title="Create a new Scenario">
        
    </div>
    
    <div id="dialog-set-survey-date" class="dialog">
        <h2>Survey Management: Set Open Period</h2>
        <%= Html.Hidden("dialog_SurveyId") %>
        <fieldset>
            <p>
                <label for="dialog_SurveyOpen" class="big">Open Date:</label>
                <%= Html.DrcogTextBox("dialog_SurveyOpen", true, DateTime.Now.ToShortDateString(), new { @class = "datepicker big nobind" })%>
            </p>
            <p>
                <label for="dialog_SurveyClose" class="big">Close Date:</label>
                <%= Html.DrcogTextBox("dialog_SurveyClose", true, DateTime.Now.ToShortDateString(), new { @class = "datepicker big nobind" })%>
            </p>
        </fieldset>
    </div>
</div>

<div style='display:none'>
    <div id="dialog-cycle-projects" class="dialog" title="Move Projects to new Cycle">
        <% Html.RenderPartial("~/Views/RTP/Partials/AmendableProjects.ascx"); %>
    </div>
</div>

<% Html.RenderPartial("~/Views/Survey/Partials/AmendPartial.ascx"); %>






</asp:Content>



