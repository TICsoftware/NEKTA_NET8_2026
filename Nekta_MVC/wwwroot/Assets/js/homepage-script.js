document.addEventListener("DOMContentLoaded", () => {
    
gsap.registerPlugin(ScrollTrigger);

// --------------------------------------------
// OUR VALUES SECTION
// --------------------------------------------
// INITIAL STATE → top-right feel
const mm = gsap.matchMedia();

mm.add("(min-width: 1024px)", () => {

  // IMAGE INITIAL STATE
  gsap.set(".img-box", {
    scale: 1.25,
    xPercent: 35,
    yPercent: 0,
    opacity: 1,
    transformOrigin: "center center",
    willChange: "transform, opacity"
  });

  // TEXT INITIAL STATE
  gsap.set(".values-text", {
    opacity: 0,
    x: 80
  });

  const tl = gsap.timeline({
    scrollTrigger: {
      trigger: ".values-section",
      start: "top 85%",
      end: "top 30%",
      scrub: 1.5
    }
  });

  // IMAGE ANIMATION
  tl.to(".img-box", {
    scale: 1,
    xPercent: 0,
    yPercent: 0,
    ease: "none",
    duration: 1
  });

  // TEXT ANIMATION
  tl.to(".values-text", {
    opacity: 1,
    x: 0,
    ease: "none",
    duration: 0.6
  }, "-=0.4");

});


// --------------------------------------------
// TESTIMONIALS SLIDER
// --------------------------------------------
document.querySelectorAll(".testimonial-slider").forEach((slider) => {

  const swiperEl = slider.querySelector(".testimonialSwiper");
  const nextBtn  = slider.querySelector(".swiper-button-next");
  const prevBtn  = slider.querySelector(".swiper-button-prev");

  // ✅ ORIGINAL slides count (REAL)
  const realSlidesCount = swiperEl.querySelectorAll(".swiper-slide:not(.swiper-slide-duplicate)").length;

  const swiper = new Swiper(swiperEl, {
    loop: false,
    speed: 800,
    slidesPerView: 3,
    spaceBetween: 40,
    parallax: true,

    navigation: {
      nextEl: nextBtn,
      prevEl: prevBtn,
    },

    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 20 },
      768: { slidesPerView: 2, spaceBetween: 40 },
      1024: { slidesPerView: 3, spaceBetween: 40 }
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

    // ✅ FINAL LOGIC
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

});

if (window.matchMedia("(min-width: 1024px)").matches) {

  gsap.registerPlugin(ScrollTrigger);

  document.querySelectorAll(".testimonial-slider").forEach((slider) => {

    const slides = slider.querySelectorAll(".testimonial-slider .swiper-slide:not(.swiper-slide-duplicate)");

    // Stagger from outside-in: left card comes from far left, right from far right
    // Middle card rises from below slightly for depth
    const startX     = ["100%",  "0%",   "-100%"];
    const startY     = ["0%",    "8%",    "0%"];
    const startScale = [0.82,    0.88,    0.82];
    const startRot   = [-6,       0,       6];   // subtle tilt adds depth

    slides.forEach((slide, i) => {
      gsap.set(slide, {
        x:       startX[i]     ?? "0%",
        y:       startY[i]     ?? "0%",
        scale:   startScale[i] ?? 0.85,
        rotation: startRot[i]  ?? 0,
        opacity: 1,
        transformOrigin: "center bottom",
      });
    });

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: slider,
        start: "top 60%",
        end:   "top 10%",
        scrub: 2, 
      }
    });

    slides.forEach((slide, i) => {
      tl.to(slide, {
        x:        "0%",
        y:        "0%",
        scale:    1,
        rotation: 0,
        opacity:  1,
        ease:     "power3.out",
        duration: 1,
      }, 0);   // slight stagger so they don't all land at once
    });

  });
}


// --------------------------------------------
// Memory
// --------------------------------------------
const mmMemory = gsap.matchMedia();

mm.add("(min-width: 1024px)", () => {

  const memoryimg = document.querySelector(".memory-img-left");
  const content = document.querySelector(".memory-content-inner");

  // 👉 INITIAL STATE
  gsap.set(memoryimg, {
    x: window.innerWidth * 0.25,
    y: -window.innerHeight * 0.1,
    scale: 1.05,
    filter: "blur(8px)",
    zIndex: 50,
    willChange: "transform"
  });

  gsap.set(content, {
    opacity: 0,
    x: 100,
    filter: "blur(6px)"
  });

  // 👉 TIMELINE
  const tla = gsap.timeline({
    scrollTrigger: {
      trigger: ".memory-section",
      start: "top 85%",
      end: "top 20%",
      scrub: 1.2,
      anticipatePin: 1
    }
  });

  // 🌌 STEP 1 — Image comes into focus
  tla.to(memoryimg, {
    x: 0,
    y: 0,
    scale: 1,
    filter: "blur(0px)",
    duration: 1.8,
    ease: "power4.out"
  }, 0);

  // ✨ STEP 2 — Depth settle
  tla.to(memoryimg, {
    scale: 0.98,
    duration: 0.6,
    ease: "power2.out"
  }, 1.2);

  // 👉 STEP 3 — Content reveal
  tla.to(content, {
    opacity: 1,
    x: 0,
    filter: "blur(0px)",
    duration: 1.4,
    ease: "power3.out"
  }, 0.6);

  // 👉 STEP 4 — Stagger text
  tla.from(content.children, {
    opacity: 0,
    y: 30,
    stagger: 0.12,
    duration: 0.8,
    ease: "power2.out"
  }, 0.9);

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

// --------------------------------------------
// TEAM SLIDER
// --------------------------------------------

document.querySelectorAll(".team-slider").forEach((slider) => {

    const swiperEl = slider.querySelector(".teamSwiper");
    const nextBtn  = slider.querySelector(".swiper-button-next");
    const prevBtn  = slider.querySelector(".swiper-button-prev");

    const swiper = new Swiper(swiperEl, {
        loop: false,
        speed: 800,
        slidesPerView: 4,
        spaceBetween: 20,
        parallax: true,
        navigation: {
            nextEl: nextBtn,
            prevEl: prevBtn,
        },

        breakpoints: {
            0: {
                slidesPerView: 1,
                spaceBetween: 20,
            },
            768: {
                slidesPerView: 2,
                spaceBetween: 20,
            },
            1024: {
                slidesPerView: 4,
                spaceBetween: 20,
            }
        },

        // ✅ MUST be outside breakpoints
        on: {
            init: function () {
                toggleNavigation(this);
            },
            resize: function () {
                toggleNavigation(this);
            },
            breakpoint: function () {
                toggleNavigation(this);
            }
        }
    });

    function toggleNavigation(swiper) {

        // ✅ total slides (real)
        const totalSlides = swiper.slides.length;

        // ✅ correct current perView from breakpoint
        let perView = swiper.currentBreakpoint
            ? swiper.params.breakpoints[swiper.currentBreakpoint].slidesPerView
            : swiper.params.slidesPerView;

        if (!perView || perView === 'auto') perView = 1;

        // ✅ toggle
        if (totalSlides <= perView) {
            nextBtn.classList.add("hidden");
            prevBtn.classList.add("hidden");
        } else {
            nextBtn.classList.remove("hidden");
            prevBtn.classList.remove("hidden");
        }
    }

});
 
 
if (window.matchMedia("(min-width: 1024px)").matches) {
 
    gsap.registerPlugin(ScrollTrigger);
 
    document.querySelectorAll(".team-slider").forEach((slider) => {
 
        const slides = slider.querySelectorAll(".team-slider .swiper-slide:not(.swiper-slide-duplicate)");
 
        // 4-card layout: cards fan in from outside → center
        const startX     = ["120%",  "60%",   "-60%",  "-120%"];
        const startY     = ["0%",    "6%",    "6%",    "0%"];
        const startScale = [0.80,    0.88,    0.88,    0.80];
        const startRot   = [-2,      -2,       2,       2];
 
        slides.forEach((slide, i) => {
            gsap.set(slide, {
                x:             startX[i]     ?? "0%",
                y:             startY[i]     ?? "0%",
                scale:         startScale[i] ?? 0.85,
                rotation:      startRot[i]  ?? 0,
                opacity:       1,
                transformOrigin: "center bottom",
            });
        });
 
        const tl = gsap.timeline({
            scrollTrigger: {
                trigger: slider,
                start:   "top 60%",
                end:     "top 10%",
                scrub:   2,
            }
        });
 
        slides.forEach((slide) => {
            tl.to(slide, {
                x:        "0%",
                y:        "0%",
                scale:    1,
                rotation: 0,
                opacity:  1,
                ease:     "power3.out",
                duration: 1,
            }, 0);
        });
 
    });
}

// --------------------------------------------
// FIND TEST
// --------------------------------------------
function initFindTestAnimation() {

  const tl = gsap.timeline({
    scrollTrigger: {
      trigger: ".find-test-outer",
      start: "top 75%",
      toggleActions: "play none none reverse"
    }
  });

  // 🌄 1. Background (slow cinematic base)
  tl.from(".find-test-bg", {
    scale: 1.15,
    duration: 2.2,
    ease: "power2.out"
  }, 0); // start at 0

  // 🔷 2. Hexagon (hero visual)
  tl.from(".hexagon-img", {
    y: 100,
    scale: 0.85,
    opacity: 0,
    duration: 1.2,
    ease: "power3.out"
  }, 0.2); // slight delay after BG

  // 🧾 3. Card (UI reveal)
  tl.from(".fade-2", {
    y: 60,
    opacity: 0,
    filter: "blur(8px)",
    duration: 0.9,
    ease: "power3.out"
  }, 0.5); // comes after hex

  gsap.to(".hexagon-img", {
 yPercent: 3,  
  duration: 3,
  ease: "sine.inOut",
  yoyo: true,
  repeat: -1,
  delay: 1.5
});

}

initFindTestAnimation();

// --------------------------------------------
// JOURNEY
// --------------------------------------------
document.querySelectorAll(".journey-slider").forEach((slider) => {

  const swiperEl = slider.querySelector(".journeySwiper");
  const nextBtn  = slider.querySelector(".swiper-button-next");
  const prevBtn  = slider.querySelector(".swiper-button-prev");

  const realSlidesCount = swiperEl.querySelectorAll(".swiper-slide").length;

  const swiper = new Swiper(swiperEl, {
    loop: false,
    speed: 800,
    slidesPerView: 3,
    spaceBetween: 20,
    // watchOverflow: true,

    observer: true,
    observeParents: true,
    resizeObserver: true,

    navigation: {
      nextEl: nextBtn,
      prevEl: prevBtn,
    },

    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 30 },
      768: { slidesPerView: 2, spaceBetween: 30 },
      1024: { slidesPerView: 3, spaceBetween: 30 }
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

});
 
if (window.matchMedia("(min-width: 1024px)").matches) {

    document.querySelectorAll(".journey-slider").forEach((slider) => {
 
        const slides = slider.querySelectorAll(".journey-slider .swiper-slide:not(.swiper-slide-duplicate)");
 
        // 4-card layout: cards fan in from outside → center
      const startX     = ["140%", "100%", "80%", "60%"];
const startY     = ["-10%", "-5%", "0%", "5%"];
const startScale = [0.75,   0.85,   0.92,  0.98];
const startRot   = [2,      2,      2,     0];
 
        slides.forEach((slide, i) => {
            gsap.set(slide, {
                x:             startX[i]     ?? "0%",
                y:             startY[i]     ?? "0%",
                scale:         startScale[i] ?? 0.85,
                rotation:      startRot[i]  ?? 0,
                opacity:       1,
                transformOrigin: "center bottom",
            });
        });
 
        const tl = gsap.timeline({
            scrollTrigger: {
                trigger: slider,
                start:   "top 90%",
                end:     "top 5%",
                scrub:   1,
            }
        });
 
        slides.forEach((slide) => {
            tl.to(slide, {
                x:        "0%",
                y:        "0%",
                scale:    1,
                rotation: 0,
                opacity:  1,
                ease:     "power3.out",
                duration: 1,
            }, 0);
        });
 
    });
}

if (window.matchMedia("(min-width: 1024px)").matches) {
  const tlJourney = gsap.timeline({
    scrollTrigger: {
      trigger: ".journey-section",
      start: "top 75%",
      end: "top 30%",
      scrub: true, // 👈 smooth scroll sync
    }
  });

  // Initial state (zoomed + shifted right)
  gsap.set(".photo-block ", {
    scale: 0.85,
    xPercent: -25,
    yPercent: 0,
    transformOrigin: "center center",
    willChange: "transform",
      filter: "blur(6px)",
  });

  // Animate to normal
  tlJourney.to(".photo-block", {
    scale: 1,
    xPercent: 0,
    yPercent: 0,
    ease: "none",
      filter: "blur(0)",
  });

  }


// --------------------------------------------
// HERO SLIDER
// --------------------------------------------
const heroSwiperEl = document.querySelector(".heroSwiper");

if (heroSwiperEl) {

  const swiper = new Swiper(heroSwiperEl, {
    loop: true,
    speed: 1400,
    effect: "fade",
    fadeEffect: {
      crossFade: true,
    },
    autoplay: {
      delay: 3500,
      disableOnInteraction: false,
    },
    on: {
      init: function () {
        animateSlide(this.slides[this.activeIndex]);
      },
      slideChangeTransitionStart: function () {
        animateSlide(this.slides[this.activeIndex]);
      }
    }
  });

  function animateSlide(slide) {
    // 🚫 ignore duplicate slides (important)
    if (slide.classList.contains("swiper-slide-duplicate")) return;

    const img = slide.querySelector("img");
    if (!img) return;

    // kill previous animation (prevents stacking)
    gsap.killTweensOf(img);

    gsap.set(img, {
      scale: 1.25,
      x: 60,
      opacity: 0.6
    });

    gsap.to(img, {
      scale: 1,
      x: 0,
      opacity: 1,
      duration: 2,
      ease: "power3.out"
    });
  }

}





/* ===========================================================
   POPUP ACCORDIAN
=========================================================== */

  const accordions = document.querySelectorAll(".accordion-item");

  accordions.forEach((item) => {
    const toggle = item.querySelector(".accordion-toggle");
    const content = item.querySelector(".accordion-content");

    toggle.addEventListener("click", () => {
      const isOpen = content.style.maxHeight && content.style.maxHeight !== "0px";
      const modalScroll = item.closest(".popup-wrapper-inner");

      // 🔴 CLOSE ALL
      document.querySelectorAll(".accordion-item").forEach((el) => {
        const c = el.querySelector(".accordion-content");
        const t = el.querySelector(".accordion-toggle");

        c.style.maxHeight = null;
        t.classList.remove("active");
      });

      // 🟢 OPEN CURRENT
      if (!isOpen) {
        content.style.maxHeight = content.scrollHeight + "px";
        toggle.classList.add("active");

        // ✅ SCROLL FIX
        if (modalScroll) {
          setTimeout(() => {
            const itemRect = item.getBoundingClientRect();
            const containerRect = modalScroll.getBoundingClientRect();

            const offset = itemRect.top - containerRect.top;

            modalScroll.scrollTo({
              top: modalScroll.scrollTop + offset - 20,
              behavior: "smooth",
            });
          }, 250);
        }

      } else {
        content.style.maxHeight = null;
        toggle.classList.remove("active");
      }
    });
  });


});