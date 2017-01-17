/**
 * footer-reveal.js
 * 
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2014 Iain Andrew
 * https://github.com/IainAndrew
 */

(function($) {

  $.fn.footerReveal = function(options) {

    var $this = $(this),
        $prev = $this.prev(),
        $win = $(window),

        defaults = $.extend ({
          shadow : true,
          shadowOpacity: 0.8,
          zIndex : -100
        }, options ),

        settings = $.extend(true, {}, defaults, options);

    if ($this.outerHeight() < $win.outerHeight()) {
      $this.css({
        'z-index' : defaults.zIndex || options.zIndex,
        position : 'fixed',
        bottom : 0
      });

      if (defaults.shadow) {
        $prev.css ({
          '-moz-box-shadow' : '0 20px 30px -20px rgba(0,0,0,' + defaults.shadowOpacity || options.shadowOpacity + ')',
          '-webkit-box-shadow' : '0 20px 30px -20px rgba(0,0,0,' + defaults.shadowOpacity || options.shadowOpacity +')',
          'box-shadow' : '0 20px 30px -20px rgba(0,0,0,' + defaults.shadowOpacity || options.shadowOpacity + ')'
        });
      }

      var footerRevealResize = function() {
        $this.css({
          'width' : $prev.outerWidth()
        });
        $prev.css({
          'margin-bottom' : $this.outerHeight()
        });
      };

      $win.on('load resize', function() {
          footerRevealResize();
      });
    }

    return this;

  };

}) (jQuery);