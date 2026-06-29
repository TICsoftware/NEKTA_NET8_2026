document.addEventListener("DOMContentLoaded", () => {

  const wrappers = document.querySelectorAll(".readmore-wrapper");

  wrappers.forEach((wrapper, index) => {

    const content = wrapper.querySelector(".readmore-content");
    const btn = wrapper.querySelector(".readmore-toggle");
    const text = btn?.querySelector("span");

    if (!content || !btn || !text) return;

    let expanded = wrapper.classList.contains("expanded");

    // Accessibility
    const contentId = `readmore-content-${index}`;
    content.setAttribute("id", contentId);
    btn.setAttribute("aria-controls", contentId);

    // Get responsive collapsed height
    const getHeight = () => {
      const desktop = parseInt(wrapper.dataset.collapsed) || 300;
      const mobile = parseInt(wrapper.dataset.collapsedMobile) || desktop;
      return window.matchMedia("(max-width: 768px)").matches ? mobile : desktop;
    };

    const update = () => {
      const collapsed = getHeight();

      // Toggle state
      if (expanded) {
        content.style.maxHeight = content.scrollHeight + "px";
        wrapper.classList.add("expanded");
        text.textContent = "Read Less";
        btn.setAttribute("aria-expanded", "true");
      } else {
        content.style.maxHeight = collapsed + "px";
        wrapper.classList.remove("expanded");
        text.textContent = "Read More";
        btn.setAttribute("aria-expanded", "false");
      }

      // 🔥 Key Logic (overflow check)
      if (content.scrollHeight <= collapsed) {
        btn.style.display = "none";
        wrapper.classList.remove("has-overflow"); // ❌ no gradient
      } else {
        btn.style.display = "";
        wrapper.classList.add("has-overflow"); // ✅ show gradient
      }
    };

    // Click toggle
    btn.addEventListener("click", () => {
      expanded = !expanded;
      update();
    });

    // Debounce resize
    let resizeTimer;
    window.addEventListener("resize", () => {
      clearTimeout(resizeTimer);
      resizeTimer = setTimeout(update, 150);
    });

    // Init styles
    content.style.overflow = "hidden";
    content.style.transition = "max-height 0.5s ease";

    update();

  });

});