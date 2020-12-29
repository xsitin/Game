using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class ActiveTeam : UserControl
    {
        private readonly Hero _hero;
        private readonly Player _player;
        private readonly Size _size = new(420, 390);

        public ActiveTeam(Hero hero, Player player, Form1 form1)
        {
            _hero = hero;
            _player = player;
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            MinimumSize = _size;
            var exile = new Button
            {
                Text = "Убрать из команды",
                Font = new Font(FontFamily.GenericSerif, 15),
                ForeColor = Color.DarkRed,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(38, 191, 235, 34)
            };
            exile.Click += (sender, args) =>
            {
                if (hero.Position == Position.Melee)
                    player.ActiveTeam?.FirstLine.Remove(hero);
                else
                    player.ActiveTeam?.SecondLine.Remove(hero);
                _hero.Position = _hero.Specialization == Specialization.Warrior ? Position.Melee : Position.Range;
                player.Heroes?.Add(hero);
                ParentForm?.Controls["MerHero"].Controls.Add(new BarrackHeroControl(hero, player, form1));
                ParentForm?.Controls["MerHero"].Refresh();
                Dispose();
            };
            var upgrade = new Button
            {
                BackColor = Color.Goldenrod,
                Text = "Прокачка" + $"[{hero.UpgradePoints}]",
                ForeColor = Color.DarkRed,
                Font = new Font(FontFamily.GenericSerif, 12),
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31, 35, 120, 34)
            };
            upgrade.Click += (sender, args) =>
            {
                if (ParentForm.Controls["Upgrade"].Controls.Count == 1) return;
                ParentForm.Controls["Upgrade"].Controls.Add(new HeroUpgradeControl(hero, player));
                Dispose();
            };
            if (hero.UpgradePoints > 0)
                Controls.Add(upgrade);
            var inventory = new Button
            {
                BackColor = Color.Goldenrod,
                Text = "Инвентарь",
                Font = new Font(FontFamily.GenericSerif, 12),
                ForeColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(153, 35, 110, 34)
            };
            inventory.Click += (sender, args) =>
            {
                if (ParentForm?.Controls["Upgrade"].Controls.Count == 1) return;
                ParentForm?.Controls["Upgrade"].Controls
                    .Add(new HeroInventoryControl(hero, player, ParentForm as Form1));
                Dispose();
            };
            Controls.Add(inventory);
            Controls.Add(exile);
            Controls.Add(new BasicHeroCardControl(_hero, _player));
        }
    }
}