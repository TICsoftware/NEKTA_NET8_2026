document.addEventListener("DOMContentLoaded", () => {

  // Prevent flash of unstyled content before ScrollTrigger fires
  gsap.set(".gallery-center", { scale: 0.65, rotate: 0, opacity: 0 });
  gsap.set(".gallery-left", { x: -200, y: 80, rotate: -35, opacity: 0 });
  gsap.set(".gallery-right", { x: 200, y: 80, rotate: 35, opacity: 0 });

  const tl = gsap.timeline({
    scrollTrigger: {
      trigger: ".story-gallery",
      start: "top 50%",
      toggleActions: "play none none reverse", // play once on enter, never reverse/reset
    }
  });

  tl.to(".gallery-center", {
      scale: 1,
      opacity: 1,
      duration: 1,
      ease: "power4.out"
    })
    .to(".gallery-left", {
      x: 0,
      y: 0,
      rotate:-10,
      opacity: 1,
      duration: 1,
      ease: "power4.out"
    }, "-=0.7")
    .to(".gallery-right", {
      x: 0,
      y: 0,
      rotate: 10,
      opacity: 1,
      duration: 1,
      ease: "power4.out"
    }, "-=1");

});