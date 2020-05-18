using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Game.Control;
using Game.Model;
using Game.Properties;

namespace Game
{
    public sealed partial class Form1 : Form
    {
        public Model.Game Game;
        public Player Player;
        public TableLayoutPanel Table;

        public Form1()
        {
            DoubleBuffered = true;
            Table = new TableLayoutPanel() {Dock = DockStyle.Fill};
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
            Controls.Add(Table);
        }

        public void ShowMenu1()
        {
            var menu = new Panel
            {
                BackColor = Color.Transparent,
                BackgroundImage = Resources.Skroll,
                BackgroundImageLayout = ImageLayout.Stretch,
                Bounds = new Rectangle(500, 100, 500, 600)
            };
            var message = new Label
            {
                Bounds = new Rectangle(190, 50, 300, 50),
                Text = "Главное меню",
                Font = new Font(FontFamily.GenericMonospace, 15),
            };
            var newGame = new Button
            {
                Bounds = new Rectangle(95, 250, 330, 100),
                BackgroundImage = Properties.Resources.newGame,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            newGame.Click += (a, b) => { CreateNewGameScreen(); };
            var loadGame = new Button
            {
                Bounds = new Rectangle(95, 350, 330, 100),
                BackgroundImage = Properties.Resources.load,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            loadGame.Click += (sender, args) =>
            {
                Controls.Clear();
                LoadScreen();
            };
            var continueGame = new Button
            {
                Bounds = new Rectangle(95, 150, 330, 100),
                BackgroundImage = Properties.Resources.cont,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            continueGame.Click += (sender, args) =>
            {
                Invalidate();
                Controls.Clear();
                Player = Helper.LoadGame(GetSaveNames().First());
                VillageControls();
            };
            menu.Controls.Add(message);
            menu.Controls.Add(continueGame);
            menu.Controls.Add(newGame);
            menu.Controls.Add(loadGame);
            Controls.Add(menu);
        }

        private void LoadScreen()
        {
            var panel = new Panel
            {
                BackColor = Color.Transparent,
                Bounds = new Rectangle(500, 150, 500, 500),
                BackgroundImage = Resources.Skroll,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            panel.Controls.Add(new Label()
            {
                Text = "Загрузить игру", Font = new Font(FontFamily.GenericSansSerif, 13),
                Bounds = new Rectangle(190, 35, 150, 30)
            });
            var subPanel = new Panel
            {
                AutoScroll = true,
                BackColor = Color.Transparent,
                Bounds = new Rectangle(100, 100, 350, 200),
            };
            var savesNames = GetSaveNames();
            var dy = 0;
            foreach (var saveName in savesNames)
            {
                var save = new Button
                {
                    Text = saveName,
                    Bounds = new Rectangle(0, 50 + dy, 300, 50)
                };
                save.Click += (o, eventArgs) =>
                {
                    Player = Helper.LoadGame(saveName);
                    File.SetLastAccessTime(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Game\" + saveName, DateTime.Now);
                    Controls.Clear();
                    VillageControls();
                };
                dy += 50;
                subPanel.Controls.Add(save);
            }
            panel.Controls.Add(subPanel);
            var back = new Button {
                Text = "Вернуться",
                Bounds = new Rectangle(150, 350, 200, 50),
            };
            back.Click += (sender, args) =>
            {
                Controls.Clear();
                ShowMenu();
            };
            panel.Controls.Add(back);
            Controls.Add(panel);
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
            Controls.Clear();
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
                var spec = (Specialization) Enum.Parse(typeof(Specialization), choice);
                var h = new Hero(spec);
                h.Skills.AddRange(Helper.BasicSkills[spec]);
                h.Characteristics[Characteristics.Evasion] = 100;
                h.Characteristics[Characteristics.MagicalProtection] = 100;
                var pl = new Player()
                {
                    PlayerName = name.Text, Gold = 10000,
                    Heroes = new List<Hero>()
                        {},
                    Mercenaries = new List<Hero>(), Shop = new List<ActiveItem>(), Storage = new List<ActiveItem>(),
                    ActiveTeam= new Team<Hero>(new List<Hero>(){h},new List<Hero>() )
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
                Controls.Clear();
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
            Controls.Clear();
            BackgroundImage = Properties.Resources.Village;
            var barrack = new Button
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(900, 525, 650, 400),
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
                    Size = new Size(420, 1080),
                    Location = new Point(1500, 0),
                    Name = "MerHero",
                };
                foreach (var hero in Player.Heroes)
                {
                    merHero.Controls.Add(new BarrackHeroControl(hero,Player,this));
                }
                var activeHero = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    Size = new Size(420, 1080),
                    Location = new Point(0, 0),
                    Name = "Active"
                };
                foreach (var hero in Player.ActiveTeam.GetTeamList())
                {
                    activeHero.Controls.Add(new ActiveTeam(hero as Hero, Player,this));
                }
                var upgrade = new Panel()
                {
                    BackColor = Color.Transparent,
                    MinimumSize = new Size(420,390),
                    Location = new Point(760,0),
                    Name = "Upgrade"
                };
                var invent = new FlowLayoutPanel()
                {
                    BackColor = Color.Transparent,
                    Size = new Size(490,580),
                    Location =new Point(430, 520),
                    Name = "Storage",
                   AutoScroll = true
                };
                var herInvent = new FlowLayoutPanel()
                {
                    AutoScroll = true,
                    BackColor = Color.Transparent,
                    MinimumSize = new Size(490, 580),
                    Location = new Point(1000, 520),
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
                Bounds = new Rectangle(0, 300, 250, 600),
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
                    Size = new Size(420, 1080),
                    Location = new Point(1500, 0)
                };
                foreach (var mercenary in Player.Mercenaries)
                {
                    floatPanel.Controls.Add(new MercenariesControl(mercenary,Player));
                }
                Controls.Add(new GoldControl(Player)
                {
                    Location = new Point(0,955),
                    Name = "GoldBag"
                });
                Controls.Add(new MpPotionControl(Player)
                {
                    Location = new Point(1150, 0)
                });
                Controls.Add(new HpPotionControl(Player)
                {
                    Location = new Point(1350,0)
                });
                Controls.Add(floatPanel);
            };                                            
            var goHunt = new Button()
            {
                Name = "Hunt",
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(300, 650, 235, 200),
            };
            goHunt.FlatAppearance.BorderSize = 0;
            goHunt.FlatAppearance.MouseDownBackColor = Color.Transparent;
            goHunt.FlatAppearance.MouseOverBackColor = Color.Transparent;
            goHunt.FlatAppearance.CheckedBackColor = Color.Transparent;
            goHunt.Click += (sender, args) =>
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
                    Controls.Add(new AllControl(Game, Player)
                    {
                        Name = "MainCntrl"
                    });
                    Refresh();
                }
            };
            Controls.Add(goHunt);
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
                Controls.Clear();
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