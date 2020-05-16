﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class AllControl : UserControl
    {
        private Model.Game _game;
        private Player _player;
        private FieldControl _fieldControl;
        private HitPointBar _hitControl;
        private InterfaceControl _skillControl;

        public AllControl(Model.Game game, Player player)
        {
            _game = game;
            _player = player;
            _fieldControl = new FieldControl(_game.Heroes, _game.Enemy) {Name = "Field"};
            _fieldControl.Location = new Point(200, 1024);
            _fieldControl.Size = new Size(1366, 150);
            _hitControl = new HitPointBar(player.Heroes.Cast<BasicCreature>().ToList()) {Name = "HitPoints"};
            _hitControl.Location = new Point(0, 170);
            var _skillControl = new InterfaceControl(_game);
            _skillControl.Location = new Point(0, 0);
            _skillControl.Size = new Size(1920, 170);
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            Controls.Add(_skillControl);
            Controls.Add(_hitControl);
            Controls.Add(_fieldControl);
        }
    }
}