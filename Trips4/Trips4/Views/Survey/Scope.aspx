<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.ScopeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Scope</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server"><%= Model.Current.Name %> Survey Projects</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript" ></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskedinput-1.2.2.min.js")%>" type="text/javascript" ></script>
    
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.SurveyName = '<%= Model.Current.Name %>';
        App.pp.ProjectVersionId = <%= Model.Project.ProjectVersionId %>;
        $(document).ready(App.tabs.initializeSurveyProjectTabs);
    </script>
    <script type="text/javascript">
        var AddSegmentUrl = '<%=Url.Action("AddSegment") %>';
        var DropSegmentUrl = '<%=Url.Action("DeleteSegment")%>';
        var EditSegmentUrl = '<%=Url.Action("UpdateSegment")%>';
        var EditSegmentSummaryUrl = '<%=Url.Action("UpdateSegmentSummary")%>';
        var GetSegmentDetailsUrl = '<%=Url.Action("GetSegmentDetails")%>';
        var UpdateProjectUpdateStatusUrl = '<%=Url.Action("UpdateProjectUpdateStatusBySegment")%>';

        var isEditable = '<%= (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin) %>';

        var $scrollingDiv = $("#update-inview");

        $(window).scroll(function() {
            $scrollingDiv
			.stop()
			.animate({ "marginTop": ($(window).scrollTop() + 80) + "px" }, "slow");
        });

        if ($('#submitForm')) {
            $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
        }
        
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
        
        //Wireup the change handlers to enable the save button...
        $().ready(function () {
            var bindDetailsColorbox = function () {
                $(".confirmAmend").colorbox({
                    width: "830px",
                    height: "590px",
                    inline: true,
                    href: "#segmentDetails",

                    onLoad: function () {
                        $('#process-segment').removeClass('add-segment-details').addClass('update-segment-details');
                        $('#process-segment').text("Save");
                        var segmentid = $(this).attr('id').replace('edit_', '');

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
                            success: function (response) {
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
                                    , SpacesBase: new item($('#segment_details_SpacesBase'), response.SpacesBase)
                                    , SpacesFuture: new item($('#segment_details_SpacesFuture'), response.SpacesFuture)
                                    , AssignmentStatusId: new item($('#segment_details_AssignmentStatusId'), response.AssignmentStatusId)
                                    , Length: new item($('#segment_details_Length'), response.Length)
                                    , ModelingCheck: new item($('#segment_details_modelingcheck'), response.ModelingCheck)
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
                                segment_details.SpacesBase.isText ? segment_details.SpacesBase.element.val(segment_details.SpacesBase.Value) : segment_details.SpacesBase.element.text(segment_details.SpacesBase.Value);
                                segment_details.SpacesFuture.isText ? segment_details.SpacesFuture.element.val(segment_details.SpacesFuture.Value) : segment_details.SpacesFuture.element.text(segment_details.SpacesFuture.Value);
                                segment_details.AssignmentStatusId.isText ? segment_details.AssignmentStatusId.element.val(segment_details.AssignmentStatusId.Value) : segment_details.AssignmentStatusId.element.text(segment_details.AssignmentStatusId.Value);
                                segment_details.Length.isText ? segment_details.Length.element.val(segment_details.Length.Value) : segment_details.Length.element.text(segment_details.Length.Value);
                                segment_details.ModelingCheck.element.attr('checked', segment_details.ModelingCheck.Value);

                                var lrs = response._LRS.Columns;
                                var lrsScheme = response._LRS.Scheme;
                                //$("#debug").html(JSON.stringify(lrs));
                                $.each(lrsScheme, function (key, value) {
                                    var displayType = value.DisplayType;
                                    if (displayType != 'none') {
                                        var columnName = value.ColumnName;
                                        var friendlyName = value.FriendlyName;
                                        var value = !$.isset(lrs[value.ColumnName]) ? lrs[value.ColumnName] : "";
                                        //alert(value.ColumnName + ": " + lrs[value.ColumnName]);

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

                                UpdateProjectUpdateStatusBySegment(segmentid);
                            }
                        });
                    },
                    onCleanup: function () {
                        unloadSegmentDetails();
                    }
                });
            }

            var $save = $('<button id="process-segment" disabled="disabled" class="update-segment-details fg-button ui-state-default ui-priority-primary ui-state-disabled">Add</button>').appendTo('#cboxContent');

            bindDetailsColorbox();

            $("#new-segmentdetails").colorbox(
                {
                    width: "830px",
                    height: "<%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>375px<% } else { %>300px<% } %>",
                    inline: true,
                    href: "#segmentDetails",
                    onLoad: function () {
                        $('#process-segment').removeClass('update-segment-details').addClass('add-segment-details');
                        $('#process-segment').text("Add");
                    },
                    onCleanup: function () { unloadSegmentDetails(); }
                });
            var isEdit = false;



            $('input').change(function () {
                isEdit = true;
            });

            function checkIsEdit() {
                if (isEdit)
                    return "You have modify some content";
            }

            window.onbeforeunload = checkIsEdit;

            $('.delete-segment').live("click", function () {
                //get the segmentid : segment_row_5022
                var segmentid = this.id.replace('delete_', '');
                var row = $('#segment_row_' + segmentid);
                if (confirmDelete()) {
                    $.ajax({
                        type: "POST",
                        url: DropSegmentUrl,
                        data: "segmentId=" + segmentid,
                        dataType: "json",
                        success: function (response) {
                            $('#result').html(response.message);
                            $(row).empty();
                            $('div#result').addClass('success').autoHide();

                            UpdateProjectUpdateStatusBySegment(segmentid);
                        }
                    });
                }
                return false;
            });

            //Update a segment in the list
            $('.update-segment-summary').live("click", function () {

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

                $.ajax({
                    type: "POST",
                    url: EditSegmentSummaryUrl,
                    data: 'segmentId=' + segmentid
                        + '&facilityName=' + facilityname
                        + '&startAt=' + startat
                        + '&endAt=' + endat
                        + '&networkId=' + networkid
                        + '&openyear=' + openyear
                        + '&lanesbase=' + lanesbase
                        + '&lanesfuture=' + lanesfuture,
                    dataType: "json",
                    success: function (response) {
                        $('#result').html(response.message);

                        //Disable the add button
                        $('#delete_' + segmentid).html("Delete").removeClass('update-segment-summary').addClass('delete-segment');
                        //$('.update-segment').html("Delete").addClass('delete-segment').removeClass('update-segment');
                        $('div#result').addClass('success').autoHide();
                        $("#actionbar").show();
                        $('#delete_' + segmentid).removeClass("bg-green");
                        UpdateProjectUpdateStatusBySegment(segmentid);
                        window.onbeforeunload = null;
                    }
                });

                return false;
            });

            function UpdateProjectUpdateStatusBySegment(segmentid) {
                $.ajax({
                    type: "POST",
                    url: UpdateProjectUpdateStatusUrl,
                    data: 'segmentId=' + segmentid,
                    dataType: "json",
                    success: function (response) {

                    },
                    error: function (response) {
                        $('#result').html(response.message);
                        $('div#result').addClass('error').autoHide();
                    }
                });
            }

            //Update a segment in the list
            $('.update-segment-details').live("click", function () {
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
                var spacesbase = $('#segment_details_SpacesBase').val();
                var spacesfuture = $('#segment_details_SpacesFuture').val();
                var assignmentstatusid = 0;
                var length = $('#segment_details_Length').val();
                var modelingcheck = $("#segment_details_modelingcheck").is(":checked");

                var lrsData = new Array();
                $.each($('input[id^=segment_details_lrs_]'), function () {
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
                + '&spacesbase=' + spacesbase
                + '&spacesfuture=' + spacesfuture
                + '&assignmentStatusId=' + assignmentstatusid
                + '&length=' + length
                + '&modelingcheck=' + modelingcheck
                + '&LRSRecord=' + lrsData,
                    dataType: "json",
                    success: function (response) {
                        $('#segment-details-error').html(response.message).addClass('success').autoHide();

                        //Disable the add button
                        $('#process-segment').addClass('ui-state-disabled').attr('disabled', 'disabled');

                        // update master data
                        $('#segment_' + segmentid + '_FacilityName').val(facilityname);
                        $('#segment_' + segmentid + '_StartAt').val(startat);
                        $('#segment_' + segmentid + '_EndAt').val(endat);
                        $('#segment_' + segmentid + '_OpenYear').val(openyear);
                        $('#segment_' + segmentid + '_LanesBase').val(lanesbase);
                        $('#segment_' + segmentid + '_LanesFuture').val(lanesfuture);
                        //$('#segment_' + segmentid + '_NetworkId').val(networkid);

                        // record should be updated so switch update back to delete
                        if ($('#delete_' + segmentid).hasClass('update-segment-summary')) {
                            $('#delete_' + segmentid).html("Delete").removeClass('update-segment-summary').addClass('delete-segment');
                        }

                        UpdateProjectUpdateStatusBySegment(segmentid);
                        window.onbeforeunload = null;
                    }
                });
                return false;
            });




            // Prevent accidental navigation away
            $('input[id^=Scope_]', document.dataForm).bind("change", function () { setConfirmUnload(true, true); });
            $('input[id^=Scope_]', document.dataForm).bind("keyup", function () { setConfirmUnload(true, true); });
            $('textarea[id^=Scope_]', document.dataForm).bind("change", function () { setConfirmUnload(true, true); });
            $('textarea[id^=Scope_]', document.dataForm).bind("keyup", function () { setConfirmUnload(true, true); });
            //disable the onbeforeunload message if we are using the submitform button
            if ($('#submitForm')) {
                $('#submitForm').click(function () { window.onbeforeunload = null; return true; });
            }

            //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
            var v = jQuery("#dataForm").validate({
                submitHandler: function (form) {
                    jQuery(form).ajaxSubmit({
                        dataType: 'json',
                        success: function (data, textStatus) {
                            $('#result').html(data.message).addClass('success').autoHide();
                            $('#submitForm').addClass('ui-state-disabled');
                            $("#actionbar").show();
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            $('#result').text(errorThrown);
                            $('#result').addClass('error');
                        }
                    });
                }
            });

            //Add a segment to the list
            $('.add-segment-details').live("click", function () {
                //grab the values from the active form

                var facilityname = $('#segment_details_FacilityName').val();
                var startat = $('#segment_details_StartAt').val();
                var endat = $('#segment_details_EndAt').val();
                var networkid = $('#segment_details_NetworkId').val();
                var improvementtypeid = $('#segment_details_ImprovementTypeId').val() || 0;
                var planfacilitytypeid = $('#segment_details_PlanFacilityTypeId').val() || 0;
                var modelingfacilitytypeid = $('#segment_details_ModelingFacilityTypeId').val() || 0;
                var openyear = $('#segment_details_OpenYear').val();
                var lanesbase = ($('#segment_details_LanesBase').val() || 0);
                var lanesfuture = $('#segment_details_LanesFuture').val();
                var spacesbase = $('#segment_details_SpacesBase').val();
                var spacesfuture = $('#segment_details_SpacesFuture').val();
                var length = $('#segment_details_Length').val();
                var assignmentstatusid = $('#new_segmentassignmentstatusid').val() || 0;

                var routename = $('#segment_details_RouteName').val();
                var beginmeasure = $('#segment_details_BeginMeasure').val();
                var endmeasure = $('#segment_details_EndMeasure').val();
                var offset = $('#segment_details_Offset').val();
                var comments = $('#segment_details_Comments').val();

                var segmentid = 0;
                var projectversionid = $('#Scope_ProjectVersionId').val();

                if (facilityname == '' || startat == '' || networkid == '') {
                    var msg = 'Please fill in:';
                    if (facilityname == '') msg = msg + ' Facility Name';
                    if (startat == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Start At';
                    if (networkid == '') msg = msg + (msg.length > 15 ? ', ' : ' ') + 'Network';
                    $('#segment-details-error').addClass('error').html(msg).show();
                    return false;
                }

                //Add to database via XHR
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
                        + '&spacesbase=' + spacesbase
                        + '&spacesfuture=' + spacesfuture
                        + '&assignmentStatusId=' + assignmentstatusid
                        + '&length=' + length,
                    dataType: "json",
                    success: function (response) {
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
                            'r1_openyear': openyear,
                            'r1_lanesbase': lanesbase,
                            'r1_lanesfuture': lanesfuture
                        };

                        var content = '<tr id="segment_row_' + segmentid + '" class="new">';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_FacilityName", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), "r1_facilityname", new { style = "width:200px;", @maxlength = "75" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_StartAt", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), "r1_startat", new { style = "width:110px;", @maxlength = "50" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_EndAt", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), "r1_endat", new { style = "width:110px;", @maxlength = "50" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_OpenYear", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), "r1_openyear" , new { style = "width:75px;", @maxlength = "4" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_LanesBase", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin) && !Model.Project.ImprovementTypeId.Equals(16), "r1_lanesbase" , new { style = "width:75px;", @maxlength = "4" })%></td>';
                        content += '<td><%= Html.DrcogTextBox("segment_r1_segmentid_LanesFuture",(Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), "r1_lanesfuture" , new { style = "width:75px;", @maxlength = "4" })%></td>';
                        content += '<td>';
                        //content += '<button class="confirmAmend fg-button ui-state-default ui-priority-primary ui-corner-all" id="edit_r1_segmentid">Details</button>';
                        content += '<%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)){ %>';
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

                        UpdateProjectUpdateStatusBySegment(segmentid);
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
                $("#actionbar").hide();
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
                $('#segment_details_SpacesBase').val('');
                $('#segment_details_SpacesFuture').val('');
                $('#segment_details_AssignmentStatusId').val('');
                $('#segment_details_Length').val('');
                $('#segment_details_modelingcheck').attr('checked', false);
                $("#lrsdetails").html("");
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

            $('input[id^=segment_details_]').live('keyup', function() {
                //var segmentid = $(this).parents("tr").attr('id').replace('segment_details_', '');
                EnableSegmentDetailsUpdate();
                setConfirmUnload(true, false);

            });
            $('input[id^=segment_details_]').live('change', function() {
                //var segmentid = $(this).parents("tr").attr('id').replace('segment_details_', '');
                EnableSegmentDetailsUpdate();
                setConfirmUnload(true, false);

            });
            $('select[id^=segment_details_]').live('change', function() {
                //var segmentid = $(this).parents("tr").attr('id').replace('segment_details_', '');
                EnableSegmentDetailsUpdate();
                setConfirmUnload(true, false);

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
            if ($('#delete_' + id).html() != 'Save') {
                $('#delete_' + id).html("Save").removeClass('delete-segment').addClass('update-segment-summary');
                $('#delete_' + id).addClass("bg-green");
            }
        }
        function EnableSegmentDetailsUpdate() {
            if ($('#process-segment').hasClass('ui-state-disabled')) {
                $('#process-segment').removeClass('ui-state-disabled').removeAttr('disabled');
            }
        }

        function confirmDelete() {
            if (confirm('Are you sure you are ready to delete this item?'))
                return true;
            else
                return false;
        }

        $.fn.GetDictionaryValue = function(array, key) {
            ///  
            /// Get the dictionary value from the array at the specified key
            ///  
            alert(key);
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
                return (x === undefined);
            }
        });

    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="tab-content-container">
    <%Html.RenderPartial("~/Views/Survey/Partials/ProjectTabPartial.ascx", Model); %>
    <div class="tab-form-container tab-scope">
        <% if ( !Model.Project.UpdateStatusId.Equals(default(int)) ) { %>
            <h2 id="status">Current Status: <%= Model.Project.UpdateStatus %></h2> 
        <% } %>
        <% using (Html.BeginForm("UpdateScope", "Survey", FormMethod.Post, new { @id = "dataForm" })) %>
        <% { %>
            <%Html.RenderPartial("~/Views/Survey/Partials/ManagerRibbonPartial.ascx", Model); %>
            <fieldset>
            
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("Current.Name", Model.Current.Name)%>
            <%=Html.Hidden("Scope.ProjectVersionId", Model.Scope.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.Scope.ProjectId)%>
                
           <p><label>Project Description:</label></p>           
           <p>
                <textarea id="Scope_ProjectDescription" class="longInputElement required" 
                    title="Please provide a description for the project." 
                    name="Scope.ProjectDescription" 
                    rows="10" cols="50"><%= Model.Scope.ProjectDescription%></textarea>
           </p>
           <%--<div id="timelapse" class="box">
            <p>
                <%= Html.LabelFor(x => x.Scope.BeginConstructionYear) %>
                <%= Html.DrcogTextBox("Scope.BeginConstructionYear", 
                    Model.Project.IsEditable(), 
                    Model.Scope.BeginConstructionYear, 
                    new { @class = "required", title="Please enter a End Construction Year.", @MAXLENGTH = 4 })%>
            </p>
            <p>
                <%= Html.LabelFor(x => x.Scope.OpenToPublicYear) %>
                <%= Html.DrcogTextBox("Scope.OpenToPublicYear", 
                    Model.Project.IsEditable(), 
                    Model.Scope.OpenToPublicYear, 
                    new { @class = "required", title="Please enter a Open to Public Year.", @MAXLENGTH = 4 })%>
            </p>
           </div>--%>
            <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
              { %>
                <div style="top: 80px;" id="update-inview">
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
       <h2>Project Staging Details</h2>
       <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)){ %>
            <button id="new-segmentdetails" style="position: absolute; top: -5px; right: 50px;" class="fg-button ui-state-default ui-priority-primary ui-corner-all">Add New Staging</button>
        <% } %>
       <table id="segments">
            <thead>
                <tr>
                    <th>Facility Name</th>
                    <th>Start At</th>
                    <th>End At</th>
                    <th>Open Year</th>
                    <th>LanesBase</th>
                    <th>LanesFuture</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
            <% 
            int rowCount = 0;
            foreach (var item in Model.Segments.ToList<DRCOG.Domain.Models.Survey.SegmentModel>()) 
            {
                rowCount++;
            %>
                <tr id="segment_row_<%=item.SegmentId.ToString() %>" <%= rowCount % 2 == 0 ? "class=\"even\"" : "class=\"odd\""%>>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_FacilityName", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), item.FacilityName.ToString(), new { style = "width:200px;", @maxlength = "75" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_StartAt", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), item.StartAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_EndAt", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), item.EndAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_OpenYear", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), item.OpenYear.ToString(), new { style = "width:75px;", @maxlength = "4" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_LanesBase", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin) && !Model.Project.ImprovementTypeId.Equals(16), item.LanesBase.ToString(), new { style = "width:75px;", @maxlength = "4" })%></td>
                    <td><%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_LanesFuture", (Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin), item.LanesFuture.ToString(), new { style = "width:75px;", @maxlength = "4" })%></td>
                    <%--<td>
                        <%= Html.DropDownList("segment_" + item.SegmentId.ToString() + "_NetworkId",
                            Model.Scope.IsEditable(),
                            new SelectList(Model.AvailableNetworks, "key", "value", item.NetworkId),
                            "-- Select --",
                            new { @class = "not-required", title = "Please select a network" })%>
                    </td>--%>
                    <td>
                        <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>
                          <button id="edit_<%=item.SegmentId.ToString() %>" class="confirmAmend fg-button ui-state-default ui-priority-primary ui-corner-all">Details</button>
                        <% } %>
                        <%if ((Model.Project.IsEditable() && Model.Current.IsOpen()) || Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin))
                          { %>
                            <button class="delete-segment fg-button ui-state-default ui-priority-primary ui-corner-all" id='delete_<%=item.SegmentId.ToString() %>'>Delete</button>
                        <% } %>
                        
                    </td>
                </tr>
            <% } %>
                <tr style="display: none;" id="segment_last_record"><td colspan="5"></td></tr>
            
            </tbody>
       </table>
    </div>
   <div class="clear"></div>
</div>
<!-- This contains the hidden content for inline calls --> 
<div style='display:none'> 
	<div id='segmentDetails' style='padding:10px; background:#fff;'>
	    <% Html.RenderPartial("~/Views/Survey/Partials/SegmentPartial.ascx"); %>
	</div>
</div> 

</asp:Content>




