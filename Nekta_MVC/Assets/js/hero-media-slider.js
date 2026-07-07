/* ==========================================================
   HERO MEDIA SLIDER (image + video slides)
   - Controls (arrows + pagination) only show when there is
     more than one slide.
   - A video slide always plays to the end before the
     slider auto-advances to the next slide.
   ========================================================== */
document.addEventListener("DOMContentLoaded", function () {
  var heroEl = document.querySelector(".hero-media-swiper");
  if (!heroEl || typeof Swiper === "undefined") return;

  var slides = heroEl.querySelectorAll(".hero-slide");
  var controls = document.querySelector(".hero-swiper-controls");
  var captionEl = document.querySelector(".hero-caption-text");
  var IMAGE_DELAY = 5000; // ms an image slide stays before advancing

  function setCaption(slide) {
    if (!captionEl) return;
    captionEl.classList.remove("is-visible");
    window.setTimeout(function () {
      var text = (slide && slide.dataset.caption) || "";
      captionEl.textContent = text;
      captionEl.classList.toggle("is-visible", !!text);
    }, 200);
  }

  // Only one slide: skip Swiper entirely, no controls, just loop the media.
  if (slides.length <= 1) {
    var soloVideo = heroEl.querySelector("video");
    if (soloVideo) {
      soloVideo.loop = true;
      soloVideo.play().catch(function () {});
    }
    setCaption(slides[0]);
    return;
  }

  var heroSwiper = new Swiper(heroEl, {
    loop: true,
    speed: 900,
    effect: "fade",
    fadeEffect: { crossFade: true },
    autoplay: { delay: IMAGE_DELAY, disableOnInteraction: false },
    pagination: { el: ".hero-swiper-pagination", clickable: true },
    navigation: { nextEl: ".hero-swiper-next", prevEl: ".hero-swiper-prev" },
  });

  if (controls) controls.classList.add("is-active");

  function syncActiveSlide() {
    var activeSlide = heroSwiper.slides[heroSwiper.activeIndex];
    setCaption(activeSlide);

    var activeVideo = activeSlide && activeSlide.querySelector("video");

    // Pause/rewind every video that isn't the active one.
    heroEl.querySelectorAll("video").forEach(function (video) {
      if (video !== activeVideo) {
        video.pause();
        video.currentTime = 0;
      }
    });

    if (!activeVideo) {
      heroSwiper.autoplay.start();
      return;
    }

    // Hold the timed autoplay until this video finishes playing.
    heroSwiper.autoplay.stop();
    activeVideo.currentTime = 0;

    var resume = function () {
      activeVideo.removeEventListener("ended", resume);
      heroSwiper.autoplay.start();
      heroSwiper.slideNext();
    };
    activeVideo.addEventListener("ended", resume);

    activeVideo.play().catch(function () {
      // Autoplay blocked (e.g. no user interaction yet) — fall back to timed autoplay.
      heroSwiper.autoplay.start();
    });
  }

  heroSwiper.on("slideChangeTransitionEnd", syncActiveSlide);
  syncActiveSlide();
});
