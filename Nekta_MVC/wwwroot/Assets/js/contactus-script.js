gsap.registerPlugin(ScrollTrigger);

window.addEventListener("load", () => {

  const rightLeaf = document.querySelector(".cu-leaf-right");
  const leftLeaf = document.querySelector(".cu-leaf-left");

  gsap.set([rightLeaf, leftLeaf], {
    opacity: 0,
    scale: 0.6
  });

  function revealLeaves() {

    gsap.fromTo(rightLeaf,
      {
        xPercent: -35,
        yPercent: 10,
        scale: 0.6,
        opacity: 0
      },
      {
        xPercent: 0,
        yPercent: 0,
        scale: 1,
        opacity: 1,
        duration: 1.2,
        ease: "power3.out",
        overwrite: true
      }
    );

    gsap.fromTo(leftLeaf,
      {
        xPercent: 35,
        yPercent: -10,
        scale: 0.6,
        opacity: 0
      },
      {
        xPercent: 0,
        yPercent: 0,
        scale: 1,
        opacity: 1,
        duration: 1.2,
        ease: "power3.out",
        delay: 0.1,
        overwrite: true
      }
    );
  }

  ScrollTrigger.create({
    trigger: ".cu-form-section",
    start: "top 75%",
    end: "bottom 25%",
  
    onEnter: revealLeaves,
    onEnterBack: revealLeaves
  });

});