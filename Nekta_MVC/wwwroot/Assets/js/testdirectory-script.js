document.addEventListener("DOMContentLoaded", function () {

  const tabs = document.querySelectorAll(".tab-btn");
  const contents = document.querySelectorAll(".tab-content");
  const indicator = document.querySelector(".tab-indicator");

  let prevLeft = 0;
  let prevWidth = 0;

  function moveIndicator(tab, isInitial = false) {
  if (!indicator || !tab) return;

  const rect = tab.getBoundingClientRect();
  const parentRect = tab.parentElement.getBoundingClientRect();

  const newLeft = rect.left - parentRect.left;
  const newWidth = rect.width;

  if (isInitial) {
    indicator.style.transition = "none";
  } else {
    indicator.style.transition = "transform 0.35s cubic-bezier(0.4, 0, 0.2, 1)";
  }

  indicator.style.width = newWidth + "px";
  indicator.style.transform = `translateX(${newLeft}px)`;
}

  function initIndicator() {
    const activeTab = document.querySelector(".tab-btn.active") || tabs[0];

    // DOUBLE RAF = guaranteed layout ready
    requestAnimationFrame(() => {
      requestAnimationFrame(() => {
        moveIndicator(activeTab, true);
      });
    });
  }

  // 🔥 Fix 1: Run after full page load
  window.addEventListener("load", initIndicator);

  // 🔥 Fix 2: Handle resize
  window.addEventListener("resize", () => {
    initIndicator();
  });

  tabs.forEach(tab => {
    tab.addEventListener("click", () => {

      const target = tab.getAttribute("data-tab");

      tabs.forEach(t => t.classList.remove("active"));
      contents.forEach(c => c.classList.remove("active"));

      tab.classList.add("active");
      document.getElementById(target).classList.add("active");

      moveIndicator(tab);
    });
  });

});