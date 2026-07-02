
 
const menuToggle = document.getElementById("menuToggle");
const mainNav = document.getElementById("mainNav");
const dropdowns = document.querySelectorAll(".nav-dropdown");

const isDesktop = () => window.innerWidth >= 1024;

// Mobile menu toggle
// Guarded: some pages (e.g. the home layout) render without the
// site header, so #menuToggle/#mainNav won't exist there. Without
// this check the script threw on load and every later block below
// (sliders, modal, marquee, animations) silently never ran.
if (menuToggle && mainNav) {
    menuToggle.addEventListener("click", (e) => {
        e.stopPropagation();
        const isOpen = mainNav.classList.toggle("is-open");
        menuToggle.setAttribute("aria-expanded", isOpen);
        menuToggle
            .querySelector(".menu-icon-open")
            ?.classList.toggle("hidden", isOpen);
        menuToggle
            .querySelector(".menu-icon-close")
            ?.classList.toggle("hidden", !isOpen);
        if (!isOpen) closeAllDropdowns();
    });
}

dropdowns.forEach((dropdown) => {
    const trigger = dropdown.querySelector(".dropdown-trigger");
    if (!trigger) return;
    let closeTimer = null;

    // MOBILE: click to open/close accordion
    trigger.addEventListener("click", (e) => {
        if (isDesktop()) return; 
        e.stopPropagation();
        const isOpen = dropdown.classList.contains("is-open");
        closeAllDropdowns();
        if (!isOpen) {
            dropdown.classList.add("is-open");
            trigger.setAttribute("aria-expanded", "true");
        }
    });
 
    // DESKTOP: hover to open/close, with a small delay so moving
    // from trigger -> panel across the gap doesn't close it
    dropdown.addEventListener("mouseenter", () => {
        if (!isDesktop()) return;
        clearTimeout(closeTimer);
        closeAllDropdowns();
        dropdown.classList.add("is-open");
        trigger.setAttribute("aria-expanded", "true");
    });
 
    dropdown.addEventListener("mouseleave", () => {
        if (!isDesktop()) return;
        closeTimer = setTimeout(() => {
            dropdown.classList.remove("is-open");
            trigger.setAttribute("aria-expanded", "false");
        }, 150); // small buffer so it doesn't feel twitchy
    });
});
 
function closeAllDropdowns() {
    dropdowns.forEach((dropdown) => {
        dropdown.classList.remove("is-open");
        dropdown
            .querySelector(".dropdown-trigger")
            ?.setAttribute("aria-expanded", "false");
    });
}

function closeMobileMenu() {
    if (!menuToggle || !mainNav) return;
    mainNav.classList.remove("is-open");
    menuToggle.setAttribute("aria-expanded", "false");
    menuToggle.querySelector(".menu-icon-open")?.classList.remove("hidden");
    menuToggle.querySelector(".menu-icon-close")?.classList.add("hidden");
}
 
document.addEventListener("click", (e) => {
    const clickedInsideHeader = e.target.closest(".site-header");
    if (!clickedInsideHeader) {
        closeAllDropdowns();
        closeMobileMenu();
    } else if (!e.target.closest(".nav-dropdown") && !isDesktop()) {
        closeAllDropdowns();
    }
});
 
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeAllDropdowns();
        closeMobileMenu();
    }
});
 
 





// Header animation
window.addEventListener("load",()=>{

const tl = gsap.timeline();


/* LOGO */

tl.fromTo(
".header-logo",

{
opacity:0,
y:-20,
scale:.96
},

{
opacity:1,
y:0,
scale:1,
duration:.45,
ease:"power3.out"
}
);



/* MENU */

tl.fromTo(
".header-menu > *",

{
opacity:0,
y:-15
},

{
opacity:1,
y:0,
duration:.35,
stagger:.05,
ease:"power2.out"
},

"-=.25"
);



/* BUTTON */

tl.fromTo(
".header-btn",

{
opacity:0,
y:-10,
scale:.96
},

{
opacity:1,
y:0,
scale:1,
duration:.35,
ease:"back.out(1.4)"
},

"-=.15"
);

});



(function () {
    "use strict";

    const prefersReducedMotion = window.matchMedia(
        "(prefers-reduced-motion: reduce)",
    ).matches;

    const qs = (selector, scope = document) => scope.querySelector(selector);
    const qsa = (selector, scope = document) => [...scope.querySelectorAll(selector)];
    const hasGSAP = () => window.gsap && window.ScrollTrigger;

    function initImages() {
        qsa("img").forEach((img, index) => {
            if (index > 2 && !img.hasAttribute("loading")) {
                img.loading = "lazy";
            }
            img.decoding = "async";
        });

        const decorativeSources = [
            "/arrows/",
            "/Clients/",
            "logo.png",
            "footerLogo.png",
            "assistant.png",
            "leafimg.png",
            "turnImg.png",
        ];

        qsa(".feature-card img, .edgeSwiper img, .insight-card img")
            .filter((img) => {
                const source = img.getAttribute("src") || "";
                return !decorativeSources.some((part) => source.includes(part));
            })
            .forEach((img) => {
                img.classList.add("js-image-parallax");
            });
    }

    function setExpanded(button, expanded) {
        button?.setAttribute("aria-expanded", String(expanded));
    }
    function initLenis() {
        if (!window.Lenis || prefersReducedMotion) return;

        const lenis = new Lenis({
            duration: 1.1,
            easing: (t) => Math.min(1, 1.001 - Math.pow(2, -10 * t)),
            smoothWheel: true,
        });

        lenis.on("scroll", () => {
            window.ScrollTrigger?.update();
        });

        function raf(time) {
            lenis.raf(time);
            requestAnimationFrame(raf);
        }

        requestAnimationFrame(raf);
    }

    function initContactModal() {
        const modal = qs("#contactModal");
        if (!modal) return;

        const panel = qs(".modal-panel", modal);
        const closeTargets = qsa("[data-modal-close]", modal);
        const openTargets = qsa("[data-modal-open]");
        const form = qs(".enquiry-form", modal);
        let lastFocused = null;

        const closeMobileNav = () => {
            const mobileMenu = qs("#mobileMenu");
            const menuToggle = qs("#menuToggle");
            const openIcon = qs(".menu-icon-open", menuToggle);
            const closeIcon = qs(".menu-icon-close", menuToggle);

            if (mobileMenu) mobileMenu.hidden = true;
            setExpanded(menuToggle, false);
            menuToggle?.setAttribute("aria-label", "Open navigation menu");
            openIcon?.classList.remove("hidden");
            closeIcon?.classList.add("hidden");
        };

        const openModal = () => {
            lastFocused = document.activeElement;
            closeMobileNav();
            modal.hidden = false;
            document.body.classList.add("modal-open");
 
            if (hasGSAP() && !prefersReducedMotion) {
                gsap.fromTo(
                    panel,
                    { autoAlpha: 0, x: 60 },          // starts off to the right
                    {
                        autoAlpha: 1,
                        x: 0,
                        duration: 0.42,
                        ease: "power3.out",
                    },
                );
            }
 
            requestAnimationFrame(() => {
                panel?.focus();
            });
        };
 
        const closeModal = () => {
            if (modal.hidden) return;
 
            const finish = () => {
                modal.hidden = true;
                document.body.classList.remove("modal-open");
                if (lastFocused && typeof lastFocused.focus === "function") {
                    lastFocused.focus();
                }
            };
 
            if (hasGSAP() && !prefersReducedMotion) {
                gsap.to(panel, {
                    autoAlpha: 0,
                    x: 60,                            // exits back off to the right
                    duration: 0.24,
                    ease: "power2.out",
                    onComplete: () => {
                        gsap.set(panel, { clearProps: "all" });
                        finish();
                    },
                });
            } else {
                finish();
            }
        };
 

        openTargets.forEach((trigger) => {
            trigger.addEventListener("click", openModal);
        });

        closeTargets.forEach((target) => {
            target.addEventListener("click", closeModal);
        });

        document.addEventListener("keydown", (event) => {
            if (event.key === "Escape" && !modal.hidden) {
                closeModal();
            }
        });

        form?.addEventListener("submit", (event) => {
            event.preventDefault();
            closeModal();
        });
    }

    function initSliders() {
        if (!window.Swiper) return;

        new Swiper(".mySwiper", {
            slidesPerView: 1,
            spaceBetween: 30,
            loop: true,
            pagination: {
                el: ".swiper-pagination-custom",
                clickable: true,
                renderBullet: (index, className) =>
                    index < 3 ? `<span class="${className}"></span>` : "",
            },
            navigation: {
                nextEl: ".swiper-button-next-custom",
                prevEl: ".swiper-button-prev-custom",
            },
            breakpoints: {
                640: { slidesPerView: 1 },
                768: { slidesPerView: 2 },
                1024: { slidesPerView: 3 },
            },
        });

        const edgeSwiper = new Swiper(".edgeSwiper", {
            slidesPerView: 1,
            spaceBetween: 0,
            loop: true,
            centeredSlides: true,
            navigation: {
                nextEl: ".edge-swiper-next",
                prevEl: ".edge-swiper-prev",
            },
            breakpoints: {
                290: { slidesPerView: 1.1, centeredSlides: true, spaceBetween: 2 },
                768: { slidesPerView: 2.6, centeredSlides: true, spaceBetween: 30 },
            },
            on: {
                init: updateEdgeArrows,
                slideChange: updateEdgeArrows,
            },
        });

        function updateEdgeArrows(swiper = edgeSwiper) {
            qsa(".edgeArrow").forEach((item) => item.classList.add("hidden"));
            qs(".edgeArrow", swiper.slides[swiper.activeIndex])?.classList.remove(
                "hidden",
            );
        }

        const testimonialSwiper = new Swiper(".testimonialSwiper", {
            slidesPerView: 1,
            spaceBetween: 32,
            centeredSlides: true,
            loop: true,
            pagination: {
                el: ".testimonial-pagination",
                clickable: true,
                renderBullet: (index, className) =>
                    index < 2 ? `<span class="${className}"></span>` : "",
            },
            navigation: {
                nextEl: ".testimonial-next",
                prevEl: ".testimonial-prev",
            },
            breakpoints: {
                640: { slidesPerView: 2, centeredSlides: false },
                1024: { slidesPerView: 3.2, centeredSlides: false },
                1280: { slidesPerView: 3.8, centeredSlides: false },
            },
            on: {
                init: updateTestimonialDepth,
                slideChange: updateTestimonialDepth,
            },
        });

        function updateTestimonialDepth(swiper = testimonialSwiper) {
            qsa(".group", swiper.el).forEach((card) => {
                card.classList.remove("is-muted");
            });

            qs(".group", swiper.slides[swiper.activeIndex])?.classList.add(
                "is-muted",
            );
        }

        new Swiper(".insightsSwiper", {
            slidesPerView: 1.1,
            spaceBetween: 20,
            centeredSlides: true,
            loop: true,
            breakpoints: {
                768: { slidesPerView: 2.2, centeredSlides: false },
                1024: {
                    slidesPerView: 3,
                    centeredSlides: false,
                    spaceBetween: 30,
                },
            },
        });
    }

    document.addEventListener("DOMContentLoaded", () => {
        initImages();
        initLenis();
        initContactModal();
        initSliders();
        window.ScrollTrigger?.refresh();
    });
})();


// Client Slider
document.addEventListener("DOMContentLoaded", () => {
    const wrapper = document.getElementById("marqueeWrapper");
    const track = document.getElementById("marqueeTrack");
 
    // Duplicate the items once so the loop has no visible seam
    track.innerHTML += track.innerHTML;
 
    // Style items based on screen size (4 / 3 / 2 visible)
    function setItemWidths() {
        let widthPercent;
        if (window.innerWidth >= 1024) widthPercent = 25;       // 4 visible
        else if (window.innerWidth >= 768) widthPercent = 33.333; // 3 visible
        else widthPercent = 50;                                  // 2 visible
 
        document.querySelectorAll(".marquee-item").forEach(item => {
            item.style.flex = "0 0 auto";
            item.style.width = `${(wrapper.offsetWidth * widthPercent) / 100}px`;
            item.style.display = "flex";
            item.style.alignItems = "center";
            item.style.justifyContent = "center";
            item.style.padding = "0 2rem";
        });
 
        document.querySelectorAll(".marquee-item img").forEach(img => {
            img.style.height = "3rem";
            img.style.filter = "grayscale(100%)";
            img.style.transition = "filter 0.3s ease";
        });
    }
 
    setItemWidths();
    window.addEventListener("resize", setItemWidths);
 
    let position = 0;
    let speed = 2;          // pixels per frame — increase for faster scroll
    let isPaused = false;
    let halfWidth = 0;
    let inView = true; // performance fix: don't animate when marquee is off-screen
 
    function calcHalfWidth() {
        halfWidth = track.scrollWidth / 2; // since content is duplicated
    }
    calcHalfWidth();
    window.addEventListener("resize", calcHalfWidth);
 
    // Pause the marquee while it's scrolled out of view or the tab is
    // in the background — no point moving pixels nobody can see, and
    // this stops the loop from silently burning CPU/battery forever.
    const marqueeObserver = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            inView = entry.isIntersecting;
        });
    });
    marqueeObserver.observe(wrapper);
 
    function animate() {
        if (!isPaused && inView && !document.hidden) {
            position -= speed;
            if (Math.abs(position) >= halfWidth) {
                position = 0; // seamless reset right at the duplicate point
            }
            track.style.transform = `translateX(${position}px)`;
        }
        requestAnimationFrame(animate);
    }
    requestAnimationFrame(animate);
 
    wrapper.addEventListener("mouseenter", () => {
        isPaused = true;
    });
    wrapper.addEventListener("mouseleave", () => {
        isPaused = false;
    });
 
    // grayscale hover-to-color effect on individual logos
    track.addEventListener("mouseover", (e) => {
        if (e.target.tagName === "IMG") e.target.style.filter = "grayscale(0%)";
    });
    track.addEventListener("mouseout", (e) => {
        if (e.target.tagName === "IMG") e.target.style.filter = "grayscale(100%)";
    });
});
 

document.addEventListener("DOMContentLoaded", () => {

const aboutSection =
document.querySelector(".about-section");

if (!aboutSection) return;

const collage =
aboutSection.querySelector(
".image-collage-sway"
);

if (!collage) return;

const itemEls = [
...collage.querySelectorAll(
".sync-pan-item"
)];

const leafEl =
aboutSection.querySelector(
".leaf-motion"
);

const lerp =
(a,b,t)=>
a+(b-a)*t;

let sectionHovered=false;
let hoveredItem=null;
let swayLoopRunning=false; // performance fix: track if the rAF loop is active



// IMAGE
const items =
itemEls.map((el)=>{

const inner =
el.querySelector(
".sync-pan-inner"
);

if(inner){

inner.style.position =
"absolute";

inner.style.left =
"50%";

inner.style.top =
"50%";

inner.style.width =
"125%";

inner.style.height =
"125%";

inner.style.objectFit =
"cover";

inner.style.objectPosition =
"center";

inner.style.transform =
`
translate(
-50%,
-50%
)
`;

inner.style.willChange =
"transform";

}

return{

el,

inner,

dir:
el.dataset.dir||
"rtl",

currentX:0,

targetX:0

};

});





// LEAF (OLD)
const leaf =
leafEl
?{

el:leafEl,

startX:0,
startY:0,

endX:350,
endY:150,

currentProgress:0,

targetProgress:0,

currentOpacity:0,

targetOpacity:0,

currentRotate:-14,

targetRotate:-14

}

:null;





aboutSection.addEventListener(
"mouseenter",
()=>{
sectionHovered=true;
if(!swayLoopRunning){
swayLoopRunning=true;
animate();
}
}
);

aboutSection.addEventListener(
"mouseleave",
()=>{
sectionHovered=false;
hoveredItem=null;
}
);




collage.addEventListener(
"mousemove",

(e)=>{

hoveredItem=
e.target.closest(
".sync-pan-item"
);

if(!swayLoopRunning){
swayLoopRunning=true;
animate();
}

}

);



collage.addEventListener(
"mouseleave",

()=>{

hoveredItem=
null;

}

);





function updateTargets(){

items.forEach(
(item)=>{


item.targetX=

hoveredItem===
item.el

?

(

item.dir==="rtl"

?

-22

:

22

)

:0;

});



if(leaf){

const active=

sectionHovered||

hoveredItem;


leaf.targetProgress=

active

?

1

:0;



leaf.targetOpacity=

active

?

1

:0;



leaf.targetRotate=

active

?

12

:-14;

}

}





function animate(){

updateTargets();




// IMAGE
items.forEach(
(item)=>{


item.currentX=

lerp(

item.currentX,

item.targetX,

0.045

);



if(item.inner){

item.inner.style.transform=

`
translate(
calc(
-50% +
${item.currentX}px
),

-50%
)
`;

}

});






// LEAF
if(leaf){

leaf.currentProgress=

lerp(
leaf.currentProgress,
leaf.targetProgress,
0.03
);


leaf.currentOpacity=

lerp(
leaf.currentOpacity,
leaf.targetOpacity,
0.03
);


leaf.currentRotate=

lerp(
leaf.currentRotate,
leaf.targetRotate,
0.03
);


const p=

leaf.currentProgress*

leaf.currentProgress*

(
3-
2*
leaf.currentProgress
);


const x=

leaf.startX+

(
leaf.endX-
leaf.startX
)

*
p;


const y=

leaf.startY+

(
leaf.endY-
leaf.startY
)

*
p;


leaf.el.style.opacity=

leaf.currentOpacity;


leaf.el.style.transform=

`
translate3d(
${x}px,
${y}px,
0
)

rotate(
${leaf.currentRotate}deg
)
`;

}



// performance fix: once everything has settled back to rest and
// nothing is being hovered, stop scheduling frames instead of
// looping forever in the background. mouseenter/mousemove above
// will restart the loop the moment it's needed again.
const settled = items.every(
(item)=> Math.abs(item.currentX-item.targetX) < 0.05
) && (!leaf || Math.abs(leaf.currentProgress-leaf.targetProgress) < 0.001);

if(!sectionHovered && !hoveredItem && settled){
swayLoopRunning=false;
return;
}

requestAnimationFrame(
animate
);

}



swayLoopRunning=true;
animate();

});




//All H2 Animation

document.querySelectorAll(".text-reveal").forEach((el)=>{

const text = el.textContent.trim();

el.innerHTML = "";

text.split("").forEach((letter,index)=>{

const span=document.createElement("span");

span.classList.add("char");

span.innerHTML=
letter===" "
? "&nbsp;"
: letter;

span.style.animationDelay=
`${index*0.05}s`;

el.appendChild(span);

});


const observer =
new IntersectionObserver((entries)=>{

entries.forEach((entry)=>{

if(entry.isIntersecting){

entry.target.classList.remove("active");

/* restart animation */
setTimeout(()=>{
entry.target.classList.add("active");
},50);

}

/* scroll up → reset */
else{

entry.target.classList.remove("active");

}

});

},{
threshold:0.5
});

observer.observe(el);

});


/* ==========================
   ABOUT IMAGE REVEAL
========================== */

gsap.registerPlugin(ScrollTrigger);



/* initial state */

gsap.set([
".collage-item-top",
".collage-turn"
], {
opacity: 0
});


/* bottom image visible */

gsap.set(".collage-item-bottom",{
opacity:1,
scale:1
});




const aboutTimeline = gsap.timeline({

scrollTrigger: {
trigger: ".about-section",
start: "top 72%",
toggleActions: "play none none none"
}

});




/* DECORATIVE IMAGE */

aboutTimeline.fromTo(

".collage-turn",

{
opacity: 0,
scale: .92
},

{
opacity: 1,
scale: 1,

duration: 1,

ease: "expo.out"
}

);





/* TOP IMAGE → RIGHT TO POSITION */

aboutTimeline.fromTo(

".collage-item-top",

{
opacity: 0,
x: 140,
scale: 1.08
},

{
opacity: 1,
x: 0,
scale: 1,

duration: 1.8,

ease: "expo.out"
},

"-=.4"

);

/* ======================
   PREMIUM PLAYGROUND
====================== */
document.addEventListener("DOMContentLoaded", () => {

    gsap.registerPlugin(ScrollTrigger);

    const section = document.querySelector("#playgrounds-section");

    if (!section) return;

    // Sirf original slides
    const cards = gsap.utils.toArray(
        "#playgrounds-section .swiper-slide:not(.swiper-slide-duplicate)"
    );

    gsap.from(cards, {

        opacity: 0,

        y: 120,

        scale: 0.96,

        rotationX: 8,

        force3D: true,

        transformOrigin: "center bottom",

        duration: 1.15,

        ease: "power4.out",

        stagger: {
            each: 0.18,
            from: "start"
        },

        scrollTrigger: {

            trigger: section,

            start: "top 65%",

            once: true,

            invalidateOnRefresh: true

        }

    });

});


/* ======================
   INSIGHT SECTION
====================== */
document.addEventListener("DOMContentLoaded", () => {

    gsap.registerPlugin(ScrollTrigger);

    const section = document.querySelector(".insightsSwiper");

    if (!section) return;

    const cards = gsap.utils.toArray(".insightsSwiper .swiper-slide");

    // Initial state
    gsap.set(cards, {
        opacity: 0,
        y: 120,
        scale: 0.9,
        rotateX: 8,
        transformPerspective: 1000,
        transformOrigin: "center bottom"
    });

    const tl = gsap.timeline({

        scrollTrigger: {

            trigger: section,

            start: "top 78%",

            once: true

        }

    });

    tl.to(cards, {

        opacity: 1,

        y: 0,

        scale: 1,

        rotateX: 0,

        duration: 1.15,

        ease: "power4.out",

        stagger: {

            each: 0.22

        }

    });

});




/* ======================
   FOOTER SECTION
====================== */

document.addEventListener("DOMContentLoaded", () => {

gsap.registerPlugin(ScrollTrigger);

const footer =
document.querySelector("footer");

if(!footer) return;


/* reset */

gsap.set(
[
".footer-link",
".social-link",
".legal-link"
],
{
clearProps:"all"
}
);


const tl =
gsap.timeline({

scrollTrigger:{

trigger:footer,

start:"top 85%",

once:true

}

});



/* columns */

tl.from(

footer.querySelectorAll(
".md\\:col-span-4, .footer-quick-links, .md\\:col-span-3, .md\\:col-span-2"
),

{

opacity:0,

y:100,

duration:1.1,

stagger:.18,

ease:"expo.out"

}

);



/* links */

tl.from(

".footer-link",

{

opacity:0,

y:18,

duration:.45,

stagger:.04,

ease:"power2.out"

},

"-=.6"

);



/* social */

tl.from(

".social-link",

{

opacity:0,

scale:.85,

duration:.4,

stagger:.05,

ease:"power2.out"

},

"-=.4"

);



/* bottom */

tl.from(

".footer-bottom",

{

opacity:0,

y:30,

duration:.7,

ease:"power2.out"

},

"-=.3"

);

});