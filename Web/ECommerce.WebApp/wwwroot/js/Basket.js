"use strict";

//= require Common.js

var BasketPage = {

    settings: {
        removeButtons: $(".removeButton"),
        purchaseButton: $(".purchaseButton")
    },

    init: function () {

        this.settings.removeButtons.click(function (e) {
            e.preventDefault();

            // TODO: implement remove functionality
        });

        this.settings.purchaseButton.click(function (e) {

            // Purchase is implemented on form post
        });
    },
};

(function () {

    BasketPage.init();

})();

