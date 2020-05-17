using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class HeroInventoryControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Player _player;
        private Form1 _form1;
        private readonly Size _size = new Size(420,390);
        public HeroInventoryControl(Hero hero, Player player,Form1 form1)
        {
            _hero = hero;
            MinimumSize = _size;
            _player = player;
            _form1 = form1;
            var back = new Button()
            {
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31,191,245,34)
            };
            foreach (var item in _player.Storage)
            {
                var add = new Button()
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31,191,245,34),
                    Text = item.Name,
                };
                add.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Heap.Add(item);
                    _player.Storage.Remove(item);
                    Refresh();
                };
                form1.Controls["Storage"].Controls.Add(add);
            }
            foreach (var item in _hero.Inventory.Heap)
            {
                var remove = new Button()
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31,191,245,34),
                    Text = item.Name,
                };
                remove.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Heap.Remove(item);
                    _player.Storage.Add(item);
                    Refresh();
                };
                form1.Controls["Inventory"].Controls.Add(remove);
            }
            back.Click += (sender, args) =>
            {
                if(player.Heroes.Contains(_hero))
                    Parent.Parent.Controls["MerHero"].Controls.Add(new BarrackHeroControl(_hero,player,form1));
                else 
                    Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(_hero,player,form1));
                Dispose();
            };
            back.Click += (sender, args) =>
            {
                form1.Controls["Inventory"].Controls.Clear();
                form1.Controls["Inventory"].Refresh();
                form1.Controls["Storage"].Controls.Clear();
                form1.Controls["Storage"].Refresh();
            };
            Controls.Add(back);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var brush = new SolidBrush(Color.Brown);
            var brush1 = new SolidBrush(Color.Gray);
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
            e.Graphics.DrawImage(Helper.ImageTransfer[_hero.Specialization], 105,70);
            var dy = 0;
            foreach (var skill in _hero.Skills.Skip(1))
            {
                e.Graphics.DrawString(skill.Name,new Font(FontFamily.GenericSerif, 12),brush,79,256 + dy);
                e.Graphics.DrawString(skill.Level.ToString(),new Font(FontFamily.GenericSerif, 12),brush,49,256 + dy);
                dy += 19;
            }
            e.Graphics.FillRectangle(brush1, new RectangleF(31,191,245,34));
            e.Graphics.DrawString("Закончить",new Font(FontFamily.GenericSerif, 15),brush,100,195);
        }

        public override void Refresh()
        {
            _form1.Controls["Inventory"].Controls.Clear();
            _form1.Controls["Storage"].Controls.Clear();
            foreach (var item in _player.Storage)
            {
                var add = new Button()
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31,191,245,34),
                    Text = item.Name,
                };
                add.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Heap.Add(item);
                    _player.Storage.Remove(item);
                    Refresh();
                };
                _form1.Controls["Storage"].Controls.Add(add);
            }
            foreach (var item in _hero.Inventory.Heap)
            {
                var remove = new Button()
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31,191,245,34),
                    Text = item.Name,
                };
                remove.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Heap.Remove(item);
                    _player.Storage.Add(item);
                    Refresh();

                };
                _form1.Controls["Inventory"].Controls.Add(remove);
            }
            base.Refresh();
        }
    }
}