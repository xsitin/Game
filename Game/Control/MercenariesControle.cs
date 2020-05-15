using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class MercenariesControle : UserControl
    {
        private Hero _hero;
        private Size _size = new Size(420,390);
        
        public MercenariesControle(Hero hero)
        {
            _hero = hero;
            MinimumSize = _size;
            BackColor = Color.Transparent;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            e.Graphics.DrawImage(Properties.Resources.HeroCard, new Rectangle(0,0,_size.Width, _size.Height));
            e.Graphics.DrawString(_hero.Name.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,34);
            e.Graphics.DrawString(_hero.Specialization.ToString(),new Font(FontFamily.GenericSerif, 12),brush,270,64);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Health].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,135);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Mana].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,172);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Initiative].ToString(),new Font(FontFamily.GenericSerif, 12),brush,355,199);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalDamage].ToString(),new Font(FontFamily.GenericSerif, 12),brush,333,305);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.MagicalProtection].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,293);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.PhysicalProtection].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,275);
            e.Graphics.DrawString(_hero.Characteristics[Characteristics.Evasion].ToString(),new Font(FontFamily.GenericSerif, 12),brush,179,256);
            var dy = 0;
            foreach (var skill in _hero.Skills)
            {
                e.Graphics.DrawString(skill.Name,new Font(FontFamily.GenericSerif, 12),brush,79,256 + dy);
                e.Graphics.DrawString(skill.Level.ToString(),new Font(FontFamily.GenericSerif, 12),brush,49,256 + dy);
                dy += 19;
            }
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization], 105,70);
        }
    }
}