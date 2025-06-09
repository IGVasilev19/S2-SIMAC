function showNotificationPreview(el) {
  // Fallback if el is a child (like a div or p tag inside <a>)
  const anchor = el.closest("a.notification");
  if (!anchor) return;

  console.log("DATA ATTRIBUTES:", anchor.dataset);

  const id = anchor.dataset.id?.trim();
  const title = anchor.dataset.title?.trim();
  const date = anchor.dataset.date?.trim();
  const body = anchor.dataset.body?.trim();

  const displayTitle = title || "Title";
  const displayDate = date || "Date";
  const displayBody = body || "Notification details";

  document.getElementById("preview-title").innerText = displayTitle;
  document.getElementById("preview-date").innerText = displayDate;
  document.getElementById("preview-body").innerText = displayBody;

  console.log("Setting hidden input to notification id:", id);
  const hiddenInput = document.getElementById("selected-notification-id");
  if (hiddenInput) hiddenInput.value = id;

  const actionContainer = document.getElementById(
    "notification-action-container"
  );
  if (actionContainer) actionContainer.classList.remove("hidden");

  // Highlight selected notification
  document.querySelectorAll("a.notification").forEach((div) => {
    div.classList.remove("bg-gray-200", "rounded");
  });
  anchor.classList.add("bg-gray-200", "rounded");

  const bottomBar = anchor.querySelector("div.h-\\[8px\\]");
  if (bottomBar) {
    bottomBar.classList.remove("bg-[#7AA0FF]");
    bottomBar.classList.add("bg-gray-200");
  }
}

// Copies search bar value for sort button
// Re-enable sort button without affecting dropdowns
document
  .getElementById("sortButton")
  ?.addEventListener("click", function (event) {
    event.preventDefault();

    const searchInput = document.getElementById("searchInput");
    const sortSearchInput = document.getElementById("sortSearchInput");
    const sortForm = document.getElementById("sortForm");

    if (searchInput && sortSearchInput && sortForm) {
      sortSearchInput.value = searchInput.value;
      sortForm.submit();
    }
  });

function markNotificationAsRead(notificationId) {
  fetch("/System/MarkNotificationAsRead", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(notificationId),
  })
    .then((response) => {
      if (!response.ok) {
        console.error("Failed to mark notification as read");
      }
    })
    .catch((error) => console.error("Error:", error));
}

document.querySelectorAll(".parent-checkbox").forEach((parent) => {
  parent.addEventListener("change", function () {
    const containerId = this.dataset.childContainerId;
    const childContainer = document.getElementById(containerId);

    if (this.checked) {
      childContainer.style.display = "block";
    } else {
      childContainer.style.display = "none";

      childContainer.querySelectorAll(".child-checkbox").forEach((child) => {
        child.checked = false;
      });
    }
  });
});

document.getElementById("preview-title").innerText = title;
document.getElementById("preview-date").innerText = date;
document.getElementById("preview-body").innerText = body;

function toggleRoleDropdown() {
  const dropdown = document.getElementById("dropdownRoleContent");
  const arrow = document.getElementById("dropdownArrow");
  dropdown.classList.toggle("hidden");
  arrow.classList.toggle("custom-rotate-neg90");
}

function selectRole(value, text) {
  const display = document.getElementById("selectedRole");
  const hiddenInput = document.getElementById("SelectedRole");
  const hiddenInput2 = document.getElementById("SelectedRoleId");

  display.textContent = text;
  hiddenInput.value = value;
  hiddenInput2.value = text;

  $(hiddenInput).valid();
  $(hiddenInput2).valid();

  toggleRoleDropdown();
}

document.addEventListener("click", function (e) {
  const dropdown = document.getElementById("dropdownRoleContent");
  const toggle = document.querySelector(".toggle-roledropdown");
  if (!toggle.contains(e.target)) {
    dropdown.classList.add("hidden");
    document
      .getElementById("dropdownArrow")
      .classList.remove("custom-rotate-neg90");
  }
});
