//------------- Two left Image Intro Animation-------------- //
gsap.registerPlugin(ScrollTrigger);

window.addEventListener('load', () => {

  const section = document.querySelector('.bc-experience-section');
  if (!section) return;

  const backPhoto = section.querySelector('.nourish-photo-back');
  const frontPhoto = section.querySelector('.nourish-photo-front');
  if (!backPhoto || !frontPhoto) return;

  gsap.set(backPhoto, {
    opacity: 0,
    x: -150   // starts off to the left
  });
  gsap.set(frontPhoto, {
    opacity: 0,
    x: 150    // starts off to the right
  });

  const tl = gsap.timeline({
    scrollTrigger: {
      trigger: section,
      start: 'top 70%',
      end: 'bottom 20%',
      toggleActions: 'restart none none reverse',
      invalidateOnRefresh: true
      // markers: true, // uncomment to debug the trigger lines while testing
    }
  });

  tl.to(backPhoto, {
      opacity: 1,
      x: 0,
      duration: 1.2,
      ease: 'power3.out'
    })
    .to(frontPhoto, {
      opacity: 1,
      x: 0,
      duration: 1.2,
      ease: 'power3.out'
    }, '<0.15'); // starts 0.15s after back photo begins, so they arrive close together but not simultaneously

  ScrollTrigger.refresh();
});

//------------- Two left Image Intro Animation-------------- //




gsap.registerPlugin(ScrollTrigger);

function buildCurvePath(curveHeight, flatY, width, totalHeight){
   const halfW = width / 2;
   if (curveHeight <= 0.5){
      return `M0,${flatY} L${width},${flatY} L${width},${totalHeight} L0,${totalHeight} Z`;
   }
   const radius = (curveHeight / 2) + (halfW * halfW) / (2 * curveHeight);
   return `M0,${flatY} A${radius},${radius} 0 0,1 ${width},${flatY} L${width},${totalHeight} L0,${totalHeight} Z`;
}

window.addEventListener('load', () => {

   const wrap = document.querySelector('.curveSvg-bg');
   const bgPath = document.querySelector('.curvePath');
   const clipPath = document.querySelector('.curveClipPath');
   if (!wrap || !bgPath) return;

   const maxCurve = parseFloat(bgPath.dataset.maxCurve) || 140;
   const flatY = 144;
   const width = 1000;
   const totalHeight = 400;

   const state = { curve: 0 };

   const applyCurve = (val) => {
      const d = buildCurvePath(val, flatY, width, totalHeight);
      bgPath.setAttribute('d', d);
      if (clipPath) clipPath.setAttribute('d', d);
   };

   applyCurve(0); // start flat/straight

   gsap.to(state, {
      curve: maxCurve,
      duration: 1.3,
      ease: 'power3.inOut',
      onUpdate: () => applyCurve(state.curve),
      scrollTrigger: {
         trigger: wrap.closest('section'),
         start: 'top 70%',
         end: 'bottom 20%',
         toggleActions: 'restart none none reverse',
         invalidateOnRefresh: true
         // markers: true, // uncomment to debug the trigger lines while testing
      }
   });
});


//outdoor dome section
gsap.registerPlugin(ScrollTrigger);

function buildCurvePath(curveHeight, flatY, width, totalHeight){
   const halfW = width / 2;
   if (curveHeight <= 0.5){
      return `M0,${flatY} L${width},${flatY} L${width},${totalHeight} L0,${totalHeight} Z`;
   }
   const radius = (curveHeight / 2) + (halfW * halfW) / (2 * curveHeight);
   return `M0,${flatY} A${radius},${radius} 0 0,1 ${width},${flatY} L${width},${totalHeight} L0,${totalHeight} Z`;
}

function buildNormalizedClipPath(curveHeight, flatY, width, totalHeight){
   const halfW = width / 2;
   const nFlatY = flatY / totalHeight;
   if (curveHeight <= 0.5){
      return `M0,${nFlatY} L1,${nFlatY} L1,1 L0,1 Z`;
   }
   const radius = (curveHeight / 2) + (halfW * halfW) / (2 * curveHeight);
   const nRx = radius / halfW;
   const nRy = radius / totalHeight;
   return `M0,${nFlatY} A${nRx},${nRy} 0 0,1 1,${nFlatY} L1,1 L0,1 Z`;
}

window.addEventListener('load', () => {

   const wrap = document.querySelector('.curveSvg-bg');
   const bgPath = document.querySelector('.curvePath');
   const clipPath = document.querySelector('.curveClipPath');
   if (!wrap || !bgPath) return;

   const section = wrap.closest('section');
   const copy = wrap.querySelector('.oce-copy');

const maxCurve = parseFloat(bgPath.dataset.maxCurve) || 420;
const flatY = 300;
const width = 1000;
const totalHeight = 500;

   const state = { curve: 0 };

   const applyCurve = (val) => {
      bgPath.setAttribute('d', buildCurvePath(val, flatY, width, totalHeight));
      if (clipPath) clipPath.setAttribute('d', buildNormalizedClipPath(val, flatY, width, totalHeight));
   };

   applyCurve(0);
   if (copy) gsap.set(copy, { opacity: 0, y: 20 });

   const tl = gsap.timeline({
      scrollTrigger: {
         trigger: section,
         start: 'top 70%',
         end: 'bottom 20%',
         toggleActions: 'restart none none reverse',
         invalidateOnRefresh: true
      }
   });

   tl.to(state, {
         curve: maxCurve,
         duration: 1.3,
         ease: 'power3.inOut',
         onUpdate: () => applyCurve(state.curve)
      })
      .to(copy, { opacity: 1, y: 0, duration: 0.7, ease: 'power2.out' }, '-=0.6');

   ScrollTrigger.refresh();
});