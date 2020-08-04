!(function (s) {
    "use strict";
    s(".sidebar-toggle").on("click", function (e) {
        s("body").toggleClass("sidebar-toggled"), s(".sidebar").toggleClass("toggled"), s(".sidebar").hasClass("toggled") && s(".sidebar .collapse").collapse("hide");
    })
})(jQuery);

