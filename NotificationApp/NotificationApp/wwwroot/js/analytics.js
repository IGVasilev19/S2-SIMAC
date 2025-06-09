function getColor(label) {
  const colors = {
    "Device Status": "rgb(67, 148, 229)", // #4394E5
    Security: "rgb(202, 70, 70)", // #A50228
    Maintenance: "rgb(135, 111, 212)", // #876FD4
  };
  return colors[label] || "rgba(153, 102, 255, 0.7)";
}

document.addEventListener("DOMContentLoaded", function () {
  renderEventFrequencyChart();
  renderEventsPerDeviceChart();
  renderEventTypePieChart();
});

function renderEventFrequencyChart() {
  const ctx = document.getElementById("eventFrequencyChart");
  if (!ctx) return;

  fetch("/Analytics/GetEventFrequency")
    .then((res) => res.json())
    .then((data) => {
      console.log("Frequency Chart Data:", data);

      const allZero = data.datasets.every((ds) =>
        ds.data.every((v) => v === 0)
      );
      if (allZero) {
        console.warn("No frequency data to display.");
        return;
      }

      // ✅ Reverse the labels and dataset data so newest dates are on the right
      const reversedLabels = [...data.labels].reverse();
      const reversedDatasets = data.datasets.map((ds) => ({
        ...ds,
        data: [...ds.data].reverse(),
        borderColor: getColor(ds.label),
        backgroundColor: getColor(ds.label),
        tension: 0.3,
        fill: false,
        borderWidth: 2,
        pointRadius: 5,
        pointHoverRadius: 7,
      }));

      new Chart(ctx, {
        type: "line",
        data: {
          labels: reversedLabels,
          datasets: reversedDatasets,
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: "Event Frequency Over Time",
              font: { size: 18 },
            },
            tooltip: {
              callbacks: {
                title: (ctx) => `Date: ${ctx[0].label}`,
                label: (ctx) => `${ctx.dataset.label}: ${ctx.raw}`,
              },
            },
            legend: {
              position: "top",
              labels: { font: { size: 14, weight: "bold" } },
            },
          },
          scales: {
            y: {
              beginAtZero: true,
              ticks: { stepSize: 1, precision: 0 },
            },
            x: {
              ticks: { maxRotation: 45, minRotation: 45 },
            },
          },
        },
      });
    });
}

function renderEventsPerDeviceChart() {
  const ctx = document.getElementById("eventsPerDeviceChart");
  if (!ctx) return;

  fetch("/Analytics/GetEventsPerDevice")
    .then((res) => res.json())
    .then((data) => {
      console.log("Events Per Device Chart Data:", data);

      const allZero = data.datasets.every((ds) =>
        ds.data.every((val) => val === 0)
      );
      if (allZero) {
        console.warn("No device-related events to display.");
        return;
      }

      new Chart(ctx, {
        type: "bar",
        data: {
          labels: data.labels,
          datasets: data.datasets.map((ds) => ({
            label: ds.label,
            data: ds.data,
            backgroundColor: getColor(ds.label),
            borderColor: getColor(ds.label),
            borderWidth: 1,
          })),
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: "Events Per Device",
              font: { size: 18 },
            },
            legend: {
              position: "top",
              labels: { font: { size: 14, weight: "bold" } },
            },
            tooltip: {
              callbacks: {
                label: (ctx) => `${ctx.dataset.label}: ${ctx.raw}`,
              },
            },
          },
          scales: {
            y: {
              beginAtZero: true,
              ticks: { stepSize: 1, precision: 0 },
            },
            x: {
              ticks: { maxRotation: 45, minRotation: 45 },
            },
          },
        },
      });
    });
}

function renderEventTypePieChart() {
  const ctx = document.getElementById("eventTypePieChart");
  if (!ctx) return;

  fetch("/Analytics/GetEventTypeDistribution")
    .then((res) => res.json())
    .then((data) => {
      console.log("Event Type Pie Chart Data:", data);

      const allZero = data.datasets[0].data.every((val) => val === 0);
      if (allZero) {
        console.warn("No event types to display.");
        return;
      }

      new Chart(ctx, {
        type: "pie",
        data: {
          labels: data.labels,
          datasets: [
            {
              data: data.datasets[0].data,
              backgroundColor: data.datasets[0].backgroundColor,
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: "Event Type Distribution",
              font: { size: 18 },
            },
            tooltip: {
              callbacks: {
                label: (ctx) => `${data.labels[ctx.dataIndex]}: ${ctx.raw}`,
              },
            },
            legend: {
              position: "bottom",
              labels: { font: { size: 14, weight: "bold" } },
            },
          },
        },
      });
    });
}
