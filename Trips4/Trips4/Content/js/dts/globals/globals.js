//////////////////////////////////////
// Global methods that can be used anywhere at anytime
//
//////////////////////////////////////
dojo.provide("dts.globals.globals");

// dojo require any classes used by this class

dojo.declare("dts.globals.globals", null,
{
    constructor: function() {
        dojo.subscribe('handleServiceReturnErrors', this, 'handleServiceReturnErrors');
        dojo.subscribe('handleServiceCallFailure', this, 'handleServiceCallFailure');
    },

    unitedStates: [
        { key: "AL", value: "Alabama" },
        { key: "AK", value: "Alaska" },
        { key: "AZ", value: "Arizona" },
        { key: "AR", value: "Arkansas" },
        { key: "CA", value: "California" },
        { key: "CO", value: "Colorado" },
        { key: "CT", value: "Connecticut" },
        { key: "DC", value: "D.C." },
        { key: "DE", value: "Delaware" },
        { key: "FL", value: "Florida" },
        { key: "GA", value: "Georgia" },
        { key: "HI", value: "Hawaii" },
        { key: "ID", value: "Idaho" },
        { key: "IL", value: "Illinois" },
        { key: "IN", value: "Indiana" },
        { key: "IA", value: "Iowa" },
        { key: "KS", value: "Kansas" },
        { key: "KY", value: "Kentucky" },
        { key: "LA", value: "Louisiana" },
        { key: "ME", value: "Maine" },
        { key: "MD", value: "Maryland" },
        { key: "MA", value: "Massachusetts" },
        { key: "MI", value: "Michigan" },
        { key: "MN", value: "Minnesota" },
        { key: "MS", value: "Mississippi" },
        { key: "MO", value: "Missouri" },
        { key: "MT", value: "Montana" },
        { key: "NE", value: "Nebraska" },
        { key: "NV", value: "Nevada" },
        { key: "NH", value: "New Hampshire" },
        { key: "NJ", value: "New Jersey" },
        { key: "NM", value: "New Mexico" },
        { key: "NY", value: "New York" },
        { key: "NC", value: "North Carolina" },
        { key: "ND", value: "North Dakota" },
        { key: "OH", value: "Ohio" },
        { key: "OK", value: "Oklahoma" },
        { key: "OR", value: "Oregon" },
        { key: "PA", value: "Pennsylvania" },
        { key: "RI", value: "Rhode Island" },
        { key: "SC", value: "South Carolina" },
        { key: "SD", value: "South Dakota" },
        { key: "TN", value: "Tennessee" },
        { key: "TX", value: "Texas" },
        { key: "UT", value: "Utah" },
        { key: "VT", value: "Vermont" },
        { key: "VA", value: "Virginia" },
        { key: "WA", value: "Washington" },
        { key: "WV", value: "West Virginia" },
        { key: "WI", value: "Wisconsin" },
        { key: "WY", value: "Wyoming" }
    ], // traditional order
    unitedStates_customOrder: [
        { key: "KS", value: "Kansas" },
        { key: "MO", value: "Missouri"}//,
    //        { key: "AL", value: "Alabama" },
    //        { key: "AK", value: "Alaska" },
    //        { key: "AZ", value: "Arizona" },
    //        { key: "AR", value: "Arkansas" },
    //        { key: "CA", value: "California" },
    //        { key: "CO", value: "Colorado" },
    //        { key: "CT", value: "Connecticut" },
    //        { key: "DC", value: "D.C." },
    //        { key: "DE", value: "Delaware" },
    //        { key: "FL", value: "Florida" },
    //        { key: "GA", value: "Georgia" },
    //        { key: "HI", value: "Hawaii" },
    //        { key: "ID", value: "Idaho" },
    //        { key: "IL", value: "Illinois" },
    //        { key: "IN", value: "Indiana" },
    //        { key: "IA", value: "Iowa" },

    //        { key: "KY", value: "Kentucky" },
    //        { key: "LA", value: "Louisiana" },
    //        { key: "ME", value: "Maine" },
    //        { key: "MD", value: "Maryland" },
    //        { key: "MA", value: "Massachusetts" },
    //        { key: "MI", value: "Michigan" },
    //        { key: "MN", value: "Minnesota" },
    //        { key: "MS", value: "Mississippi" },

    //        { key: "MT", value: "Montana" },
    //        { key: "NE", value: "Nebraska" },
    //        { key: "NV", value: "Nevada" },
    //        { key: "NH", value: "New Hampshire" },
    //        { key: "NJ", value: "New Jersey" },
    //        { key: "NM", value: "New Mexico" },
    //        { key: "NY", value: "New York" },
    //        { key: "NC", value: "North Carolina" },
    //        { key: "ND", value: "North Dakota" },
    //        { key: "OH", value: "Ohio" },
    //        { key: "OK", value: "Oklahoma" },
    //        { key: "OR", value: "Oregon" },
    //        { key: "PA", value: "Pennsylvania" },
    //        { key: "RI", value: "Rhode Island" },
    //        { key: "SC", value: "South Carolina" },
    //        { key: "SD", value: "South Dakota" },
    //        { key: "TN", value: "Tennessee" },
    //        { key: "TX", value: "Texas" },
    //        { key: "UT", value: "Utah" },
    //        { key: "VT", value: "Vermont" },
    //        { key: "VA", value: "Virginia" },
    //        { key: "WA", value: "Washington" },
    //        { key: "WV", value: "West Virginia" },
    //        { key: "WI", value: "Wisconsin" },
    //        { key: "WY", value: "Wyoming" }
    ], // custom order
    ///////////
    // add/removes a stylesheet to the head of the page
    // _cfg = {nameSpace:'', fileName:'', id: ''}
    ///////////
    addRemoveStyleSheet: function(_cfg /*obj*/, remove /*bool*/) {
        var elem = dojo.byId(_cfg.id);
        if (elem) {
            elem.parentNode.removeChild(elem);
            elem = null;
        }
        if (!remove) {
            var element = document.createElement("link");
            element.rel = "stylesheet";
            element.type = "text/css";
            //element.media = "screen";
            if (_cfg.nameSpace) {
                element.href = dojo.moduleUrl(_cfg.nameSpace, _cfg.fileName);
            } else {
                element.href = _cfg.fileName;
            }
            element.id = _cfg.id;
            document.getElementsByTagName("head")[0].appendChild(element);
        }
    },
    ///////////////
    // load select box values and text
    // cleare existing values and text if any and fil with new values
    // _cfg = {data:[{value:'', key:''}], id:'' || el:object}
    ////////////////
    loadSelectBox: function(_cfg) { // using cfg.el instead of _cfg.id seems to have less than favorable results.
        var selectBox = (_cfg.id) ? dojo.byId(_cfg.id) : (_cfg.el) ? _cfg.el : null;
        var data = _cfg.data;

        if (selectBox) {
            for (var i = selectBox.options.length - 1; i >= 0; i--) {
                selectBox.options[i] = null; // remove the option
            }

            var value, key;
            for (i = 0; i < data.length; i++) {
                value = (data[i].value) ? data[i].value : (data[i].Value) ? data[i].Value : '';
                key = (data[i].key) ? data[i].key : (data[i].Key) ? data[i].Key : '';
                selectBox.options[i] = new Option(value, key);
            }
        } else {
            return false;
        }
    },
    ///////////////
    // load united states into a drop down menu
    // clear existing values and text if any and fill list of the united states
    // _cfg = {id:''} // select box's id
    ////////////////
    loadSelectBox_US: function(_cfg) {
        _cfg = { id: _cfg.id, data: this.unitedStates };
        this.loadSelectBox(_cfg)
    },
    loadSelectBox_US_custom: function(_cfg) { // custom list of US
        _cfg = { id: _cfg.id, data: this.unitedStates_customOrder };
        this.loadSelectBox(_cfg)
    },
    //////////////
    // turn off and on loading icon. Example of usage: triggered by service calls
    // _cfg = {state: true|false, imageDivId:'', classStyleName:''}
    //////////////
    toggleLoadingIcon: function(_cfg) {
        var divId = (_cfg.imageDivId) ? _cfg.imageDivId : 'loadingIcon'; // default id of icon is 'loadingIcon' if not passed into method through _cfg.imageDivId
        var className = (_cfg.classStyleName) ? _cfg.classStyleName : 'hidden'; // default className is 'hidden' if not passed into method through _cfg.classStyleName
        if (!_cfg.state) {
            dojo.addClass(divId, className);
        } else {
            dojo.removeClass(divId, className);
        }
    },

    //--------------------------------//
    // Generic ui methods
    //--------------------------------//

    // update inner html of an element
    // _cfg = {id:'', text: '', elObject:''}
    //------------------------------//
    updateInnerHTML: function(_cfg) {
        if (!_cfg.id) {
            _cfg.elObject.innerHTML = _cfg.text;
        } else {
            dojo.byId(_cfg.id).innerHTML = _cfg.text;
        }
    },

    // return the id and/or value of the chosen value in a select box
    // _cfg = {id: '' || el: element object, returnVal: text || value || both}
    returnSelectBoxValues: function(_cfg) {
        var el = (_cfg.id) ? dojo.byId(_cfg.id) : (_cfg.el) ? _cfg.el : '';
        if (el) {
            for (var i = el.options.length - 1; i >= 0; i--) {
                if (el.options[i].selected) {
                    if (_cfg.returnVal === 'text') {
                        return el.options[i].text;
                    } else if (_cfg.returnVal === 'value') {
                        return el.options[i].value;
                    } else if (_cfg.returnVal === 'both') {
                        return { id: el.options[i].value, text: el.options[i].text }
                    }
                }
            }
        }
        return false;
    },

    // set one of the values to selected in a traditional dropdown select html element
    // _cfg = {id: '/*selectBox Id*/' || el: element object, value:''/*value to select*/, text: ''/*text to select*/}
    setSelectedValueInDropDown: function(_cfg) {
        var el = (_cfg.id) ? dojo.byId(_cfg.id) : (_cfg.el) ? _cfg.el : null;
        if (el) {
            for (var i = el.options.length - 1; i >= 0; i--) {
                if (_cfg.value) {
                    if (el.options[i].value === _cfg.value || parseInt(el.options[i].value) === parseInt(_cfg.value)) {
                        el.options[i].selected = true;
                        break;
                    }
                }
                if (_cfg.text) {
                    if (el.options[i].text === _cfg.text || el.options[i].text.toLowerCase().match(_cfg.text.toLowerCase())) {
                        el.options[i].selected = true;
                        break;
                    }
                }
            }
        }
        else {
            return false;
        }
    },

    //{id: ''| el: element object}
    setSelectIntialValueInDropDown: function(_cfg) {
        var el = (_cfg.id) ? dojo.byId(_cfg.id) : (_cfg.el) ? _cfg.el : null;
        if (el && el.options[0]) {
            el.options[0].selected = true;
        }
    },

    // _cfg = {id: '/*selectBox Id*/' || el: element object, value:'/*value to select*/, checked: /*boolean*/'}
    setValueInCheckbox: function(_cfg) {
        var el = (_cfg.id) ? dojo.byId(_cfg.id) : (_cfg.el) ? _cfg.el : null;
        el.checked = _cfg.checked;
    },
    //--------------------------------//
    // Generic string methods
    //------------------------------//
    str_replace: function(search, replace, subject) {
        // *     example 1: str_replace(' ', '.', 'Kevin van Zonneveld');
        // *     returns 1: 'Kevin.van.Zonneveld'
        // *     example 2: str_replace(['{name}', 'l'], ['hello', 'm'], '{name}, lars');
        // *     returns 2: 'hemmo, mars'

        var f = search, r = replace, s = subject;
        var ra = r instanceof Array, sa = s instanceof Array, f = [].concat(f), r = [].concat(r), i = (s = [].concat(s)).length;

        while (j = 0, i--) {
            if (s[i]) {
                while (s[i] = (s[i] + '').split(f[j]).join(ra ? r[j] || "" : r[0]), ++j in f) { };
            }
        };

        return sa ? s : s[0];
    },

    // right now this is for translating a string passed in from c# (i.e. /Date(1234324333)/). This will and can be improved upon in the future
    translateDate: function(data) {
        data = globalsManager.str_replace('/', '', data);
        data = globalsManager.str_replace('Date(', '', data);
        data = globalsManager.str_replace(')', '', data);
        return new Date(Date(data));
    },

    trim: function(str, charlist) {
        // http://kevin.vanzonneveld.net
        // +   original by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +   improved by: mdsjack (http://www.mdsjack.bo.it)
        // +   improved by: Alexander Ermolaev (http://snippets.dzone.com/user/AlexanderErmolaev)
        // +      input by: Erkekjetter
        // +   improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +      input by: DxGx
        // +   improved by: Steven Levithan (http://blog.stevenlevithan.com)
        // +    tweaked by: Jack
        // +   bugfixed by: Onno Marsman
        // *     example 1: trim('    Kevin van Zonneveld    ');
        // *     returns 1: 'Kevin van Zonneveld'
        // *     example 2: trim('Hello World', 'Hdle');
        // *     returns 2: 'o Wor'
        // *     example 3: trim(16, 1);
        // *     returns 3: 6

        var whitespace, l = 0, i = 0;
        str += '';

        if (!charlist) {
            // default list
            whitespace = " \n\r\t\f\x0b\xa0\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u200b\u2028\u2029\u3000";
        } else {
            // preg_quote custom list
            charlist += '';
            whitespace = charlist.replace(/([\[\]\(\)\.\?\/\*\{\}\+\$\^\:])/g, '\$1');
        }

        l = str.length;
        for (i = 0; i < l; i++) {
            if (whitespace.indexOf(str.charAt(i)) === -1) {
                str = str.substring(i);
                break;
            }
        }

        l = str.length;
        for (i = l - 1; i >= 0; i--) {
            if (whitespace.indexOf(str.charAt(i)) === -1) {
                str = str.substring(0, i + 1);
                break;
            }
        }

        return whitespace.indexOf(str.charAt(0)) === -1 ? str : '';
    },

    // url pattern test
    urlPattern: /^\s*(https?):\/\/([\-\w\.]+)+(:\d+)?(\/([\w\/_\.\-]*(\?\S+)?)?)?\s*$/,
    // Although not implemented this maybe a better solution
    urlPattern2: /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/,
    isUrl: function(/*String*/value) {
        if (value) {
            value = value + ""; // ensure it's a string
            return this.urlPattern.test(value);
        }
        return false;
    },

    explode: function(delimiter, string, limit) {
        // http://kevin.vanzonneveld.net
        // +     original by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +     improved by: kenneth
        // +     improved by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +     improved by: d3x
        // +     bugfixed by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // *     example 1: explode(' ', 'Kevin van Zonneveld');
        // *     returns 1: {0: 'Kevin', 1: 'van', 2: 'Zonneveld'}
        // *     example 2: explode('=', 'a=bc=d', 2);
        // *     returns 2: ['a', 'bc=d']

        var emptyArray = { 0: '' };

        // third argument is not required
        if (arguments.length < 2
            || typeof arguments[0] == 'undefined'
            || typeof arguments[1] == 'undefined') {
            return null;
        }

        if (delimiter === ''
            || delimiter === false
            || delimiter === null) {
            return false;
        }

        if (typeof delimiter == 'function'
            || typeof delimiter == 'object'
            || typeof string == 'function'
            || typeof string == 'object') {
            return emptyArray;
        }

        if (delimiter === true) {
            delimiter = '1';
        }

        if (!limit) {
            return string.toString().split(delimiter.toString());
        } else {
            // support for limit argument
            var splitted = string.toString().split(delimiter.toString());
            var partA = splitted.splice(0, limit - 1);
            var partB = splitted.join(delimiter.toString());
            partA.push(partB);
            return partA;
        }
    },

    // low level check file ending to make sure this is the type of file you are looking for
    // _cfg = {fileType: '/*sting*/', fileLocation:'/* string (example:test.html)*/'}
    validateUploadedFile: function(_cfg) {
        var fileName = _cfg.fileName;
        var validFileType = null;
        validFileType = this.checkFileType({ fileName: fileName, type: _cfg.fileType.toLowerCase() })
        if (fileName && validFileType) {
            return fileName;
        }
        return false;
    },
    // _cfg = {fileName:'/*string*/', type:'/*string*/'}
    checkFileType: function(_cfg) {
        var fileParts = globalsManager.explode('.', _cfg.fileName);
        if (fileParts[fileParts.length - 1].toLowerCase() === _cfg.type) {
            return true;
        }
        return false;
    },
    decodeString: function(string) {
        if (string.match('&#39;')) {
            string = this.str_replace(['\\&#39;', '&#39;'], "'", string);
        }
        string = decodeURIComponent(string);
        //        if (string.match('"')) {
        //            string = this.str_replace('"', '\\"', string);
        //        }
        return string;
    },
    escapeSingleQuotes: function(string) {
        if (string.match("'")) {
            string = this.str_replace("'", "\\'", string);
        } else if (string.match("&#39;")) {
            string = this.str_replace(["&#39;", "\\&#39;"], "\\'", string);
        }
        if (string.match("\"")) {
            string = this.str_replace('"', '\\"', string);
        } else if (string.match('%22')) {
            string = this.str_replace('"', '\\"', string);
        }
        return string;
    },

    asciiToCharacters: function(ascii) {
        var adjustedAscii = ascii.replace(/&#(\d+);/g, function(m, n) { return String.fromCharCode(n); })
        return adjustedAscii;
    },
    htmlentities: function(string, quote_style) {
        // http://kevin.vanzonneveld.net
        // +   original by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +    revised by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +   improved by: nobbler
        // +    tweaked by: Jack
        // +   bugfixed by: Onno Marsman
        // +    revised by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // -    depends on: get_html_translation_table
        // *     example 1: htmlentities('Kevin & van Zonneveld');
        // *     returns 1: 'Kevin &amp; van Zonneveld'
        // *     example 2: htmlentities("foo'bar","ENT_QUOTES");
        // *     returns 2: 'foo&#039;bar'

        var histogram = {}, symbol = '', tmp_str = '', entity = '';
        tmp_str = string.toString();

        if (false === (histogram = this.get_html_translation_table('HTML_ENTITIES', quote_style))) {
            return false;
        }

        for (symbol in histogram) {
            entity = histogram[symbol];
            tmp_str = tmp_str.split(symbol).join(entity);
        }

        return tmp_str;
    },
    get_html_translation_table: function(table, quote_style) {
        // http://kevin.vanzonneveld.net
        // +   original by: Philip Peterson
        // +    revised by: Kevin van Zonneveld (http://kevin.vanzonneveld.net)
        // +   bugfixed by: noname
        // +   bugfixed by: Alex
        // +   bugfixed by: Marco
        // +   bugfixed by: madipta
        // %          note: It has been decided that we're not going to add global
        // %          note: dependencies to php.js. Meaning the constants are not
        // %          note: real constants, but strings instead. integers are also supported if someone
        // %          note: chooses to create the constants themselves.
        // %          note: Table from http://www.the-art-of-web.com/html/character-codes/
        // *     example 1: get_html_translation_table('HTML_SPECIALCHARS');
        // *     returns 1: {'"': '&quot;', '&': '&amp;', '<': '&lt;', '>': '&gt;'}

        var entities = {}, histogram = {}, decimal = 0, symbol = '';
        var constMappingTable = {}, constMappingQuoteStyle = {};
        var useTable = {}, useQuoteStyle = {};

        useTable = (table ? table.toUpperCase() : 'HTML_SPECIALCHARS');
        useQuoteStyle = (quote_style ? quote_style.toUpperCase() : 'ENT_COMPAT');

        // Translate arguments
        constMappingTable[0] = 'HTML_SPECIALCHARS';
        constMappingTable[1] = 'HTML_ENTITIES';
        constMappingQuoteStyle[0] = 'ENT_NOQUOTES';
        constMappingQuoteStyle[2] = 'ENT_COMPAT';
        constMappingQuoteStyle[3] = 'ENT_QUOTES';

        // Map numbers to strings for compatibilty with PHP constants
        if (!isNaN(useTable)) {
            useTable = constMappingTable[useTable];
        }
        if (!isNaN(useQuoteStyle)) {
            useQuoteStyle = constMappingQuoteStyle[useQuoteStyle];
        }

        if (useTable == 'HTML_SPECIALCHARS') {
            // ascii decimals for better compatibility
            entities['38'] = '&amp;';
            if (useQuoteStyle != 'ENT_NOQUOTES') {
                entities['34'] = '&quot;';
            }
            if (useQuoteStyle == 'ENT_QUOTES') {
                entities['39'] = '&#039;';
            }
            entities['60'] = '&lt;';
            entities['62'] = '&gt;';
        } else if (useTable == 'HTML_ENTITIES') {
            // ascii decimals for better compatibility
            entities['38'] = '&amp;';
            if (useQuoteStyle != 'ENT_NOQUOTES') {
                entities['34'] = '&quot;';
            }
            if (useQuoteStyle == 'ENT_QUOTES') {
                entities['39'] = '&#039;';
            }
            entities['60'] = '&lt;';
            entities['62'] = '&gt;';
            entities['160'] = '&nbsp;';
            entities['161'] = '&iexcl;';
            entities['162'] = '&cent;';
            entities['163'] = '&pound;';
            entities['164'] = '&curren;';
            entities['165'] = '&yen;';
            entities['166'] = '&brvbar;';
            entities['167'] = '&sect;';
            entities['168'] = '&uml;';
            entities['169'] = '&copy;';
            entities['170'] = '&ordf;';
            entities['171'] = '&laquo;';
            entities['172'] = '&not;';
            entities['173'] = '&shy;';
            entities['174'] = '&reg;';
            entities['175'] = '&macr;';
            entities['176'] = '&deg;';
            entities['177'] = '&plusmn;';
            entities['178'] = '&sup2;';
            entities['179'] = '&sup3;';
            entities['180'] = '&acute;';
            entities['181'] = '&micro;';
            entities['182'] = '&para;';
            entities['183'] = '&middot;';
            entities['184'] = '&cedil;';
            entities['185'] = '&sup1;';
            entities['186'] = '&ordm;';
            entities['187'] = '&raquo;';
            entities['188'] = '&frac14;';
            entities['189'] = '&frac12;';
            entities['190'] = '&frac34;';
            entities['191'] = '&iquest;';
            entities['192'] = '&Agrave;';
            entities['193'] = '&Aacute;';
            entities['194'] = '&Acirc;';
            entities['195'] = '&Atilde;';
            entities['196'] = '&Auml;';
            entities['197'] = '&Aring;';
            entities['198'] = '&AElig;';
            entities['199'] = '&Ccedil;';
            entities['200'] = '&Egrave;';
            entities['201'] = '&Eacute;';
            entities['202'] = '&Ecirc;';
            entities['203'] = '&Euml;';
            entities['204'] = '&Igrave;';
            entities['205'] = '&Iacute;';
            entities['206'] = '&Icirc;';
            entities['207'] = '&Iuml;';
            entities['208'] = '&ETH;';
            entities['209'] = '&Ntilde;';
            entities['210'] = '&Ograve;';
            entities['211'] = '&Oacute;';
            entities['212'] = '&Ocirc;';
            entities['213'] = '&Otilde;';
            entities['214'] = '&Ouml;';
            entities['215'] = '&times;';
            entities['216'] = '&Oslash;';
            entities['217'] = '&Ugrave;';
            entities['218'] = '&Uacute;';
            entities['219'] = '&Ucirc;';
            entities['220'] = '&Uuml;';
            entities['221'] = '&Yacute;';
            entities['222'] = '&THORN;';
            entities['223'] = '&szlig;';
            entities['224'] = '&agrave;';
            entities['225'] = '&aacute;';
            entities['226'] = '&acirc;';
            entities['227'] = '&atilde;';
            entities['228'] = '&auml;';
            entities['229'] = '&aring;';
            entities['230'] = '&aelig;';
            entities['231'] = '&ccedil;';
            entities['232'] = '&egrave;';
            entities['233'] = '&eacute;';
            entities['234'] = '&ecirc;';
            entities['235'] = '&euml;';
            entities['236'] = '&igrave;';
            entities['237'] = '&iacute;';
            entities['238'] = '&icirc;';
            entities['239'] = '&iuml;';
            entities['240'] = '&eth;';
            entities['241'] = '&ntilde;';
            entities['242'] = '&ograve;';
            entities['243'] = '&oacute;';
            entities['244'] = '&ocirc;';
            entities['245'] = '&otilde;';
            entities['246'] = '&ouml;';
            entities['247'] = '&divide;';
            entities['248'] = '&oslash;';
            entities['249'] = '&ugrave;';
            entities['250'] = '&uacute;';
            entities['251'] = '&ucirc;';
            entities['252'] = '&uuml;';
            entities['253'] = '&yacute;';
            entities['254'] = '&thorn;';
            entities['255'] = '&yuml;';
        } else {
            throw Error("Table: " + useTable + ' not supported');
            return false;
        }

        // ascii decimals to real symbols
        for (decimal in entities) {
            symbol = String.fromCharCode(decimal);
            histogram[symbol] = entities[decimal];
        }

        return histogram;
    },
    //--------------------------------//
    // Generic number methods
    //------------------------------//
    isNumber: function(variable) {
        var x = typeof (variable);
        if (x == 'number') {
            return true;
        } else {
            return false;
        }
    },

    //--------------------------------//
    // Generic Object methods
    //------------------------------//
    // TODO: make sure this works on an Object or on an array of objects
    copyObject: function(object, excludeFunctions /* optional */) {
        var newObj = (object instanceof Array) ? [] : {};
        for (i in object) {
            if (i == 'clone') continue;
            if (object[i] && typeof object[i] == "object") {
                newObj[i] = this.copyObject(object[i]); //object[i].clone(); 
            } else newObj[i] = object[i]
        } return newObj;

    },

    //--------------------------------//
    // Generic Sand Box methods
    //------------------------------//
    getLocationData: function() {
        return location;
    },


    //--------------------------------//
    // App Specfic methods
    //------------------------------//
    getCountOfManualIncidents: function() {
        var layers;
        var count = 0;
        var widgets = configData.ui.widgets;
        for (var i = 0; i < widgets.length; i++) {
            if (widgets[i].type === 'layers') {
                layers = widgets[i].config.layers;
                break;
            }
        }
        for (i = 0; i < layers.length; i++) {
            if (layers[i].submittedData) {
                count++;
            }
        }
        return count;
    },
    
     translateDate: function(date) {
        if (date) {
            date = globalsManager.str_replace('/', '', date);
            date = globalsManager.str_replace('Date(', '', date);
            date = globalsManager.str_replace(')', '', date);

            var shortDate = new Date(parseInt(date)).toDateString();
            var titleDate = new Date(parseInt(date));

            return "<div title='" + titleDate + "'>" + shortDate + "</div>";
        } else {
            return "<div title='" + titleDate + "'></div>";
        }
    },

    handleServiceReturnErrors: function(errorMessage, errorSource) {
        console.log("GlobalsManager::handleServiceReturnErrors");
        alert(errorSource + ' :: ' + errorMessage);
    },
    handleServiceCallFailure: function(response, io_args, errorSource) { // this is to process CallBackError reponse
        console.log("GlobalsManager::handleServiceCallFailure");
        console.error(errorSource + ' ::Message: ' + response.message + ', fileName: ' + response.fileName + ", lineNumber: " + response.lineNumber);
    }
});