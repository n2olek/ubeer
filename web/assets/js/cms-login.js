(function ($) {
    "use strict"; // Start of use strict

    var cookieValue = $.cookie("token");
    if (cookieValue != null) //token invalid
        location.href = "dashboard.html";

    $('#login-submit').click(function (event) {
        event.preventDefault();

        var api = new API(appConfig.API_URL, "");
        api.login($('#user').val(), $('#pass').val(), loginCompleted);
    });

    $('#pass').keypress(function (e) {
        if (e.which == 13) {
            $('#login-submit').click();
        }
    });

    var loginCompleted = function (response) {
        $.removeCookie('token', { path: '/' });
        $.cookie("token", response.message, { expires: 5, path: '/' });
        location.href = "dashboard.html";
    }

})(jQuery); // End of use strict
