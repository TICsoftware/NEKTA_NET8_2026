 document.addEventListener("DOMContentLoaded", function () {

            const form = document.getElementById("enquiryForm");
            const successMessage = document.getElementById("successMessage");

            if (!form || !successMessage) return;

            form.addEventListener("submit", function (e) {
                e.preventDefault();

                // Show success message
                successMessage.classList.remove("hidden");

                // Reset form (optional)
                form.reset();

                // Hide ONLY the message after 4 sec
                setTimeout(() => {
                successMessage.classList.add("hidden");
                }, 4000);

            });

        });