# S2-SIMAC 🔔

[![C#](https://img.shields.io/badge/language-C%23-blue.svg)](https://dotnet.microsoft.com/) [![Tailwind CSS](https://img.shields.io/badge/styling-TailwindCSS-38bdf8.svg)](https://tailwindcss.com/) [![SignalR](https://img.shields.io/badge/realtime-SignalR-orange.svg)](https://dotnet.microsoft.com/apps/aspnet/signalr) ![License](https://img.shields.io/badge/license-MIT-green.svg)

## 📋 Description
S2-SIMAC is a robust notification and management system designed for industrial or organizational environments. It leverages **ASP.NET Core** for backend operations, **SignalR** for real-time communication, and a modern **Tailwind CSS**-powered frontend to provide a responsive and feature-rich administrative dashboard.

The system facilitates monitoring device statuses, managing organization-specific roles/permissions, and visualizing analytical data regarding system events.

---

## 📑 Table of Contents
1. [Features](#features)
2. [Tech Stack](#tech-stack)
3. [Installation](#installation)
4. [Usage](#usage)
5. [Project Structure](#project-structure)
6. [Contributing](#contributing)
7. [License](#license)

---

## ✨ Features
- 🔄 **Real-time Notifications**: Uses SignalR to push instant alerts to users without page refreshes.
- 📊 **Interactive Dashboards**: Integrated with Chart.js to visualize Event Frequencies, Device Activity, and Event Type distributions.
- 🔐 **Role-Based Access Control (RBAC)**: Comprehensive management of users, roles, and fine-grained permissions.
- ⚙️ **Administrative Panel**: CRUD operations for Accounts, Organizations, Devices, and System configuration.
- 🎨 **Responsive UI**: Custom Tailwind CSS styling with glassmorphism effects and dynamic gradients.

---

## 🛠 Tech Stack
| Component | Technology |
| :--- | :--- |
| **Backend** | C#, .NET, ASP.NET Core MVC |
| **Real-time** | Microsoft SignalR |
| **Frontend** | JavaScript (ES6+), Chart.js |
| **Styling** | Tailwind CSS v4 |
| **Database** | SQL Server (via custom DAL) |

---

## 🚀 Installation
1. **Clone the repository**:
   `git clone https://github.com/IGVasilev19/S2-SIMAC.git`
2. **Database Setup**:
   Execute the provided `SQLQueries.sql` file in your SQL Server instance to initialize the database schema.
3. **Configure Settings**:
   Update the connection strings in `NotificationApp/NotificationApp/appsettings.json`.
4. **Run the Project**:
   Open the `.sln` file in Visual Studio and run the `NotificationApp` project.

---

## 💡 Usage
### Real-time Monitoring
The system automatically joins users to organizational groups via SignalR. When an event occurs, it is pushed to the client and rendered in the inbox without reloading.

### Analytics
Access the `/Analytics/` endpoint to view graphical data:
- `GetEventFrequency`: Line chart showing trends over time.
- `GetEventsPerDevice`: Bar chart comparing device performance.
- `GetEventTypeDistribution`: Pie chart showing alert category breakdown.

---

## 📂 Project Structure
- `NotificationApp/BLL/`: Business Logic Layer (Models, Services).
- `NotificationApp/DAL/`: Data Access Layer (Repositories, Interfaces).
- `NotificationApp/NotificationApp/`: Web UI (Controllers, Views, Hubs, wwwroot).
- `NotificationApp/Service/`: Service abstraction layer for business processes.
