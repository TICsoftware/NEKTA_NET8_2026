document.addEventListener("DOMContentLoaded", function () {

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


});