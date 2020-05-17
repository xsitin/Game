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
                BackColor = Color.Gray,
                ForeColor = Color.DarkRed,
                Text = "КУПИТЬ: 25",
                Font = new Font(FontFamily.GenericSerif, 11),
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(0, 100, 100, 30),
            };
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
            e.Graphics.DrawImage(Properties.Resources.HPpotion, new Rectangle(0, 0, 100, 100));
        }
    }
}