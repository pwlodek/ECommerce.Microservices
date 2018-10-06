"use strict";

var Common = {

    sendPost: function (postUrl, postData) {
        $.ajax({
            type: "POST",
            url: postUrl,
            data: postData,
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
    },

    sendProductAdded: function (name) {
        toastr.success(name);
    }
};

toastr.options = {
    "closeButton": false,
    "newestOnTop": false,
    "preventDuplicates": false
};