dojo.provide('dts.widgets.DateTextBox');

dojo.require('dijit.form.DateTextBox');

//A dojo datetextbox that handles sending dates to and recieving dates from asp.net

//.net uses the format \/Date(<ticks>)\/ for json serializing dates
//if it comes from aspajax, it will be the ticks that js expects: ticks since midnight 01/01/1970 where a tick == 1 millisecond
//under some circumstances, it might be number of ticks since midnight 01/01/0001 where a tick == 100 nanoseconds (1 million milliseconds),
//      since that's what system.datetime.ticks means - we're not handling that case right now
//also, sometimes it might come down as a date string

dojo.declare('dts.widgets.DateTextBox', dijit.form.DateTextBox, {

    value: "", // prevent parser from trying to convert to Date object
    hiddenFieldId: null,
    postMixInProperties: function() {
        this.inherited(arguments);

        // Convert value to Date object

        //if it's null, return
        if (!this.value) {
            return;
        }

        //aspajax date serialization
        try {
            if (this.value.indexOf('/Date(') > -1) {
                //extract the ticks from the json object
                var serial = this.value.substring(this.value.indexOf('(') + 1);
                var endChar = (serial.indexOf('-') === -1) ? ')' : '-';
                serial = serial.substring(0, serial.indexOf(endChar));
                //instantiate a date
                this.value = new Date(parseInt(serial));
                //if it's invalid, set value to null
                if (this.value == 'Invalid Date') { this.value = null; }
                return;
            }
        }
        catch (ex) { /*swallow it*/ }

        //short date string
        try {

            //instantiate a date
            this.value = new Date(this.value);
            //if it's invalid, set value to null
            if (this.value == 'Invalid Date') { this.value = null; }
            return;
        }
        catch (ex) { /*swallow it*/ }
    }//,

//    // To write back to the server in proper format, override the serialize method:
//    serialize: function(dateObject, options) {
//        var month = dateObject.getMonth();
//        var day = dateObject.getDay();
//        var year = dateObject.getYear();
//        return year;
//        //return '/Date(' + dateObject.getTime() + ')/';
//    } //,
    //    //override setValue so we can set it with the json date object we get from .net
    //    setValue: function(dateString) {
    //        if (dateString.indexOf('/Date(') > -1 || dateString.indexOf('/date(') > -1) {
    //            //extract the ticks from the json object
    //            var serial = dateString.substring(dateString.indexOf('(') + 1);
    //            var endChar = (serial.indexOf('-') === -1) ? ')' : '-';
    //            serial = serial.substring(0, serial.indexOf(endChar));

    //            arguments[0] = new Date(parseInt(serial))
    //            this.inherited(arguments);
    //        }
    //    }
});