using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class HpPotionControl : UserControl
    {
        private Player _player;
        public HpPotionControl(Player player)
        {
            _player = player;
            BackColor = Color.Transparent;
            var buy = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(0, 100, 100, 30),
            };
            buy.FlatAppearance.BorderSize = 0;
            buy.FlatAppearance.MouseDownBackColor = Color.Transparent;
            buy.FlatAppearance.MouseOverBackColor = Color.Transparent;
            buy.FlatAppearance.CheckedBackColor = Color.Transparent;
            buy.Click += (sender, args) =>
            {
                if (player.Gold >= 25)
                {
                    player.Gold -= 25;
                    player.Storage.Add(new ActiveItem("HP Potion", new (Characteristics characteristic, int value)[] { (Characteristics.Health, 50) }));
                    Parent.Controls["GoldBag"].Refresh();
                }
            };
            Controls.Add(buy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Gray);
            var brush1 = new SolidBrush(Color.Red);
            e.Graphics.DrawImage(Properties.Resources.HPpotion, new Rectangle(0, 0, 100, 100));
            e.Graphics.FillRectangle(brush, new RectangleF(0, 100, 100, 30));
            e.Graphics.DrawString("BUY: 25", new Font(FontFamily.GenericSerif, 14), brush1, new Point(10, 100));
        }
    }
}