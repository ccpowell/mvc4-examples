<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.ScopeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project General Information</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.ProjectSummary.RtpYear%></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript" ></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
    
    <script type="text/javascript">
        var AddSegmentUrl = '<%=Url.Action("AddSegment") %>';
        var DropSegmentUrl = '<%=Url.Action("DeleteSegment")%>';
        var EditSegmentUrl = '<%=Url.Action("UpdateSegment")%>';
        var EditSegmentSummaryUrl = '<%=Url.Action("UpdateSegmentSummary")%>';
        var EditLRSUrl = '<%=Url.Action("UpdateLRSRecord")%>';
        var AddLRSUrl = '<%=Url.Action("AddLRSRecord")%>';
        var DeleteLRSUrl = '<%=Url.Action("DeleteLRSRecord")%>';
        
        
        var GetSegmentDetailsUrl = '<%=Url.Action("GetSegmentDetails")%>';
        var GetSegmentLRSUrl = '<%=Url.Action("GetSegmentLRS")%>';
        var GetSegmentLRSDetailsUrl = '<%=Url.Action("GetSegmentLRSDetails")%>';
        var isEditable = '<%= Model.ProjectSummary.IsEditable() %>';
        
        var cboxClose = $.fn.colorbox.close;
        $.fn.colorbox.close = function(){ 
            if ($("#cboxClose").hasClass("dirty")) {
                if (confirm("Are you sure you want to exit?\nAny unsubmitted changes will be discarded.")) {
                    cboxClose();
                }
            } else { cboxClose(); }
        } 
        
        
        
        //Wireup the change handlers to enable the save button...
        $().ready(function() {
            
            var bindDetailsColorbox = function() {
                var lrs;
                var schemeBase;
            
            
                $(".confirmAmend").colorbox({
                    width: "830px",
                    height: "620px",
                    inline: true,
                    href: "#segmentDetails",
                    onLoad: function() {
                        $('#process-segment').removeClass('add-segment-details').addClass('update-segment-details');
                        $('#process-segment').text("Update");

                        <% if(!Model.ProjectSummary.IsEditable()) { %>
                            $('#process-segment').hide();
                        <% } %>

                        var segmentid = $(this).attr('id').replace('edit_', '');

                        $("#lrs-add, #lrs-update").hide();

                        function item(obj, value) {
                            //this.isText = isText;
                            this.Value = value;
                            this.element = obj;

                            this.isText = isText(obj);
                        }

                        function isText(obj) {
                            return obj.is(":text");
                        }

                        $.ajax({
                            type: "POST",
                            url: GetSegmentDetailsUrl,
                            data: "segmentId=" + segmentid,
                            dataType: "json",
                            success: function(response) {
                                var segment_details = {
                                    SegmentId: segmentid
                                    , FacilityName: new item($('#segment_details_FacilityName'), response.FacilityName)
                                    , StartAt: new item($('#segment_details_StartAt'), response.StartAt)
                                    , EndAt: new item($('#segment_details_EndAt'), response.EndAt)
                                    , OpenYear: new item($('#segment_details_OpenYear'), response.OpenYear)
                                    , NetworkId: new item($('#segment_details_NetworkId'), response.NetworkId)
                                    , ImprovementTypeId: new item($('#segment_details_ImprovementTypeId'), response.ImprovementTypeId)
                                    , PlanFacilityTypeId: new item($('#segment_details_PlanFacilityTypeId'), response.PlanFacilityTypeId)
                                    , ModelingFacilityTypeId: new item($('#segment_details_ModelingFacilityTypeId'), response.ModelingFacilityTypeId)
                                    , LanesBase: new item($('#segment_details_LanesBase'), response.LanesBase)
                                    , LanesFuture: new item($('#segment_details_LanesFuture'), response.LanesFuture)
                                    , SpacesFuture: new item($('#segment_details_SpacesFuture'), response.SpacesFuture)
                                    , AssignmentStatusId: new item($('#segment_details_AssignmentStatusId'), response.AssignmentStatusId)
                                };

                                $('#segment_details_SegmentId').val(segment_details.SegmentId);
                                segment_details.FacilityName.isText ? segment_details.FacilityName.element.val(segment_details.FacilityName.Value) : segment_details.FacilityName.element.text(segment_details.FacilityName.Value);
                                segment_details.StartAt.isText ? segment_details.StartAt.element.val(segment_details.StartAt.Value) : segment_details.StartAt.element.text(segment_details.StartAt.Value);
                                segment_details.EndAt.isText ? segment_details.EndAt.element.val(segment_details.EndAt.Value) : segment_details.EndAt.element.text(segment_details.EndAt.Value);
                                segment_details.OpenYear.isText ? segment_details.OpenYear.element.val(segment_details.OpenYear.Value) : segment_details.OpenYear.element.text(segment_details.OpenYear.Value);
                                segment_details.NetworkId.element.val(segment_details.NetworkId.Value);
                                segment_details.ImprovementTypeId.element.val(segment_details.ImprovementTypeId.Value);
                                segment_details.PlanFacilityTypeId.element.val(segment_details.PlanFacilityTypeId.Value);
                                segment_details.ModelingFacilityTypeId.element.val(segment_details.ModelingFacilityTypeId.Value);
                                segment_details.LanesBase.isText ? segment_details.LanesBase.element.val(segment_details.LanesBase.Value) : segment_details.LanesBase.element.text(segment_details.LanesBase.Value);
                                segment_details.LanesFuture.isText ? segment_details.LanesFuture.element.val(segment_details.LanesFuture.Value) : segment_details.LanesFuture.element.text(segment_details.LanesFuture.Value);
                                segment_details.SpacesFuture.isText ? segment_details.SpacesFuture.element.val(segment_details.SpacesFuture.Value) : segment_details.SpacesFuture.element.text(segment_details.SpacesFuture.Value);
                                segment_details.AssignmentStatusId.isText ? segment_details.AssignmentStatusId.element.val(segment_details.AssignmentStatusId.Value) : segment_details.AssignmentStatusId.element.text(segment_details.AssignmentStatusId.Value);
                                

                                lrs = response._LRS;
                                schemeBase = response.LRSSchemeBase;
                                
                                $.LoadSegmentLrsSummary(lrs);
                                
                                
                            }
                        });
                        
                        $("#button-hide-lrs").bind("click", function() {
                            var lrscontainer = $("#lrsdetails-container");
                            var amendmentform = $("#AmendmentForm");

                            //var lrsid = $(this).attr("id").replace("lrs-", "");

                            $(amendmentform).show();
                            $(lrscontainer).hide();
                            
                            $('#lrsdetails').html("");
                            
                            //$("#process-lrs").removeClass().addClass("cboxBtn").addClass("showAdd").text("Add LRS Record");
                            $("#lrs-showadd").show();
                            $("#lrs-add").hide();
                            $("#lrs-update").hide();
                            $("#button-hide-lrs").hide();
                            $("#process-segment").show();


                            $.LoadSegmentLrsSummary(null, segmentid);

                            $('input[id^=segment_details_]').unbind();
                            $('select[id^=segment_details_]').unbind();

                            $('input[id^=segment_details_]').bind('keyup', function() {
                                EnableSegmentDetailsUpdate();
                                setConfirmUnload(true, false);
                            });
                            $('select[id^=segment_details_]').bind('change', function() {
                                EnableSegmentDetailsUpdate();
                                setConfirmUnload(true, false);
                            });

                            $.fn.colorbox.resize();
                        });
                        
                        $("#lrs-showadd").bind("click", function() {
                            showAdd();
                            $.fn.colorbox.resize();
                        });

                        function showAdd() {
                            var lrscontainer = $("#lrsdetails-container");
                            var amendmentform = $("#AmendmentForm");

                            //var lrsid = $(this).attr("id").replace("lrs-", "");

                            $(amendmentform).hide();
                            $(lrscontainer).show();
                            
                            //$("#button-add-lrs").hide();
                            $("#button-hide-lrs").show();
                            $("#process-segment").hide();
                            
                            $("#lrs-add").show();
                            $("#lrs-showadd").hide();

                            //alert(JSON.stringify(schemeBase));
                            $.each(schemeBase, function(key, value) {
                                var displayType = value.DisplayType;
                                if (displayType != 'none') {
                                    var columnName = value.ColumnName;
                                    var friendlyName = value.FriendlyName;
                                    var value = value.ColumnDefault;
                                    //var value = !$.isset(columns[value.ColumnName]) ? columns[value.ColumnName] : "";

                                    var content = '<p>';
                                    content += '<label for="' + columnName + '">' + friendlyName + '</label>';
                                    if (isEditable != 'False') {
                                        if (displayType != 'readonly') {
                                            content += '<input name="segment_details_lrs_' + columnName + '" id="segment_details_lrs_' + columnName + '" style="width: 200px;" type="text" maxLength="55" value="' + value + '" />';
                                        }
                                    }
                                    else {
                                        content += '<span name="segment_details_lrs_' + columnName + '" id="segment_details_lrs_' + columnName + '" class="fakeinput" style="width: 200px;" type="text">' + value + '</span>';
                                        $('#process-segment').hide();
                                    }
                                    content += '</p>';
                                    $('#lrsdetails').append(content);
                                }
                            });
                        }

                        $('#lrs-add').bind("click", function() {
                            var lrsData = new Array();

                            $.each($('input[id^=segment_details_lrs_]'), function() {
                                var key = $(this).attr("id").replace("segment_details_lrs_", "");
                                var value = $(this).val();
                                if (value != "") { lrsData.push(key + "_" + value); }
                            });

                            //$.xmlBusinessRules(lrsData);

                            $.ajax({
                                type: "POST",
                                url: AddLRSUrl,
                                data: 'LRSRecordRaw=' + lrsData
                                    + '&SegmentId=' + segmentid,
                                dataType: "json",
                                success: function(response) {
                                    $('#process-segment-result').html(response.message).addClass('success').autoHide();
                                    //$("#lrsdetails").html("");
                                    window.onbeforeunload = null;
                                        
                                    $("#button-hide-lrs").trigger("click");
                                    $.fn.colorbox.resize();
                                }
                            });

                            return false;
                        });

                        $("a.lrs-edit").live("click", function() {
                            var lrscontainer = $("#lrsdetails-container");
                            var amendmentform = $("#AmendmentForm");

                            var lrsid = $(this).attr("id").replace("lrs-", "");
                            

                            $(amendmentform).hide();
                            $(lrscontainer).show();
                            
                            //$("#process-lrs").addClass("update-lrs").addClass("disabled").text("Update");
                            $("#lrs-update").show();
                            $("#lrs-showadd").hide();
                            $("#button-hide-lrs").show();
                            
                            $.ajax({
                                type: "POST",
                                url: GetSegmentLRSDetailsUrl,
                                data: "LRSId=" + lrsid,
                                dataType: "json",
                                success: function(response) {
                                    var segment_details = {
                                        SegmentId: segmentid
                                    , FacilityName: new item($('#segment_details_FacilityName'), response.FacilityName)
                                    , StartAt: new item($('#segment_details_StartAt'), response.StartAt)
                                    , EndAt: new item($('#segment_details_EndAt'), response.EndAt)
                                    , OpenYear: new item($('#segment_details_OpenYear'), response.OpenYear)
                                    , NetworkId: new item($('#segment_details_NetworkId'), response.NetworkId)
                                    , ImprovementTypeId: new item($('#segment_details_ImprovementTypeId'), response.ImprovementTypeId)
                                    , PlanFacilityTypeId: new item($('#segment_details_PlanFacilityTypeId'), response.PlanFacilityTypeId)
                                    , ModelingFacilityTypeId: new item($('#segment_details_ModelingFacilityTypeId'), response.ModelingFacilityTypeId)
                                    , LanesBase: new item($('#segment_details_LanesBase'), response.LanesBase)
                                    , LanesFuture: new item($('#segment_details_LanesFuture'), response.LanesFuture)
                                    , SpacesFuture: new item($('#segment_details_SpacesFuture'), response.SpacesFuture)
                                    , AssignmentStatusId: new item($('#segment_details_AssignmentStatusId'), response.AssignmentStatusId)
                                    };

                                    var lrs = response.LRSRecord;
                                    var scheme = lrs.Scheme;
                                    var columns = lrs.Columns

                                    // load the scheme and fill with data
                                    $.each(scheme, function(key, value) {
                                        var displayType = value.DisplayType;
                                        if (displayType != 'none') {
                                            var columnName = value.ColumnName;
                                            var friendlyName = value.FriendlyName;
                                            var value = !$.isset(columns[value.ColumnName]) ? columns[value.ColumnName] : "";

                                            var content = '<p>';
                                            content += '<label for="' + columnName + '">' + friendlyName + '</label>';
                                            if (isEditable != 'False') {
                                                if (displayType != 'readonly') {
                                                    content += '<input name="segment_details_lrs_' + columnName + '" id="segment_details_lrs_' + columnName + '" style="width: 200px;" type="text" maxLength="55" value="' + value + '" />';
                                                }
                                            }
                                            else {
                                                content += '<span name="segment_details_lrs_' + columnName + '" id="segment_details_lrs_' + columnName + '" class="fakeinput" style="width: 200px;" type="text">' + value + '</span>';
                                                $('#process-segment').hide();
                                            }
                                            content += '</p>';
                                            $('#lrsdetails').append(content);
                                        }
                                    });
                                    
                                    // make the link active
                                    $('#lrs-update').unbind();
                                    $('#lrs-update').bind("click", function() {
                                        var lrsData = new Array();

                                        $.each($('input[id^=segment_details_lrs_]'), function() {
                                            var key = $(this).attr("id").replace("segment_details_lrs_", "");
                                            var value = $(this).val();
                                            if (value != "") { lrsData.push(key + "_" + value); }
                                        });

                                        $.xmlBusinessRules(lrsData);
                                        
                                        $.ajax({
                                            type: "POST",
                                            url: EditLRSUrl,
                                            data: 'LRSRecord.Id=' + lrsid
                                                + '&LRSRecordRaw=' + lrsData
                                                + '&SegmentId=' + segmentid,
                                            dataType: "json",
                                            success: function(response) {
                                                $('#process-segment-result').html(response.message).addClass('success').autoHide();

                                                //Disable the add button
                                                $('#process-segment').addClass('ui-state-disabled').attr('disabled', 'disabled');

                                                window.onbeforeunload = null;
                                                
                                                $("#button-hide-lrs").trigger("click");
                                            }
                                        });

                                        return false;
                                    });

                                    $.fn.colorbox.resize();
                                }
                            });

                            $('#process-segment').hide();
                        });
                        
                        $("a.lrs-delete").live("click", function() {
                            if(confirmDelete()) {
                                var lrsid = $(this).attr("id").replace("lrs-delete-", "");
                                $.ajax({
                                    type: "POST",
                                    url: DeleteLRSUrl,
                                    data: 'lrsid=' + lrsid,
                                    dataType: "json",
                                    success: function(response) {
                                        $('#process-segment-result').html(response.message).addClass('success').autoHide();
                                        $.LoadSegmentLrsSummary(null, segmentid);
                                        
                                        $.fn.colorbox.resize();
                                    }
                                });
                            }
                            
                            return false;
                        });

                        function EnableSegmentLRSDetailsUpdate() {
                            if ($('#process-lrs').hasClass('disabled')) {
                                $('#process-lrs').removeClass('disabled');//.removeAttr('disabled');
                            }
                        }

                        $('input[id^=segment_details_lrs_]').live('keyup', function() {
                            EnableSegmentLRSDetailsUpdate();
                            setConfirmUnload(true, false);
                        });

                        $('input[id^=segment_details_]').live('keyup', function() {
                            EnableSegmentDetailsUpdate();
                            setConfirmUnload(true, false);
                        });
                        $('select[id^=segment_details_]').live('change', function() {
                            EnableSegmentDetailsUpdate();
                            setConfirmUnload(true, false);
                        });
                        
                        
                    },
                    onOpen: function() {
                        $("#cboxClose").addClass("dirty1");
                    },
                    onComplete: function() {
                        $.fn.colorbox.resize();
                    },
                    onClosed: function() {
                        
                    },
                    onCleanup: function() {
                        unloadSegmentDetails();
                        //alert("Clean");
                    }
                });
                
                
            
            }

            var $save = $('<span id="process-segment" class="update-segment-details cboxBtn disabled">Add</span>').appendTo('#cboxContent');
            <%if (Model.ProjectSummary.IsEditable()) { %>
                //var $buttonAddLrs = $('<span id="button-add-lrs" class="cboxBtn">Add LRS Record</span>').appendTo('#cboxContent');
                //var $thing = $('<span id="process-lrs" class="update-lrs-details cboxBtn">Update and Close</span>').appendTo('#cboxContent');
                var $processLrs_showadd = $('<span id="lrs-showadd" class="cboxBtn">Add LRS Record</span>').appendTo('#cboxContent');
                var $processLrs_addlrs = $('<span id="lrs-add" class="cboxBtn">Save</span>').appendTo('#cboxContent');
                var $processLrs_updatelrs = $('<span id="lrs-update" class="cboxBtn">Update</span>').appendTo('#cboxContent');
            <% } %>
            var $buttonHideLrsContainer = $('<span id="button-hide-lrs" class="cboxBtn">Return to Segment Details</span>').appendTo('#cboxContent');
            
            
            bindDetailsColorbox();
            
            

            $("#new-segmentdetails").colorbox(
            {
                width: "830px",
                height: "550px",
                inline: true,
                href: "#segmentDetails",
                onLoad: function() {
                    $('#process-segment').removeClass('update-segment-details').addClass('add-segment-details');
                    $('#process-segment').text("Add");
                },
                onCleanup: function() { unloadSegmentDetails(); }
            });
            var isEdit = false;



            $('input').change(function() {
                isEdit = true;
            });

            function checkIsEdit() {
                if (isEdit)
                    return "You have modify some content";
            }

            window.onbeforeunload = checkIsEdit;

            $('.delete-segment').live("click", function() {
                //get the segmentid : segment_row_5022
                var segmentid = this.id.replace('delete_', '');
                var row = $('#segment_row_' + segmentid);
                if (confirmDelete()) {
                    $.ajax({
                        type: "POST",
                        url: DropSegmentUrl,
                        data: "segmentId=" + segmentid,
                        dataType: "json",
                        success: function(response) {
                            $('#result').html(response.message);
                            //var div = $('#pool_div_' + poolid);
                            $(row).empty();
                            $('div#result').addClass('success').autoHide();
                        }
                    });
                }
                return false;
            });

            //Update a segment in the list
            $('.update-segment-summary').live("click", function() {
                
                var segmentid = this.id.replace('delete_', '');
                //grab the values from the active form
                var facilityname = $('#segment_' + segmentid + '_FacilityName').val();
                var startat = $('#segment_' + segmentid + '_StartAt').val();
                var endat = $('#segment_' + segmentid + '_EndAt').val();
                var networkid = $('#segment_' + segmentid + '_NetworkId').val();
                var improvementtypeid = $('#segment_' + segmentid + '_ImprovementTypeId').val() || 0;
                var planfacilitytypeid = $('#segment_' + segmentid + '_PlanFacilityTypeId').val() || 0;
                var modelfacilitytypeid = $('#segment_' + segmentid + '_ModelingFacilityTypeId').val() || 0;
                var openyear = $('#segment_' + segmentid + '_OpenYear').val();
                var lanesbase = $('#segment_' + segmentid + '_LanesBase').val();
                var lanesfuture = $('#segment_' + segmentid + '_LanesFuture').val();
                var spacesfuture = $('#segment_' + segmentid + '_SpacesFuture').val();
                var assignmentstatusid = 0; //$('#segment_' + segmentid + '_AssignmentStatusId').val();
                //console.log(openyear);
//                alert('Add: '
//                + 'segmentId=' + segmentid
//                + '&facilityName=' + facilityname
//                + '&startAt=' + startat
//                + '&networkId=' + networkid
//                + '&openyear=' + openyear
//                );
                $.ajax({
                    type: "POST",
                    url: EditSegmentSummaryUrl,
                    data: 'segmentId=' + segmentid
                        + '&facilityName=' + facilityname
                        + '&startAt=' + startat
                        + '&endAt=' + endat
                        + '&networkId=' + networkid
                        + '&openyear=' + openyear,
                    dataType: "json",
                    success: function(response) {
                        $('#result').html(response.message);

                        //Disable the add button
                        $('#delete_' + segmentid).html("Delete").removeClass('update-segment-summary').addClass('delete-segment');
                        //$('.update-segment').html("Delete").addClass('delete-segment').removeClass('update-segment');
                        $('div#result').addClass('success').autoHide();
                        window.onbeforeunload = null; return true;
                    }
                });
                return false;
            });

            //Update a segment in the list
            $('.update-segment-details').live("click", function() {
                //alert("here");
                if (!$('#process-segment').hasClass('disabled')) {
                    var segmentid = $('#segment_details_SegmentId').val();
                    //grab the values from the active form
                    var facilityname = $('#segment_details_FacilityName').val();
                    var startat = $('#segment_details_StartAt').val();
                    var endat = $('#segment_details_EndAt').val();
                    var networkid = $('#segment_details_NetworkId').val();
                    var improvementtypeid = $('#segment_details_ImprovementTypeId').val() || 0;
                    var planfacilitytypeid = $('#segment_details_PlanFacilityTypeId').val() || 0;
                    var modelingfacilitytypeid = $('#segment_details_ModelingFacilityTypeId').val() || 0;
                    var openyear = $('#segment_details_OpenYear').val();
                    var lanesbase = $('#segment_details_LanesBase').val();
                    var lanesfuture = $('#segment_details_LanesFuture').val();
                    var spacesfuture = $('#segment_details_SpacesFuture').val();
                    var assignmentstatusid = 0;

                    var lrsData = new Array();

                    $.each($('input[id^=segment_details_lrs_]'), function() {
                        var key = $(this).attr("id").replace("segment_details_lrs_", "");
                        var value = $(this).val();
                        if (value != "") { lrsData.push(key + "_" + value); }
                    });

                    $.xmlBusinessRules(lrsData);

                    $.ajax({
                        type: "POST",
                        url: EditSegmentUrl,
                        data: 'segmentId=' + segmentid
                            + '&facilityName=' + facilityname
                            + '&startAt=' + startat
                            + '&endAt=' + endat
                            + '&networkId=' + networkid
                            + '&improvementTypeId=' + improvementtypeid
                            + '&planFacilityTypeId=' + planfacilitytypeid
                            + '&modelingFacilityTypeId=' + modelingfacilitytypeid
                            + '&openyear=' + openyear
                            + '&lanesbase=' + lanesbase
                            + '&lanesfuture=' + lanesfuture
                            + '&spacesfuture=' + spacesfuture
                            + '&assignmentStatusId=' + assignmentstatusid
                            + '&LRSRecord=' + lrsData,
                        dataType: "json",
                        success: function(response) {
                            $('#process-segment-result').html(response.message).addClass('success').autoHide();

                            //Disable the add button
                            $('#process-segment').addClass('ui-state-disabled').addClass('disabled').attr('disabled', 'disabled');

                            // update master data
                            $('#segment_' + segmentid + '_FacilityName').val(facilityname);
                            $('#segment_' + segmentid + '_StartAt').val(startat);
                            $('#segment_' + segmentid + '_EndAt').val(endat);
                            $('#segment_' + segmentid + '_OpenYear').val(openyear);
                            $('#segment_' + segmentid + '_NetworkId').val(networkid);

                            // record should be updated so switch update back to delete
                            if ($('#delete_' + segmentid).hasClass('update-segment-summary')) {
                                $('#delete_' + segmentid).html("Delete").removeClass('update-segment-summary').addClass('delete-segment');
                            }

                            //window.onbeforeunload = null;
                            //window.onbeforeunload = null; return true;

                            setConfirmUnload(true, false);
                            $("#cboxClose").removeClass("dirty");

                            $.fn.colorbox.resize();
                        }
                    });
                }

                return false;
            });

            // Prevent accidental navigation away
            $('input[id^=RtpProjectScope_]', document.dataForm).bind("change", function() { setConfirmUnload(true, true); });
            $('input[id^=RtpProjectScope_]', document.dataForm).bind("keyup", function() { setConfirmUnload(true, true); });
            $('textarea[id^=RtpProjectScope_]', document.dataForm).bind("change", function() { setConfirmUnload(true, true); });
            $('textarea[id^=RtpProjectScope_]', document.dataForm).bind("keyup", function() { setConfirmUnload(true, true); });
            //disable the onbeforeunload message if we are using the submitform button
            if ($('#submitForm')) {
                $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
            }

            //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
            var v = jQuery("#dataForm").validate({
                submitHandler: function(form) {
                    jQuery(form).ajaxSubmit({
                        dataType: 'json',
                        success: function(data, textStatus) {
                            $('#result').html(data.message).addClass('success');
                            $('#submitForm').addClass('ui-state-disabled').autoHide();
                        },
                        error: function(XMLHttpRequest, textStatus, errorThrown) {
                            $('#result').text(data.message);
                            $('#result').addClass('error');
                        }
                    });
                }
            });

            $.extend({
                xmlBusinessRules: function(obj) {
                    if (Object.prototype.toString.call(obj) === '[object Array]') {
                        var v1, v2, improveType = "";

                        $.each(obj, function(index, value) {
                            var temp = value.split("_");
                            if (new RegExp('^(' + temp[0] + ')$').test("BEGINMEASU")) {
                                v1 = temp[1];
                            }
                            if (new RegExp('^(' + temp[0] + ')$').test("ENDMEASURE")) {
                                v2 = temp[1];
                            }
                        });

                        if (v1 === undefined && v2 === undefined) {
                        } else {
                            if (v1 === v2 || v1 === undefined || v2 === undefined) {
                                improveType = "Point";
                            } else improveType = "Line";

                            obj.push("Improvetype_" + improveType);
                        }
                        $("#segment_details_lrs_Improvetype").text(improveType);

                    }
                }
            });

            //Add a segment to the list
            $('.add-segment-details').live("click", function() {
                //grab the values from the active form

                var facilityname = $('#segment_details_FacilityName').val();
                var startat = $('#segment_details_StartAt').val();
                var endat = $('#segment_details_EndAt').val();
                var networkid = $('#segment_details_NetworkId').val();
                var improvementtypeid = $('#segment_details_ImprovementTypeId').val() || 0;
                var planfacilitytypeid = $('#segment_details_PlanFacilityTypeId').val() || 0;
                var modelingfacilitytypeid = $('#segment_details_ModelingFacilityTypeId').val() || 0;
                var openyear = $('#segment_details_OpenYear').val();
                var lanesbase = $('#segment_details_LanesBase').val();
                var lanesfuture = $('#segment_details_LanesFuture').val();
                var spacesfuture = $('#segment_details_SpacesFuture').val();
                var assignmentstatusid = $('#new_segmentassignmentstatusid').val() || 0;

                var routename = $('#segment_details_RouteName').val();
                var beginmeasure = $('#segment_details_BeginMeasure').val();
                var endmeasure = $('#segment_details_EndMeasure').val();
                var offset = $('#segment_details_Offset').val();
                var comments = $('#segment_details_Comments').val();

                var segmentid = 0;
                var projectversionid = $('#RtpProjectScope_ProjectVersionId').val();

                if (facilityname == '' || startat == '' || networkid == '') {
                    var msg = 'Please fill in:';
                    if (facilityname == '') msg = msg + ' Facility Name';
                    if (startat == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Start At';
                    if (networkid == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Network';
                    $('#segment-details-error').addClass('error').html(msg).show();
                    return false;
                }

                //Add to database via XHR
                //alert('Need XHR Big Test to Add: projectVersionId=' + projectversionid + '&facilityName=' + facilityname + '&startAt=' + startat + '&endAt=' + endat + '&networkId=' + networkid + '&improvementTypeId=' + improvementtypeid + '&planFacilityTypeId=' + planfacilitytypeid + '&assignmentStatusId=' + assignmentstatusid);

                $.ajax({
                    type: "POST",
                    url: AddSegmentUrl,
                    data: 'projectVersionId=' + projectversionid
                        + '&facilityName=' + facilityname
                        + '&startAt=' + startat
                        + '&endAt=' + endat
                        + '&networkId=' + networkid
                        + '&improvementTypeId=' + improvementtypeid
                        + '&planFacilityTypeId=' + planfacilitytypeid
                        + '&modelingFacilityTypeId=' + modelingfacilitytypeid
                        + '&openyear=' + openyear
                        + '&lanesbase=' + lanesbase
                        + '&lanesfuture=' + lanesfuture
                        + '&spacesfuture=' + spacesfuture
                        + '&assignmentStatusId=' + assignmentstatusid
                        + '&lrs.routeName=' + routename
                        + '&lrs.beginMeasure=' + beginmeasure
                        + '&lrs.endMeasure=' + endmeasure
                        + '&lrs.offset=' + offset
                        + '&lrs.comments=' + comments,
                    dataType: "json",
                    success: function(response) {
                        $('#result').html(response.message);
                        //Add into the DOM
                        segmentid = response.segmentId;

                        var segment = {
                            'r1_segmentid': segmentid,
                            'r1_facilityname': facilityname,
                            'r1_startat': startat,
                            'r1_endat': endat,
                            // for some reason these do not change here. Updated them after dom insert.
                            //'r1_networkid': networkid,
                            //'r1_improvementtypeid': improvementtypeid,
                            //'r1_facilitytypeid': facilitytypeid,
                            'r1_openyear': openyear
                        };


                        var content = '<tr id="segment_row_' + segmentid + '" class="new">';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_FacilityName", Model.ProjectSummary.IsEditable(), "r1_facilityname", new { style = "width:200px;", @maxlength = "75" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_StartAt", Model.ProjectSummary.IsEditable(), "r1_startat", new { style = "width:110px;", @maxlength = "50" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_EndAt", Model.ProjectSummary.IsEditable(), "r1_endat", new { style = "width:110px;", @maxlength = "50" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_OpenYear", Model.ProjectSummary.IsEditable(), "r1_openyear" , new { style = "width:75px;", @maxlength = "4" })%></td>';
                        content += '<td><%= Html.DropDownList("segment_r1_segmentid_NetworkId", Model.ProjectSummary.IsEditable(), new SelectList(Model.AvailableNetworks, "key", "value", "r1_networkid"), "-- Select --", new { @class = "not-required", title = "Please select a network" }, true)%></td>';
                        content += '<td>';
                        content += '<button class="confirmAmend fg-button ui-state-default ui-priority-primary ui-corner-all" id="edit_r1_segmentid">Details</button>';
                        content += '<%if(Model.ProjectSummary.IsEditable()){ %>';
                        content += '<button class="delete-segment fg-button ui-state-default ui-priority-primary ui-corner-all" id="delete_r1_segmentid">Delete</button>';
                        content += '<% } %>';
                        content = replaceFromArray(content, segment);
                        content += "</td>";
                        content += '</tr>';
                        $('#segment_last_record').before(content);

                        // set the values of the dropdown after dom insert
                        $("#segment_" + segmentid + "_NetworkId").val(networkid);

                        //Disable the add button
                        $('#add-segment-details').addClass('ui-state-disabled').attr('disabled', 'disabled');
                        $('div#result').addClass('success').autoHide();
                        parent.$.fn.colorbox.close();
                        window.onbeforeunload = null;

                        bindDetailsColorbox();
                    }
                });
                return false;
            });

        });

            function setConfirmUnload(on, enableFullPageSave) {
                if (enableFullPageSave) { $('#submitForm').removeClass('ui-state-disabled'); }
                $('#result').html("");
                window.onbeforeunload = (on) ? unloadMessage : null;
            }

            function unloadMessage() {
                return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
            }

            function unloadSegmentDetails() {
                $('#segment_details_FacilityName').val('');
                $('#segment_details_StartAt').val('');
                $('#segment_details_EndAt').val('');
                $('#segment_details_OpenYear').val('');
                $('#segment_details_NetworkId').val('');
                $('#segment_details_ImprovementTypeId').val('');
                $('#segment_details_PlanFacilityTypeId').val('');
                $('#segment_details_ModelingFacilityTypeId').val('');
                $('#segment_details_LanesBase').val('');
                $('#segment_details_LanesFuture').val('');
                $('#segment_details_SpacesFuture').val('');
                $('#segment_details_AssignmentStatusId').val('');
                $("#lrsdetails").html("");
                $("#lrslinks").html("");
                $("#cboxClose").removeClass("dirty");
                $('#process-segment').addClass('disabled');
            }

            //Hook in the keyup event so we can keep track of changes to the shares
            $('input[id^=segment_]').live('keyup', function() {
                var id = $(this).attr('id');
                if (id.indexOf('details') == -1) {
                    var segmentid = $(this).parents("tr").attr('id').replace('segment_row_', '');

                    ChangeSegmentToUpdate(segmentid);
                    setConfirmUnload(true, false);
                }

            });
            $('select[id^=segment_]').live('change', function() {
                var id = $(this).attr('id');
                if (id.indexOf('details') == -1) {
                    var segmentid = $(this).parents("tr").attr('id').replace('segment_row_', '');

                    ChangeSegmentToUpdate(segmentid);
                    setConfirmUnload(true, false);
                }

            });

            
            
            $('input[id^=new_segment]').live('keyup', function() { EnablePoolProjectAddButton(); });
            
            

        function replaceFromArray(string, object) {
            var value = string;
            var intIndexOfMatch;
            for (var index in object) {
                //alert(index + ':' + object[index]);
                intIndexOfMatch = value.indexOf(index);
                while (intIndexOfMatch != -1) {
                    value = value.replace(index, object[index]);
                    
                    intIndexOfMatch = value.indexOf(index);
                }
                //alert(value);
            }
            return value;
        }

        function ChangeSegmentToUpdate(id) {
            //$('.delete-pool').html("Update").removeClass('delete-pool').addClass('update-pool');
            if ($('delete_' + id).html() != 'Update') {
                $('#delete_' + id).html("Update").removeClass('delete-segment').addClass('update-segment-summary');
            }
        }
        function EnableSegmentDetailsUpdate() {
            if ($('#process-segment').hasClass('disabled')) {
                $('#process-segment').removeClass('disabled').removeClass('ui-state-disabled').removeAttr('disabled');
                $("#cboxClose").addClass("dirty");
            }
        }

        function confirmDelete() {
            if (confirm('Are you sure you are ready to delete this item?'))
                return true;
            else
                return false;
        }

        $.fn.GetDictionaryValue = function(array, key) {
            // Get the dictionary value from the array at the specified key
            //alert(key);
            var keyValue = key;
            var result;
            jQuery.each(array, function() {
                if (this.Key == keyValue) {
                    result = this.Value;
                    return false;
                }
            });
            return result;
        }
        $.extend({
            isset: function(x) {
                return ((x === undefined) || (x === null));
            },
            LoadSegmentLrsSummary: function(lrs, segmentid) {
                $("#lrslinks").html("");
                if($.isset(lrs)) {
                    //alert("need to get LRS: " + segmentid);
                    
                    $.ajax({
                        type: "POST",
                        url: GetSegmentLRSUrl,
                        data: 'segmentId=' + segmentid,
                        dataType: "json",
                        success: function(response) {
                            lrs = response._LRS;
                            
                            $.BuildSegmentLrsSummary(lrs);
                            //alert(JSON.stringify(lrs));
                            //$('#result').html(response.message);

                            //Disable the add button
                            //$('#delete_' + segmentid).html("Delete").removeClass('update-segment-summary').addClass('delete-segment');
                            //$('.update-segment').html("Delete").addClass('delete-segment').removeClass('update-segment');
                            //$('div#result').addClass('success').autoHide();
                            //window.onbeforeunload = null;
                        }
                    });
                } else { $.BuildSegmentLrsSummary(lrs); }
            },
            BuildSegmentLrsSummary: function(lrs) {
                $.each(lrs, function(key, value) {
                    var record = value;
                    var id = record.Id;
                    var content = "<p>"
                    //alert(JSON.stringify(value));
                    var columns = record.Columns;
                    var scheme = record.Scheme;
                    $.each(scheme, function(key, value) {
                        var displayType = value.DisplayType;
                        if (displayType != 'none') {
                            var data = columns[value.ColumnName];
                            if (typeof data != "undefined") {
                                content +=
                                (
                                    value.ColumnName == "Routename"
                                    ? <% if(Model.ProjectSummary.IsEditable()) { %>'<a href="#" id="lrs-' + id + '" class="lrs-edit">edit</a> | <a href="#" id="lrs-delete-' + id + '" class="lrs-delete">remove</a> ' + <% } %>value.FriendlyName + ": " + data
                                    :
                                    (
                                        value.ColumnName == "BEGINMEASU"
                                        ? " | Measures " + data
                                        : " - " + data
                                    )
                                );
                            }
                        }
                    });
                    content += "</p>";
                    $("#lrslinks").append(content);
                });
                
                $.fn.colorbox.resize();
            }
        });
        
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/RtpProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    <div class="tab-form-container tab-scope">
        <% using (Html.BeginForm("UpdateScope", "RtpProject", FormMethod.Post, new { @id = "dataForm" })) %>
        <% { %>
            <fieldset>
            
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("RtpProjectScope.RtpYear", Model.ProjectSummary.RtpYear)%>
            <%=Html.Hidden("RtpProjectScope.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
           
           <p><label>Short Description:</label>           
            <input id="RtpProjectScope_ShortDescription" class="longInputElement" 
                title="Please provide a short description for the project." 
                name="RtpProjectScope.ShortDescription" maxlength="256" 
                value="<%= Model.RtpProjectScope.ShortDescription%>" />
            </p>     
           <p><label>Project Scope:</label></p>           
           <p>
                <textarea id="RtpProjectScope_ProjectDescription" class="longInputElement required" 
                    title="Please provide a description for the project." 
                    name="RtpProjectScope.ProjectDescription" 
                    rows="10" cols="50"><%= Model.RtpProjectScope.ProjectDescription%></textarea>
           </p>
            <%if(Model.ProjectSummary.IsEditable()){ %>
                <div class="relative">
                <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
                <div id="result"></div>
                </div>
                <br />
            <%} %>

            

            </fieldset>
            
    <%} %>
    
    </div>
    <div class="clear"></div>
    <%--<% Html.RenderPartial("~/Views/Project/Partials/PoolProjectPartial.ascx", Model.PoolProjects); %>--%>
    <div style="position: relative;">
       <h2>Project Segments</h2>
       <%if(Model.ProjectSummary.IsEditable()){ %>
            <button id="new-segmentdetails" style="position: absolute; top: -5px; right: 50px;" class="fg-button ui-state-default ui-priority-primary ui-corner-all">Add New</button>
        <% } %>
       <table id="segments">
            <thead>
                <tr>
                    <th>Facility Name</th>
                    <th>Start At</th>
                    <th>End At</th>
                    <th>Open Year</th>
                    <th>Network</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
            <% 
            int rowCount = 0;
            foreach (var item in Model.Segments.ToList<DRCOG.Domain.Models.RTP.SegmentModel>()) 
            {
                rowCount++;
            %>
                <tr id="segment_row_<%=item.SegmentId.ToString() %>" <%= rowCount % 2 == 0 ? "class=\"even\"" : "class=\"odd\""%>>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_FacilityName", Model.ProjectSummary.IsEditable(), item.FacilityName.ToString(), new { style = "width:200px;", @maxlength = "75" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_StartAt", Model.ProjectSummary.IsEditable(), item.StartAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_EndAt", Model.ProjectSummary.IsEditable(), item.EndAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_OpenYear", Model.ProjectSummary.IsEditable(), item.OpenYear.ToString(), new { style = "width:75px;", @maxlength = "4" })%></td>
                    <td>
                        <%= Html.DropDownList("segment_" + item.SegmentId.ToString() + "_NetworkId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableNetworks, "key", "value", item.NetworkId),
                            "-- Select --",
                            new { @class = "not-required", title = "Please select a network" })%>
                    </td>
                    <td>
                        <button id="edit_<%=item.SegmentId.ToString() %>" class="confirmAmend fg-button ui-state-default ui-priority-primary ui-corner-all">Details</button>
                        <%if(Model.ProjectSummary.IsEditable()){ %>
                            <button class="delete-segment fg-button ui-state-default ui-priority-primary ui-corner-all" id='delete_<%=item.SegmentId.ToString() %>'>Delete</button>
                        <% } %>
                        
                    </td>
                </tr>
            <% } %>
                <tr style="display: none;" id="segment_last_record"><td colspan="5"></td></tr>
            
            </tbody>
            
        
    
       </table>
    </div>
       
    <%--<h2>Segments</h2>
    <% Html.RenderPartial("~/Views/Project/Partials/SegmentSummaryPartial.ascx", Model.Segments); %>--%>
   <div class="clear"></div>
   <%--<div id="tab-scope-singleprojects">
        <h2>Projects</h2>
    </div>--%>
</div>
<!-- This contains the hidden content for inline calls --> 
	<div style='display:none'> 
		<div id='segmentDetails' style='padding:10px; background:#fff;'>
		    <% Html.RenderPartial("~/Views/RtpProject/Partials/SegmentPartial.ascx"); %>
		</div>
	</div> 



</asp:Content>




