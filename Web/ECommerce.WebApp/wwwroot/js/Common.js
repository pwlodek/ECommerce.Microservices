"use strict";

var Common = {

    sendPost: function (postUrl) {
        $.ajax({
            type: "POST",
            url: postUrl,
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            dataType: "json",
            success: function (response) {
            },
            failure: function (response) {
            }
        });
    }

};