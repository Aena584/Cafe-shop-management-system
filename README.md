# A&S Cafe Shop Management System (C# Windows Forms)

## How to run
1. Install **Visual Studio 2022** (the purple one) with the ".NET desktop development" workload, or install the **.NET 8 SDK**.
2. Double-click `CafeShop.csproj` (opens in Visual Studio) and press **F5**.
   Or in a terminal inside this folder: `dotnet run`
3. Login: **admin / admin123**

Note: Windows Forms runs on Windows only. The plain "VS Code" editor has no form designer, but this project builds the whole UI in code, so `dotnet run` works from there too.

## Features
- Login screen with the A&S logo and show-password option
- Dashboard: today's sales, total orders, menu items, total revenue, recent orders
- New Order: click item buttons, + / - / remove, discount %, 5% tax, auto total
- Pay and print receipt (also saved in the `Receipts` folder)
- Menu Manager: add, update, delete items (saved in menu.txt)
- Sales History: all sales, filter by date, total (saved in sales.txt)

## File guide (what to say at the exhibition)
| File | Job |
|------|-----|
| Program.cs | Starts the app: load data, show login, open main form |
| Theme.cs | Colors, logo drawing, button and table styling |
| Data.cs | 3 data classes + loading/saving text files |
| LoginForm.cs | Login window |
| MainForm.cs | Sidebar + 4 pages (Dashboard, Order, Menu, Sales) |

## Likely viva questions
1. **Which concepts did you use?** Classes and objects, properties, collections (List, BindingList), LINQ, events, file handling, exception-safe input checking (TryParse), static classes.
2. **What is an event?** Something the user does (click). `button.Click += handler;` means "run this method when clicked".
3. **Why BindingList?** A table linked to a BindingList updates automatically when items are added or removed.
4. **How is data saved?** Each record is one line in a text file, fields separated by `|`. `File.WriteAllLines` saves, `File.ReadAllLines` loads.
5. **What is LINQ?** Short code to filter and total lists, e.g. `cart.Sum(l => l.Total)`.
6. **How is the logo made?** Drawn with GDI+ (`Graphics`, `FillEllipse`, `DrawString`) in `Theme.MakeLogo`.
7. **Why a static class `DataStore`?** One shared copy of the menu and sales used by all forms.
8. **How is the total calculated?** Subtotal minus discount, plus 5% tax on the discounted amount.
9. **How could it be improved?** Database (SQL Server or SQLite), multiple users and roles, printing to a real printer, charts.
