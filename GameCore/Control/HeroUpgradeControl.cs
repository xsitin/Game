using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class HeroUpgradeControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Player _player;
        private readonly Size _size = new(420, 390);

        public HeroUpgradeControl(Hero hero, Player player)
        {
            var dy = 0;
            _hero = hero;
            DoubleBuffered = true;
            MinimumSize = _size;
            _player = player;
            Name = "HeroUpgrade";
            var back = new Button
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
                if (player.Heroes.Contains(_hero))
                    ParentForm.Controls["MerHero"].Controls
                        .Add(new BarrackHeroControl(_hero, player, ParentForm as Form1));
                else
                    ParentForm.Controls["Active"].Controls.Add(new ActiveTeam(_hero, player, ParentForm as Form1));
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
            if (_hero.UpgradePoints < 1)
            {
                Controls.Clear();
                var back = new Button
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
                    if (_player.Heroes.Contains(_hero))
                        ParentForm.Controls["MerHero"].Controls
                            .Add(new BarrackHeroControl(_hero, _player, ParentForm as Form1));
                    else
                        ParentForm.Controls["Active"].Controls.Add(new ActiveTeam(_hero, _player, ParentForm as Form1));
                    Dispose();
                };
                Controls.Add(back);
                Controls.Add(new BasicHeroCardControl(_hero, _player));
            }

            base.Refresh();
        }
    }
}