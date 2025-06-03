function getColor(label) {
  const colors = {
    "Device Status": "rgba(255, 99, 132, 0.7)", // ID 3
    Security: "rgba(54, 162, 235, 0.7)", // ID 4
    Maintenance: "rgba(255, 206, 86, 0.7)", // ID 5
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
      new Chart(ctx, {
        type: "line",
        data: {
          labels: data.labels,
          datasets: data.datasets.map((ds) => ({
            label: ds.label,
            data: ds.data,
            borderColor: getColor(ds.label),
            backgroundColor: getColor(ds.label),
            tension: 0.3,
            fill: false,
            borderWidth: 2,
            pointRadius: 5,
            pointHoverRadius: 7,
          })),
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
              labels: {
                font: { size: 14, weight: "bold" },
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

function renderEventsPerDeviceChart() {
  const ctx = document.getElementById("eventsPerDeviceChart");
  if (!ctx) return;

  fetch("/Analytics/GetEventsPerDevice")
    .then((res) => res.json())
    .then((data) => {
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
              text: "Events per Device",
              font: { size: 18 },
            },
            tooltip: {
              callbacks: {
                label: (ctx) => `${ctx.dataset.label}: ${ctx.raw}`,
              },
            },
            legend: {
              position: "top",
              labels: {
                font: { size: 12 },
              },
            },
          },
          scales: {
            y: {
              beginAtZero: true,
              ticks: { precision: 0 },
            },
            x: {
              ticks: {
                font: { size: 12 },
              },
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
      new Chart(ctx, {
        type: "pie",
        data: {
          labels: data.labels,
          datasets: data.datasets.map((ds) => ({
            ...ds,
            backgroundColor: ds.backgroundColor.map(getColor),
          })),
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: "Event Types Distribution",
              font: { size: 18 },
            },
            legend: {
              position: "bottom",
              labels: { font: { size: 12 } },
            },
            tooltip: {
              callbacks: {
                label: (ctx) => `${ctx.label}: ${ctx.raw}`,
              },
            },
          },
        },
      });
    });
}
