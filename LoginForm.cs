using System;
using System.Drawing;
using System.Windows.Forms;

namespace CafeShop
{
    public class LoginForm : Form
    {
        public string UserName = "";
        TextBox txtUser, txtPass;
        CheckBox chkShow;

        public LoginForm()
        {
            Text = "A&S Cafe - Login";
            ClientSize = new Size(800, 450);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 10);
            Icon = Icon.FromHandle(Theme.MakeLogo(64).GetHicon());

            // ---- left purple panel with logo ----
            Panel left = new Panel();
            left.SetBounds(0, 0, 360, 450);
            left.BackColor = Theme.DarkPurple;

            PictureBox logo = new PictureBox();
            logo.Image = Theme.MakeLogo(200);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.SetBounds(80, 70, 200, 200);

            Label name = new Label();
            name.Text = "CAFE SHOP\nMANAGEMENT SYSTEM";
            name.ForeColor = Theme.Gold;
            name.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            name.TextAlign = ContentAlignment.MiddleCenter;
            name.SetBounds(0, 290, 360, 70);

            left.Controls.Add(logo);
            left.Controls.Add(name);

            // ---- right white panel with login fields ----
            Panel right = new Panel();
            right.SetBounds(360, 0, 440, 450);
            right.BackColor = Color.White;

            Label title = new Label();
            title.Text = "Welcome Back!";
            title.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            title.ForeColor = Theme.Purple;
            title.SetBounds(50, 50, 340, 50);

            Label l1 = new Label();
            l1.Text = "Username";
            l1.SetBounds(50, 125, 340, 25);
            txtUser = new TextBox();
            txtUser.Font = new Font("Segoe UI", 12);
            txtUser.SetBounds(50, 152, 340, 30);

            Label l2 = new Label();
            l2.Text = "Password";
            l2.SetBounds(50, 195, 340, 25);
            txtPass = new TextBox();
            txtPass.Font = new Font("Segoe UI", 12);
            txtPass.UseSystemPasswordChar = true;
            txtPass.SetBounds(50, 222, 340, 30);

            chkShow = new CheckBox();
            chkShow.Text = "Show password";
            chkShow.SetBounds(50, 260, 200, 25);
            chkShow.CheckedChanged += (s, e) => txtPass.UseSystemPasswordChar = !chkShow.Checked;

            Button btnLogin = Theme.MakeButton("LOGIN", Theme.Purple);
            btnLogin.SetBounds(50, 305, 340, 48);
            btnLogin.Click += BtnLogin_Click;
            AcceptButton = btnLogin;      // pressing Enter = clicking Login

            Label hint = new Label();
            hint.Text = "Demo login:  admin / admin123";
            hint.ForeColor = Color.Gray;
            hint.SetBounds(50, 370, 340, 25);

            right.Controls.AddRange(new Control[] { title, l1, txtUser, l2, txtPass, chkShow, btnLogin, hint });

            Controls.Add(left);
            Controls.Add(right);
        }

        void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim() == "admin" && txtPass.Text == "admin123")
            {
                UserName = txtUser.Text.Trim();
                DialogResult = DialogResult.OK;      // this closes the login form
            }
            else
            {
                MessageBox.Show("Wrong username or password!", "Login Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Clear();
                txtPass.Focus();
            }
        }
    }
}
