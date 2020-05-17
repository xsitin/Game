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
            var dy = 0;
            _hero = hero;
            MinimumSize = _size;
            this.player = player;
            Name = "HeroUpgrade";
            var back = new Button()
            {
                Text = "Закончить прокачку",
                Font = new Font(FontFamily.GenericSerif, 14),
                ForeColor = Color.DarkRed,
                BackColor = Color.Gray,
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
            Controls.Add(new BasicHeroCardControl(_hero, player));
        }

        public override void Refresh()
        {
            if(_hero.UpgradePoints < 1)
            {
                Controls.Clear();
                var back = new Button()
                {
                    Text = "Закончить прокачку",
                    Font = new Font(FontFamily.GenericSerif, 14),
                    ForeColor = Color.DarkRed,
                    BackColor = Color.Gray,
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
                Controls.Add(new BasicHeroCardControl(_hero, player));
            }
            base.Refresh();
        }
    }
}    