using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed class HeroInventoryControl : UserControl
    {
        private readonly Hero _hero;
        private readonly Player _player;
        private readonly Size _size = new(420, 390);
        private readonly Form1 _form1;

        public HeroInventoryControl(Hero hero, Player player, Form1? form1)
        {
            _hero = hero;
            MinimumSize = _size;
            _player = player;
            _form1 = form1;
            DoubleBuffered = true;
            var inventory = new Label
            {
                Text = "Инвентарь Героя",
                Name = "LabelInventory",
                Font = new Font(FontFamily.GenericSerif, 25),
                ForeColor = Color.DarkRed,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(1000, 450, 490, 68)
            };
            var storage = new Label
            {
                Text = "Склад",
                Name = "LabelStore",
                Font = new Font(FontFamily.GenericSerif, 25),
                ForeColor = Color.DarkRed,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(430, 450, 490, 68)
            };
            form1.Controls.Add(storage);
            form1.Controls.Add(inventory);
            var back = new Button
            {
                Text = "Закончить",
                Font = new Font(FontFamily.GenericSerif, 14),
                ForeColor = Color.DarkRed,
                BackColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Bounds = new Rectangle(31, 191, 245, 34)
            };
            foreach (var item in _player.Storage)
            {
                var add = new Button
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31, 191, 490, 34),
                    Text = item.Name
                };
                add.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Add(item);
                    _player.Storage.Remove(item);
                    Refresh();
                };
                form1.Controls["Storage"].Controls.Add(add);
            }

            foreach (var item in _hero.Inventory)
            {
                var remove = new Button
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31, 191, 490, 34),
                    Text = item.Name
                };
                remove.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Remove(item);
                    _player.Storage.Add(item);
                    Refresh();
                };
                form1.Controls["Inventory"].Controls.Add(remove);
            }

            back.Click += (sender, args) =>
            {
                if (player.Heroes.Contains(_hero))
                    Parent.Parent.Controls["MerHero"].Controls.Add(new BarrackHeroControl(_hero, player, form1));
                else
                    Parent.Parent.Controls["Active"].Controls.Add(new ActiveTeam(_hero, player, form1));
                Parent.Parent.Controls["LabelStore"].Dispose();
                Parent.Parent.Controls["LabelInventory"].Dispose();
                Dispose();
            };
            back.Click += (sender, args) =>
            {
                _form1.Controls["Inventory"].Controls.Clear();
                _form1.Controls["Inventory"].Refresh();
                _form1.Controls["Storage"].Controls.Clear();
                _form1.Controls["Storage"].Refresh();
            };
            Controls.Add(back);
            Controls.Add(new BasicHeroCardControl(_hero, _player));
        }

        public override void Refresh()
        {
            _form1.Controls["Inventory"].Controls.Clear();
            _form1.Controls["Storage"].Controls.Clear();
            foreach (var item in _player.Storage)
            {
                var add = new Button
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31, 191, 490, 34),
                    Text = item.Name
                };
                add.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Add(item);
                    _player.Storage.Remove(item);
                    Refresh();
                };
                _form1.Controls["Storage"].Controls.Add(add);
            }

            foreach (var item in _hero.Inventory)
            {
                var remove = new Button
                {
                    BackColor = Color.Gray,
                    FlatStyle = FlatStyle.Flat,
                    Bounds = new Rectangle(31, 191, 490, 34),
                    Text = item.Name
                };
                remove.Click += (sender, eventArgs) =>
                {
                    _hero.Inventory.Remove(item);
                    _player.Storage.Add(item);
                    Refresh();
                };
                _form1.Controls["Inventory"].Controls.Add(remove);
            }

            base.Refresh();
        }
    }
}