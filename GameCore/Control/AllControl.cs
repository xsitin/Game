using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GameCore.Model;

namespace GameCore.Control
{
    public partial class AllControl : UserControl
    {
        private HitPointBar _enemyHitControl;
        private FieldControl _fieldControl;
        private readonly Model.Game _game;
        private HitPointBar _hitControl;
        private InterfaceControl _interfaceControl;
        private readonly Player _player;

        public AllControl(Model.Game game, Player player)
        {
            DoubleBuffered = true;
            _game = game;
            _player = player;
            _fieldControl = new FieldControl(_game.Heroes, _game.Enemy) {Name = "Field"};
            _fieldControl.Location = new Point(250, 200);
            _fieldControl.Size = new Size(1920, 800);
            _hitControl = new HitPointBar(_player.ActiveTeam?.GetTeamList()) {Name = "HitPoints"};
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
                foreach (var key in hero.StandardChars.Keys.Where(key => hero.Characteristics != null))
                    if (hero.Characteristics != null)
                        hero.Characteristics[key] = hero.StandardChars[key];
                _player.Gold += _game.Reward.money;
                var mers = new List<Hero>();
                var fac = new HeroesFactory(_player);
                for (var i = 0; i < 4; i++) mers.Add(fac.GetRandomHero());
                _player.Mercenaries = mers;
                ((View.Form1) Application.OpenForms["Main"]! ?? throw new InvalidOperationException()).VillageControls();
                Dispose();
                return;
            }

            Controls.Clear();
            if (!_fieldControl.Enemies.Any(x => x.Characteristics[Characteristics.Health] > 0))
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