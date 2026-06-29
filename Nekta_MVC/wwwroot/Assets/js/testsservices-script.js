document.addEventListener("DOMContentLoaded", () => {
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
      0: { slidesPerView: 1, spaceBetween: 20, },
      768: { slidesPerView: 2, spaceBetween: 20,},
      1024: { slidesPerView: 2, spaceBetween: 20, },
      1280: {slidesPerView: 3,spaceBetween: 15,},
      1366: {slidesPerView: 3, spaceBetween: 15,},
      1440: { slidesPerView: 3, spaceBetween: 30, },
      1600: {slidesPerView: 3, spaceBetween: 30,}
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
const startRot   = [0,      0,      0,     0];
 
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
   IMAGE PARALLAX
=========================================================== */
ScrollTrigger.matchMedia({
  "(min-width: 1024px)": () => {
    gsap.utils.toArray(".feature-item").forEach((section) => {
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



/* ===========================================================
   HEXAGON IMAGE ANIMATION (MULTIPLE SUPPORT)
=========================================================== */
if (typeof gsap === "undefined" || typeof ScrollTrigger === "undefined") return;
gsap.registerPlugin(ScrollTrigger);



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




});