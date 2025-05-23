function showNotificationPreview(el) {
  const title = el.dataset.title;
  const date = el.dataset.date;
  const body = el.dataset.body;

  document.getElementById("preview-title").innerText = title;
  document.getElementById("preview-date").innerText = date;
  document.getElementById("preview-body").innerText = body;
}

function toggleRoleDropdown() {
  const dropdown = document.getElementById("dropdownRoleContent");
  const arrow = document.getElementById("dropdownArrow");
  dropdown.classList.toggle("hidden");
  arrow.classList.toggle("custom-rotate-neg90");
}

function selectRole(value, text) {
  const display = document.getElementById("selectedRole");
  const hiddenInput = document.getElementById("SelectedRole");

  display.textContent = text;
  hiddenInput.value = value;

  $(hiddenInput).valid();

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
