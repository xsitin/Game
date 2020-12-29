using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class MercenariesControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Size _size = new(420, 390);

        public MercenariesControl(Hero hero, Player player)
        {
            DoubleBuffered = true;
            _hero = hero;
            MinimumSize = _size;
            BackColor = Color.Transparent;
            var buy = new Button
            {
                BackColor = Color.DarkRed,
                Text = $"НАНЯТЬ: {hero.Level * 100}",
                Font = new Font(FontFamily.GenericSerif, 14),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(42, 193, 230, 34)
            };
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
            Controls.Add(new BasicHeroCardControl(_hero, player));
        }
    }
}