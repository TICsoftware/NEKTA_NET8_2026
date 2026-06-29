$(document).ready(function () {

    $("#btnLoadmore").click(function (e) {

        e.preventDefault();

        var button = $(this);

        var pageNumber = parseInt($("#CurrentPage").val()) + 1;
        var pageSize = parseInt($("#PageSize").val());
        var totalCount = parseInt($("#TotalCount").val());

        var contId = $("#ContentId").val();

        button.text("Loading...");
        button.css("pointer-events", "none");

        $.ajax({
            url: '/About/LoadMorePartnersCollaborators',
            type: 'GET',
            data: {
                Cont_id: contId,
                PageNumber: pageNumber,
                PageSize: pageSize
            },
            success: function (response) {

                if (response.trim() == "") {

                    button.fadeOut(300);
                    return;
                }

                // Convert response to jQuery object
                var $items = $(response);

                // Hide initially
                $items.css("display", "none");

                // Append items
                $("#partnersContainer").append($items);

                // Smooth show
                $items.fadeIn(500);

                $("#CurrentPage").val(pageNumber);

                button.text("View More");
                button.css("pointer-events", "auto");

                // Total loaded items
                var loadedItems =
                    $("#partnersContainer .reveal-item").length;

                // Hide button when completed
                if (loadedItems >= totalCount) {

                    button.fadeOut(300);
                }
            },
            error: function () {

                button.text("View More");
                button.css("pointer-events", "auto");

                alert("Something went wrong.");
            }
        });

    });

});