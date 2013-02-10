//*** RTP Search Page

$(document).ready(function() {
    //*** TIP Financial Funding code [Methods: Update]
    if ($('#ProjectSearchModel_RtpYearID option:selected').val() != "") {
        getScenarios($('#ProjectSearchModel_RtpYearID option:selected'));
        $('#ProjectSearchModel_RtpYear').val($('#ProjectSearchModel_RtpYearID option:selected').text());
    }
    //Update a Financial Record
    $('#ProjectSearchModel_RtpYearID').change(function() {

        var selected = $('#ProjectSearchModel_RtpYearID option:selected');
        if (selected.val() != "") {
            getScenarios(selected);
        }
        return false;
    });
});

function getScenarios(selected) {
    $.ajax({
        type: "POST",
        url: GetPlanScenarios,
        data: "planYearId=" + parseInt(selected.val()),
        dataType: "json",
        success: function(response, textStatus, XMLHttpRequest) {
            $('#ProjectSearchModel_NetworkID').clearSelect().addItems(response);
            //$('#scenario-container').show();
        },
        error: function(response, textStatus, AjaxException) {
            alert("error");
        }
    });
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
    });
};

function reset_form_elements(formElements) {
    $(formElements).find(':input').each(function() {
        switch (this.type) {
            case 'password':
            case 'select-multiple':
            case 'select-one':
            case 'text':
            case 'textarea':
                $(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
    });
} 