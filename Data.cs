using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace CafeShop
{
    // ---------- MODEL CLASSES (just hold data) ----------

    public class MenuItemData
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderLine
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total { get { return Price * Qty; } }   // calculated automatically
    }

    public class SaleRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Cashier { get; set; }
        public decimal Total { get; set; }
        public string Items { get; set; }
    }

    // ---------- STORAGE (saves to simple text files, no database needed) ----------

    static class DataStore
    {
        static string MenuFile  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "menu.txt");
        static string SalesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sales.txt");

        public static BindingList<MenuItemData> Menu = new BindingList<MenuItemData>();
        public static List<SaleRecord> Sales = new List<SaleRecord>();

        public static void Load()
        {
            Menu.Clear();
            Sales.Clear();

            // ----- menu -----
            if (File.Exists(MenuFile))
            {
                foreach (string line in File.ReadAllLines(MenuFile))
                {
                    string[] p = line.Split('|');          // Name|Category|Price
                    if (p.Length == 3)
                        Menu.Add(new MenuItemData { Name = p[0], Category = p[1],
                                 Price = decimal.Parse(p[2], CultureInfo.InvariantCulture) });
                }
            }
            else
            {
                AddDefault("Espresso", "Beverages", 250);
                AddDefault("Cappuccino", "Beverages", 350);
                AddDefault("Latte", "Beverages", 380);
                AddDefault("Mocha", "Beverages", 420);
                AddDefault("Cold Coffee", "Beverages", 400);
                AddDefault("Iced Tea", "Beverages", 280);
                AddDefault("Green Tea", "Beverages", 200);
                AddDefault("Chocolate Cake", "Desserts", 450);
                AddDefault("Brownie", "Desserts", 300);
                AddDefault("Cheesecake", "Desserts", 500);
                AddDefault("Club Sandwich", "Food", 550);
                AddDefault("Chicken Burger", "Food", 650);
                AddDefault("French Fries", "Food", 300);
                AddDefault("Pasta", "Food", 700);
                SaveMenu();
            }

            // ----- sales -----
            if (File.Exists(SalesFile))
            {
                foreach (string line in File.ReadAllLines(SalesFile))
                {
                    string[] p = line.Split('|');          // Id|Date|Cashier|Total|Items
                    if (p.Length == 5)
                        Sales.Add(new SaleRecord
                        {
                            Id = int.Parse(p[0]),
                            Date = DateTime.Parse(p[1], CultureInfo.InvariantCulture),
                            Cashier = p[2],
                            Total = decimal.Parse(p[3], CultureInfo.InvariantCulture),
                            Items = p[4]
                        });
                }
            }
        }

        static void AddDefault(string name, string cat, decimal price)
        {
            Menu.Add(new MenuItemData { Name = name, Category = cat, Price = price });
        }

        public static void SaveMenu()
        {
            List<string> lines = new List<string>();
            foreach (MenuItemData m in Menu)
                lines.Add(m.Name + "|" + m.Category + "|" + m.Price.ToString(CultureInfo.InvariantCulture));
            File.WriteAllLines(MenuFile, lines);
        }

        public static void SaveSales()
        {
            List<string> lines = new List<string>();
            foreach (SaleRecord s in Sales)
                lines.Add(s.Id + "|" + s.Date.ToString("s", CultureInfo.InvariantCulture) + "|" + s.Cashier + "|" +
                          s.Total.ToString(CultureInfo.InvariantCulture) + "|" + s.Items.Replace("|", " "));
            File.WriteAllLines(SalesFile, lines);
        }
    }
}
