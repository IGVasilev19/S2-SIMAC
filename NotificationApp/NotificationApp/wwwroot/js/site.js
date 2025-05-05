function showNotificationPreview(el)
{
    const title = el.dataset.title;
    const date = el.dataset.date;
    const body = el.dataset.body;

    document.getElementById('preview-title').innerText = title;
    document.getElementById('preview-date').innerText = date;
    document.getElementById('preview-body').innerText = body;
}