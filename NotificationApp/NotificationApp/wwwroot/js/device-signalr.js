const orgId = document.getElementById("orgId")?.value;

if (orgId) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.on("DeviceStatusUpdated", ({ deviceId, status }) => {
        console.log("📶 Device status update received", deviceId, status);

        const row = document.querySelector(`tr[data-device-id='${deviceId}']`);
        if (!row) return;

        const statusCell = row.querySelector("td:last-child");
        if (!statusCell) return;

        statusCell.innerHTML = ""; // Clear

        const container = document.createElement("div");
        container.classList.add("flex", "gap-2", "items-center", "justify-center", "pl-15");

        const circle = document.createElement("div");
        circle.className = `w-[25px] h-[25px] shadow-md rounded-full ${status === 0 ? "bg-green-500" : "bg-red-500"}`;

        const label = document.createElement("p");
        label.classList.add("font-bold");
        label.textContent = status === 0 ? "Online" : "Offline";

        container.appendChild(circle);
        container.appendChild(label);
        statusCell.appendChild(container);
    });

    connection.start()
        .then(() => {
            console.log("✅ SignalR connected (Devices Panel)");
            return connection.invoke("JoinOrganization", orgId);
        })
        .catch(err => console.error("❌ SignalR connection failed", err));
}
