using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class UpgradeChar : UserControl
    {
        private readonly Characteristics _ch;
        private readonly Hero _hero;
        private readonly Size _size = new(22, 22);

        public UpgradeChar(Characteristics characteristic, Hero hero, int x, int y)
        {
            DoubleBuffered = true;
            MaximumSize = _size;
            _hero = hero;
            _ch = characteristic;
            Location = new Point(x, y);
            var up = new Button
            {
                MaximumSize = _size,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat
            };
            up.Click += (sender, args) =>
            {
                if (_hero.Characteristics != null &&
                    (_ch == Characteristics.Evasion || _ch == Characteristics.PhysicalProtection ||
                     _ch == Characteristics.MagicalProtection) && _hero.Characteristics[_ch] * 1.2 < 100)
                {
                    _hero.Characteristics[_ch] = (int) (_hero.Characteristics[_ch] * 1.2);
                    _hero.StandardChars[_ch] = (int) (_hero.Characteristics[_ch] * 1.2);
                }
                else if (_ch == Characteristics.PhysicalDamage)
                {
                    if (_hero.Skills != null)
                        foreach (var sk in _hero.Skills)
                        {
                            if (sk.Name == "Base Hit")
                                sk.Upgrade();
                            if (_hero.Characteristics == null) continue;
                            _hero.Characteristics[_ch] = (int) (_hero.Characteristics[_ch] * 1.2);
                            _hero.StandardChars[_ch] = (int) (_hero.Characteristics[_ch] * 1.2);
                        }
                }
                else if (_ch != Characteristics.Evasion && _ch != Characteristics.MagicalProtection &&
                         _ch != Characteristics.PhysicalProtection)
                {
                    if (_hero.Characteristics != null)
                        _hero.Characteristics[_ch] = (int) (_hero.Characteristics[_ch] * 1.2);
                }

                _hero.UpgradePoints--;
                ParentForm?.Controls["Upgrade"].Controls["HeroUpgrade"].Refresh();
            };
            Controls.Add(up);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Gold);
            var brush1 = new SolidBrush(Color.DarkRed);
            e.Graphics.FillRectangle(brush, new RectangleF(1, 0, _size.Width, _size.Width));
            e.Graphics.DrawString("➕", new Font(FontFamily.GenericSerif, 13), brush1, 0, 1);
        }
    }
}