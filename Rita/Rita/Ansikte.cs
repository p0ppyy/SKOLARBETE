using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Ansikte
    {
        public int x, y;
        public int size = 20;

        public Ansikte(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void Rita(Graphics g) {

            g.FillEllipse(new SolidBrush(Color.LightPink), x, y, size, size);
            g.FillRectangle(new SolidBrush(Color.Black), x + size/ 2 + size / 4, y + (size / 3), size / 10, size / 10);
            g.FillRectangle(new SolidBrush(Color.Black), x + size / 2 - size / 4, y +  (size / 3), size / 10, size / 10);
        }


    }
}
