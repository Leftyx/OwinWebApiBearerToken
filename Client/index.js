(function (oAuth, $, undefined) {
    'use strict';

    oAuth.Name = 'Owin';
    // oAuth.AuthorizationServer = 'http://localhost.fiddler:8686/oauth/Token';
    oAuth.AuthorizationServer = 'http://localhost:8686/oauth/Token';
    oAuth.FetchCustomerUrl = 'http://localhost:8686/api/customer';
    oAuth.Token = null;

    oAuth.init = function () {

    };

    oAuth.render = function () {
        // alert(this.Name);
    };

    oAuth.requestToken = function () {
        
        var clientId = "MyApp";
        var clientSecret = "MySecret";
        
        var authorizationBasic = $.base64.btoa(clientId + ':' + clientSecret);

        $.ajax({
            type: 'POST',
            url: oAuth.AuthorizationServer,
            data: { username: 'John', password: 'Smith', grant_type: 'password' },
            dataType: "json",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            xhrFields: {
               withCredentials: true
            },
            // crossDomain: true,
            headers: {
                'Authorization': 'Basic ' + authorizationBasic
            },
            beforeSend: function (xhr) {
            },
            success: function (result) {
                $('#name').val(result.name);
                $('#surname').val(result.surname);

                oAuth.Token = result.access_token;
            },
            complete: function (jqXHR, textStatus) {
                // alert(textStatus);
            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    };

    oAuth.FetchCustomer = function () {

        $.ajax({
            type: 'GET',
            url: oAuth.FetchCustomerUrl + '/1',
            data: { },
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + oAuth.Token
            },
            beforeSend: function (xhr) {
            },
            success: function (result) {
                alert(result.FirstName);
                alert(result.LastName);
            },
            complete: function (jqXHR, textStatus) {
                // alert(textStatus);
            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    };

}(window.oAuth = window.oAuth || {}, jQuery));