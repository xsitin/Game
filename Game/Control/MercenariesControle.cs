﻿using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class MercenariesControle : UserControl
    {
        private Hero _hero;
        private Size _size = new Size(420,390);
        
        public MercenariesControle(Hero hero, Player player)
        {
            _hero = hero;
            MinimumSize = _size;
            BackColor = Color.Transparent;
            var buy = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(42,194,230,34)
            };
            buy.FlatAppearance.BorderSize = 0;
            buy.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buy.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buy.FlatAppearance.CheckedBackColor = Color.Transparent;
            buy.Click += (sender, args) =>
            {
                if (player.Gold >= _hero.Level * 100)
                {
                    player.Gold -= _hero.Level * 100;
                    player.Mercenaries.Remove(_hero);
                    player.Heroes.Add(_hero);
                    Parent.Parent.Controls["GoldBag"].Refresh();
                    Dispose();
                }
            };
            Controls.Add(buy);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            var brush1 = new SolidBrush(Color.Black);
            e.Graphics.DrawImage(Properties.Resources.HeroCard, new Rectangle(0,0,_size.Width, _size.Height));
            e.Graphics.DrawString(_hero.Name.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,34);
            e.Graphics.DrawString(_hero.Specialization.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,64);
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
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization], 105,70);
            e.Graphics.DrawString((_hero.Level * 100).ToString(),new Font(FontFamily.GenericSerif, 15),brush1,185,198);
        }
    }
}