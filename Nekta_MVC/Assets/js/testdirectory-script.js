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

    // First load → no animation
    if (isInitial) {
      indicator.style.left = newLeft + "px";
      indicator.style.width = newWidth + "px";
      prevLeft = newLeft;
      prevWidth = newWidth;
      return;
    }

    // Stretch animation
    const startLeft = Math.min(prevLeft, newLeft);
    const endRight = Math.max(prevLeft + prevWidth, newLeft + newWidth);

    indicator.style.transition = "none";
    indicator.style.left = startLeft + "px";
    indicator.style.width = (endRight - startLeft) + "px";

    // Force reflow
    indicator.offsetWidth;

    // Shrink to target
    indicator.style.transition = "all 0.4s ease";
    indicator.style.left = newLeft + "px";
    indicator.style.width = newWidth + "px";

    prevLeft = newLeft;
    prevWidth = newWidth;
  }

  // Initial position
  const activeTab = document.querySelector(".tab-btn.active");
  moveIndicator(activeTab, true);

  tabs.forEach(tab => {
    tab.addEventListener("click", () => {

      const target = tab.getAttribute("data-tab");

      // Switch active
      tabs.forEach(t => t.classList.remove("active"));
      contents.forEach(c => c.classList.remove("active"));

      tab.classList.add("active");
      document.getElementById(target).classList.add("active");

      // Animate indicator
      moveIndicator(tab);

    });
  });

});