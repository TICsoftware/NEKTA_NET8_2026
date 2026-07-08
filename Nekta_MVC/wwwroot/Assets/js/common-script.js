const headerWrap = document.getElementById('headerWrap');
  const navBar = document.getElementById('navBar');
  const navViewport = document.getElementById('navViewport');
  const dropdownAnchor = document.getElementById('dropdownAnchor');
  const desktopNav = document.getElementById('desktopNav');
  let menuItems = [...desktopNav.querySelectorAll('.nav-item[data-menu]')];
  const panels = [...document.querySelectorAll('.nav-panel')];

  let activeMenu = null;
  let prevIndex = null;
  let closeTimer = null;

  function bindDesktopMenuEvents(){
    menuItems.forEach(item => {
      item.addEventListener('mouseenter', () => openMenu(item));
      item.addEventListener('focusin', () => openMenu(item));
      const btn = item.querySelector('button');
      if (!btn) return;
      btn.addEventListener('click', e => {
        e.preventDefault();
        activeMenu === item.dataset.menu ? closeMenu() : openMenu(item);
      });
    });
  }

  function wireMobileAccordions(){
    document.querySelectorAll('[data-accordion]').forEach(row => {
      const trigger = row.querySelector('.accordion-trigger');
      const panel = row.querySelector('.accordion-panel');
      const inner = row.querySelector('.accordion-panel-inner');
      if (!trigger || !panel || !inner) return;

      trigger.addEventListener('click', () => {
        const isOpen = row.classList.contains('open');

        document.querySelectorAll('[data-accordion]').forEach(r => {
          r.classList.remove('open');
          const p = r.querySelector('.accordion-panel');
          const t = r.querySelector('.accordion-trigger');
          if (p) p.style.maxHeight = '0';
          if (t) t.setAttribute('aria-expanded', 'false');
        });

        if (!isOpen) {
          row.classList.add('open');
          panel.style.maxHeight = inner.scrollHeight + 'px';
          trigger.setAttribute('aria-expanded', 'true');
        }
      });
    });
  }

  function measurePanel(panel){
    const clone = panel.cloneNode(true);
    clone.classList.add('is-active');
    clone.style.cssText = 'position:fixed;left:-9999px;top:0;display:block;visibility:hidden;pointer-events:none;';
    document.body.appendChild(clone);
    const w = clone.offsetWidth;
    const h = clone.offsetHeight;
    document.body.removeChild(clone);
    return { w, h };
  }

  function applySize(panel){
    const { w, h } = measurePanel(panel);
    navViewport.style.width = w + 'px';
    navViewport.style.minHeight = h + 'px';
  }

  function showPanel(panel, direction){
    panels.forEach(p => p.classList.remove('is-active','from-start','from-end'));
    panel.classList.add('is-active');
    if (direction) panel.classList.add(direction);
    applySize(panel);
  }

  function positionViewportToItem(item){
    const btn = item.querySelector('button');
    if (!btn) return;
    const btnRect = btn.getBoundingClientRect();
    const anchorRect = dropdownAnchor.getBoundingClientRect();
    const centerX = btnRect.left + (btnRect.width / 2) - anchorRect.left;
    navViewport.style.left = centerX + 'px';
  }

  function openMenu(item){
    clearTimeout(closeTimer);
    const menu = item.dataset.menu;
    const index = Number(item.dataset.index);
    const panel = document.getElementById('panel-' + menu);
    if (!panel) return;

    const direction = activeMenu && prevIndex !== null
      ? (index > prevIndex ? 'from-end' : 'from-start') : '';

    menuItems.forEach(i => {
      i.classList.toggle('open', i === item);
      const btn = i.querySelector('button');
      if (btn) btn.setAttribute('aria-expanded', i === item ? 'true' : 'false');
    });

    positionViewportToItem(item);
    showPanel(panel, direction);
    navViewport.classList.add('open');
    navViewport.setAttribute('aria-hidden', 'false');
    activeMenu = menu;
    prevIndex = index;
  }

  function closeMenu(){
    clearTimeout(closeTimer);
    menuItems.forEach(i => {
      i.classList.remove('open');
      const btn = i.querySelector('button');
      if (btn) btn.setAttribute('aria-expanded', 'false');
    });
    navViewport.classList.remove('open');
    navViewport.setAttribute('aria-hidden', 'true');
    activeMenu = null;
    prevIndex = null;
    closeTimer = setTimeout(() => {
      panels.forEach(p => p.classList.remove('is-active','from-start','from-end'));
      navViewport.style.width = '';
      navViewport.style.minHeight = '';
    }, 160);
  }

  function scheduleClose(){
    clearTimeout(closeTimer);
    closeTimer = setTimeout(closeMenu, 220);
  }

  headerWrap.addEventListener('mouseleave', scheduleClose);
  headerWrap.addEventListener('focusout', e => {
    if (!headerWrap.contains(e.relatedTarget)) scheduleClose();
  });
  dropdownAnchor.addEventListener('mouseenter', () => clearTimeout(closeTimer));

  // Header content is now static in _Header.cshtml; JS handles behavior only.
  menuItems = [...desktopNav.querySelectorAll('.nav-item[data-menu]')];
  bindDesktopMenuEvents();
  wireMobileAccordions();

function updateScrolledState(){
    const isScrolled = window.scrollY > 8;
    navBar.classList.toggle('elevated', isScrolled);
    headerWrap.classList.toggle('scrolled', isScrolled);
  }

  window.addEventListener('scroll', updateScrolledState);
  updateScrolledState(); // set correct state immediately on page load

  const hamburgerBtn = document.getElementById('hamburgerBtn');
  const drawer = document.getElementById('drawer');
  const overlay = document.getElementById('overlay');
  const drawerClose = document.getElementById('drawerClose');

  function openDrawer(){
    drawer.classList.add('active');
    overlay.classList.add('active');
    overlay.setAttribute('aria-hidden', 'false');
    hamburgerBtn.setAttribute('aria-expanded', 'true');
    document.body.style.overflow = 'hidden';
  }
  function closeDrawer(){
    drawer.classList.remove('active');
    overlay.classList.remove('active');
    overlay.setAttribute('aria-hidden', 'true');
    hamburgerBtn.setAttribute('aria-expanded', 'false');
    document.body.style.overflow = '';
  }

  hamburgerBtn.addEventListener('click', openDrawer);
  drawerClose.addEventListener('click', closeDrawer);
  overlay.addEventListener('click', closeDrawer);

  document.addEventListener('keydown', e => {
    if (e.key !== 'Escape') return;
    if (drawer.classList.contains('active')) closeDrawer();
    else closeMenu();
  });

  window.addEventListener('resize', () => {
    const p = panels.find(x => x.classList.contains('is-active'));
    if (p && navViewport.classList.contains('open')) applySize(p);
    if (activeMenu) {
      const activeItem = menuItems.find(i => i.dataset.menu === activeMenu);
      if (activeItem) positionViewportToItem(activeItem);
    }
    document.querySelectorAll('[data-accordion].open').forEach(row => {
      const panel = row.querySelector('.accordion-panel');
      const inner = row.querySelector('.accordion-panel-inner');
      if (!panel || !inner) return;
      panel.style.maxHeight = inner.scrollHeight + 'px';
    });
  });


// --------------------------------------------
// FOOTER YEAR
// --------------------------------------------
document.getElementById("year-foot").innerHTML = (new Date().getFullYear());



(function () {
    "use strict";

    const prefersReducedMotion = window.matchMedia(
        "(prefers-reduced-motion: reduce)",
    ).matches;

    const qs = (selector, scope = document) => scope.querySelector(selector);
    const qsa = (selector, scope = document) => [...scope.querySelectorAll(selector)];
    const hasGSAP = () => window.gsap && window.ScrollTrigger;

    

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

      

       

      

       
    }

    document.addEventListener("DOMContentLoaded", () => {
        initLenis();
        initContactModal();
        window.ScrollTrigger?.refresh();
    });
})();




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
   FOOTER SECTION
====================== */
document.addEventListener("DOMContentLoaded", () => {

const footer =
document.querySelector("footer");

if(!footer) return;

function initFooterAccordions(){
  const sections = [...footer.querySelectorAll('.footer-quick-link-click')];
  if (!sections.length) return;

  const desktopMq = window.matchMedia('(min-width: 768px)');

  function closeAll(){
    sections.forEach(section => {
      section.classList.remove('open');
      const list = section.querySelector('.footer-nav-list');
      const heading = section.querySelector('h4');
      if (list) list.style.maxHeight = '0';
      if (heading) heading.setAttribute('aria-expanded', 'false');
    });
  }

  function resetForDesktop(){
    if (!desktopMq.matches) return;
    sections.forEach(section => {
      section.classList.remove('open');
      const list = section.querySelector('.footer-nav-list');
      if (list) list.style.maxHeight = '';
    });
  }

  sections.forEach(section => {
    const heading = section.querySelector('h4');
    const list = section.querySelector('.footer-nav-list');
    if (!heading || !list) return;

    heading.setAttribute('role', 'button');
    heading.setAttribute('tabindex', '0');
    heading.setAttribute('aria-expanded', 'false');

    const toggle = () => {
      if (desktopMq.matches) return;

      const isOpen = section.classList.contains('open');
      closeAll();

      if (!isOpen) {
        section.classList.add('open');
        list.style.maxHeight = list.scrollHeight + 'px';
        heading.setAttribute('aria-expanded', 'true');
      }
    };

    heading.addEventListener('click', toggle);
    heading.addEventListener('keydown', e => {
      if (e.key !== 'Enter' && e.key !== ' ') return;
      e.preventDefault();
      toggle();
    });
  });

  desktopMq.addEventListener('change', resetForDesktop);
  resetForDesktop();
}

initFooterAccordions();

gsap.registerPlugin(ScrollTrigger);


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