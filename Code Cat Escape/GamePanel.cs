using System;
using System.Collections.Generic;
using System.Text;

namespace Code_Cat_Escape
{
    public class GamePanel : Panel
    {
        public GamePanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

                SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            UpdateStyles();
        }
    }
}
