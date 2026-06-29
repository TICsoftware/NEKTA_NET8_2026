document.addEventListener("DOMContentLoaded", function () {
    if (!$.parseJSON) {
    $.parseJSON = JSON.parse;
}
  
  if ($("#load_contact_form").length > 0) {
    $("#load_contact_form").load('/Connect/Enquiry');
  }

  

  $(document).on("submit", "#contactForm", function (e) {
    e.preventDefault();
    var token = document.getElementById("contactForm").querySelector('.g-recaptcha-response').value;
   // var token = grecaptcha.getResponse(); 
    var formData = new FormData(this);
    formData.set('g-recaptcha-response', token);
    document.getElementById("submitBtn").classList.add("is-processing");
    document.getElementById("submitBtn").appendChild(document.querySelector(".btn-loader"))   
    
    $.ajax({
      url: "/Connect/Enquiry",
      type: "POST",
      data: formData,
      processData: false,
      contentType: false,
      success: function (html) {     
        $("#load_contact_form").html(html);  
        $.validator.unobtrusive.parse("#contactForm");
       
        if ($("#strstatus").val() == "true") {
          const successMessage = document.getElementById("successMessage");
          successMessage.classList.remove("hidden");
          //alert(response.success);
          //document.getElementById("#contactForm").reset();
          successMessage.focus()
          setTimeout(() => {
            successMessage.classList.add("hidden");
          }, 6000);
        }
        else
          { $("#divvalidation").show();
            $("#divvalidation").focus();
          }     
         
      },
      error: function (xhr, ajaxOptions, response) {
        alert("Error " + xhr.status + ": " + xhr.responseText);
        alert("Something wrong. Please try again.");
      },
      finally: function(xhr)
      {
        $(".btn-loader").css('display', 'none');
      }
    });
  });


  // --------------------------------------------
  // FLOAT ANIMATION
  // --------------------------------------------
  function initFloatIcons() {
    const elements = document.querySelectorAll(".floating-icon");

    elements.forEach((el, i) => {
      const depth = (i % 3) + 1;

      // small variation per element
      const moveX = gsap.utils.random(8, 15) * depth * 0.3;
      const moveY = gsap.utils.random(10, 18) * depth * 0.3;
      const rotate = gsap.utils.random(2, 5);
      const duration = gsap.utils.random(3, 5);

      // initial slight offset
      gsap.set(el, {
        x: gsap.utils.random(-20, 20),
        y: gsap.utils.random(-20, 20),
        rotation: gsap.utils.random(-5, 5)
      });

      // smooth floating loop (no harsh jumps)
      gsap.to(el, {
        x: `+=${moveX}`,
        y: `+=${moveY}`,
        rotation: `+=${rotate}`,
        duration: duration,
        ease: "sine.inOut",
        yoyo: true,
        repeat: -1
      });
    });
  }

  initFloatIcons();

  gsap.fromTo(
    ".location-wrapper .map-wrapper",
    { scale: 0.75, opacity: 0 },
    {
      scale: 1,
      opacity: 1,
      duration: 0.9,
      ease: "power2.out",
      scrollTrigger: {
        trigger: ".location-wrapper",
        start: "top 80%",
        toggleActions: "play none none reset"
      }
    }
  );

  // --------------------------------------------
  // MAP WRAPPER
  // --------------------------------------------
  const mapWrapper = document.querySelector(".map-wrapper");
  if (!mapWrapper) return;

  // Activate on click
  mapWrapper.addEventListener("click", function () {
    mapWrapper.classList.add("active");
  });

  // Disable when clicking outside
  document.addEventListener("click", function (e) {
    if (!mapWrapper.contains(e.target)) {
      mapWrapper.classList.remove("active");
    }
  });

  // âœ… Disable ONLY when section leaves viewport
  ScrollTrigger.create({
    trigger: ".location-wrapper",
    start: "top 45%",
    end: "bottom 15%",
    onLeave: () => mapWrapper.classList.remove("active"),
    onLeaveBack: () => mapWrapper.classList.remove("active")
  });


  /* ===========================================================
     IMAGE PARALLAX
  =========================================================== */
  ScrollTrigger.matchMedia({
    "(min-width: 1024px)": () => {
      gsap.utils.toArray(".bg-parallax").forEach((section) => {
        const image = section.querySelector("img");

        gsap.fromTo(
          image,
          { y: -100 },
          {
            y: 100,
            ease: "none",
            scrollTrigger: {
              trigger: section,
              start: "top bottom",
              end: "bottom top",
              scrub: true
            }
          }
        );
      });
    }
  });



});