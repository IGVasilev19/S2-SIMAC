function showNotificationPreview(el) {
  console.log("DATA ATTRIBUTES:", el.dataset);

  const title = el.dataset.title?.trim();
  const date = el.dataset.date?.trim();
  const body = el.dataset.body?.trim();

  const displayTitle = title ? title : "Title";
  const displayDate = date ? date : "Date";
  const displayBody = body ? body : "Notification details";

  document.getElementById("preview-title").innerText = displayTitle;
  document.getElementById("preview-date").innerText = displayDate;
  document.getElementById("preview-body").innerText = displayBody;

  document.getElementById('notification-action-container').classList.remove('hidden');

  // Highlight selected notification
  document.querySelectorAll(".notification").forEach((div) => {
    div.classList.remove("bg-gray-200", "rounded");
  });
  el.querySelector(".notification").classList.add("bg-gray-200", "rounded");
}

function markNotificationAsRead(notificationId) {
    fetch('/System/MarkNotificationAsRead', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(notificationId)
    })
    .then(response => {
        if (!response.ok) {
            console.error('Failed to mark notification as read');
        }
    })
    .catch(error => console.error('Error:', error));
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
