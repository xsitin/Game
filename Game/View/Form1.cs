using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Control;
using Game.Model;

namespace Game
{
    public partial class Form1 : Form
    {
        public Model.Game Game;
        public Player Player;
        public TableLayoutPanel Table;

        public Form1()
        {
            DoubleBuffered = true;
            Table = new TableLayoutPanel();
            Table.Dock = DockStyle.Fill;
            Controls.Add(Table);
        }

        public void ShowMenu()
        {
            GetSaveNames();
            Table.RowStyles.Clear();
            Table.ColumnStyles.Clear();
            Table.Controls.Clear();
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            var menu = new TableLayoutPanel();
            Table.BackColor = Color.Transparent;
            var newGame = new Button() {Dock = DockStyle.Fill, Margin = new Padding(20)};
            newGame.BackgroundImage = Properties.Resources.newGame;
            newGame.BackgroundImageLayout = ImageLayout.Stretch;
            newGame.Click += (a, b) => { CreateNewGameScreen(); };
            var loadGame = new Button() {Dock = DockStyle.Fill, Margin = new Padding(20)};
            loadGame.BackgroundImage = Properties.Resources.load;
            loadGame.BackgroundImageLayout = ImageLayout.Stretch;
            loadGame.Click += (sender, args) =>
            {
                CleanForm();
                LoadScreen();
            };
            var continueGame = new Button() {Dock = DockStyle.Fill, Margin = new Padding(20)};
            continueGame.BackgroundImage = Properties.Resources.cont;
            continueGame.BackgroundImageLayout = ImageLayout.Stretch;
            continueGame.Click += (sender, args) =>
            {
                CleanForm();
                Player = Helper.LoadGame(GetSaveNames().First());
                VillageControls();
            };
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

        private void LoadScreen()
        {
            var flowPanel = new FlowLayoutPanel()
            {
                AutoScroll = true,
                BackColor = Color.Transparent,
                Size = new Size(420, 900),
                Location = new Point(550, 250)
            };
            var savesNames = GetSaveNames();
            foreach (var saveName in savesNames)
            {
                var save = new Button()
                {
                    Text = saveName,
                    Size = new Size(400, 50)
                };
                save.Click += (o, eventArgs) =>
                {
                    Player = Helper.LoadGame(saveName);
                    File.SetLastAccessTime(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Game\" + saveName, DateTime.Now);
                    CleanForm();
                    VillageControls();
                };
                flowPanel.Controls.Add(save);
            }

            Controls.Add(flowPanel);
        }

        private static List<string> GetSaveNames()
        {
            var folderWithSaves = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folderWithSaves = Path.Combine(folderWithSaves, "Game");
            var dirInfo = new DirectoryInfo(folderWithSaves);
            var files = dirInfo.GetFiles();
            Array.Sort(files,
                new Comparison<FileInfo>((f, f2) => f2.LastAccessTime.CompareTo(f.LastAccessTime))
            );
            return files
                .Select(f => Path.GetFileName(f.Name))
                .ToList();;
        }

        public void CreateNewGameScreen()
        {
            ClearTable();
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            var playMenu = new TableLayoutPanel() {Dock = DockStyle.Fill};
            Table.SuspendLayout();
            playMenu.SuspendLayout();
            Table.Controls.Add(new Panel(), 0, 1);
            Table.Controls.Add(new Panel(), 1, 0);
            Table.Controls.Add(new Panel(), 1, 1);
            playMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            playMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            playMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            playMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            playMenu.ColumnStyles.Add(new ColumnStyle());
            var name = new TextBox();
            name.TextAlign = HorizontalAlignment.Center;
            name.Dock = DockStyle.None;
            name.Size = new Size(Width / 5, Height / 5);
            name.Text = "Your name";
            name.MouseClick += (a, e) => { ((TextBox) a).Text = ""; };
            name.AcceptsReturn = true;
            name.Margin = new Padding(name.Width / 10);
            var groupClasses = new Panel();
            var warriorButton = new RadioButton {Bounds =  new Rectangle(60,10,100,50), Text = "Warrior"};
            var archerButton = new RadioButton {Bounds =  new Rectangle(160,10,100,50), Text = "Archer"};
            var wizardButton = new RadioButton {Bounds =  new Rectangle(260,10,100,50), Text = "Wizard"};
            groupClasses.Controls.Add(warriorButton);
            groupClasses.Controls.Add(archerButton);
            groupClasses.Controls.Add(wizardButton);
            groupClasses.BackColor = Color.Transparent;
            groupClasses.Bounds = new Rectangle(0,0,500,100);
            var choice = "";
            warriorButton.Click += (sender, args) => { choice = warriorButton.Text; }; 
            archerButton.Click += (sender, args) => { choice = archerButton.Text; }; 
            wizardButton.Click += (sender, args) => { choice = wizardButton.Text; };
            // persClass.Items.AddRange(Enum.GetNames(typeof(Model.Specialization)));
            // persClass.MinimumSize = new Size(300, 50);
            // persClass.Dock = DockStyle.Top;
            var apply = new Button {Text = "Подтвердить",Bounds = new Rectangle(0,0,100,40),Margin = new Padding(150,20,0,0)};
            apply.Click += (b, e) =>
            {
                var pl = new Player()
                {
                    PlayerName = name.Text, Gold = 100,
                    Heroes = new List<Hero>()
                    {
                        new Hero((Specialization) Enum.Parse(typeof(Specialization), choice))
                    },
                    ActiveCommand = new Team<Hero>(),
                    Mercenaries = new List<Hero>(), Shop = new List<ActiveItem>(), Storage = new List<ActiveItem>()
                };
                var hf = new HeroesFactory(pl);
                pl.Mercenaries.AddRange(new[]
                    {hf.GetRandomHero(), hf.GetRandomHero(), hf.GetRandomHero(), hf.GetRandomHero()});
                Helper.SaveGame(pl);
                Player = pl;
                VillageControls();
            };
            playMenu.Controls.Add(name, 0, 0);
            playMenu.Controls.Add(groupClasses, 0, 1);
            playMenu.Controls.Add(apply, 0, 2);
            Table.Controls.Add(playMenu, 0, 0);
            playMenu.ResumeLayout();
            Table.ResumeLayout();
        }

        public void VillageControls()
        {
            CleanForm();
            BackgroundImage = Properties.Resources.Village;
            var barrack = new Button
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(730, 450, 500, 300),
            };
            barrack.FlatAppearance.BorderSize = 0;
            barrack.FlatAppearance.MouseDownBackColor = Color.Transparent;
            barrack.FlatAppearance.MouseOverBackColor = Color.Transparent;
            barrack.FlatAppearance.CheckedBackColor = Color.Transparent;
            barrack.Click += (a, b) =>
            {
                Controls.Clear();
                BackgroundImage = Properties.Resources.barrack;
                BackToVillage();
            };
            var store = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(0, 250, 235, 500),
            };
            store.FlatAppearance.BorderSize = 0;
            store.FlatAppearance.MouseDownBackColor = Color.Transparent;
            store.FlatAppearance.MouseOverBackColor = Color.Transparent;
            store.FlatAppearance.CheckedBackColor = Color.Transparent;
            store.Click += (sender, args) =>
            {
                Controls.Clear();
                BackgroundImage = Properties.Resources.Shop;
                BackToVillage();
                var floatPanel = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Size = new Size(420, 900),
                    Location = new Point(1120, 0)
                };
                foreach (var mercenary in Player.Mercenaries)
                {
                    floatPanel.Controls.Add(new MercenariesControle(mercenary,Player));
                }
                Controls.Add(new GoldControl(Player)
                {
                    Location = new Point(0,745)
                });
                Controls.Add(floatPanel);
            };
            var GoHunt = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(250, 500, 235, 200),
            };
            GoHunt.FlatAppearance.BorderSize = 0;
            GoHunt.FlatAppearance.MouseDownBackColor = Color.Transparent;
            GoHunt.FlatAppearance.MouseOverBackColor = Color.Transparent;
            GoHunt.FlatAppearance.CheckedBackColor = Color.Transparent;
            GoHunt.Click += (sender, args) =>
            {
                Controls.Clear();
                Game = new Model.Game(new Team<Hero>(Player.Heroes, new List<Hero>()), Model.Location.SomeLocation);
                BackgroundImage = Properties.Resources.DarkForest;
                Controls.Add(new AllControl(Game, Player));
                
            };
            Controls.Add(GoHunt);
            Controls.Add(store);
            Controls.Add(barrack);
        }

        public void BackToVillage()
        {
            var back = new Button()
            {
                Text = "Back to Village",
                Bounds = new Rectangle(0, 0, 100, 40)
            };
            back.Click += (sender, args) =>
            {
                CleanForm();
                VillageControls();
            };
            Controls.Add(back);
        }

        public void CleanForm()
        {
            Controls.Clear();
            Table.RowStyles.Clear();
            Table.Controls.Clear();
            Table.ColumnStyles.Clear();
        }

        public void ClearTable()
        {
            Table.RowStyles.Clear();
            Table.Controls.Clear();
            Table.ColumnStyles.Clear();
        }
    }
}