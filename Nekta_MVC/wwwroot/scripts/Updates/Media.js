$(document).ready(function () {

    $("#btnAnnualReportLoad").click(function (e) {

        e.preventDefault();

        var button = $(this);

        var pageNumber = parseInt($("#CurrentPage").val()) + 1;
        var pageSize = parseInt($("#PageSize").val());
        var totalCount = parseInt($("#AnnualTotalCount").val());

        var contId = $("#AnnualId").val();

        button.text("Loading...");
        button.css("pointer-events", "none");

        $.ajax({
            url: '/Updates/LoadMoreAnnualArticles',
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
                $("#DivAnnual").append($items);

                // Smooth show
                $items.fadeIn(500);

                $("#CurrentPage").val(pageNumber);

                button.text("View More");
                button.css("pointer-events", "auto");

                // Total loaded items
                var loadedItems =
                    $("#DivAnnual .annual-list-item").length;

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


    $("#btnMediaLoad").click(function (e) {

        e.preventDefault();

        var button = $(this);

        var pageNumber = parseInt($("#MediaCurrentPage").val()) + 1;
        var pageSize = parseInt($("#MediaPageSize").val());
        var totalCount = parseInt($("#MediaTotalCount").val());

        var contId = $("#MediaId").val();

        button.text("Loading...");
        button.css("pointer-events", "none");

        $.ajax({
            url: '/Updates/LoadMoreMediaArticles',
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
                $("#DivMedia").append($items);

                // Smooth show
                $items.fadeIn(500);

                $("#CurrentPage").val(pageNumber);

                button.text("View More");
                button.css("pointer-events", "auto");

                // Total loaded items
                var loadedItems =
                    $("#DivMedia .media-list-item").length;

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

    $("#btnSearch").click(function (e) {

        e.preventDefault();
    
        var button = $(this);
        var buttonSearch = $("#btnAnnualReportLoad");
    
        // Reset paging
        $("#CurrentPage").val(1);
        $("#PageSize").val(10);
    
        var pageNumber = parseInt($("#CurrentPage").val());
        var pageSize = parseInt($("#PageSize").val());
    
        var contId = $("#AnnualId").val();
    
        // Financial Year
        var financialYear = $("#ddlYear").val();
    
        //button.text("Loading...");
        //button.css("pointer-events", "none");
    
        $.ajax({
            url: '/Updates/Get_AnnualReport_ByYear',
            type: 'GET',
            dataType: 'json',
            data: {
                Cont_id: contId,
                FinancialYear: financialYear,
                PageNumber: pageNumber,
                PageSize: pageSize
            },
    
            success: function (response) {
    
              
                if (!response || !response.html || response.html === "") {
    
                    $("#DivAnnual").html("");
    
                    buttonSearch.fadeOut(300);
    
                    return;
                }
    
                // Set total count
                $("#AnnualTotalCount").val(response.totalCount);
    
                // Convert HTML to jQuery
                var $items = $(response.html);
    
                // Hide initially
                $items.hide();
    
                // Clear and append
                $("#DivAnnual").html($items);
    
                // Smooth show
                $items.fadeIn(500);
    
                // Update current page
                $("#CurrentPage").val(pageNumber);
    
              
                buttonSearch.show();
    
                // Total loaded items
                var loadedItems =
                    $("#DivAnnual .annual-list-item").length;
    
                // Hide button if all loaded
                if (loadedItems >= response.totalCount) {
    
                    buttonSearch.fadeOut(300);
                }
            },
    
            error: function (xhr) {

                console.log(xhr.responseText);
            
                alert(xhr.responseText);
            }
        });
    
    });

  





});