using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class HeroUpgradeControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Player player;
        private Form1 _form1;
        private Size _size = new Size(420,390);
        public HeroUpgradeControl(Hero hero, Player player, Form1 form1)
        {
            _hero = hero;
            MinimumSize = _size;
            this.player = player;
            Name = "HeroUpgrade";
            var back = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31, 193, 240, 34)
            };
            back.Click += (sender, args) =>
            {
                if(player.Heroes.Contains(_hero))
                    Parent.Parent.Controls["MerHero"].Controls.Add(new BarrackHeroControl(_hero,player,form1));
                else 
                    Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(_hero,player,form1));
                Dispose();
            };
            Controls.Add(back);
            var dy = 0;
            Controls.Add(new UpgradeChar(Characteristics.PhysicalDamage, _hero, 363, 305));
            Controls.Add(new UpgradeChar(Characteristics.MagicalProtection, _hero, 280, 293));
            Controls.Add(new UpgradeChar(Characteristics.PhysicalProtection, _hero, 280, 275));
            Controls.Add(new UpgradeChar(Characteristics.Evasion, _hero, 280, 256));
            Controls.Add(new UpgradeChar(Characteristics.Initiative, _hero, 375, 199));
            Controls.Add(new UpgradeChar(Characteristics.Mana, _hero, 368, 172));
            Controls.Add(new UpgradeChar(Characteristics.Health, _hero, 368, 135));
            foreach (var skill in _hero.Skills.Skip(1))
            {
                Controls.Add(new UpgradeSkill(hero, skill, 145, 256 + dy));
                dy += 19;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            var brush1 = new SolidBrush(Color.Black);
            e.Graphics.DrawImage(Properties.Resources.HeroCard, new Rectangle(0, 0, _size.Width, _size.Height));
            e.Graphics.DrawString(_hero.Name.ToString(), new Font(FontFamily.GenericSerif, 12), brush, 270, 34);
            e.Graphics.DrawString(_hero.Specialization.ToString(), new Font(FontFamily.GenericSerif, 12), brush, 270,
                64);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Health].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 333, 135);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Mana].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 333, 172);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Initiative].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 355, 199);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalDamage].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 333, 305);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.MagicalProtection].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 179, 293);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalProtection].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 179, 275);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Evasion].ToString(),
                new Font(FontFamily.GenericSerif, 12), brush, 179, 256);
            var dy = 0;
            foreach (var skill in _hero.Skills.Skip(1))
            {
                e.Graphics.DrawString(skill.Name, new Font(FontFamily.GenericSerif, 12), brush, 79, 256 + dy);
                e.Graphics.DrawString(skill.Level.ToString(), new Font(FontFamily.GenericSerif, 12), brush, 49,
                    256 + dy);
                dy += 19;
            }
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization], 105, 70);
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(31, 193, 240, 34));
            e.Graphics.DrawString("Закончить Прокачку", new Font(FontFamily.GenericSerif, 18), brush, 38, 195);
        }

        public override void Refresh()
        {
            if(_hero.UpgradePoints < 1)
            {
                Controls.Clear();
                var back = new Button()
                {
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31, 193, 240, 34)
                };
                back.Click += (sender, args) =>
                {
                    if(player.Heroes.Contains(_hero))
                        Parent.Parent.Controls["MerHero"].Controls.Add(new BarrackHeroControl(_hero,player,_form1));
                    else 
                        Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(_hero,player,_form1));
                    Dispose();
                };
                Controls.Add(back);
            }
            base.Refresh();
        }
    }
}    