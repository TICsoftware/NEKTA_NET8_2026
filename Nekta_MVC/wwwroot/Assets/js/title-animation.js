document.addEventListener("DOMContentLoaded", (event) => {
  gsap.registerPlugin(ScrollTrigger); // safe to call even if already registered

  function createScrollTrigger(triggerElement, timeline) {
    ScrollTrigger.create({
      trigger: triggerElement,
      start: "top bottom",
      onLeaveBack: () => {
        timeline.progress(0);
        timeline.pause();
      },
    });
    ScrollTrigger.create({
      trigger: triggerElement,
      start: "top 80%",
      onEnter: () => timeline.play(),
    });
  }

  // 1. Split the text FIRST — this creates the .char spans
  const splitTypeElements = document.querySelectorAll("[text-split]");
  splitTypeElements.forEach((splitTypeElement) => {
    new SplitType(splitTypeElement, {
      types: "words, chars",
      tagName: "span",
    });
  });

  // 2. Now the .char elements exist and can be animated
  const lettersSlideDownElements = document.querySelectorAll("[letters-slide-down]");
  lettersSlideDownElements.forEach((element) => {
    const tl = gsap.timeline({ paused: true });
    const chars = element.querySelectorAll(".char");
    tl.from(chars, {
      yPercent: -120,
      duration: 0.3,
      ease: "power1.out",
      stagger: { amount: 0.7 },
    });
    createScrollTrigger(element, tl);
  });

  // 3. Reveal the elements (they're usually hidden via CSS until split is done)
  gsap.set("[text-split]", { opacity: 1 });
});

