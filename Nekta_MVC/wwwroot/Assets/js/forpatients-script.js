document.addEventListener("DOMContentLoaded", () => {
  // --------------------------------------------
  // SILDER
  // --------------------------------------------
document.querySelectorAll(".common-card-slider").forEach((slider) => {

  const swiperEl = slider.querySelector(".commonSwiperslider");
  const nextBtn  = slider.querySelector(".swiper-button-next");
  const prevBtn  = slider.querySelector(".swiper-button-prev");

  const realSlidesCount = swiperEl.querySelectorAll(".swiper-slide").length;

  const swiper = new Swiper(swiperEl, {
    loop: false,
    speed: 800,
    slidesPerView: 3,
    spaceBetween: 20,

    observer: true,
    observeParents: true,
    resizeObserver: true,

    navigation: {
      nextEl: nextBtn,
      prevEl: prevBtn,
    },

    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 25 },
      768: { slidesPerView: 2, spaceBetween: 20 },
      1024: { slidesPerView: 3, spaceBetween: 20 }
    },

    on: {
      init: function () {
        updateNav(this);
      },
      resize: function () {
        updateNav(this);
      },
      breakpoint: function () {
        updateNav(this);
      }
    }
  });

  function getPerView(swiper) {
    if (swiper.currentBreakpoint && swiper.params.breakpoints) {
      return swiper.params.breakpoints[swiper.currentBreakpoint].slidesPerView;
    }
    return swiper.params.slidesPerView;
  }

  function updateNav(swiper) {
    let perView = getPerView(swiper);

    if (!perView || perView === "auto") perView = 1;

    if (realSlidesCount <= perView) {
      nextBtn.classList.add("hidden");
      prevBtn.classList.add("hidden");
      swiper.allowTouchMove = false;
    } else {
      nextBtn.classList.remove("hidden");
      prevBtn.classList.remove("hidden");
      swiper.allowTouchMove = true;
    }
  }


  // ==========================
  // 🔥 GSAP ANIMATION (DESKTOP ONLY)
  // ==========================
  if (window.matchMedia("(min-width: 1024px)").matches) {

    gsap.registerPlugin(ScrollTrigger);

    const slides = slider.querySelectorAll(".swiper-slide:not(.swiper-slide-duplicate)");

    // Fan layout (same as your team slider)
    const startX     = ["120%", "60%", "-60%", "-120%"];
    const startY     = ["0%", "6%", "6%", "0%"];
    const startScale = [0.80, 0.88, 0.88, 0.80];
    const startRot   = [-7, -2, 2, 7];

    slides.forEach((slide, i) => {
      gsap.set(slide, {
        x: startX[i] ?? "0%",
        y: startY[i] ?? "0%",
        scale: startScale[i] ?? 0.85,
        rotation: startRot[i] ?? 0,
        opacity: 1,
        transformOrigin: "center bottom",
      });
    });

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: slider,
        start: "top 85%",
        end: "top 10%",
        scrub: 1.2,
      }
    });

    slides.forEach((slide) => {
      tl.to(slide, {
        x: "0%",
        y: "0%",
        scale: 1,
        rotation: 0,
        opacity: 1,
        ease: "power3.out",
        duration: 1,
      }, 0);
    });

  }

});


  /* =========================
    COP BB
  ========================= */
  if (window.matchMedia("(min-width: 1024px)").matches) {

    // 👉 RIGHT PUSH (whole row)
    gsap.utils.toArray(".cop_bb_row_push_right").forEach((row) => {

      gsap.from(row, {
        x: 150,
        opacity: 0,
        duration: 1,
        ease: "power3.out",
        scrollTrigger: {
          trigger: row,
          start: "top 80%",
          toggleActions: "play none none reverse"
        }
      });

    });

    // 👉 LEFT PUSH (whole row)
    gsap.utils.toArray(".cop_bb_row_push_left").forEach((row) => {

      gsap.from(row, {
        x: -150,
        opacity: 0,
        duration: 1,
        ease: "power3.out",
        scrollTrigger: {
          trigger: row,
          start: "top 80%",
          toggleActions: "play none none reverse"
        }
      });

    });

  }


  /* =========================
  Anim
  ========================= */
  if (window.matchMedia("(min-width: 1024px)").matches) {

    const img = document.querySelector(".extreme-left-imgwrap");

    requestAnimationFrame(() => {

      // START slightly from LEFT
      gsap.set(img, {
        x: -80,
        scale: 1,
        transformOrigin: "center center"
      });

      // ANIMATE TO ORIGINAL
      gsap.to(img, {
        x: 0,
        scale: 1,
        ease: "none",
        scrollTrigger: {
          trigger: ".animation-area",
          start: "top 85%",
          end: "top 30%",
          scrub: 1.5
        }
      });

    });

  }


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


  /* ===========================================================
    IMAGE Rotation
  =========================================================== */

  if (typeof gsap === "undefined" || typeof ScrollTrigger === "undefined") return;
  gsap.registerPlugin(ScrollTrigger);

  /* ===========================================================
    HEXAGON IMAGE ANIMATION (MULTIPLE SUPPORT)
  =========================================================== */
  if (window.matchMedia("(min-width: 1024px)").matches) {
    
  gsap.utils.toArray(".hexagon-img-wrapper").forEach((wrapper, index) => {
    const hex = wrapper.querySelector(".bt_bb_image");
    if (!hex) return;

    const baseRotation = index % 2 === 0 ? -8 : 8;

    // Entry animation
    gsap.fromTo(
      hex,
      {
        rotation: baseRotation,
        scale: 0.9,
        opacity: 0,
        filter: "blur(6px)",
      },
      {
        rotation: 0,
        scale: 1,
        opacity: 1,
        filter: "blur(0px)",
        duration: 1.4,
        ease: "power3.out",
        scrollTrigger: {
          trigger: wrapper,
          start: "top 75%",
          toggleActions: "play none none reverse",
        },
      }
    );

    /* =========================
      DESKTOP ONLY PARALLAX
    ========================= */

    gsap.fromTo(hex,
    { y: 60 },
    {
      y: 0,
      duration: 1,
      ease: "power2.out",
      scrollTrigger: {
        trigger: wrapper,
        start: "top 75%",
        toggleActions: "play none none reverse",
      }
    }
  );

  });

  }


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

  // --------------------------------------------
  // scheduleserviceSwiper
  // --------------------------------------------

  document.querySelectorAll(".scheduleservice-slider").forEach((sliderSS) => {

  const swiperElSS = sliderSS.querySelector(".scheduleserviceSwiper");
  const nextBtnSS  = sliderSS.querySelector(".swiper-button-next");
  const prevBtnSS  = sliderSS.querySelector(".swiper-button-prev");

  const realSlidesCountSS = swiperElSS.querySelectorAll(".swiper-slide").length;

  const swiperSS = new Swiper(swiperElSS, {
    loop: false,
    speed: 800,
    slidesPerView: 3,
    spaceBetween: 20,

    observer: true,
    observeParents: true,
    resizeObserver: true,

    navigation: {
      nextEl: nextBtnSS,
      prevEl: prevBtnSS,
    },

    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 0 },
      768: { slidesPerView: 2, spaceBetween: 20 },
      1024: { slidesPerView: 3, spaceBetween: 20 }
    },

    on: {
      init: function () {
        updateNav(this);
      },
      resize: function () {
        updateNav(this);
      },
      breakpoint: function () {
        updateNav(this);
      }
    }
  });

  function getPerView(swiperSS) {
    if (swiperSS.currentBreakpoint && swiperSS.params.breakpoints) {
      return swiperSS.params.breakpoints[swiperSS.currentBreakpoint].slidesPerView;
    }
    return swiperSS.params.slidesPerView;
  }

  function updateNav(swiperSS) {
    let perView = getPerView(swiperSS);

    if (!perView || perView === "auto") perView = 1;

    if (realSlidesCountSS <= perView) {
      nextBtnSS.classList.add("hidden");
      prevBtnSS.classList.add("hidden");
      swiperSS.allowTouchMove = false;
    } else {
      nextBtnSS.classList.remove("hidden");
      prevBtnSS.classList.remove("hidden");
      swiperSS.allowTouchMove = true;
    }
  }


  // ==========================
  // 🔥 GSAP FAN ANIMATION (3 CARDS)
  // ==========================
  if (window.matchMedia("(min-width: 1024px)").matches) {

    gsap.registerPlugin(ScrollTrigger);

    const slides = sliderSS.querySelectorAll(".swiper-slide:not(.swiper-slide-duplicate)");

    // ✅ 3-card fan layout
    const startX     = ["80%", "0%", "-80%"];
    const startY     = ["5%",  "0%",  "5%"];
    const startScale = [0.85,  1,     0.85];
    const startRot   = [-6,    0,     6];
    const startBlur  = ["6px", "0px", "6px"]; 

    slides.forEach((slide, i) => {
      gsap.set(slide, {
        x: startX[i] ?? "0%",
        y: startY[i] ?? "0%",
        scale: startScale[i] ?? 0.9,
        rotation: startRot[i] ?? 0,
        filter: `blur(${startBlur[i] ?? "4px"})`,
        opacity: 1,
        transformOrigin: "center bottom",
      });
    });

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: sliderSS,
        start: "top 60%",
        end: "top 20%",
        scrub: 1.5,
      }
    });

    slides.forEach((slide) => {
      tl.to(slide, {
        x: "0%",
        y: "0%",
        scale: 1,
        rotation: 0,
         filter: "blur(0px)", 
        opacity: 1,
        ease: "power3.out",
        duration: 1,
      }, 0);
    });

  }

});






});