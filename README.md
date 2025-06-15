# 🌱⚡ Agri-Energy Connect

**Agri-Energy Connect** is a web-based platform designed to bridge the gap between sustainable agriculture and renewable energy. The application empowers farmers, energy innovators, researchers, and investors to connect, collaborate, trade, and learn through a centralized digital ecosystem.

Built using **ASP.NET Core MVC**, the platform offers features such as:
- A green energy marketplace 🛒
- Online training and resources 📚
- Collaborative project funding portals 💰
- Discussion forums and community spaces 🗣️
- Real-time project updates and live events 🔔

---
## Home Page
![Screenshot_15-5-2025_163742_localhost](https://github.com/user-attachments/assets/0b8d3453-b4da-4981-a7f6-e3ca62dc20d0)

## 📌 Key Features

- ✅ Role-based authentication and authorization (Admin, Farmer, Energy Expert, Investor, etc.)
- 🧑‍🌾 Sustainable agriculture knowledge hub
- 🛍️ Marketplace for renewable energy products and services
- 🎓 Online courses and webinar hosting
- 🤝 Project collaboration and funding request tools
- 🧵 Interactive forums and real-time Q&A
- 📊 Admin dashboard for analytics and content management

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC 7
- **Frontend:** Razor Pages, Bootstrap 5, jQuery
- **Database:** SQL Server + Entity Framework Core
- **Authentication:** ASP.NET Identity
- **Real-Time:** SignalR (for live chat, updates)
- **Other:** AJAX, Partial Views, REST APIs

---

## 💻 System Requirements

Before running the application, ensure your development environment includes:

- **.NET SDK:** .NET 7.0 or later  
- **IDE:** Visual Studio 2022+ or VS Code with C# extensions  
- **Database:** SQL Server (LocalDB or SQL Server Express recommended)  
- **Node.js & npm (optional):** For front-end tooling  
- **Git:** To clone the repository  

---

## 📦 Dependencies

This project uses the following NuGet packages:

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.AspNetCore.SignalR`
- `Microsoft.AspNetCore.Authentication.Cookies`
- `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`
- `Microsoft.Extensions.Caching.Memory`

(These are restored automatically when you build the solution.)

---

## 🚀 Getting Started

### 1. Clone the Repository
git clone https://github.com/yourusername/agri-energy-connect.git
cd agri-energy-connect

### 2. Open the Project
Open the solution file `AgriEnergyConnect.sln` in **Visual Studio**.

### 3. Configure the Database
Update the connection string in `appsettings.json`:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AgriEnergyConnectDB;Trusted_Connection=True;"
}

### 4. Apply Migrations and Seed the Database
Open the Package Manager Console and run:
Update-Database

### 5. Run the Application
Click Start in Visual Studio or run the following in the terminal:
dotnet run
```
📂 Project Structure Overview
AgriEnergyConnect/
├── Controllers/           → MVC controllers
├── Models/                → Entity models
├── Views/                 → Razor views
├── Areas/                 → Admin area for role-specific dashboards
├── Data/                  → DbContext and seeding scripts
├── wwwroot/               → Static files (CSS, JS, images)
├── appsettings.json       → Configuration file
└── Program.cs / Startup.cs → App configuration and services
