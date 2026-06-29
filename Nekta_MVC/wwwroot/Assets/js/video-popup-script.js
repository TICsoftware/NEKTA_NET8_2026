document.addEventListener('DOMContentLoaded', function () {

  const modal = document.getElementById("videoModal");
  const videoBox = document.getElementById("videoBox");
  const closeBtn = document.getElementById("closeVideo");

  const mp4 = document.getElementById("mp4Video") || null;
  const youtube = document.getElementById("youtubeVideo") || null;

  // 👉 STOP if modal structure not present
  if (!modal || !videoBox) return;

  let originRect = null;
  let clickPoint = { x: 0, y: 0 };
  let isOpen = false;

  /* =========================
     OPEN
  ========================= */
  document.addEventListener("click", (e) => {

    const trigger = e.target.closest("[data-video-trigger]");
    if (!trigger) return;

    e.preventDefault();
    if (isOpen) return;

    isOpen = true;

    // 👉 click position
    clickPoint.x = e.clientX;
    clickPoint.y = e.clientY;

    // 👉 fallback origin
    const circle = trigger.querySelector(".play-btn") || trigger;
    const rect = circle.getBoundingClientRect();

    originRect = {
      top: rect.top,
      left: rect.left,
      width: rect.width,
      height: rect.height,
      borderRadius: window.getComputedStyle(circle).borderRadius
    };

    modal.classList.remove("hidden");

    if (typeof lenis !== "undefined") lenis.stop();

    // 👉 start tiny from click
    gsap.set(videoBox, {
      top: clickPoint.y,
      left: clickPoint.x,
      width: 20,
      height: 20,
      xPercent: -50,
      yPercent: -50,
      borderRadius: "50%",
      position: "fixed",
      overflow: "hidden",
      willChange: "transform, width, height"
    });

    // 👉 VIDEO SETUP (SAFE)
    const type = trigger.dataset.videoType;
    const src = trigger.dataset.videoSrc;

    if (type === "mp4") {

      if (mp4 && src) {
        mp4.src = src;
        mp4.play();
        mp4.classList.remove("hidden");
      }

      if (youtube) {
        youtube.classList.add("hidden");
        youtube.src = "";
      }

    } else {

      let videoId = "";

      if (src && src.includes("youtu.be")) {
        videoId = src.split("/").pop().split("?")[0];
      } else if (src && src.includes("youtube.com")) {
        try {
          const url = new URL(src);
          videoId = url.searchParams.get("v");
        } catch (err) {
          console.warn("Invalid YouTube URL");
        }
      }

      if (youtube && videoId) {
        youtube.src = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
        youtube.classList.remove("hidden");
      }

      if (mp4) {
        mp4.classList.add("hidden");
        mp4.pause();
        mp4.src = "";
      }
    }

    // 👉 RESPONSIVE SIZE
    const isMobile = window.innerWidth < 640;

    let width = isMobile
      ? window.innerWidth * 0.92
      : Math.min(window.innerWidth * 0.9, 1000);

    let height = width * 9 / 16;

    const maxHeight = window.innerHeight * 0.85;

    if (height > maxHeight) {
      height = maxHeight;
      width = height * 16 / 9;
    }

    // 👉 ANIMATION
    gsap.to(videoBox, {
      duration: 0.7,
      ease: "power4.inOut",
      top: "50%",
      left: "50%",
      width: width,
      height: height,
      borderRadius: isMobile ? "12px" : "16px"
    });

    gsap.to(".modal-bg", {
      opacity: 1,
      backdropFilter: "blur(14px)",
      duration: 0.5
    });

  });


  /* =========================
     CLOSE
  ========================= */
  function closeModal() {

    if (!isOpen) return;

    // 👉 SAFE cleanup
    if (mp4) {
      mp4.pause();
      mp4.src = "";
    }

    if (youtube) {
      youtube.src = "";
    }

    // 👉 animate back to click
    gsap.to(videoBox, {
      duration: 0.5,
      ease: "power3.inOut",
      top: clickPoint.y,
      left: clickPoint.x,
      width: 20,
      height: 20,
      xPercent: -50,
      yPercent: -50,
      borderRadius: "50%",
      onComplete: () => {
        modal.classList.add("hidden");

        if (typeof lenis !== "undefined") lenis.start();

        isOpen = false;
      }
    });

    gsap.to(".modal-bg", {
      opacity: 0,
      backdropFilter: "blur(0px)",
      duration: 0.3
    });
  }


  /* =========================
     EVENTS (SAFE)
  ========================= */

  if (closeBtn) {
    closeBtn.addEventListener("click", closeModal);
  }

  const modalBg = modal.querySelector(".modal-bg");

  if (modalBg) {
    modalBg.addEventListener("click", closeModal);
  }

  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") closeModal();
  });

});