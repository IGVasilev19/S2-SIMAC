const orgId = document.getElementById("orgId")?.value;

if (orgId) {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.on("ReceiveNotification", (notification) => {
        console.log("SignalR RECEIVED notification", notification);

        const container = document.getElementById("notificationList");
        if (!container) return;

        const a = document.createElement("a");
        a.href = "#";
        a.className =
            "notification mx-2 mt-2 flex h-[100px] w-[250px] flex-col items-center justify-between rounded-md border-t border-r border-l cursor-pointer";

        a.setAttribute("data-id", notification.notificationId);
        a.setAttribute("data-title", notification.title);
        a.setAttribute("data-date", notification.date);
        a.setAttribute("data-body", notification.content);
        a.setAttribute("data-important", notification.important); // Needed for checking
        a.setAttribute(
            "onclick",
            `showNotificationPreview(this); markNotificationAsRead(${notification.notificationId}); return false`
        );

        a.innerHTML = `
      <div class="notification flex h-full w-full items-center justify-center gap-2 p-2 hover:bg-gray-200 hover:rounded-md hover:cursor-pointer">
        <div class="rounded-4xl h-[25px] w-[30px] border ${notification.important ? "bg-[#FF3131]" : ""
            }"></div>
        <div class="flex h-full w-full flex-col items-center justify-center gap-2">
          <p class="text-sm font-bold select-none">${notification.title}</p>
          <p class="text-[12px] !text-black !font-normal select-none">
            ${new Date(notification.date).toLocaleString()}
          </p>
        </div>
      </div>
      <div class="h-[8px] w-full rounded-b-md border ${notification.read ? "bg-gray-200" : "bg-[#7AA0FF]"
            }"></div>
    `;

        if (notification.important) {
            container.prepend(a);
        } else {
            const notifications = container.querySelectorAll("a.notification");
            const firstNonImportant = Array.from(notifications).find(
                (el) => el.dataset.important === "false"
            );

            if (firstNonImportant) {
                firstNonImportant.before(a);
            } else {
                container.prependChild(a); // Fix this!!!!
            }
        }
    });

    connection
        .start()
        .then(() => {
            console.log("SignalR Connected");
            return connection.invoke("JoinOrganization", orgId);
        })
        .then(() => console.log("Joined org group", orgId))
        .catch((err) => console.error("SignalR error:", err));
}
