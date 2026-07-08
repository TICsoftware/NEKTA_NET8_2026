/* ======================
   TESTIMONIAL SWIPER
====================== */

(function () {
    const root = document.querySelector(".testimonialSwiper");
    if (!root || typeof Swiper === "undefined") return;

    const wrapper = root.querySelector(".swiper-wrapper");
    if (!wrapper) return;

    const DESKTOP_SLIDES_PER_VIEW = 3.5;

    const originalSlides = [
        ...wrapper.querySelectorAll(":scope > .swiper-slide:not([data-testimonial-clone])"),
    ];
    const ORIGINAL_SLIDE_COUNT = originalSlides.length;
    if (!ORIGINAL_SLIDE_COUNT) return;

    const getMinSlidesForLoop = (perView) => Math.ceil(perView) + 2;
    const minSlidesForLoop = getMinSlidesForLoop(DESKTOP_SLIDES_PER_VIEW);
    const targetSlideCount = Math.max(
        ORIGINAL_SLIDE_COUNT * 2,
        minSlidesForLoop,
    );

    let cloneIndex = 0;
    while (wrapper.querySelectorAll(":scope > .swiper-slide").length < targetSlideCount) {
        const clone = originalSlides[cloneIndex % ORIGINAL_SLIDE_COUNT].cloneNode(true);
        clone.setAttribute("data-testimonial-clone", "true");
        clone.setAttribute("aria-hidden", "true");
        wrapper.appendChild(clone);
        cloneIndex += 1;
    }

    const totalSlides = wrapper.querySelectorAll(":scope > .swiper-slide").length;
    const canLoop = totalSlides >= minSlidesForLoop;
    const START_LOGICAL_INDEX = 0;

    const getSlidesPerView = (desired) =>
        Math.min(desired, Math.max(ORIGINAL_SLIDE_COUNT - 0.5, 1.1));

    let testimonialSwiper = null;
    let hasStarted = false;

    function getCenteredIndexForLogicalSlide(swiper, logicalIndex) {
        const target = String(logicalIndex);
        const middle = Math.floor(swiper.slides.length / 2);
        let bestIndex = middle;
        let bestDistance = Number.POSITIVE_INFINITY;

        swiper.slides.forEach((slide, index) => {
            if (slide.getAttribute("data-swiper-slide-index") !== target) return;
            const distance = Math.abs(index - middle);
            if (distance < bestDistance) {
                bestDistance = distance;
                bestIndex = index;
            }
        });

        return bestIndex;
    }

    function syncLoopLayout(swiper, index, options = {}) {
        if (!swiper || swiper.destroyed) return;

        const targetIndex = index ?? swiper.realIndex ?? 0;
        const preferCenteredClone = Boolean(options.preferCenteredClone);

        swiper.update();

        if (swiper.params.loop) {
            if (preferCenteredClone) {
                const centeredIndex = getCenteredIndexForLogicalSlide(
                    swiper,
                    targetIndex % ORIGINAL_SLIDE_COUNT,
                );
                swiper.slideTo(centeredIndex, 0, false);
            } else {
                swiper.slideToLoop(targetIndex, 0, false);
            }
            swiper.loopFix?.();
        } else {
            swiper.slideTo(targetIndex, 0);
        }

        updateTestimonialDepth(swiper);
    }

    function keepCurrentSlide(swiper) {
        if (!swiper || swiper.destroyed) return;
        const activeSlide = swiper.slides[swiper.activeIndex];
        const logicalIndex = Number(
            activeSlide?.getAttribute("data-swiper-slide-index") ??
                swiper.realIndex ??
                0,
        );

        syncLoopLayout(swiper, logicalIndex, { preferCenteredClone: true });
    }

    function updateTestimonialDepth(swiper = testimonialSwiper) {
        if (!swiper || swiper.destroyed) return;

        swiper.slides.forEach((slide) => {
            const card = slide.querySelector(".testimonial-card");
            if (!card) return;

            const distance = Math.min(Math.abs(slide.progress), 1);
            const opacity = Math.max(0.3, 1 - distance * 0.7);
            const scale = Math.max(0.95, 1 - distance * 0.05);
            const blur = Math.min(1, distance * 1.5);

            card.style.opacity = opacity;
            card.style.transform = `scale(${scale})`;
            card.style.filter = blur > 0.05 ? `blur(${blur}px)` : "none";
            card.classList.toggle("is-muted", distance > 0.35);
        });
    }

    function createSwiper() {
        if (testimonialSwiper) return testimonialSwiper;

        testimonialSwiper = new Swiper(root, {
            initialSlide: 0,
            slidesPerView: getSlidesPerView(1.15),
            spaceBetween: 24,
            centeredSlides: true,
            centerInsufficientSlides: !canLoop,
            loop: canLoop,
            rewind: !canLoop,
            loopAdditionalSlides: Math.ceil(DESKTOP_SLIDES_PER_VIEW),
            loopedSlides: totalSlides,
            speed: 850,
            grabCursor: true,
            watchSlidesProgress: true,
            slideToClickedSlide: true,
            observer: true,
            observeParents: true,
            observeSlideChildren: true,
            pagination: {
                el: ".testimonial-pagination",
                clickable: true,
                renderBullet: (index, className) =>
                    index < ORIGINAL_SLIDE_COUNT
                        ? `<span class="${className}"></span>`
                        : "",
            },
            navigation: {
                nextEl: ".testimonial-next",
                prevEl: ".testimonial-prev",
            },
            breakpoints: {
                640: {
                    slidesPerView: getSlidesPerView(1.8),
                    spaceBetween: 28,
                    centeredSlides: true,
                    centerInsufficientSlides: !canLoop,
                },
                1024: {
                    slidesPerView: DESKTOP_SLIDES_PER_VIEW,
                    spaceBetween: 32,
                    centeredSlides: true,
                    centerInsufficientSlides: false,
                },
                1280: {
                    slidesPerView: DESKTOP_SLIDES_PER_VIEW,
                    spaceBetween: 32,
                    centeredSlides: true,
                    centerInsufficientSlides: false,
                },
            },
            on: {
                touchStart() {
                    root.classList.add("is-dragging");
                },
                touchEnd() {
                    root.classList.remove("is-dragging");
                },
                init(swiper) {
                    requestAnimationFrame(() => {
                        syncLoopLayout(swiper, START_LOGICAL_INDEX, {
                            preferCenteredClone: true,
                        });
                    });
                },
                resize(swiper) {
                    keepCurrentSlide(swiper);
                },
                setTranslate(swiper) {
                    updateTestimonialDepth(swiper);
                },
                slideChange(swiper) {
                    updateTestimonialDepth(swiper);
                },
                transitionEnd(swiper) {
                    if (swiper.params.loop) {
                        swiper.loopFix?.();
                    }
                    updateTestimonialDepth(swiper);
                },
            },
        });

        return testimonialSwiper;
    }

    function startSwiper() {
        if (hasStarted) return;
        hasStarted = true;

        createSwiper();

        requestAnimationFrame(() => {
            requestAnimationFrame(() => {
                syncLoopLayout(testimonialSwiper, START_LOGICAL_INDEX, {
                    preferCenteredClone: true,
                });
            });
        });
    }

    function isInViewport() {
        const rect = root.getBoundingClientRect();
        return rect.top < window.innerHeight * 0.9 && rect.bottom > 0;
    }

    function boot() {
        if (isInViewport()) {
            startSwiper();
            return;
        }

        const viewportObserver = new IntersectionObserver(
            (entries) => {
                if (entries.some((entry) => entry.isIntersecting)) {
                    startSwiper();
                    viewportObserver.disconnect();
                }
            },
            { threshold: 0.05, rootMargin: "120px 0px" },
        );

        viewportObserver.observe(root);
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", boot, { once: true });
    } else {
        boot();
    }
})();
