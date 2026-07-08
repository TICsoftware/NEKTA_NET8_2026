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