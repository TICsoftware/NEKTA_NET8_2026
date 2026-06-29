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
if (window.matchMedia("(min-width: 1024px)").matches) {

  gsap.registerPlugin(ScrollTrigger);

  document.querySelectorAll(".mentorshipmoments-grid").forEach((grid) => {

    const cards = grid.querySelectorAll(".grid-item-box");

    // 3-card layout (your case)
    const startX     = ["100%", "0%", "-100%"];
    const startY     = ["10%",  "0%",  "10%"];
    const startScale = [0.85,   0.9,   0.85];
    const startRot   = [-6,     0,     6];

    cards.forEach((card, i) => {
      gsap.set(card, {
        x: startX[i] ?? "0%",
        y: startY[i] ?? "0%",
        scale: startScale[i] ?? 0.9,
        rotation: startRot[i] ?? 0,
        opacity: 1,
        transformOrigin: "center bottom",
      });
    });

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: grid,
        start: "top 70%",
        end: "top 20%",
        scrub: 2,
      }
    });

    cards.forEach((card) => {
      tl.to(card, {
        x: "0%",
        y: "0%",
        scale: 1,
        rotation: 0,
        opacity: 1,
        ease: "power3.out",
        duration: 1,
      }, 0);
    });

  });

}


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