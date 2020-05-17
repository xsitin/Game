using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Control;
using Game.Model;
using Game.Properties;

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
            Table = new TableLayoutPanel {Dock = DockStyle.Fill};
        }

        public void ShowMenu()
        {
            //ClearTable();
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            var menu = new TableLayoutPanel();
            Table.BackColor = Color.Transparent;
            var newGame = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(20),
                BackgroundImage = Properties.Resources.newGame,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            newGame.Click += (a, b) => { CreateNewGameScreen(); };
            var loadGame = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(20),
                BackgroundImage = Properties.Resources.load,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            loadGame.Click += (sender, args) =>
            {
                CleanForm();
                LoadScreen();
            };
            var continueGame = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(20),
                BackgroundImage = Properties.Resources.cont,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            continueGame.Click += (sender, args) =>
            {
                Invalidate();
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
            Controls.Add(Table);
        }

        private void LoadScreen()
        {
            var flowPanel = new FlowLayoutPanel()
            {
                AutoScroll = true,
                BackColor = Color.Transparent,
                Bounds = new Rectangle(550, 250, 500, 600),
                BackgroundImage = Resources.Skroll,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            flowPanel.Controls.Add(new Label() {Text = "Загрузить игру"});
            var savesNames = GetSaveNames();
            foreach (var saveName in savesNames)
            {
                var save = new Button()
                {
                    Text = saveName,
                    Bounds = new Rectangle(200, 10, 400, 50)
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

            var back = new Button {
                Text = "Вернуться",
                Size = new Size(200, 50),
            };
            back.Click += (sender, args) =>
            {
                CleanForm();
                ShowMenu();
            };
            flowPanel.Controls.Add(back);
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
            CleanForm();
            var playMenu = new Panel
            {
                Bounds = new Rectangle(500, 180, 500, 400),
                BackColor = Color.Transparent,
                BackgroundImage = Resources.Skroll,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            var message = new Label()
            {
                Text = "Создание Вашего персонажа", 
                Bounds = new Rectangle(160, 30, 300, 20),
                Font = new Font(FontFamily.GenericSerif, 12)
            };
            var message2 = new Label() { 
                Text = "Введите Ваше имя", 
                Bounds = new Rectangle(190, 80, 300, 20),
                Font = new Font(FontFamily.GenericSerif, 12)
            };
            var message3 = new Label()
            {
                Text = "Выберите класс", 
                Bounds = new Rectangle(205, 125, 300, 20),
                Font = new Font(FontFamily.GenericSerif, 12)
            };
            var name = new TextBox
            {
                TextAlign = HorizontalAlignment.Center,
                Location = new Point(110, 100),
                Dock = DockStyle.None,
                Size = new Size(300, 10),
            };
            name.MouseClick += (a, e) => { ((TextBox) a).Text = ""; };
            name.AcceptsReturn = true;
            name.Margin = new Padding(name.Width / 10);
            var groupClasses = new Panel()
            {
                Bounds = new Rectangle(115, 130, 500, 50),
                BackColor = Color.Transparent
            };
            var warriorButton = new RadioButton {Bounds = new Rectangle(35,0,70,50), Text = "Воин"};
            var archerButton = new RadioButton {Bounds =  new Rectangle(115,0,70,50), Text = "Лучник"};
            var wizardButton = new RadioButton {Bounds =  new Rectangle(205,0,70,50), Text = "Маг"};
            groupClasses.Controls.Add(warriorButton);
            groupClasses.Controls.Add(archerButton);
            groupClasses.Controls.Add(wizardButton);
            var choice = "";
            warriorButton.Click += (sender, args) => { choice = "Warrior"; }; 
            archerButton.Click += (sender, args) => { choice = "Archer"; }; 
            wizardButton.Click += (sender, args) => { choice = "Wizard"; };
            var apply = new Button
            {
                Text = "Подтвердить",
                Bounds = new Rectangle(300,290,100,40),
            };
            apply.Click += (b, e) =>
            {
                var pl = new Player()
                {
                    PlayerName = name.Text, Gold = 100,
                    Heroes = new List<Hero>()
                    {
                        new Hero((Specialization) Enum.Parse(typeof(Specialization), choice))
                    },
                    Mercenaries = new List<Hero>(), Shop = new List<ActiveItem>(), Storage = new List<ActiveItem>()
                };
                var hf = new HeroesFactory(pl);
                pl.Mercenaries.AddRange(new[]
                    {hf.GetRandomHero(), hf.GetRandomHero(), hf.GetRandomHero(), hf.GetRandomHero()});
                Helper.SaveGame(pl);
                Player = pl;
                VillageControls();
            };
            var back = new Button()
            {
                Text = "Отмена",
                Bounds = new Rectangle(100, 290, 100, 40)
            };
            back.Click += (sender, args) =>
            {
                CleanForm();
                ShowMenu();
            };
            playMenu.Controls.Add(message);
            playMenu.Controls.Add(message2);
            playMenu.Controls.Add(message3);
            playMenu.Controls.Add(name);
            playMenu.Controls.Add(groupClasses);
            playMenu.Controls.Add(apply);
            playMenu.Controls.Add(back);
            Controls.Add(playMenu);
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
                var merHero = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Size = new Size(420, 900),
                    Location = new Point(1120, 0),
                    Name = "MerHero"
                };
                foreach (var hero in Player.Heroes)
                {
                    merHero.Controls.Add(new BarrackHeroControl(hero,Player, this));
                }
                var activeHero = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Size = new Size(420, 900),
                    Location = new Point(0, 0),
                    Name = "Active"
                };
                foreach (var hero in Player.ActiveTeam.GetTeamList())
                {
                    activeHero.Controls.Add(new ActiveTeam(hero as Hero, Player));
                }
                var upgrade = new Panel()
                {
                    BackColor = Color.Transparent,
                    MinimumSize = new Size(420,390),
                    Location = new Point(560,0),
                    Name = "Upgrade"
                };
                var invent = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Size = new Size(420,390),
                    Location =new Point(3000, 3000),
                    Name = "Storage"
                };
                var herInvent = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    MinimumSize = new Size(420, 390),
                    Location = new Point(560, 500),
                    Name = "Inventory"
                };
                Controls.Add(upgrade);
                Controls.Add(activeHero);
                Controls.Add(merHero);
                Controls.Add(invent);
                Controls.Add(herInvent);
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
                    Location = new Point(0,745),
                    Name = "GoldBag"
                });
                Controls.Add(new MpPotionControl(Player)
                {
                    Location = new Point(750, 0)
                });
                Controls.Add(new HpPotionControl(Player)
                {
                    Location = new Point(950, 0)
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
                if (Player.ActiveTeam.FirstLine.Count == 0)
                {
                    MessageBox.Show("Нужно выбрать активную команду перед боем.", "", MessageBoxButtons.OK);
                }
                else
                {
                    Game = new Model.Game(Player.ActiveTeam, Model.Location.SomeLocation);
                    BackgroundImage = Properties.Resources.DarkForest;
                    Controls.Add(new AllControl(Game, Player, this));
                }
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
            ClearTable();
        }

        public void ClearTable()
        {
            Table.RowStyles.Clear();
            Table.Controls.Clear();
            Table.ColumnStyles.Clear();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "Form1";
            this.ResumeLayout(false);
        }
    }
}