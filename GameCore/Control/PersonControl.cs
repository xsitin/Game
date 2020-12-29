using System.Drawing;
using System.Windows.Forms;
using Game.Model;
using GameCore.Resources;

namespace Game.Control
{
    public partial class PersonControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Size _size = Resource.Warrior.Size;

        public PersonControl(Hero hero)
        {
            _hero = hero;
            Size = new Size(10 * _size.Width, 10 * _size.Height);
            Location = new Point(0, 600);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization],
                new Rectangle(0, 0, Size.Width, Size.Height));
        }
    }
}