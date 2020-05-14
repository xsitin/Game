using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Controls;

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
            newGame.BackgroundImage = new Bitmap(@"C:\Users\xsitin\Game\Game\Properties\newGame.png");
            newGame.BackgroundImageLayout = ImageLayout.Stretch;
            var loadGame = new Button(){Dock = DockStyle.Fill,Margin = new Padding(20)};
            loadGame.BackgroundImage = new Bitmap(@"C:\Users\xsitin\Game\Game\Properties\load.png");
            loadGame.BackgroundImageLayout = ImageLayout.Stretch;
            var continueGame = new Button(){Dock = DockStyle.Fill,Margin = new Padding(20)};
            continueGame.BackgroundImage = new Bitmap(@"C:\Users\xsitin\Game\Game\Properties\cont.png");
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
            BackgroundImage = new Bitmap(@"C:\Users\xsitin\Game\Game\Properties\StartGameArt.jpg");
            Table.BackColor = Color.Transparent;
            Table.Controls.Add(new SomeControl(){Dock = DockStyle.Fill}, 1, 0);
            Table.Controls.Add(new Panel(), 1, 1);
            Table.Controls.Add(new Panel(), 0, 1);
        }

    }
}