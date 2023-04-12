﻿var search = {
    init: function () {
        search.registerEvent();
    },
    registerEvent: function () {    
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Vehicle/ListName",
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearch").val(ui.item.label);

                return false;
            }
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<div style='padding-block:20px;'>" + item.label + "</div>")
                    .appendTo(ul);
            };
    }
}
search.init();