using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.Model;

namespace Game.Control
{
    public partial class EnemyButton : UserControl
    {
        private BasicCreature _enemyHero;
        public EnemyButton(BasicCreature enemyHero)
        {
            _enemyHero = enemyHero;
            Size = new Size(35, 35);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 3), 0, 0, 35, 35);
            //e.Graphics.DrawImage(Helper.Transfer[_enemyHero.Specialization], new Point());
        }
    }
}