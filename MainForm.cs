using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CafeShop
{
    public class MainForm : Form
    {
        // ---------- fields (variables shared by the whole form) ----------
        string cashier;
        const decimal TaxRate = 0.05m;                       // 5% tax
        static readonly string[] Categories = { "Beverages", "Desserts", "Food" };

        Panel sidebar, content;
        Panel pageDashboard, pageOrder, pageMenu, pageSales;

        // dashboard
        Label lblToday, lblOrders, lblItems, lblRevenue;
        DataGridView gridRecent;

        // order page
        FlowLayoutPanel itemsPanel;
        ComboBox cmbFilter;
        DataGridView gridCart;
        BindingList<OrderLine> cart = new BindingList<OrderLine>();
        NumericUpDown numDiscount;
        Label lblSub, lblDisc, lblTax, lblTotal;
        decimal subTotal, discountAmt, taxAmt, grandTotal;

        // menu manager page
        DataGridView gridMenu;
        TextBox txtName, txtPrice;
        ComboBox cmbCat;

        // sales page
        DataGridView gridSales;
        DateTimePicker dtpDate;
        Label lblSalesTotal;

        // ---------- constructor: builds the whole window ----------
        public MainForm(string user)
        {
            cashier = user;
            Text = "Cafe Shop Management System - A&S";
            ClientSize = new Size(1200, 720);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Theme.Cream;
            Font = new Font("Segoe UI", 10);
            Icon = Icon.FromHandle(Theme.MakeLogo(64).GetHicon());

            BuildSidebar();

            content = new Panel();
            content.SetBounds(220, 0, 980, 720);
            Controls.Add(content);

            BuildDashboard();
            BuildOrderPage();
            BuildMenuPage();
            BuildSalesPage();

            ShowPage(pageDashboard);
        }

        // ================= SIDEBAR =================
        void BuildSidebar()
        {
            sidebar = new Panel();
            sidebar.SetBounds(0, 0, 220, 720);
            sidebar.BackColor = Theme.DarkPurple;

            PictureBox logo = new PictureBox();
            logo.Image = Theme.MakeLogo(130);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.SetBounds(45, 25, 130, 130);

            Label name = new Label();
            name.Text = "CAFE SHOP\nMANAGEMENT SYSTEM";
            name.ForeColor = Theme.Gold;
            name.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.SetBounds(0, 160, 220, 50);

            Label user = new Label();
            user.Text = "Logged in: " + cashier;
            user.ForeColor = Color.White;
            user.TextAlign = ContentAlignment.MiddleCenter;
            user.SetBounds(0, 570, 220, 25);

            sidebar.Controls.Add(logo);
            sidebar.Controls.Add(name);
            sidebar.Controls.Add(user);

            AddNav("Dashboard", 240, (s, e) => ShowPage(pageDashboard));
            AddNav("New Order", 295, (s, e) => ShowPage(pageOrder));
            AddNav("Menu Manager", 350, (s, e) => ShowPage(pageMenu));
            AddNav("Sales History", 405, (s, e) => ShowPage(pageSales));
            AddNav("Logout", 610, (s, e) =>
            {
                if (MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                    Application.Restart();
            });

            Controls.Add(sidebar);
        }

        void AddNav(string text, int y, EventHandler click)
        {
            Button b = new Button();
            b.Text = text;
            b.SetBounds(0, y, 220, 50);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Theme.Purple;
            b.ForeColor = Color.White;
            b.BackColor = Theme.DarkPurple;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(30, 0, 0, 0);
            b.Font = new Font("Segoe UI", 11);
            b.Cursor = Cursors.Hand;
            b.Click += click;
            sidebar.Controls.Add(b);
        }

        // Shows one page and hides the others.
        void ShowPage(Panel page)
        {
            foreach (Panel p in new Panel[] { pageDashboard, pageOrder, pageMenu, pageSales })
                p.Visible = false;
            page.Visible = true;
            page.BringToFront();

            if (page == pageDashboard) RefreshDashboard();
            if (page == pageOrder) ShowItemButtons();
            if (page == pageSales) LoadSales(false);
        }

        Panel NewPage()
        {
            Panel p = new Panel();
            p.SetBounds(0, 0, 980, 720);
            p.BackColor = Theme.Cream;
            p.Visible = false;
            content.Controls.Add(p);
            return p;
        }

        Label Title(string text)
        {
            Label l = new Label();
            l.Text = text;
            l.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            l.ForeColor = Theme.DarkPurple;
            l.SetBounds(20, 12, 600, 45);
            return l;
        }

        // ================= DASHBOARD =================
        void BuildDashboard()
        {
            pageDashboard = NewPage();
            pageDashboard.Controls.Add(Title("Dashboard"));

            Label welcome = new Label();
            welcome.Text = "Welcome, " + cashier + "!  Have a great day at A&S Cafe.";
            welcome.Font = new Font("Segoe UI", 12);
            welcome.ForeColor = Color.Gray;
            welcome.SetBounds(20, 62, 900, 30);
            pageDashboard.Controls.Add(welcome);

            lblToday   = MakeCard("Today's Sales", Theme.Purple, 20, 110);
            lblOrders  = MakeCard("Total Orders", Theme.Green, 260, 110);
            lblItems   = MakeCard("Menu Items", Theme.Orange, 500, 110);
            lblRevenue = MakeCard("Total Revenue", Theme.DarkPurple, 740, 110);

            Label recent = new Label();
            recent.Text = "Recent Orders";
            recent.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            recent.ForeColor = Theme.DarkPurple;
            recent.SetBounds(20, 250, 400, 30);
            pageDashboard.Controls.Add(recent);

            gridRecent = new DataGridView();
            gridRecent.SetBounds(20, 285, 940, 400);
            Theme.StyleGrid(gridRecent);
            pageDashboard.Controls.Add(gridRecent);
        }

        Label MakeCard(string title, Color color, int x, int y)
        {
            Panel card = new Panel();
            card.SetBounds(x, y, 220, 120);
            card.BackColor = color;

            Label t = new Label();
            t.Text = title;
            t.ForeColor = Color.White;
            t.Font = new Font("Segoe UI", 11);
            t.SetBounds(15, 12, 190, 25);

            Label v = new Label();
            v.Text = "0";
            v.ForeColor = Color.White;
            v.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            v.SetBounds(15, 50, 195, 50);

            card.Controls.Add(t);
            card.Controls.Add(v);
            pageDashboard.Controls.Add(card);
            return v;
        }

        void RefreshDashboard()
        {
            var todaySales = DataStore.Sales.Where(s => s.Date.Date == DateTime.Today);
            lblToday.Text = "Rs " + todaySales.Sum(s => s.Total).ToString("N0");
            lblOrders.Text = DataStore.Sales.Count.ToString();
            lblItems.Text = DataStore.Menu.Count.ToString();
            lblRevenue.Text = "Rs " + DataStore.Sales.Sum(s => s.Total).ToString("N0");

            gridRecent.DataSource = DataStore.Sales.OrderByDescending(s => s.Id).Take(10).ToList();
            if (gridRecent.Columns["Date"] != null)
                gridRecent.Columns["Date"].DefaultCellStyle.Format = "dd-MMM-yyyy hh:mm tt";
        }

        // ================= NEW ORDER PAGE =================
        void BuildOrderPage()
        {
            pageOrder = NewPage();
            pageOrder.Controls.Add(Title("New Order"));

            Label lc = new Label();
            lc.Text = "Category:";
            lc.SetBounds(20, 70, 80, 25);
            cmbFilter = new ComboBox();
            cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilter.SetBounds(100, 66, 200, 30);
            cmbFilter.Items.Add("All");
            cmbFilter.Items.AddRange(Categories);
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => ShowItemButtons();

            itemsPanel = new FlowLayoutPanel();
            itemsPanel.SetBounds(20, 105, 470, 590);
            itemsPanel.AutoScroll = true;
            itemsPanel.BackColor = Color.White;

            // cart table
            gridCart = new DataGridView();
            gridCart.SetBounds(510, 66, 450, 290);
            gridCart.DataSource = cart;
            Theme.StyleGrid(gridCart);

            Button bPlus = Theme.MakeButton("+", Theme.Green);
            bPlus.SetBounds(510, 365, 100, 38);
            bPlus.Click += (s, e) => ChangeQty(1);

            Button bMinus = Theme.MakeButton("-", Theme.Orange);
            bMinus.SetBounds(620, 365, 100, 38);
            bMinus.Click += (s, e) => ChangeQty(-1);

            Button bRemove = Theme.MakeButton("Remove", Theme.Red);
            bRemove.SetBounds(730, 365, 110, 38);
            bRemove.Click += (s, e) => ChangeQty(-9999);

            Button bClear = Theme.MakeButton("Clear", Color.Gray);
            bClear.SetBounds(850, 365, 110, 38);
            bClear.Click += (s, e) => { cart.Clear(); UpdateTotals(); };

            // summary box
            Panel sum = new Panel();
            sum.SetBounds(510, 420, 450, 275);
            sum.BackColor = Color.White;

            lblSub = SummaryRow(sum, "Subtotal", 15);
            lblDisc = SummaryRow(sum, "Discount (%)", 55);
            lblTax = SummaryRow(sum, "Tax (5%)", 95);

            numDiscount = new NumericUpDown();
            numDiscount.Maximum = 100;
            numDiscount.SetBounds(130, 52, 70, 28);
            numDiscount.ValueChanged += (s, e) => UpdateTotals();
            sum.Controls.Add(numDiscount);
            numDiscount.BringToFront();

            Label lt = new Label();
            lt.Text = "TOTAL";
            lt.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lt.ForeColor = Theme.DarkPurple;
            lt.SetBounds(15, 140, 150, 35);
            sum.Controls.Add(lt);

            lblTotal = new Label();
            lblTotal.Text = "Rs 0";
            lblTotal.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTotal.ForeColor = Theme.Purple;
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            lblTotal.SetBounds(180, 135, 250, 40);
            sum.Controls.Add(lblTotal);

            Button bPay = Theme.MakeButton("PAY && PRINT RECEIPT", Theme.Purple);
            bPay.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            bPay.SetBounds(15, 195, 420, 58);
            bPay.Click += (s, e) => Pay();
            sum.Controls.Add(bPay);

            pageOrder.Controls.AddRange(new Control[] { lc, cmbFilter, itemsPanel, gridCart,
                                                         bPlus, bMinus, bRemove, bClear, sum });
        }

        Label SummaryRow(Panel p, string text, int y)
        {
            Label l = new Label();
            l.Text = text;
            l.SetBounds(15, y, 200, 28);
            Label v = new Label();
            v.Text = "Rs 0";
            v.TextAlign = ContentAlignment.MiddleRight;
            v.SetBounds(220, y, 210, 28);
            p.Controls.Add(l);
            p.Controls.Add(v);
            return v;
        }

        Color CategoryColor(string cat)
        {
            if (cat == "Beverages") return Theme.Purple;
            if (cat == "Desserts") return Theme.Orange;
            return Theme.Green;
        }

        // Creates one big button for every menu item.
        void ShowItemButtons()
        {
            if (itemsPanel == null) return;
            itemsPanel.Controls.Clear();
            string cat = cmbFilter.SelectedItem.ToString();

            foreach (MenuItemData item in DataStore.Menu.Where(m => cat == "All" || m.Category == cat))
            {
                Button b = new Button();
                b.Text = item.Name + "\nRs " + item.Price.ToString("N0");
                b.Size = new Size(140, 90);
                b.Margin = new Padding(8);
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = CategoryColor(item.Category);
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
                b.Tag = item;                                   // remember which item this button is for
                b.Click += (s, e) => AddToCart((MenuItemData)((Button)s).Tag);
                itemsPanel.Controls.Add(b);
            }
        }

        void AddToCart(MenuItemData item)
        {
            OrderLine line = cart.FirstOrDefault(l => l.Name == item.Name);
            if (line != null)
                line.Qty++;                                     // already in cart -> increase quantity
            else
                cart.Add(new OrderLine { Name = item.Name, Price = item.Price, Qty = 1 });

            cart.ResetBindings();                               // refresh the table
            UpdateTotals();
        }

        void ChangeQty(int change)
        {
            OrderLine line = gridCart.CurrentRow == null ? null : gridCart.CurrentRow.DataBoundItem as OrderLine;
            if (line == null) return;

            line.Qty += change;
            if (line.Qty <= 0) cart.Remove(line);
            cart.ResetBindings();
            UpdateTotals();
        }

        void UpdateTotals()
        {
            subTotal = cart.Sum(l => l.Total);
            discountAmt = subTotal * numDiscount.Value / 100;
            taxAmt = (subTotal - discountAmt) * TaxRate;
            grandTotal = subTotal - discountAmt + taxAmt;

            lblSub.Text = "Rs " + subTotal.ToString("N0");
            lblDisc.Text = "- Rs " + discountAmt.ToString("N0");
            lblTax.Text = "Rs " + taxAmt.ToString("N0");
            lblTotal.Text = "Rs " + grandTotal.ToString("N0");
        }

        void Pay()
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("The cart is empty!", "A&S Cafe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaleRecord sale = new SaleRecord();
            sale.Id = DataStore.Sales.Count == 0 ? 1 : DataStore.Sales.Max(x => x.Id) + 1;
            sale.Date = DateTime.Now;
            sale.Cashier = cashier;
            sale.Total = grandTotal;
            sale.Items = string.Join(", ", cart.Select(l => l.Name + " x" + l.Qty));

            string receipt = BuildReceipt(sale);

            DataStore.Sales.Add(sale);
            DataStore.SaveSales();

            // also save the receipt as a text file
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Receipts");
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder, "receipt_" + sale.Id + ".txt"), receipt);

            MessageBox.Show(receipt, "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            cart.Clear();
            numDiscount.Value = 0;
            UpdateTotals();
        }

        string BuildReceipt(SaleRecord s)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("======= A&S CAFE SHOP =======");
            sb.AppendLine("Receipt #" + s.Id);
            sb.AppendLine("Date: " + s.Date.ToString("dd-MMM-yyyy hh:mm tt"));
            sb.AppendLine("Cashier: " + s.Cashier);
            sb.AppendLine("-----------------------------");
            foreach (OrderLine l in cart)
                sb.AppendLine(l.Name + "  x" + l.Qty + "   Rs " + l.Total.ToString("N0"));
            sb.AppendLine("-----------------------------");
            sb.AppendLine("Subtotal:  Rs " + subTotal.ToString("N0"));
            sb.AppendLine("Discount:  Rs " + discountAmt.ToString("N0"));
            sb.AppendLine("Tax (5%):  Rs " + taxAmt.ToString("N0"));
            sb.AppendLine("TOTAL:     Rs " + grandTotal.ToString("N0"));
            sb.AppendLine("-----------------------------");
            sb.AppendLine("Thank you! Visit again :)");
            return sb.ToString();
        }

        // ================= MENU MANAGER PAGE =================
        void BuildMenuPage()
        {
            pageMenu = NewPage();
            pageMenu.Controls.Add(Title("Menu Manager"));

            Label ln = new Label(); ln.Text = "Item name";  ln.SetBounds(20, 505, 200, 25);
            Label lc = new Label(); lc.Text = "Category";   lc.SetBounds(290, 505, 150, 25);
            Label lp = new Label(); lp.Text = "Price (Rs)"; lp.SetBounds(480, 505, 120, 25);

            txtName = new TextBox(); txtName.SetBounds(20, 532, 255, 30);
            cmbCat = new ComboBox();
            cmbCat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCat.Items.AddRange(Categories);
            cmbCat.SelectedIndex = 0;
            cmbCat.SetBounds(290, 532, 175, 30);
            txtPrice = new TextBox(); txtPrice.SetBounds(480, 532, 120, 30);

            Button bAdd = Theme.MakeButton("Add", Theme.Green);
            bAdd.SetBounds(20, 590, 130, 44);
            bAdd.Click += (s, e) => AddMenuItem();

            Button bUpdate = Theme.MakeButton("Update", Theme.Purple);
            bUpdate.SetBounds(160, 590, 130, 44);
            bUpdate.Click += (s, e) => UpdateMenuItem();

            Button bDelete = Theme.MakeButton("Delete", Theme.Red);
            bDelete.SetBounds(300, 590, 130, 44);
            bDelete.Click += (s, e) => DeleteMenuItem();

            Button bClear = Theme.MakeButton("Clear", Color.Gray);
            bClear.SetBounds(440, 590, 130, 44);
            bClear.Click += (s, e) => ClearMenuFields();

            gridMenu = new DataGridView();
            gridMenu.SetBounds(20, 66, 940, 420);
            gridMenu.DataSource = DataStore.Menu;      // table is linked to the menu list
            Theme.StyleGrid(gridMenu);
            gridMenu.SelectionChanged += (s, e) => FillMenuFields();

            pageMenu.Controls.AddRange(new Control[] { gridMenu, ln, lc, lp, txtName, cmbCat, txtPrice,
                                                        bAdd, bUpdate, bDelete, bClear });
        }

        void FillMenuFields()
        {
            if (gridMenu.CurrentRow == null) return;
            MenuItemData item = gridMenu.CurrentRow.DataBoundItem as MenuItemData;
            if (item == null) return;
            txtName.Text = item.Name;
            cmbCat.Text = item.Category;
            txtPrice.Text = item.Price.ToString("0.##");
        }

        void ClearMenuFields()
        {
            txtName.Clear();
            txtPrice.Clear();
            cmbCat.SelectedIndex = 0;
            txtName.Focus();
        }

        bool ReadMenuInput(out string name, out string cat, out decimal price)
        {
            name = txtName.Text.Trim().Replace("|", " ");
            cat = cmbCat.Text;
            price = 0;

            if (name == "")
            {
                MessageBox.Show("Please enter the item name.");
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.");
                return false;
            }
            return true;
        }

        void AddMenuItem()
        {
            string name, cat; decimal price;
            if (!ReadMenuInput(out name, out cat, out price)) return;

            DataStore.Menu.Add(new MenuItemData { Name = name, Category = cat, Price = price });
            DataStore.SaveMenu();
            ClearMenuFields();
        }

        void UpdateMenuItem()
        {
            if (gridMenu.CurrentRow == null) { MessageBox.Show("Select an item first."); return; }
            string name, cat; decimal price;
            if (!ReadMenuInput(out name, out cat, out price)) return;

            int i = gridMenu.CurrentRow.Index;
            MenuItemData item = DataStore.Menu[i];
            item.Name = name;
            item.Category = cat;
            item.Price = price;
            DataStore.Menu.ResetItem(i);          // refresh that table row
            DataStore.SaveMenu();
        }

        void DeleteMenuItem()
        {
            if (gridMenu.CurrentRow == null) { MessageBox.Show("Select an item first."); return; }
            if (MessageBox.Show("Delete this item?", "Confirm", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes) return;

            DataStore.Menu.RemoveAt(gridMenu.CurrentRow.Index);
            DataStore.SaveMenu();
            ClearMenuFields();
        }

        // ================= SALES HISTORY PAGE =================
        void BuildSalesPage()
        {
            pageSales = NewPage();
            pageSales.Controls.Add(Title("Sales History"));

            Label ld = new Label();
            ld.Text = "Pick a date:";
            ld.SetBounds(20, 70, 100, 25);

            dtpDate = new DateTimePicker();
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.SetBounds(120, 66, 160, 30);
            dtpDate.ValueChanged += (s, e) => LoadSales(true);

            Button bAll = Theme.MakeButton("Show All", Theme.Purple);
            bAll.SetBounds(295, 62, 120, 36);
            bAll.Click += (s, e) => LoadSales(false);

            gridSales = new DataGridView();
            gridSales.SetBounds(20, 110, 940, 500);
            Theme.StyleGrid(gridSales);

            lblSalesTotal = new Label();
            lblSalesTotal.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblSalesTotal.ForeColor = Theme.DarkPurple;
            lblSalesTotal.SetBounds(20, 630, 940, 35);

            pageSales.Controls.AddRange(new Control[] { ld, dtpDate, bAll, gridSales, lblSalesTotal });
        }

        void LoadSales(bool filterByDate)
        {
            var list = DataStore.Sales.AsEnumerable();
            if (filterByDate)
                list = list.Where(s => s.Date.Date == dtpDate.Value.Date);

            var data = list.OrderByDescending(s => s.Id).ToList();
            gridSales.DataSource = data;
            if (gridSales.Columns["Date"] != null)
                gridSales.Columns["Date"].DefaultCellStyle.Format = "dd-MMM-yyyy hh:mm tt";

            lblSalesTotal.Text = "Orders: " + data.Count + "      Total sales: Rs " +
                                 data.Sum(s => s.Total).ToString("N0");
        }
    }
}
