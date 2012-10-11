// drop down box helpers

$.fn.clearSelect = function() {
    return this.each(function() {
        if (this.tagName == 'SELECT')
            this.options.length = 0;
    });
}

$.fn.fillSelect = function(data, options) {
    var defaults = {
        defaultOptionText: '-- Select --',
        defaultOptionValue: 0
    };
    var options = $.extend(defaults, options);

    return this.clearSelect().each(function() {
        if (this.tagName == 'SELECT') {
            var o = options;
            var defaultOption = new Option(o.defaultOptionText, o.defaultOptionValue);
            var dropdownList = this;
            if ($.browser.msie) {
                dropdownList.add(defaultOption);
            }
            else {
                dropdownList.add(defaultOption, null);
            }

            $.each(data, function(index, optionData) {
                var option = new Option(optionData.Text, optionData.Value);
                if ($.browser.msie) {
                    dropdownList.add(option);
                }
                else {
                    dropdownList.add(option, null);
                }
            });
        }
    });
}