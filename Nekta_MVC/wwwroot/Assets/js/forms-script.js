document.addEventListener("DOMContentLoaded", function () {
    if (!$.parseJSON) {
        $.parseJSON = JSON.parse;
    }

    $(document).on("click", '[data-target="#secondOpinionPopup"]', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        $.get('/Connect/MedicalForm', { Id: id }, function (res) {
            $("#divsecondopinionform").html(res);
            // rebind validation
            $.validator.unobtrusive.parse("#frmMedicalenquiry");
        });
    });

    $("#divsecondopinionform").on("click scroll", function (e) {

        // if ($("#showMessage").length > 0) {
        //     //alert("successMessage detected") 
        //     const successMessage = document.getElementById("showMessage");
        //     successMessage.classList.remove("hidden");
        //     successMessage.focus();
        //     //alert(response.success);
        //     //document.getElementById("#contactForm").reset();
        //     setTimeout(() => {
        //         successMessage.classList.add("hidden");               
        //         $("#successMessage").remove();
        //     }, 4000);
        // }
    });



    $(document).on("submit", "#frmMedicalenquiry", function (e) {
        e.preventDefault();
        var token = document.getElementById("frmMedicalenquiry").querySelector('.g-recaptcha-response').value;
        // var token = grecaptcha.getResponse(); 

        var formData = new FormData(this);
        formData.set('g-recaptcha-response', token);
        document.getElementById("submitformBtn").classList.add("is-processing");
        document.getElementById("submitformBtn").appendChild(document.querySelector(".btn-loader"))
        $.ajax({
            url: "/Connect/Medicalenquiry",
            type: "POST",
            data: formData,
            success: function (response) {
                $("#divsecondopinionform").html(response);
                $.validator.unobtrusive.parse("#frmMedicalenquiry");


                if ($("#strstatus_subform").val() == "true") {

                    const successMessage = document.getElementById("showMessage");
                    successMessage.classList.remove("hidden");
                    successMessage.focus();
                    //alert(response.success);
                    //document.getElementById("#contactForm").reset();
                    setTimeout(() => {
                        successMessage.classList.add("hidden");
                        $("#successMessage").remove();
                    }, 4000);
                }
            },
            error: function (xhr, ajaxOptions, response) {
                alert("Something wrong. Please try again.");
            },
            finally: function (xhr) {
                $(".btn-loader").css('display', 'none');
            }
        });
    });


    $(document).on("click", '[data-target="#applynowPopup"]', function (e) {
        e.preventDefault();
        var id = $(this).data("jobvalue");
        $.get('/Careers/ApplyJob', { Id: id }, function (res) {
            $("#applynowPopup_form").html(res);
            // rebind validation
            $.validator.unobtrusive.parse("#frmapplyjob");
        });
    });

    $(document).on("submit", "#frmapplyjob", function (e) {
        e.preventDefault();
        var token = document.getElementById("frmapplyjob").querySelector('.g-recaptcha-response').value;
       // var token = grecaptcha.getResponse(); 
       
        var formData = new FormData(this);
        formData.set('g-recaptcha-response', token);
        document.getElementById("submitapplyjob").classList.add("is-processing");
        document.getElementById("submitapplyjob").appendChild(document.querySelector(".btn-loader")) 
        $.ajax({
            url: "/Careers/ApplyJob",
            type: "POST",
            data: formData,
            success: function (response) {
                $("#applynowPopup_form").html(response);
                $.validator.unobtrusive.parse("#frmapplyjob");                
                
                 if ($("#strstatus_applyjob").val() == "true") {
                   
                    const successMessage = document.getElementById("successMessage_apply");
                    successMessage.classList.remove("hidden");
                    successMessage.focus();
                    //alert(response.success);
                    //document.getElementById("#contactForm").reset();
                    setTimeout(() => {
                        successMessage.classList.add("hidden");               
                        $("#successMessage_apply").remove();
                    }, 4000);
                }
            },
            error: function (xhr, ajaxOptions, response) {                 
                alert("Something wrong. Please try again.");
            },            
            finally: function(xhr)
            {
                $(".btn-loader").css('display', 'none');
            }
        });
    });


     $(document).on("click", '[data-target="#eventsregisterPopup"]', function (e) {
        e.preventDefault();
        var id = $(this).data("eventid");
        $.get('/Events/Registration', { Id: id }, function (res) {
            $("#div_eventsregisterPopup").html(res);
            // rebind validation
            $.validator.unobtrusive.parse("#frmeventregistration");
        });
    });

    $(document).on("submit", "#frmeventregistration", function (e) {
        e.preventDefault();
        var token = document.getElementById("frmeventregistration").querySelector('.g-recaptcha-response').value;
       // var token = grecaptcha.getResponse(); 
       
        var formData = new FormData(this);
        formData.set('g-recaptcha-response', token);
        document.getElementById("submiteventregister").classList.add("is-processing");
        document.getElementById("submiteventregister").appendChild(document.querySelector(".btn-loader")) 
        $.ajax({
            url: "/Events/Registration",
            type: "POST",
            data: formData,
            success: function (response) {
                $("#div_eventsregisterPopup").html(response);
                $.validator.unobtrusive.parse("#frmeventregistration");                
                
                 if ($("#strstatus_eventsregistration").val() == "true") {
                   
                    const successMessage = document.getElementById("successMessage_eventregister");
                    successMessage.classList.remove("hidden");
                    successMessage.focus();
                    //alert(response.success);
                    //document.getElementById("#contactForm").reset();
                    setTimeout(() => {
                        successMessage.classList.add("hidden");               
                        $("#successMessage_eventregister").remove();
                    }, 4000);
                }
            },
            error: function (xhr, ajaxOptions, response) {                 
                alert("Something wrong. Please try again.");
            },            
            finally: function(xhr)
            {
                $(".btn-loader").css('display', 'none');
            }
        });
    });

});

