function showNotificationPreview(el)
{
    const title = el.dataset.title;
    const date = el.dataset.date;
    const body = el.dataset.body;

    document.getElementById('preview-title').innerText = title;
    document.getElementById('preview-date').innerText = date;
    document.getElementById('preview-body').innerText = body;
}

document.querySelectorAll('.parent-checkbox').forEach(parent => {
    parent.addEventListener('change', function () {
        const containerId = this.dataset.childContainerId;
        const childContainer = document.getElementById(containerId);

        if (this.checked) {
            childContainer.style.display = 'block';
        } else {
            childContainer.style.display = 'none';

            // Uncheck all children
            childContainer.querySelectorAll('.child-checkbox').forEach(child => {
                child.checked = false;
            });
        }
    });
});