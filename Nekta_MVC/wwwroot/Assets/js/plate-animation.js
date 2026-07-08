gsap.registerPlugin(ScrollTrigger);

gsap.fromTo(
  ".signature-plate-image",
  {
    rotation: 90,
    transformOrigin: "50% 50%"
  },
  {
    rotation: 0,
    ease: "none",
    scrollTrigger: {
      trigger: ".signature-section",
      start: "top 80%",
      end: "top 20%",
      scrub: true,
      // markers: true,
    }
  }
);


gsap.fromTo(
  ".signature-bg",
  {
    rotation: 90,
    transformOrigin: "50% 50%"
  },
  {
    rotation: 0,
    ease: "none",
    scrollTrigger: {
      trigger: ".signature-section",
      start: "top 80%",
      end: "top 20%",
      scrub: true,
      // markers: true,
    }
  }
);

