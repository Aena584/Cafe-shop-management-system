using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CafeShop
{
    // All colors and small helper methods live here so every form looks the same.
    static class Theme
    {
        public static readonly Color Purple      = Color.FromArgb(94, 53, 177);
        public static readonly Color DarkPurple  = Color.FromArgb(49, 27, 146);
        public static readonly Color LightPurple = Color.FromArgb(237, 231, 246);
        public static readonly Color Gold        = Color.FromArgb(255, 193, 7);
        public static readonly Color Cream       = Color.FromArgb(250, 247, 255);
        public static readonly Color Green       = Color.FromArgb(46, 160, 90);
        public static readonly Color Red         = Color.FromArgb(211, 47, 47);
        public static readonly Color Orange      = Color.FromArgb(239, 108, 0);

        // Draws the "A&S" cafe logo in code (so no image file is needed).
        public static Bitmap MakeLogo(int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                float s = size / 100f;   // scale: we design on a 100x100 canvas

                // purple circle with gold border
                Rectangle rect = new Rectangle(1, 1, size - 3, size - 3);
                using (LinearGradientBrush br = new LinearGradientBrush(rect, Purple, DarkPurple, 45f))
                    g.FillEllipse(br, rect);
                using (Pen gold = new Pen(Gold, 3 * s))
                    g.DrawEllipse(gold, rect);

                // coffee cup (white)
                using (SolidBrush white = new SolidBrush(Color.White))
                {
                    g.FillRectangle(white, 30 * s, 22 * s, 36 * s, 20 * s);
                    g.FillPie(white, 30 * s, 32 * s, 36 * s, 20 * s, 0, 180);
                }
                using (Pen handle = new Pen(Color.White, 4 * s))
                    g.DrawArc(handle, 58 * s, 26 * s, 20 * s, 16 * s, -90, 180);

                // steam (gold)
                using (Pen steam = new Pen(Gold, 2.5f * s))
                {
                    g.DrawArc(steam, 38 * s, 8 * s, 8 * s, 12 * s, 90, 180);
                    g.DrawArc(steam, 50 * s, 8 * s, 8 * s, 12 * s, 90, 180);
                }

                // "A&S" text under the cup
                using (Font f = new Font("Segoe UI", 24 * s, FontStyle.Bold, GraphicsUnit.Pixel))
                using (SolidBrush gb = new SolidBrush(Gold))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString("A&S", f, gb, new RectangleF(0, 56 * s, size, 32 * s), sf);
                }
            }
            return bmp;
        }

        // A colored flat button (used everywhere).
        public static Button MakeButton(string text, Color back)
        {
            Button b = new Button();
            b.Text = text;
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        // Gives every table (DataGridView) the same purple look.
        public static void StyleGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 30;
            g.ColumnHeadersHeight = 36;
            g.ColumnHeadersDefaultCellStyle.BackColor = Purple;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            g.AlternatingRowsDefaultCellStyle.BackColor = LightPurple;
            g.DefaultCellStyle.SelectionBackColor = Gold;
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
        }
    }
}
