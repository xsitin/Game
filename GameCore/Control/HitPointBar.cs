using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class HitPointBar : UserControl
    {
        private readonly List<BasicCreature> heroes;

        public HitPointBar(List<BasicCreature>? heroes)
        {
            DoubleBuffered = true;
            this.heroes = heroes;
            BackColor = Color.Transparent;
            Bounds = new Rectangle(0, 0, 170, heroes.Count * 55);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(5, 5, 165, heroes.Count * 55 - 5));
            e.Graphics.DrawRectangle(new Pen(Color.Black, 10), new Rectangle(0, 0, 170, heroes.Count * 55));
            var dx = 0;
            var dy = 0;
            foreach (var hero in heroes)
            {
                e.Graphics.DrawString(hero.Name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk),
                    10 + dx, 10 + dy);
                e.Graphics.DrawString("HP: " + hero.Characteristics[Characteristics.Health],
                    new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk), 100 + dx, 5 + dy);
                e.Graphics.DrawString("MP: " + hero.Characteristics[Characteristics.Mana],
                    new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk), 100 + dx, 25 + dy);
                dy += 50;
            }
        }
    }
}