using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class FieldControl : UserControl
    {
        private Point[][] _points = new Point[4][] {new Point[4], new Point[4], new Point[4], new Point[4]};
        private List<BasicCreature>[] _creatures;
        private List<BasicCreature> _heroes;
        private List<BasicCreature> _enemies;


        public FieldControl(Team<Hero> heroes, Team<EnemyHero> enemies)
        {
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
            _points[0][0] = new Point(100, 600);
            _points[0][1] = new Point(100, 500);
            _points[0][2] = new Point(100, 400);
            _points[0][3] = new Point(100, 300);
            _points[1][0] = new Point(0, 600);
            _points[1][1] = new Point(0, 500);
            _points[1][2] = new Point(0, 400);
            _points[1][3] = new Point(0, 300);
            _points[2][0] = new Point(1200, 600);
            _points[2][1] = new Point(1200, 500);
            _points[2][2] = new Point(1200, 400);
            _points[2][3] = new Point(1200, 300);
            _points[3][0] = new Point(1300, 600);
            _points[3][1] = new Point(1300, 500);
            _points[3][2] = new Point(1300, 400);
            _points[3][3] = new Point(1300, 300);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i = 0; i < _creatures.Length; i++)
            for (int j = 0; j < _creatures[i].Count; j++)
            {
                if (_creatures[i][j].Characteristics[Characteristics.Health] > 0)
                    e.Graphics.DrawImage(
                        _creatures[i][j] is Hero
                            ? Helper.ImageTransfer[_creatures[i][j].Specialization]
                            : Helper.EnemyImageTransfer[_creatures[i][j].Specialization],new Rectangle(_points[i][j],new Size(130,200)));
                else
                    _creatures[i].RemoveAt(j);
            }
        }
    }
}