document.addEventListener("DOMContentLoaded", () => {


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


// --------------------------------------------
  // whatgainSwiper
  // --------------------------------------------

  document.querySelectorAll(".whatgain-slider").forEach((sliderSS) => {

  const swiperElSS = sliderSS.querySelector(".whatgainSwiper");
  const nextBtnSS  = sliderSS.querySelector(".swiper-button-next");
  const prevBtnSS  = sliderSS.querySelector(".swiper-button-prev");

  const realSlidesCountSS = swiperElSS.querySelectorAll(".swiper-slide").length;

  const swiperSS = new Swiper(swiperElSS, {
    loop: false,
    speed: 800,
    slidesPerView: 3,
    spaceBetween: 30,

    observer: true,
    observeParents: true,
    resizeObserver: true,

    navigation: {
      nextEl: nextBtnSS,
      prevEl: prevBtnSS,
    },

    breakpoints: {
      0: { slidesPerView: 1, spaceBetween: 15 },
      768: { slidesPerView: 2, spaceBetween: 30 },
      1024: { slidesPerView: 3, spaceBetween: 30 }
    },

    on: {
      init: function () {
        updateNav(this);
      },
      resize: function () {
        updateNav(this);
      },
      breakpoint: function () {
        updateNav(this);
      }
    }
  });

  function getPerView(swiperSS) {
    if (swiperSS.currentBreakpoint && swiperSS.params.breakpoints) {
      return swiperSS.params.breakpoints[swiperSS.currentBreakpoint].slidesPerView;
    }
    return swiperSS.params.slidesPerView;
  }

  function updateNav(swiperSS) {
    let perView = getPerView(swiperSS);

    if (!perView || perView === "auto") perView = 1;

    if (realSlidesCountSS <= perView) {
      nextBtnSS.classList.add("hidden");
      prevBtnSS.classList.add("hidden");
      swiperSS.allowTouchMove = false;
    } else {
      nextBtnSS.classList.remove("hidden");
      prevBtnSS.classList.remove("hidden");
      swiperSS.allowTouchMove = true;
    }
  }


  // ==========================
  // 🔥 GSAP FAN ANIMATION (3 CARDS)
  // ==========================
  if (window.matchMedia("(min-width: 1024px)").matches) {

    gsap.registerPlugin(ScrollTrigger);

    const slides = sliderSS.querySelectorAll(".swiper-slide:not(.swiper-slide-duplicate)");

    // ✅ 3-card fan layout
    const startX     = ["80%", "0%", "-80%"];
    const startY     = ["5%",  "0%",  "5%"];
    const startScale = [0.85,  1,     0.85];
    const startRot   = [-6,    0,     6];
    const startBlur  = ["6px", "0px", "6px"]; 

    slides.forEach((slide, i) => {
      gsap.set(slide, {
        x: startX[i] ?? "0%",
        y: startY[i] ?? "0%",
        scale: startScale[i] ?? 0.9,
        rotation: startRot[i] ?? 0,
        filter: `blur(${startBlur[i] ?? "4px"})`,
        opacity: 1,
        transformOrigin: "center bottom",
      });
    });

    const tl = gsap.timeline({
      scrollTrigger: {
        trigger: sliderSS,
        start: "top 60%",
        end: "top 20%",
        scrub: 1.5,
      }
    });

    slides.forEach((slide) => {
      tl.to(slide, {
        x: "0%",
        y: "0%",
        scale: 1,
        rotation: 0,
         filter: "blur(0px)", 
        opacity: 1,
        ease: "power3.out",
        duration: 1,
      }, 0);
    });

  }

});


// --------------------------------------------
  // Current Opportunities
// --------------------------------------------

if (window.matchMedia("(min-width: 1024px)").matches) {


    // ------------------------------
    // IMAGE PARALLAX (SUBTLE)
    // ------------------------------
    gsap.fromTo(".currentopportunities-img-wrapper",
  {
    scale: 0.6,   // start small
    opacity: 1
  },
  {
    scale: 1,     // original size
    opacity: 1,
    ease: "power2.out",
    scrollTrigger: {
      trigger: ".currentopportunities-img-wrapper",
      start: "top 80%",
      end: "top 50%",
      scrub: 1.5,
       toggleActions: "play none none reverse"
    }
  }
);


    // ------------------------------
    // TABLE CARD REVEAL
    // ------------------------------
    gsap.from(".currentopportunities-table", {
        opacity: 1,
        y: 80,
        scale: 0.97,
        duration: 0.9,
        ease: "power3.out",
        scrollTrigger: {
            trigger: ".currentopportunities-table",
            start: "top 85%",
            toggleActions: "play none none reverse"
        }
    });

    // ------------------------------
    // ROW STAGGER ANIMATION
    // ------------------------------
    gsap.from(".currentopportunities-table-row", {
        opacity: 0,
        y: 40,
        duration: 0.6,
        ease: "power2.out",
        stagger: 0.15,
        scrollTrigger: {
            trigger: ".currentopportunities-table",
            start: "top 80%",
            toggleActions: "play none none reverse"
        }
    });

}




/* ---------------------------------------------
   ODOMETER ANIMATION FOR COUNTERS
--------------------------------------------- */
const BASE_ROLLS = 2;              // minimum full 0-9 cycles per digit
const EXTRA_ROLLS_PER_POS = 1;     // extra cycles added towards leftmost digits
const BASE_DURATION = 900;         // ms for rightmost digit
const DURATION_PER_ROLL = 220;     // ms per full 10-digit roll

/* ---------------------------------------------
   FORMAT NUMBER WITH COMMAS
--------------------------------------------- */
function formatNumberString(nStr) {
  const num = Number(nStr);
  if (isNaN(num)) return "0";

  const abs = Math.abs(num);
  const [intPartRaw, decPartRaw] = abs.toString().split(".");

  // format integer with commas
  const intPart = intPartRaw.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

  // no decimals â†’ return integer only
  if (!decPartRaw) return intPart;

  // trim trailing zeros in decimal
  const decPart = decPartRaw.replace(/0+$/, "");

  // all decimals were zero (e.g. .00)
  if (!decPart) return intPart;

  return `${intPart}.${decPart}`;
}

/* ---------------------------------------------
   BUILD ODOMETER DOM
--------------------------------------------- */
function buildOdometer(counterEl) {
  const rawTarget = counterEl.getAttribute("data-target") || "0";
  const suffix = counterEl.getAttribute("data-suffix") || "";
  const targetStr = formatNumberString(rawTarget);

  counterEl.textContent = "";

  const odometer = document.createElement("span");
  odometer.className = "counter-odometer";

  const chars = targetStr.split("");

  for (let i = 0; i < chars.length; i++) {
    const char = chars[i];

    /* -------- COMMA (STATIC) -------- */
    if (char === "," || char === ".") {
  const staticChar = document.createElement("span");
  staticChar.className = "odometer-separator";
  staticChar.textContent = char;
  odometer.appendChild(staticChar);
  continue;
}

    /* -------- DIGIT (ANIMATED) -------- */
    const digit = parseInt(char, 10);

    const slot = document.createElement("span");
    slot.className = "odometer-digit";

    const column = document.createElement("span");
    column.className = "odometer-column";

    // count numeric digits only (ignore commas)
    const numericIndex =
       chars.slice(i).filter(c => c !== "," && c !== ".").length - 1;

    const rolls = BASE_ROLLS + numericIndex * EXTRA_ROLLS_PER_POS;

    for (let r = 0; r <= rolls; r++) {
      for (let d = 0; d < 10; d++) {
        const line = document.createElement("span");
        line.className = "odometer-digit-line";
        line.textContent = d;
        column.appendChild(line);
      }
    }

    slot.appendChild(column);
    odometer.appendChild(slot);

    slot._finalIndex = rolls * 10 + digit;
    slot._rolls = rolls;
  }

  /* -------- SUFFIX (STATIC) -------- */
  if (suffix) {
    const suf = document.createElement("span");
    suf.className = "counter-suffix";
    suf.textContent = suffix;
    odometer.appendChild(suf);
  }

  counterEl.appendChild(odometer);

  return Array.from(odometer.querySelectorAll(".odometer-digit"));
}

/* ---------------------------------------------
   ANIMATE DIGITS
--------------------------------------------- */
function animateOdometerSlots(slots) {
  if (!slots.length) return;

  const firstLine = slots[0].querySelector(".odometer-digit-line");
  const digitHeight = firstLine
    ? firstLine.getBoundingClientRect().height
    : 0;

  slots.forEach((slot, idx) => {
    const column = slot.querySelector(".odometer-column");
    const finalIndex = slot._finalIndex;

    const duration =
      BASE_DURATION + slot._rolls * DURATION_PER_ROLL;

    const totalSlots = slots.length;
    const staggerFactor = 0.08;
    const delay = Math.round(
      duration * staggerFactor * (totalSlots - idx - 1)
    );

    column.style.transition = `transform ${duration}ms cubic-bezier(.22,.9,.35,1) ${delay}ms`;
    column.style.transform = "translateY(0px)";

    const offset = finalIndex * digitHeight;

    setTimeout(() => {
      column.style.transform = `translateY(-${offset}px)`;
    }, 20);
  });
}

/* ---------------------------------------------
   INIT WITH INTERSECTION OBSERVER
--------------------------------------------- */
const counters = document.querySelectorAll(".counter");
const stateMap = new WeakMap();

if (counters.length) {
  const io = new IntersectionObserver(
    (entries, obs) => {
      entries.forEach(entry => {
        if (!entry.isIntersecting) return;

        const el = entry.target;
        if (stateMap.get(el)) {
          obs.unobserve(el);
          return;
        }

        const slots = buildOdometer(el);
        void el.offsetHeight; // force reflow
        animateOdometerSlots(slots);

        stateMap.set(el, true);
        obs.unobserve(el);
      });
    },
    { threshold: 0.3 }
  );

  counters.forEach(c => io.observe(c));
}



/* ---------------------------------------------
   shareprofile SECTION ANIMATION
--------------------------------------------- */
if (window.matchMedia("(min-width: 1024px)").matches) {
    const tl = gsap.timeline({
    scrollTrigger: {
        trigger: ".shareprofile-section",
        start: "top 75%",
        toggleActions: "play none none reverse"
    }
    });

    // 🔹 Boxes (clean stagger + better easing)
    tl.from(".box-itm", {
    y: 100,
    opacity: 0,
    duration: 0.7,
    ease: "power4.out",
    stagger: 0.3
    })

    // 🔹 Image reveal (slight scale + smoother feel)
    .from(".shareprofile-image .sp-imgwrap", {
    y: 100,
    opacity: 0,
    scale: 0.75,
    duration: 1,
    ease: "power4.out",
    stagger: 0.3
    }, "-=0.4");

}


/* ---------------------------------------------
   TRIGGER SCROLLTO
--------------------------------------------- */
document.querySelectorAll("[target-trigger]").forEach((btn) => {

  btn.onclick = () => {

    const target = btn.getAttribute("target-trigger");
    const section = document.querySelector(target);

    if (!section) return;

    const top =
      section.getBoundingClientRect().top +
      window.scrollY - 80;

    window.scrollTo({
      top,
      behavior: "smooth"
    });

  };

});


});

