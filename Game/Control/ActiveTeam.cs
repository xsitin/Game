using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class ActiveTeam : UserControl
    {
         private readonly Hero _hero;
        private Size _size = new Size(420,390);
        private readonly Player _player;
        public ActiveTeam(Hero hero, Player player, Form1 form1)
        {
            _hero = hero;
            _player = player;
            BackColor = Color.Transparent;
            MinimumSize = _size;
            var exile = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(38,191,235,34),
            };
            exile.Click += (sender, args) =>
            {
                if (hero.Position == Position.Melee)
                    player.ActiveTeam.FirstLine.Remove(hero);
                else
                    player.ActiveTeam.SecondLine.Remove(hero);
                player.Heroes.Add(hero);
                Parent.Parent.Controls["MerHero"].Controls.Add(new BarrackHeroControl(hero,player,form1));
                Parent.Parent.Controls["MerHero"].Refresh();
                Dispose();
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
                BackColor = Color.Transparent,
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
            Controls.Add(exile);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            e.Graphics.DrawImage(Properties.Resources.HeroCard, new Rectangle(0,0,_size.Width, _size.Height));
            e.Graphics.DrawString(_hero.Name.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,34);
            e.Graphics.DrawString(_hero.Specialization.ToString() +"|"+ _hero.Position.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,64);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Health].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,135);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Mana].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,172);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Initiative].ToString(),new Font(FontFamily.GenericSerif, 12),brush,355,199);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalDamage].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,305);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.MagicalProtection].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,293);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalProtection].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,275);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Evasion].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,256);
            var dy = 0;
            foreach (var skill in _hero.Skills.Skip(1))
            {
                e.Graphics.DrawString(skill.Name,new Font(FontFamily.GenericSerif, 12),brush,79,256 + dy);
                e.Graphics.DrawString(skill.Level.ToString(),new Font(FontFamily.GenericSerif, 12),brush,49,256 + dy);
                dy += 19;
            }
            if (_hero.UpgradePoints > 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Goldenrod),new RectangleF(31,35 ,120,34));
                e.Graphics.DrawString("Прокачка", new Font(FontFamily.GenericSerif, 14), brush, 44, 39);
            }
            e.Graphics.FillRectangle(new SolidBrush(Color.Goldenrod),new RectangleF(153,35 ,110,34));
            e.Graphics.DrawString("Инвентарь", new Font(FontFamily.GenericSerif, 14), brush, 158, 39);
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization], 105,70);
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(38,191,235,34));
            e.Graphics.DrawString("Убрать из команды",new Font(FontFamily.GenericSerif, 15),brush,72,195);
        }
    }
}