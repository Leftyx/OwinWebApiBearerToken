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

    oAuth.requestTokenVanilla = function () {
        var clientId = "MyApp";
        var clientSecret = "MySecret";

        var authorizationBasic = $.base64.btoa(clientId + ':' + clientSecret);

        var request = new XMLHttpRequest();
        request.open('POST', oAuth.AuthorizationServer, true);
        request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        request.setRequestHeader('Authorization', 'Basic ' + authorizationBasic);
        request.setRequestHeader('Accept', 'application/json');
        request.send("username=John&password=Smith&grant_type=password");

        request.onreadystatechange = function () {
            if (request.readyState == 4) {
                var result = $.parseJSON(request.responseText);
                if (result)
                {
                    $('#response-server').text(request.responseText);
                    $('#name').val(result.name);
                    $('#surname').val(result.surname);

                    oAuth.Token = result.access_token;

                    $('#token').val(result.access_token);
                }
            }
        };
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
            //beforeSend: function (xhr) {
            //},
            success: function (result) {

                $('#response-server').text(JSON.stringify(result));

                $('#name').val(result.name);
                $('#surname').val(result.surname);

                oAuth.Token = result.access_token;

                $('#token').val(result.access_token);
            },
            //complete: function (jqXHR, textStatus) {
            //},
            error: function (req, status, error) {
                if (req.responseJSON)
                {
                    alert(req.responseJSON.error_description);
                }
            }
        });
    };

    oAuth.FetchCustomer = function () {

        $.ajax({
            type: 'GET',
            url: oAuth.FetchCustomerUrl + '/1001',
            // data: { extended: 'something' },
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + oAuth.Token
            },
            //beforeSend: function (xhr) {
            //},
            success: function (result) {
                $('#result').text(JSON.stringify(result));
            },
            //complete: function (jqXHR, textStatus) {
            //},
            error: function (req, status, error) {
                if (req.responseJSON) {
                    alert(req.responseJSON.error_description || req.responseJSON.Message);
                }
            }
        });
    };

}(window.oAuth = window.oAuth || {}, jQuery));