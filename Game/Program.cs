using System;
using System.Windows.Forms;

namespace Game
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1();
            form.ShowMenu();
            Application.Run(form);
        }
    }
}