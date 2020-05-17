using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class BarrackHeroControl : UserControl
    {
        private readonly Hero _hero;
        private Size _size = new Size(420,390);
        private readonly Player _player;
        public BarrackHeroControl(Hero hero, Player player, Form1 form1)
        {
            _hero = hero;
            _player = player;
            BackColor = Color.Transparent;
            MinimumSize = _size;
            var toFirstLine = new Button()
            {
                BackColor = Color.Gray,
                Text = "Первая Линия",
                Font = new Font(FontFamily.GenericSerif, 12) ,
                ForeColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31,193,120,34)
            };
            toFirstLine.Click += (sender, args) =>
            {
                if (player.ActiveTeam.GetTeamList().Count < 4)
                {
                    hero.Position = Position.Melee;
                    player.ActiveTeam.FirstLine.Add(hero);
                    player.Heroes.Remove(hero);
                    Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(hero,player, form1));
                    Parent.Parent.Controls["Active"].Refresh();
                    Dispose();
                }
            };
            MinimumSize = _size;
            var toSecondLine = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(153,193,120,34)
            };
            toSecondLine.Click += (sender, args) =>
            {
                if (player.ActiveTeam.GetTeamList().Count < 4)
                {
                    hero.Position = Position.Range;
                    player.ActiveTeam.SecondLine.Add(hero);
                    player.Heroes.Remove(hero);
                    Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(hero,player, form1));
                    Parent.Parent.Controls["Active"].Refresh();
                    Dispose();
                }
            };
            var upgrade = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31, 35, 120, 34)
            };
            upgrade.Click += (sender, args) =>
            {
                if (Parent.Parent.Controls["Upgrade"].Controls.Count == 1) return;
                Parent.Parent.Controls["Upgrade"].Controls.Add(new HeroUpgradeControl(hero,player,form1));
                Dispose();
            };
            if(hero.UpgradePoints > 0)
                Controls.Add(upgrade);
            var inventory = new Button()
            {
                BackColor = Color.Goldenrod,
                Text = "ИНВЕНТАРЬ",
                Font = new Font(FontFamily.GenericSerif, 12) ,
                ForeColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Bounds =new Rectangle(153,35 ,110,34)
            };
            inventory.Click += (sender, args) =>
            {
                if (Parent.Parent.Controls["Upgrade"].Controls.Count == 1) return;
                Parent.Parent.Controls["Upgrade"].Controls.Add(new HeroInventoryControl(hero,player,form1));
                Dispose();
            };
            Controls.Add(inventory);
            Controls.Add(toSecondLine);
            Controls.Add(toFirstLine);
            Controls.Add(new BasicHeroCardControl(_hero,_player));
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            var brush1 = new SolidBrush(Color.Black);
            if (_hero.UpgradePoints > 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Goldenrod),new RectangleF(31,35 ,120,34));
                e.Graphics.DrawString("Прокачка", new Font(FontFamily.GenericSerif, 14), brush, 44, 39);
            }
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(153,193,120,34));
            e.Graphics.DrawString("Во Вторую\n  Линию",new Font(FontFamily.GenericSerif, 11),brush,178,193);
        }
    }
}