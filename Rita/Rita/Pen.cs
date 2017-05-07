using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Pen : Tool
    {
        public Pen(int x, int y, Color color, int size)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.color = color;
        }

        override public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), x, y, size, size);
        }

    }
}
