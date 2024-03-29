﻿using System.Drawing;
using System.Windows.Forms;
using GameCore.Model;

namespace GameCore.Control
{
    public sealed class BarrackHeroControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Player _player;
        private View.Form1 _form1;
        private readonly Size _size = new(420, 390);

        public BarrackHeroControl(Hero hero, Player player, View.Form1 form1)
        {
            _form1 = form1;
            _hero = hero;
            DoubleBuffered = true;
            _player = player;
            BackColor = Color.Transparent;
            MinimumSize = _size;
            var toFirstLine = new Button
            {
                BackColor = Color.Gray,
                Text = "Первая Линия",
                Font = new Font(FontFamily.GenericSerif, 12),
                ForeColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31, 193, 120, 34)
            };
            toFirstLine.Click += (sender, args) =>
            {
                if (player.ActiveTeam.GetTeamList().Count < 4)
                {
                    hero.Position = Position.Melee;
                    player.ActiveTeam.FirstLine.Add(hero);
                    player.Heroes.Remove(hero);
                    ParentForm.Controls["Active"].Controls.Add(new ActiveTeam(hero, player, form1));
                    ParentForm.Controls["Active"].Refresh();
                    Dispose();
                }
            };
            MinimumSize = _size;
            var toSecondLine = new Button
            {
                BackColor = Color.Gray,
                Text = "Вторая Линия",
                Font = new Font(FontFamily.GenericSerif, 12),
                ForeColor = Color.DarkRed,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(153, 193, 120, 34)
            };
            toSecondLine.Click += (sender, args) =>
            {
                if (player.ActiveTeam.GetTeamList().Count < 4)
                {
                    hero.Position = Position.Range;
                    player.ActiveTeam.SecondLine.Add(hero);
                    player.Heroes.Remove(hero);
                    ParentForm.Controls["Active"].Controls.Add(new ActiveTeam(hero, player, form1));
                    ParentForm.Controls["Active"].Refresh();
                    Dispose();
                }
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
                if (ParentForm.Controls["Upgrade"].Controls.Count == 1) return;
                ParentForm.Controls["Upgrade"].Controls
                    .Add(new HeroInventoryControl(hero, player, ParentForm as View.Form1));
                Dispose();
            };
            Controls.Add(inventory);
            Controls.Add(toSecondLine);
            Controls.Add(toFirstLine);
            Controls.Add(new BasicHeroCardControl(_hero, _player));
        }
    }
}