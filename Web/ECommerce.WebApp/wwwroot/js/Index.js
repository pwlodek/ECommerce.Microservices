"use strict";

//= require Common.js

var IndexPage = {

    settings: {
        buyButtons: $(".buyButton")
    },

    init: function () {


        this.settings.buyButtons.click(function (e) {
            e.preventDefault();

            var x = "/api/basket?id=" + $(this).val();

            Common.sendPost(x);
        });
    },
};

(function () {

    IndexPage.init();

})();

