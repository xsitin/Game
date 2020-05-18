using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Game.Model;
using Game.Properties;

namespace Game
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1() {Name = "Main"};
            Application.ApplicationExit += (a, e) =>
            {
                if (form.Player != null)
                    Helper.SaveGame(form.Player);
            };
            form.ShowMenu();
            Application.Run(form);
        }
    }
}