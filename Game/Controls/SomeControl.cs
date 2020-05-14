using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game.Controls
{
    public class SomeControl : UserControl
    {
        private bool clicked;
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var c = new TextureBrush(new Bitmap(@"C:\Users\xsitin\Game\Game\Properties\StartGameArt.jpg"));
            g.DrawImage(Image.FromFile(@"C:\Users\xsitin\Game\Game\Properties\StartGameArt.jpg"), new Point(0, 0));
            if(clicked)
                g.DrawString("clicked", new Font(FontFamily.GenericMonospace, 10),
                    new SolidBrush(Color.Brown) , 20, 20);
        }

        protected override void OnClick(EventArgs e)
        {
            clicked = true;
        }
    }
}