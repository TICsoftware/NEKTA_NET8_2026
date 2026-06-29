document.addEventListener('DOMContentLoaded', function () {

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
