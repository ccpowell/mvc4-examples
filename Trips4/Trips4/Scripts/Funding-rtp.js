//*** TIP Funding code

$(document).ready(function() {

    var projectVersionId = $(".update-financialrecord").attr('id').replace('update_', '');

    var $scrollingDiv = $("#update-inview");

    $(window).scroll(function() {
        $scrollingDiv
			.stop()
			.animate({ "marginTop": ($(window).scrollTop() + 30) + "px" }, "slow");
    });


    //Prevent accidental navigation away
    $(':input', document.dataForm).bind("change", function() { setConfirmUnload(true); });
    $(':input', document.dataForm).bind("keyup", function() { setConfirmUnload(true); });

    //disable the onbeforeunload message if we are using the submitform button
    if ($('#submitForm')) {
        $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
    }

    var categoryid = $('#Funding_ReportGroupingCategoryId :selected').val();
    getCategoryDetails(categoryid);

    $("span[id^=fundingsource_]").each(function() {
        var id = $(this).parents("tr").attr('id').replace('fundingsource_row_', '');
        removeFundingOption(id);
    });

    //*** TIP Financial Funding code [Methods: Update]
    //Update a Financial Record
    $('.update-financialrecord').live("click", function() {
        var recordid = this.id.replace('update_', '');
        var constantcost = $('#Funding_ConstantCost').val();//.saveSafe();
        var visioncost = $('#Funding_VisionCost').val();//.saveSafe();
        var yoecost = $('#Funding_YOECost').val();//.saveSafe();
        //var previous = $('#Funding_Previous').val();
        //var future = $('#Funding_Future').val();
        //var totalcost = $('#Funding_TotalCost').val();
        var plantypeid = $('#Funding_PlanTypeId').val();
        var reportgroupingcategoryid = $('#Funding_ReportGroupingCategoryId').val();

        //alert(constantcost + " " + visioncost + " " + yoecost);
        //Add to database via XHR
        $.ajax({
            type: "POST",
            url: EditFinancialRecordUrl,
            data: "projectversionid=" + recordid
                    + "&ConstantCost=" + constantcost
                    + "&VisionCost=" + visioncost
                    + "&YOECost=" + yoecost
            //+ "&Previous=" + previous
            //+ "&Future=" + future
            //+ "&TotalCost=" + totalcost
                    + "&PlanTypeId=" + plantypeid
                    + "&ReportGroupingCategoryId=" + reportgroupingcategoryid,
            dataType: "json",
            success: function(response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    $('div#resultRecord').html(response.message);
                    $('div#resultRecord').addClass('success');
                    autoHide(2500);
                }
                else {
                    $('div#resultRecord').html(response.message + " Details: " + response.exceptionMessage);
                    $('div#resultRecord').addClass('error');
                    autoHide(10000);
                }
                window.onbeforeunload = null;
            },
            error: function(response, textStatus, AjaxException) {
                //alert("here");
                $('div#resultRecord').html(response.statusText);
                $('div#resultRecord').addClass('error');
                autoHide(10000);
            }
        });

        return false;
    });

    //Add a funding resource
    $("#btn-fundingsource-new").live("click", function() {
        if ($('#btn-fundingsource-new').hasClass('ui-state-disabled')) { return false; }
        var fundingResource = $("#fundingsource_new_name :selected");
        var fundingResourceId = fundingResource.val();

        $.ajax({
            type: "POST",
            url: addFundingSource,
            data: 'projectVersionId=' + projectVersionId
                + '&fundingResourceId=' + fundingResourceId,
            dataType: "json",
            success: function(response) {
                //$('#result').html(response.message);
                var resource = {
                    'r1_fundingResourceId': fundingResourceId,
                    'r1_fundingResourceName': fundingResource.text()
                };

                var content = '<tr id="fundingsource_row_r1_fundingResourceId">';
                content += '<td><span class="fakeinput w250" id="fundingsource_r1_fundingResourceId_name">r1_fundingResourceName</span>'; //<select name="fundingsource_r1_fundingResourceId_name" title="Please select a funding source" class="longInputElement not-required" id="fundingsource_r1_fundingResourceId_name"></td>';
                content += '<td>';
                content += '<button class="fundingsource-delete fg-button ui-state-default ui-priority-primary ui-corner-all" id="fundingsource_delete_r1_fundingResourceId" type="submit">Delete</button>';
                content += "</td>";
                content += '</tr>';
                content = replaceFromArray(content, resource);
                $('#fundingsource-editor').before(content);

                $("#fundingsource_new_name").copyOptions("#fundingsource_" + fundingResourceId + "_name", "all");
                $("#fundingsource_" + fundingResourceId + "_name").val(fundingResourceId);

                removeFundingOption(fundingResourceId);
                $("#fundingsource_new_name").removeOption(fundingResourceId);

                //Disable the add button
                //$('#add-segment-details').addClass('ui-state-disabled').attr('disabled', 'disabled');
                //$('div#result').addClass('success').autoHide();
                window.onbeforeunload = null;
            }
        });
        return false;
    });

    $('.fundingsource-delete').live("click", function() {
        var resourceId = this.id.replace('fundingsource_delete_', '');
        $.ajax({
            type: "POST",
            url: deleteFundingSource,
            data: 'projectVersionId=' + projectVersionId
                + '&fundingResourceId=' + resourceId,
            dataType: "json",
            success: function(response) {
                //$('#result').html(response.message);
                copyFundingOption($("#fundingsource_" + resourceId + "_name :selected"));
                $('#fundingsource_row_' + resourceId).empty();
                window.onbeforeunload = null;
            }
        });
        return false;
    });

    $('#Funding_ReportGroupingCategoryId').change(function() {
        var categoryid = $('#Funding_ReportGroupingCategoryId :selected').val();
        getCategoryDetails(categoryid);
    });

    $('select[id^=fundingsource_]').live('change', function() {
        var id = $(this).parents("tr").attr('id').replace('fundingsource_row_', '');
        ChangeFundingAction(id);
    });

    $('select[id^=fundingsource_new]').live('change', function() {
        if ($("#fundingsource_new_name :selected").val() > 0) {
            if ($('#btn-fundingsource-new').hasClass('ui-state-disabled')) {
                $('#btn-fundingsource-new').removeClass('ui-state-disabled').removeAttr('disabled');
            }
        } else {
            $('#btn-fundingsource-new').addClass('ui-state-disabled').attr('disabled', 'disabled');
        }
    });
    //$(".money").maskMoney();

});

function ChangeFundingAction(btnId) {
    var btn = $('#fundingsource_delete_' + btnId);
    if( btn.html() != "Update") {
        btn.removeClass('fundingsource-delete').html("Update").addClass('fundingsource-update');
    }
}

// *** General functions

function getCategoryDetails(categoryId) {
    categoryId = parseInt(categoryId);

    $.ajax({
        type: "POST",
        url: GetCategoryDetails,
        data: "id=" + categoryId,
        dataType: "json",
        success: function(response) {
            $('#catShort').text(response.ShortTitle);
            $('#catLong').text(response.Description);
        },
        error: function(response) {
            $('#result').html(response.error);
        }
    });
}

function autoHide(timeout) {
    if (isNaN(timeout)) timeout = 5000;
    setTimeout(function() {
            $("div#resultRecordDetail").fadeOut("slow", function() {
            $("div#resultRecordDetail").empty().removeClass().removeAttr('style');
        });
    }, timeout);
    setTimeout(function() {
            $("div#resultRecord").fadeOut("slow", function() {
            $("div#resultRecord").empty().removeClass().removeAttr('style');
        });
    }, timeout); 
}

// *** Functions to update the Financial Record from the Details updates (not being used)

function ChangeFinancialRecordToUpdate(id) {
    $('#delete_' + id).html("Update").removeClass('delete-pool').addClass('update-financialrecord');
}

function UpdateFinancialRecordTotal() {
    //Enable Add button...
    var addButton = $('#add-financialrecord');
    addButton.removeClass('ui-state-disabled').removeAttr('disabled');
}

function setConfirmUnload(on) {
    $('#submitForm').removeClass('ui-state-disabled');
    $('#result').html("");
    window.onbeforeunload = (on) ? unloadMessage : null;
}

function unloadMessage() {
    return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
}

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

function removeFundingOption(id) {
    $("select[id^=fundingsource_]").each(function() {
        var value = $(this).val();
        if (!(value == id)) {
            $(this).removeOption(id);
        }
    });
}

function copyFundingOption(object) {
    $("select[id^=fundingsource_]").each(function() {
        $(this).addOption(object.val(), object.text(), false);
    });
}

