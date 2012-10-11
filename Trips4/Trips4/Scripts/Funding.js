//*** TIP Funding code

$(document).ready(function () {

    $.mask.masks.money = { mask: '999,999,999', fixedChars: '[$,]', type: 'reverse' };
    $('input:text').setMask();


    //Prevent accidental navigation away
    $(':input', document.dataForm).bind("change", function () { setConfirmUnload(true); });
    $(':input', document.dataForm).bind("keyup", function () { setConfirmUnload(true); });

    $(':input', "#phase-editor").bind('change', function () {
        var button = $("#add-phase");

        if ($("#new_year").val() !== "" && $("#new_phase").val() !== "" & $("#new_fundingresource").val() !== "") {
            if ($(button).hasClass("ui-state-disabled")) {
                $(button).removeClass("ui-state-disabled").removeAttr('disabled');
            }
        } else if (!$(button).hasClass("ui-state-disabled")) {
            $(button).addClass("ui-state-disabled").attr('disabled');
        }
    });

    //disable the onbeforeunload message if we are using the submitform button
    if ($('#submitForm')) {
        $('#submitForm').click(function () { window.onbeforeunload = null; return true; });
    }

    $("#phase-editor #add-phase").bind("click", function () {
        //var id = $(this).attr("id").replace("delete_", "");

        var projectfinancialrecordid = $("#ProjectFinancialRecordId")
        , fundingincrementid = $("#new_year option:selected")
        , fundingresourceid = $("#new_fundingresource option:selected")
        , phaseid = $("#new_phase option:selected");

        $.ajax({
            type: "POST",
            url: AddPhaseUrl,
            data: "projectFinancialRecordId=" + projectfinancialrecordid.val()
                + "&fundingIncrementId=" + fundingincrementid.val()
                + "&fundingResourceId=" + fundingresourceid.val()
                + "&phaseId=" + phaseid.val(),
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    //Add into the DOM

                    var content = '<tr id="phase_row_' + projectfinancialrecordid + '_' + fundingincrementid + '_' + fundingresourceid + '_' + phaseid + '">';
                    content += "<td>" + fundingincrementid.text() + "</td>";
                    content += "<td>" + phaseid.text() + "</td>";
                    content += "<td>" + fundingresourceid.text() + "</td>";
                    content += "<td></td>";
                    content += "<td><button class='delete-phase fg-button ui-state-default ui-priority-primary ui-corner-all' id='delete_" + projectfinancialrecordid.val() + "_" + fundingincrementid.val() + "_" + fundingresourceid.val() + "_" + phaseid.val() + "'>Delete</button></td></tr>";
                    $('#phase-editor').before(content);

                    // reset
                    $("#new_year").val("");
                    $("#new_fundingresource").val("");
                    $("#new_phase").val("");
                    $("#add-phase").addClass("ui-state-disabled").attr('disabled', 'disabled');
                }
                else {

                    $('div#resultRecordDetail').html(response.message + " Details: " + response.exceptionMessage);
                    $('div#resultRecordDetail').addClass('error');
                    autoHide(10000);
                }
                window.onbeforeunload = null;
            },
            error: function (response, textStatus, AjaxException) {

                $('div#resultRecordDetail').html(response.statusText);
                $('div#resultRecordDetail').addClass('error');
                autoHide(10000);
            }
        });


        return false;
    });

    //Calculate starting totals
    $('div#programmedTotalLabel').html(UpdateProgrammedTotals());
    $('div#outyearTotalLabel').html(UpdateOutyearTotals());
    $('div#federalTotalLabel').html(UpdateFederalTotals());
    $('div#stateTotalLabel').html(UpdateStateTotals());
    $('div#localTotalLabel').html(UpdateLocalTotals());
    $('div#projectCostTotalLabel').html(UpdateProjectTotals());

    $(".currency").formatCurrency({ roundToDecimalPlace: 0 });
    //*** TIP Financial Funding code [Methods: Update]

    //Update a Financial Record
    $('.update-financialrecord').live("click", function () {
        var recordid = this.id.replace('updateFR_', '');
        var previous = parseInt($('#previousTotalTextBox').val().replace(/[,]/g, "")) || parseInt($('#previousTotalTextBox').text() || "0");
        var future = parseInt($('#futureTotalTextBox').val().replace(/[,]/g, "")) || parseInt($('#futureTotalTextBox').text() || "0");
        var programmed = parseInt($('#programmedTotalLabel').html().replace(/[,$]/g, ""));
        var outyear = parseInt($('#outyearTotalLabel').html().replace(/[,$]/g, ""));
        var federalTotal = parseInt($('#federalTotalLabel').html().replace(/[,$]/g, ""));
        var stateTotal = parseInt($('#stateTotalLabel').html().replace(/[,$]/g, ""));
        var localTotal = parseInt($('#localTotalLabel').html().replace(/[,$]/g, ""));

        var TIPFunding = programmed + outyear;
        var totalCost = previous + programmed + outyear + future;

        //Add to database via XHR
        $.ajax({
            type: "POST",
            url: EditFinancialRecordUrl,
            data: "financialrecordid=" + recordid
                    + "&previous=" + previous
                    + "&future=" + future
                    + "&tipfunding=" + TIPFunding
                    + "&federaltotal=" + federalTotal
                    + "&statetotal=" + stateTotal
                    + "&localtotal=" + localTotal
                    + "&totalcost=" + totalCost,
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    $('div#projectCostTotalLabel').html(UpdateProjectTotals());
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
            error: function (response, textStatus, AjaxException) {
                $('div#resultRecord').html(response.statusText);
                $('div#resultRecord').addClass('error');
                autoHide(10000);
            }
        });

        return false;
    });

    //*** TIP Financial Detail Funding code [Methods: Update & Add New Group]

    //Update Financial Detail record
    $('.update-financialrecorddetail').live("click", function () {
        var key = this.id.replace('updateFRD_', '');
        var key_array = key.split("_");
        var versionid = key_array[0];
        var fundingtypeid = key_array[1];
        var fundinglevelid = key_array[2];
        var shortkey = key_array[1] + '_' + key_array[2];
        var rowid = $('#fundingdetail_row_' + key);
        var fundingperiodid = $('#CurrentFundingPeriodID').val();

        var i1 = $('#fundingdetail_' + shortkey + '_i1').val().replace(/[,]/g, "");
        var i2 = $('#fundingdetail_' + shortkey + '_i2').val().replace(/[,]/g, "");
        var i3 = $('#fundingdetail_' + shortkey + '_i3').val().replace(/[,]/g, "");
        var i4 = $('#fundingdetail_' + shortkey + '_i4').val().replace(/[,]/g, "");
        var i5 = 0;
        var outYearIs5 = false;
        var checkForItem5 = document.getElementById('fundingdetail_' + shortkey + '_i5');
        if (isNaN(checkForItem5)) {
            i5 = $('#fundingdetail_' + shortkey + '_i5').val().replace(/[,]/g, "");
            outYearIs5 = true;
        }

        if (isNaN(i1)) i1 = 0;
        if (isNaN(i2)) i2 = 0;
        if (isNaN(i3)) i3 = 0;
        if (isNaN(i4)) i4 = 0;
        if (isNaN(i5)) i5 = 0;

        //Update hidden total fields
        hdnPT = $('#hdnProgrammedTotal_' + shortkey);
        hdnOT = $('#hdnOutyearTotal_' + shortkey);
        hdnFT = $('#hdnFederalTotal_' + shortkey);
        hdnST = $('#hdnStateTotal_' + shortkey);
        hdnLT = $('#hdnLocalTotal_' + shortkey);

        if (outYearIs5) {
            hdnPTval = parseInt(i1) + parseInt(i2) + parseInt(i3) + parseInt(i4);
            hdnOTval = parseInt(i5);
        }
        else {
            hdnPTval = parseInt(i1) + parseInt(i2) + parseInt(i3);
            hdnOTval = parseInt(i4);
        }

        var hdnFTval = 0;
        var hdnSTval = 0;
        var hdnLTval = 0;
        if (fundinglevelid == 3) { hdnFTval = parseInt(i1) + parseInt(i2) + parseInt(i3) + parseInt(i4) + parseInt(i5); }
        if (fundinglevelid == 2) { hdnSTval = parseInt(i1) + parseInt(i2) + parseInt(i3) + parseInt(i4) + parseInt(i5); }
        if (fundinglevelid == 1) { hdnLTval = parseInt(i1) + parseInt(i2) + parseInt(i3) + parseInt(i4) + parseInt(i5); }

        hdnPT.val(hdnPTval);
        hdnOT.val(hdnOTval);
        hdnFT.val(hdnFTval);
        hdnST.val(hdnSTval);
        hdnLT.val(hdnLTval);

        //Add to database via XHR
        $.ajax({
            type: "POST",
            url: EditFinancialRecordDetailUrl,
            data: "projectfinancialrecordID=" + versionid
                        + "&fundingTypeID=" + fundingtypeid
                        + "&fundingLevelID=" + fundinglevelid
                        + "&fundingPeriodID=" + fundingperiodid
                        + "&incr01=" + i1
                        + "&incr02=" + i2
                        + "&incr03=" + i3
                        + "&incr04=" + i4
                        + "&incr05=" + i5,
            dataType: "json",
            success: function (response, textStatus, XMLHttpRequest) {
                if (response.error == "false") {
                    $('div#resultRecordDetail').html(response.message);
                    $('div#resultRecordDetail').addClass('success');
                    $('div#programmedTotalLabel').html(UpdateProgrammedTotals());
                    $('div#outyearTotalLabel').html(UpdateOutyearTotals());
                    $('div#federalTotalLabel').html(UpdateFederalTotals());
                    $('div#stateTotalLabel').html(UpdateStateTotals());
                    $('div#localTotalLabel').html(UpdateLocalTotals());
                    $('div#projectCostTotalLabel').html(UpdateProjectTotals());
                    autoHide(2500);
                }
                else {
                    $('div#resultRecordDetail').html(response.message + " Details: " + response.exceptionMessage);
                    $('div#resultRecordDetail').addClass('error');
                    autoHide(10000);
                }
                window.onbeforeunload = null;
            },
            error: function (response, textStatus, AjaxException) {
                $('div#resultRecordDetail').html(response.statusText);
                $('div#resultRecordDetail').addClass('error');
                autoHide(10000);
            }
        });
        return false;
    });
});
  
      //Add Financial Detail record
$(".add-financialrecorddetail").live("click", function() {
    var timeperiodid = $("#CurrentFundingPeriodID").val();
    $.fn.colorbox({
        width: "800px",
        height: "290px",
        inline: true,
        href: "#add-newGroup-panel",
        onLoad: function() {
            $('.add-newFundingGroup-final').live("click", function() {
                var versionID = this.id.replace('AddFRD_', '');
                var fundingperiodID = timeperiodid; // TODO: Remove this hard-code
                var fundingtypeID = $('#FinancialRecordDetail_FundingTypes').val();
                var fundingtype = $('#FinancialRecordDetail_FundingTypes option:selected').text();

                //Add it visually to the output???
                //alert(versionID + ' ' + fundingperiodID + ' ' + fundingtypeID);
                $.ajax({
                    type: "POST",
                    url: AddFinancialRecordDetailUrl,
                    data: "projectversionID=" + versionID
                        + "&fundingPeriodID=" + fundingperiodID
                        + "&fundingTypeID=" + fundingtypeID,
                    dataType: "json",
                    success: function(response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            location.reload();
                            $.fn.colorbox.close();
                            $('div#resultRecordDetail').html(response.message);
                            $('div#resultRecordDetail').addClass('success');
                            autoHide(2500);
                        }
                        else {
                            $.fn.colorbox.close();
                            $('div#resultRecordDetail').html(response.message + " Details: " + response.exceptionMessage);
                            $('div#resultRecordDetail').addClass('error');
                            autoHide(10000);
                        }
                        window.onbeforeunload = null;
                    },
                    error: function(response, textStatus, AjaxException) {
                        $.fn.colorbox.close();
                        $('div#resultRecordDetail').html(response.statusText);
                        $('div#resultRecordDetail').addClass('error');
                        autoHide(10000);
                    }
                });
                return false;
            });
        },
        onClosed: function() {
            $('div#programmedTotalLabel').html(UpdateProgrammedTotals());
            $('div#outyearTotalLabel').html(UpdateOutyearTotals());
            $('div#federalTotalLabel').html(UpdateFederalTotals());
            $('div#stateTotalLabel').html(UpdateStateTotals());
            $('div#localTotalLabel').html(UpdateLocalTotals());
            $('div#projectCostTotalLabel').html(UpdateProjectTotals());
        }
    });
});

$(".delete-financialrecorddetail").live("click", function() {
    var versionID = $("#ProjectVersionId").val();
    var fundingresourceID = $('#FinancialRecordDetail_ProjectFundingResources').val();

    //Add it visually to the output???
    //alert(versionID + ' ' + fundingresourceID);
    $.ajax({
        type: "POST",
        url: DeleteFinancialRecordDetail,
        data: "projectVersionId=" + versionID
            + "&fundingResourceId=" + fundingresourceID,
        dataType: "json",
        success: function(response, textStatus, XMLHttpRequest) {
            if (response.error == "false") {
                location.reload();
                $('div#resultRecordDetail').html(response.message);
                $('div#resultRecordDetail').addClass('success');
                autoHide(2500);
            }
            else {
                $('div#resultRecordDetail').html(response.message + " Details: " + response.exceptionMessage);
                $('div#resultRecordDetail').addClass('error');
                autoHide(10000);
            }
            window.onbeforeunload = null;
        },
        error: function(response, textStatus, AjaxException) {
            $('div#resultRecordDetail').html(response.statusText);
            $('div#resultRecordDetail').addClass('error');
            autoHide(10000);
        }
    });
    return false;
});

$("#funding-phases .delete-phase").live("click", function() {
    var row = $(this).parent().parent().attr("id");
    
    var id = $(this).attr("id").replace("delete_", "");
    //ProjectFinancialRecordId_FundingIncrementId_FundingResourceId_PhaseId

    var valueSplit
    , projectfinancialrecordid
    , fundingincrementid
    , fundingresourceid
    , phaseid;

    valueSplit = id.split("_");
    projectfinancialrecordid = valueSplit[0];
    fundingincrementid = valueSplit[1];
    fundingresourceid = valueSplit[2];
    phaseid = valueSplit[3];

    $.ajax({
        type: "POST",
        url: DeletePhaseUrl,
        data: "projectFinancialRecordId=" + projectfinancialrecordid
            + "&fundingIncrementId=" + fundingincrementid
            + "&fundingResourceId=" + fundingresourceid
            + "&phaseId=" + phaseid,
        dataType: "json",
        success: function(response, textStatus, XMLHttpRequest) {
            if (response.error == "false") {
                $("#" + row).empty();
            }
            else {

                $('div#resultRecordDetail').html(response.message + " Details: " + response.exceptionMessage);
                $('div#resultRecordDetail').addClass('error');
                autoHide(10000);
            }
            window.onbeforeunload = null;
        },
        error: function(response, textStatus, AjaxException) {

            $('div#resultRecordDetail').html(response.statusText);
            $('div#resultRecordDetail').addClass('error');
            autoHide(10000);
        }
    });
    return false;

});




    
// *** General functions

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

//Added for new Project Financial Detail Summary. -DBD 06/24/2010
//These are all columns in Details except the last one
function UpdateProgrammedTotals() {
    //Total up all values that start with "ProgrammedTotal"
    var sum = 0;
    $("input[id*='hdnProgrammedTotal']").each(function() {
        //add only if the value is number
        if(!isNaN(this.value) && this.value.length!=0) {
            sum += parseFloat(this.value);
        }
    });
    return sum;
}

//Added for new Project Financial Detail Summary. -DBD 06/24/2010
//This is just the last column in Details
function UpdateOutyearTotals() {
    //Total up all values that start with "OutyearTotal"
    var sum = 0;
    $("input[id*='hdnOutyearTotal']").each(function() {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum;
}

function UpdateFederalTotals() {
    //Total up all values that start with "FederalTotal"
    var sum = 0;
    $("input[id*='hdnFederalTotal']").each(function() {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum;
}

function UpdateStateTotals() {
    //Total up all values that start with "StateTotal"
    var sum = 0;
    $("input[id*='hdnStateTotal']").each(function() {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum;
}

function UpdateLocalTotals() {
    //Total up all values that start with "LocalTotal"
    var sum = 0;
    $("input[id*='hdnLocalTotal']").each(function() {
        //add only if the value is number
        if (!isNaN(this.value) && this.value.length != 0) {
            sum += parseFloat(this.value);
        }
    });
    return sum;
}

function UpdateProjectTotals() {
    var sum = 0;
    var previous = parseInt($('#previousTotalTextBox').val().replace(/[,]/g, "")) || parseInt($('#previousTotalTextBox').text() || 0);
    var programmed = parseInt($('#programmedTotalLabel').html().replace(/[,$]/g, ""));
    var outyear = parseInt($('#outyearTotalLabel').html().replace(/[,$]/g, ""));
    var future = parseInt($('#futureTotalTextBox').val().replace(/[,]/g, "")) || parseInt($('#futureTotalTextBox').text() || 0);
    //alert(previous + '+' + programmed + '+' + outyear + '+' + future);
    sum = previous + programmed + outyear + future;
    return sum;
}

function getClearTotal(key) {
    var i1temp = parseInt($('#fundingdetail_' + key + '_i1').html());
    var i2temp = parseInt($('#fundingdetail_' + key + '_i2').html());
    var i3temp = parseInt($('#fundingdetail_' + key + '_i3').html());
    var i4temp = parseInt($('#fundingdetail_' + key + '_i4').html());
    var i5temp = parseInt($('#fundingdetail_' + key + '_i5').html());
    var total_pre = i1temp + i2temp + i3temp + i4temp + i5temp;
    return total_pre
}

function UpdateFinancialRecordTotals(fundingLevel, value, recordid) {
    var leveltotal = parseInt($('#financialrecord_' + recordid + '_' + fundingLevel + 'Total').val().replace(/[,]/g, ""));
    var total = parseInt($('#financialrecord_' + recordid + '_TotalCost').val().replace(/[,]/g, ""));
    $('#financialrecord_' + recordid + '_' + fundingLevel + 'Total').val(leveltotal + value);
    $('#financialrecord_' + recordid + '_' + 'TotalCost').val(total + value);
}

function setConfirmUnload(on) {
    $('#submitForm').removeClass('ui-state-disabled');
    $('#result').html("");
    window.onbeforeunload = (on) ? unloadMessage : null;
}

function unloadMessage() {
    return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
}