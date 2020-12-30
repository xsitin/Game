using System.Drawing;
using System.Windows.Forms;
using GameCore.Model;
using GameCore.Resources;

namespace GameCore.Control
{
    public sealed class MpPotionControl : UserControl
    {
        private Player _player;

        public MpPotionControl(Player player)
        {
            _player = player;
            BackColor = Color.Transparent;
            DoubleBuffered = true;
            var buy = new Button
            {
                BackColor = Color.Gray,
                ForeColor = Color.DarkBlue,
                Text = "КУПИТЬ: 25",
                Font = new Font(FontFamily.GenericSerif, 11),
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(0, 100, 100, 30)
            };

            buy.Click += (sender, args) =>
            {
                if (player.Gold >= 25)
                {
                    player.Gold -= 25;
                    player.Storage.Add(new ActiveItem("MP Potion",
                        new (Characteristics characteristic, int value)[] {(Characteristics.Mana, 50)}));
                    Parent.Controls["GoldBag"].Refresh();
                }
            };
            Controls.Add(buy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Resource.MPpotion, new Rectangle(0, 0, 100, 100));
        }
    }
}