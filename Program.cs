using System;
using System.Windows.Forms;

namespace CafeShop
{
    static class Program
    {
        // Every C# program starts here.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DataStore.Load();                       // 1. load menu + sales from text files

            LoginForm login = new LoginForm();      // 2. show login window first
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm(login.UserName));   // 3. open main window
            }
        }
    }
}
