using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        public TableLayoutPanel Table;

        public Form1()
        {
            Table = new TableLayoutPanel();
            Table.Dock = DockStyle.Fill;
            Controls.Add(Table);
        }

        public void ShowMenu()
        {
            var directoryWithImages = AppDomain.CurrentDomain.BaseDirectory;
            Table.RowStyles.Clear();
            Table.ColumnStyles.Clear();
            Table.Controls.Clear();
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            var menu = new TableLayoutPanel();
            Table.BackColor = Color.Transparent;
            var newGame = new Button(){Dock = DockStyle.Fill,Margin = new Padding(20)};
            newGame.BackgroundImage = Properties.Resources.newGame;
            newGame.BackgroundImageLayout = ImageLayout.Stretch;
            newGame.Click += (a, b) =>
            {
                this.CleanMenu();
            };
            var loadGame = new Button(){Dock = DockStyle.Fill,Margin = new Padding(20)};
            loadGame.BackgroundImage = Properties.Resources.load;
            loadGame.BackgroundImageLayout = ImageLayout.Stretch;
            var continueGame = new Button(){Dock = DockStyle.Fill,Margin = new Padding(20)};
            continueGame.BackgroundImage = Properties.Resources.cont;
            continueGame.BackgroundImageLayout = ImageLayout.Stretch;
            menu.Dock = DockStyle.Fill;
            menu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            menu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            menu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            menu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            menu.Controls.Add(newGame, 0, 0);
            menu.Controls.Add(continueGame, 0, 1);
            menu.Controls.Add(loadGame, 0, 2);
            Table.Controls.Add(menu, 0, 0);
            BackgroundImage = Properties.Resources.StartGameArt;
            BackgroundImageLayout = ImageLayout.Stretch;
            Table.BackColor = Color.Transparent;
            Table.Controls.Add(new Panel(), 1, 0);
            Table.Controls.Add(new Panel(), 1, 1);
            Table.Controls.Add(new Panel(), 0, 1);
            WindowState = FormWindowState.Maximized;
        }

        public void CleanMenu()
        {
            Table.Controls.Clear();
        }
    }
}