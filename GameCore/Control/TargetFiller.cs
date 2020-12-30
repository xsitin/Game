using System.Drawing;
using System.Windows.Forms;

namespace GameCore.Control
{
    public class TargetFiller : UserControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Red, 3), 0, 0, 35, 35);
        }
    }
}