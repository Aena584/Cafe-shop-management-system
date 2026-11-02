# ☕ A&S Cafe Shop Management System

A colorful desktop application for managing a cafe: taking orders, calculating bills, printing receipts, managing the menu and tracking sales. Built with **C#** and **Windows Forms** (.NET 8).

> Final project for the **Advanced Programming** course.

---

## 📸 Screenshots

<!-- Add your screenshots to a "screenshots" folder and update the paths below -->
| Login | Dashboard |
|-------|-----------|
| ![Login](screenshots/login.png) | ![Dashboard](screenshots/dashboard.png) |

| New Order | Menu Manager |
|-----------|--------------|
| ![Order](screenshots/order.png) | ![Menu](screenshots/menu.png) |

---

## ✨ Features

- 🔐 **Login screen** with show-password option
- 📊 **Dashboard** with today's sales, total orders, menu items, total revenue and recent orders
- 🛒 **New Order** screen: click items to add them, change quantity (+ / -), remove items, apply a discount % and automatic 5% tax
- 🧾 **Receipt generation** after payment (also saved as a text file in the `Receipts` folder)
- 🍰 **Menu Manager**: add, update and delete items by category (Beverages, Desserts, Food)
- 📅 **Sales History**: view all sales, filter by date, see total sales
- 💾 **Simple storage** in text files, so no database setup is needed
- 🎨 Custom purple theme and a logo drawn in code (no image files required)

---

## 🛠️ Tech Stack

- C#
- Windows Forms (.NET 8)
- LINQ, BindingList, file handling (text files)
- GDI+ for drawing the logo

---

## 🚀 Getting Started

### Requirements
- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 **or** VS Code (with the C# extension)

### Run the project

```bash
git clone https://github.com/<your-username>/<your-repo-name>.git
cd <your-repo-name>
dotnet run
```

Or open `CafeShop.csproj` in Visual Studio and press **F5**.

### Demo login

| Username | Password |
|----------|----------|
| admin    | admin123 |

---

## 📁 Project Structure

| File | Purpose |
|------|---------|
| `Program.cs` | Entry point: loads data, shows login, opens the main window |
| `Theme.cs` | Colors, logo drawing and shared UI styling |
| `Data.cs` | Data classes (menu item, order line, sale) and file storage |
| `LoginForm.cs` | Login window |
| `MainForm.cs` | Sidebar and the four pages (Dashboard, New Order, Menu Manager, Sales History) |

---

## 🔮 Future Improvements

- Connect to a database (SQL Server or SQLite)
- Multiple users with roles (admin / cashier)
- Print receipts to a real printer
- Sales charts and reports
- Table management and inventory tracking

---

## 👤 Author

**Aena Arshad**

