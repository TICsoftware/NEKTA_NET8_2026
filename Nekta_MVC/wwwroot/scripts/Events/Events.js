$(document).ready(function () {

    function loadEvents() {

        var eventType = $("#ddlEventType").val() || 0;
        var eventMode = $("#ddlEventMode").val() || 0;
        var dateSearch = document.getElementById("txtSearchDate").value;
    
       
        $("#DivPastEvents").html("");
    
        // Create request object
        var requestData = {
            EventTypeId: eventType,
            EventModeId: eventMode
        };
    
        // Add date only if selected
        if (dateSearch && dateSearch.trim() !== "") {
            requestData.DateSearch = dateSearch;
        }
    
        $.ajax({
            url: '/Events/GetEventsBySearch',
            type: 'GET',
            data: requestData,
    
            success: function (response) {
    
                if (!response || response === "") {
   
                    $("#DivPastEvents").html(
                        "<div class='text-center'>No records found.</div>"
                    );
    
                    return;
                }
   
                //$("#DivPastEvents").html(response);
                $("#DivPastEvents").hide().html(response).fadeIn(500);
            },
    
            error: function (xhr, status, error) {
    
                console.log("Status:", status);
                console.log("Error:", error);
                console.log("Response:", xhr.responseText);
    
                $("#DivPastEvents").html(
                    "<div class='text-center text-danger'>Something went wrong.</div>"
                );
            }
        });
    }

    // Dropdown Change
    $("#ddlEventType, #ddlEventMode").change(function () {
        loadEvents();
    });

    // Date Change
    // $("#txtDateSearch").change(function () {
    //     alert('hi');
    //     loadEvents();
    // });

    $("#txtSearchDate").change(function () {
       
        loadEvents();
    });

    $("#btnExpand").click(function (e) {
        e.preventDefault();
    });

});


$(document).on("click", "#btnexpand", function () {
    var eventId = $(this).data('id');

    $.ajax({
        url: '/Events/GetEventsExpand',
        type: 'GET',
        data: {
            EventId: eventId
        },
        success: function (response) {
    
            if (!response.success) {
                alert(response.message);
                return;
            }
    
            $("#txtexpandIntro").html(
                (response.intro || '').replace(/<\/?p[^>]*>/g, '')
            );
            
            $("#txtexpandContent").html(
                (response.content || '').replace(/<\/?p[^>]*>/g, '')
            );
        },
        error: function (xhr) {
            console.log(xhr.responseText);
            alert("Something went wrong.");
        }
    });

});