gsap.registerPlugin(ScrollTrigger);

const signatureSection = document.querySelector(".signature-section");

if (signatureSection) {
  const signatureTimeline = gsap.timeline({
    scrollTrigger: {
      trigger: ".signature-section",
      start: "top 80%",
      end: "top 20%",
      scrub: true,
      // markers: true,
    },
  });

  signatureTimeline
    .fromTo(
      ".signature-plate-image",
      {
        rotation: 90,
        transformOrigin: "50% 50%",
      },
      {
        rotation: 0,
        ease: "none",
      },
      0,
    )
    .fromTo(
      ".signature-img",
      {
        rotation: 0,
        transformOrigin: "50% 50%",
      },
      {
        rotation: 90,
        ease: "none",
      },
      0,
    )
    .fromTo(
      ".signature-garlic-img",
      {
        xPercent: 0,
        yPercent: 40,
        rotation: -28,
        opacity: 0,
        transformOrigin: "50% 50%",
      },
      {
        xPercent: 0,
        yPercent: -100,
        rotation: 0,
        opacity: 1,
        ease: "none",
      },
      0.08,
    )
    .fromTo(
      ".signature-leaf-img",
      {
        xPercent: 90,
        yPercent: 40,
        rotation: 28,
        opacity: 0,
        transformOrigin: "50% 50%",
      },
      {
        xPercent: 80,
        yPercent: -100,
        rotation: 0,
        opacity: 1,
        ease: "none",
      },
      0.08,
    );
}

