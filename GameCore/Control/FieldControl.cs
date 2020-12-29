using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public sealed partial class FieldControl : UserControl
    {
        private readonly List<BasicCreature>[] _creatures;
        public List<BasicCreature> _enemies;
        public List<BasicCreature> _heroes;
        private readonly Point[][] _points = new Point[4][] {new Point[4], new Point[4], new Point[4], new Point[4]};


        public FieldControl(Team<Hero> heroes, Team<EnemyHero> enemies)
        {
            DoubleBuffered = true;
            _heroes = heroes.GetTeamList();
            _enemies = enemies.GetTeamList();
            BackColor = Color.Transparent;
            _creatures = new[]
            {
                heroes.FirstLine.Cast<BasicCreature>().ToList(),
                heroes.SecondLine.Cast<BasicCreature>().ToList(),
                enemies.FirstLine.Cast<BasicCreature>().ToList(),
                enemies.SecondLine.Cast<BasicCreature>().ToList()
            };
            _points[0][0] = new Point(100, 550);
            _points[0][1] = new Point(100, 400);
            _points[0][2] = new Point(100, 200);
            _points[0][3] = new Point(100, 50);
            _points[1][0] = new Point(0, 550);
            _points[1][1] = new Point(0, 400);
            _points[1][2] = new Point(0, 200);
            _points[1][3] = new Point(0, 50);
            _points[2][0] = new Point(1200, 550);
            _points[2][1] = new Point(1200, 400);
            _points[2][2] = new Point(1200, 200);
            _points[2][3] = new Point(1200, 50);
            _points[3][0] = new Point(1300, 550);
            _points[3][1] = new Point(1300, 400);
            _points[3][2] = new Point(1300, 200);
            _points[3][3] = new Point(1300, 50);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (var i = 0; i < _creatures.Length; i++)
            for (var j = _creatures[i].Count - 1; j >= 0; j--)
                if (_creatures[i][j].Characteristics[Characteristics.Health] > 0)
                    e.Graphics.DrawImage(
                        _creatures[i][j] is Hero
                            ? Helper.ImageTransfer[_creatures[i][j].Specialization]
                            : Helper.EnemyImageTransfer[_creatures[i][j].Specialization],
                        new Rectangle(_points[i][j], new Size(130, 200)));
                else
                    e.Graphics.DrawImage(
                        _creatures[i][j] is Hero
                            ? Helper.DeadImageTransfer[_creatures[i][j].Specialization]
                            : Helper.DeadEnemyImageTransfer[_creatures[i][j].Specialization],
                        new Rectangle(_points[i][j], new Size(130, 200)));
        }
    }
}