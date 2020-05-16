using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class GoldControl : UserControl
    {
        private readonly Player Player;
        public GoldControl(Player player)
        {
            Player = player;
            BackColor = Color.Transparent;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Gold);
            e.Graphics.DrawImage(Properties.Resources.GoldBack,new Rectangle(0,0,100,100));
            e.Graphics.DrawString(Player.Gold.ToString(),new Font(FontFamily.GenericSerif, 26),brush,90,35);
        }
    }
}