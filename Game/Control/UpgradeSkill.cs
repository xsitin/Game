﻿using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class UpgradeSkill : UserControl
    {
        private Size _size = new Size(22,22);
        private Skill _skill;

        public UpgradeSkill(Hero hero, Skill skill, int x, int y)
        {
            MaximumSize = _size;
            DoubleBuffered = true;
            _skill = skill;
            Location = new Point(x,y);
            var up = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(1, 0, 20, 20)
            };
            up.Click += (sender, args) =>
            {
                foreach (var s in hero.Skills)
                {
                    if(s.Name == _skill.Name)
                        skill.Upgrade();
                }
                hero.UpgradePoints--;
                ParentForm.Controls["Upgrade"].Controls["HeroUpgrade"].Refresh();
            };
            Controls.Add(up);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Gold);
            var brush1 = new SolidBrush(Color.DarkRed);
            e.Graphics.FillRectangle(brush, new RectangleF(1, 0, _size.Width, _size.Width));
            e.Graphics.DrawString("➕",new Font(FontFamily.GenericSerif, 13),brush1,0,1);
        }
    }
}