using System.Drawing;
using System.Windows.Forms;
using GameCore.Model;
using GameCore.Resources;

namespace GameCore.Control
{
    public sealed class GoldControl : UserControl
    {
        private readonly Player _player;

        public GoldControl(Player player)
        {
            DoubleBuffered = true;
            _player = player;
            BackColor = Color.Transparent;
            Size = new Size(400, 400);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Gold);
            e.Graphics.DrawImage(Resource.GoldBack, new Rectangle(0, 0, 100, 100));
            e.Graphics.DrawString(_player.Gold.ToString(), new Font(FontFamily.GenericSerif, 26), brush, 90, 35);
        }
    }
}