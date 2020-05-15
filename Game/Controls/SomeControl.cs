﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game.Controls
{
    public class SomeControl : UserControl
    {
        private bool clicked;
        protected override void OnPaint(PaintEventArgs e)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "Image\\");
            var g = e.Graphics;
            var c = new TextureBrush(new Bitmap(exePath + "StartGameArt.jpg"));
            g.DrawImage(Image.FromFile(@exePath + "StartGameArt.jpg"), new Point(0, 0));
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