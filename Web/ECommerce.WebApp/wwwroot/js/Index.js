"use strict";

//= require Common.js

var IndexPage = {

    settings: {
        buyButtons: $(".buyButton")
    },

    init: function () {


        this.settings.buyButtons.click(function (e) {
            e.preventDefault();

            var data = $(this).val();

            Common.sendPost("/api/basket", data);
        });
    },
};

(function () {

    IndexPage.init();

})();

