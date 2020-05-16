using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class EnemyHitBar : UserControl
    {
        private List<BasicCreature> _enemies;
        public EnemyHitBar(List<BasicCreature> enemies)
        {
            this._enemies = enemies;
            BackColor = Color.Transparent;
            Bounds = new Rectangle(1366, 0, 170, enemies.Count * 55);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(5, 5, 165, _enemies.Count * 55 - 5));
            e.Graphics.DrawRectangle(new Pen(Color.Black, 10), new Rectangle(0, 0, 170, _enemies.Count * 55));
            var dx = 0;
            var dy = 0;
            foreach (var enemy in _enemies)
            {
                e.Graphics.DrawString(enemy.Name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk),
                    10 + dx, 10 + dy);
                e.Graphics.DrawString("HP: " + enemy.Characteristics[Characteristics.Health].ToString(),
                    new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk), 100 + dx, 5 + dy);
                e.Graphics.DrawString("MP: " + enemy.Characteristics[Characteristics.Mana].ToString(),
                    new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Cornsilk), 100 + dx, 25 + dy);
                dy += 50;
            }
        }
    }
}