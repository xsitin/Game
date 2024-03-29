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
        private InterfaceControl _interfaceControl;
        private HitPointBar _enemyHitControl;

        public AllControl(Model.Game game, Player player)
        {
            DoubleBuffered = true;
            _game = game;
            _player = player;
            _fieldControl = new FieldControl(_game.Heroes, _game.Enemy) {Name = "Field"};
            _fieldControl.Location = new Point(250, 200);
            _fieldControl.Size = new Size(1920, 800);
            _hitControl = new HitPointBar(_player.ActiveTeam.GetTeamList()) {Name = "HitPoints"};
            _hitControl.Location = new Point(0, 170);
            _enemyHitControl = new HitPointBar(_game.Enemy.GetTeamList()) {Name = "EnemyHitPoints"};
            _enemyHitControl.Location = new Point(1736, 170);
            _interfaceControl = new InterfaceControl(_game) {Name = "Interface"};
            _interfaceControl.Location = new Point(0, 0);
            _interfaceControl.Size = new Size(1920, 170);
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            Controls.Add(_interfaceControl);
            Controls.Add(_hitControl);
            Controls.Add(_enemyHitControl);
            Controls.Add(_fieldControl);
        }

        public override void Refresh()
        {
            if (_game.IsEnd)
            {
                foreach (var hero in _player.ActiveTeam.FirstLine)
                    if (hero.StandardChars != null)
                        foreach (var key in hero.StandardChars.Keys)
                            hero.Characteristics[key] = hero.StandardChars[key];
                _player.Gold += _game._reward.money;
                var mers = new List<Hero>();
                var fac = new HeroesFactory(_player);
                for (int i = 0; i < 4; i++) mers.Add(fac.GetRandomHero());
                _player.Mercenaries = mers;
                ((Form1) Application.OpenForms["Main"]).VillageControls();
                Dispose();
                return;
            }

            Controls.Clear();
            if(!_fieldControl._enemies.Any(x=>x.Characteristics[Characteristics.Health]>0))
                _fieldControl = new FieldControl(_game.Heroes, _game.Enemy) {Name = "Field"};
            _fieldControl.Location = new Point(250, 200);
            _fieldControl.Size = new Size(1920, 800);
            _hitControl = new HitPointBar(_player.ActiveTeam.GetTeamList()) {Name = "HitPoints"};
            _hitControl.Location = new Point(0, 170);
            _enemyHitControl = new HitPointBar(_game.Enemy.GetTeamList()) {Name = "EnemyHitPoints"};
            _enemyHitControl.Location = new Point(1736, 170);
            _interfaceControl = new InterfaceControl(_game) {Name = "Interface"};
            _interfaceControl.Location = new Point(0, 0);
            _interfaceControl.Size = new Size(1920, 170);
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            Controls.Add(_interfaceControl);
            Controls.Add(_hitControl);
            Controls.Add(_enemyHitControl);
            Controls.Add(_fieldControl);
            base.Refresh();
        }
    }
}