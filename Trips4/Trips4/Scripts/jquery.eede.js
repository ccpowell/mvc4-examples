/**
 *******************************************************************************
 * EEDE (Eniac Essentials Detail Expander)                                     *
 * version 1.2                                                                 *
 * better check for cookie support                                             *
 *                                                                             *
 * version 1.1                                                                 *
 * cookie support added                                                        *
 *                                                                             *
 * version 1.0                                                                 *
 * Initial version                                                             *
 *                                                                             *
 * This jQuery plugin is meant to show and hide detail sections in your page,  *
 * using jQuery. It aims to be extremely flexible and simple in use at the     *
 * same time.                                                                  *
 *                                                                             *
 * How it works                                                                *
 * - Create any dom element structure in you page on which the user will click *
 *   to show an hidden detail-section.                                         *
 * - Add an attribute called 'detail' to the main element of the clickable     *
 *   section.                                                                  *
 * - The value of this attribute must be a css-style jQuery selector that      *
 *   references the element(s) to show and hide.                               *
 * - instantiate the detail expander by calling the constructor for the        *
 *   clickable element.                                                        *
 *                                                                             *
 * Extra features                                                              *
 * - the constructor will also accept an option called 'speed'. Any speed      *
 *   indicator, accepted by the jQuery animations is valid. (default=slow)     *
 *   e.g. "slow", "fast", 0, 1.4                                               *
 * - Conditional presentation. If the clickable element contains sub elements  *
 *   with class='collapsed', it will then only show this element if the        *
 *   detail-section is collapse. If a sub element with class='expanded' is     *
 *   somewhere within the clickable area, then it will only be shown if the    *
 *   detail section is visble as well. When you combine these the classes, in  *
 *   the right way, you'll be able to show [+] and [-], depending on the       *
 *   situation.                                                                *
 * - Cookie support. If you include jquery.cookie.js in your page and provide  *
 *   the clickable element with a unique attribute called 'cookie'. Then it    *
 *   will be used as identifying key to persist the state of mthe detail       *
 *   section.
 *                                                                             *
 * Example                                                                     *
 * <script>                                                                    *
 *   $(document).ready(function(){                                             *
 *     $("h3").eede({speed:"fast"});                                           *
 *   });                                                                       *
 * </script>                                                                   *
 * <h3 detail="#contactinfo">                                                  *
 *   Contact Information <span class="collapsed">[+]</span>                    *
 *                       <span class="expanded">[-]</span>                     *
 * </h3>                                                                       *
 * <div id="contactinfo">                                                      *
 *   Eniac Essentials<br>                                                      *
 *   Hogeweg 41<br>                                                            *
 *   5301 LJ Zaltbommel<br>                                                    *
 *   T. +31 (0)418 57 07 00<br>                                                *
 *   F. +31 (0)418 57 07 60                                                    *
 * </div>                                                                      *
 *******************************************************************************
 * EEDE is currently licenced under the GPL v2.                                *
 *            (http://www.gnu.org/licenses/gpl-2.0.html)                       *
 *                                                                             *
 * If this doesn't fit your needs, please contact us for an alternative:       *
 *                                                                             *
 *   Eniac Essentials<br>                                                      *
 *   Hogeweg 41<br>                                                            *
 *   5301 LJ Zaltbommel<br>                                                    *
 *   T. +31 (0)418 57 07 00<br>                                                *
 *   F. +31 (0)418 57 07 60                                                    *
 *******************************************************************************
 */
jQuery.fn.eede = function(options) {
	var defaults = {
	    speed: 'slow'
	},

	opts = jQuery.extend(defaults, options);
	
	var getCookie = function(key) {
		if (jQuery.cookie)
			return jQuery.cookie(key);
		return false;
	};
	var setCookie = function(key, value) {
		if (jQuery.cookie)
			jQuery.cookie(key, value)
	};
	
	return this.each(function(){
		var selector = jQuery(this).attr('detail'),
			cookiename = jQuery(this).attr('cookie'),
			collapse = jQuery(selector),
			doshow = false,
			futurestate,
			clickable;
		if (cookiename)	doshow = (getCookie(cookiename) == 'true');
		if (!doshow) collapse.hide();
		futurestate = doshow;
		jQuery(this)
			.css('cursor','pointer')
			.css('cursor','hand')
			.click(function() {
				futurestate = !futurestate;
				collapse.toggle(opts.speed);
				clickable = jQuery(this);
				clickable.children('.collapsed').toggle();
				clickable.children('.expanded').toggle();
				setCookie(cookiename, futurestate);
			});
		if (!doshow) {
			jQuery(this).children('.expanded').hide()
		} else {
			jQuery(this).children('.collapsed').hide();
		}
	});
};