document.addEventListener("DOMContentLoaded", () => {
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
// ANIMATION
// --------------------------------------------


document.querySelectorAll(".mentorshipmoments-slider").forEach((slider) => {

  const swiperEl = slider.querySelector(".mentorshipmomentsSwiper");
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


  // ==========================
  // 🔥 GSAP ANIMATION (DESKTOP ONLY)
  // ==========================
  if (window.matchMedia("(min-width: 1024px)").matches) {

    gsap.registerPlugin(ScrollTrigger);

    const slides = slider.querySelectorAll(".swiper-slide:not(.swiper-slide-duplicate)");

    // Fan layout (same as your team slider)
    const startX     = ["120%", "60%", "-120%"];
    const startY     = ["0%", "6%", "0%"];
    const startScale = [0.80, 0.88, 0.80];
    const startRot   = [-7, -2, 7];

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
        start: "top 70%",
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
        opacity: 1,
        ease: "power3.out",
        duration: 1,
      }, 0);
    });

  }

});

// if (window.matchMedia("(min-width: 1024px)").matches) {

//   gsap.registerPlugin(ScrollTrigger);

//   document.querySelectorAll(".mentorshipmoments-grid").forEach((grid) => {

//     const cards = grid.querySelectorAll(".grid-item-box");

//     // 3-card layout (your case)
//     const startX     = ["100%", "0%", "-100%"];
//     const startY     = ["10%",  "0%",  "10%"];
//     const startScale = [0.85,   0.9,   0.85];
//     const startRot   = [-6,     0,     6];

//     cards.forEach((card, i) => {
//       gsap.set(card, {
//         x: startX[i] ?? "0%",
//         y: startY[i] ?? "0%",
//         scale: startScale[i] ?? 0.9,
//         rotation: startRot[i] ?? 0,
//         opacity: 1,
//         transformOrigin: "center bottom",
//       });
//     });

//     const tl = gsap.timeline({
//       scrollTrigger: {
//         trigger: grid,
//         start: "top 70%",
//         end: "top 20%",
//         scrub: 2,
//       }
//     });

//     cards.forEach((card) => {
//       tl.to(card, {
//         x: "0%",
//         y: "0%",
//         scale: 1,
//         rotation: 0,
//         opacity: 1,
//         ease: "power3.out",
//         duration: 1,
//       }, 0);
//     });

//   });

// }


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
   IMAGE PARALLAX
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


});