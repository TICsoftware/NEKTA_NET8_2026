document.addEventListener("DOMContentLoaded", function () {

 const accordions = document.querySelectorAll(".accordion-item");

  accordions.forEach((item) => {
    const toggle = item.querySelector(".accordion-toggle");
    const content = item.querySelector(".accordion-content");

    toggle.addEventListener("click", () => {
      const isOpen = content.style.maxHeight && content.style.maxHeight !== "0px";
      const modalScroll = item.closest(".popup-wrapper-inner");

      // 🔴 CLOSE ALL
      document.querySelectorAll(".accordion-item").forEach((el) => {
        const c = el.querySelector(".accordion-content");
        const t = el.querySelector(".accordion-toggle");

        c.style.maxHeight = null;
        t.classList.remove("active");
      });

      // 🟢 OPEN CURRENT
      if (!isOpen) {
        content.style.maxHeight = content.scrollHeight + "px";
        toggle.classList.add("active");

        // ✅ SCROLL FIX
        if (modalScroll) {
          setTimeout(() => {
            const itemRect = item.getBoundingClientRect();
            const containerRect = modalScroll.getBoundingClientRect();

            const offset = itemRect.top - containerRect.top;

            modalScroll.scrollTo({
              top: modalScroll.scrollTop + offset - 20,
              behavior: "smooth",
            });
          }, 250);
        }

      } else {
        content.style.maxHeight = null;
        toggle.classList.remove("active");
      }
    });
  });


  // --------------------------------------------
// LINE ANIMATION
// --------------------------------------------

function initLineAnimation() {

    const sections = document.querySelectorAll(".tq-line-vector");

    sections.forEach((section) => {

      const path = section.querySelector(".linePath");
      const dot1 = section.querySelector(".dot1");
      const dot2 = section.querySelector(".dot2");

      if (!path || !dot1 || !dot2) return;

      const length = path.getTotalLength();

      const startOffset = 0.001;
      const endOffset = 0.999;

      // ✅ IMPORTANT (keep this)
      gsap.set(path, {
        strokeDasharray: length,
        strokeDashoffset: length
      });

      function move(dot, progress) {
        const point = path.getPointAtLength(progress * length);
        gsap.set(dot, {
          x: point.x,
          y: point.y
        });
      }

      let dotAnim1, dotAnim2;

      function resetAll() {
        // ✅ FIX: start from LEFT
        gsap.set(path, { strokeDashoffset: -length });

        move(dot1, startOffset);
        move(dot2, endOffset);

        gsap.set([dot1, dot2], { opacity: 0 });

        dotAnim1 && dotAnim1.kill();
        dotAnim2 && dotAnim2.kill();
      }

      function startDots() {

        // dot1 → left to right
        dotAnim1 = gsap.to({}, {
          duration: 15,
          repeat: -1,
          ease: "none",
          onUpdate() {
            const p = startOffset + (endOffset - startOffset) * this.progress();
            move(dot1, p);
          }
        });

        // dot2 → right to left
        dotAnim2 = gsap.to({}, {
          duration: 15,
          repeat: -1,
          ease: "none",
          onUpdate() {
            const p = endOffset - (endOffset - startOffset) * this.progress();
            move(dot2, p);
          }
        });
      }

      resetAll();

      const tl = gsap.timeline({
        scrollTrigger: {
          trigger: section,
          start: "top 60%",
          end: "bottom top",
          toggleActions: "play none none none",

          onEnter: () => {
            resetAll();
            tl.restart();
          },

          onLeaveBack: () => {
            resetAll();
          },

          invalidateOnRefresh: true
        }
      });

      // ✅ FIX: animate from LEFT → RIGHT
      tl.to(path, {
        strokeDashoffset: 0,
        duration: 2,
        ease: "power2.out"
      });

      tl.to([dot1, dot2], {
        opacity: 1,
        duration: 0.3,
        onComplete: startDots
      });

    });

  }

  initLineAnimation();
// LINE ANIMATION END



});