using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Brush : Tool
    {
        
        //Class för pensel verktyget

        public Brush(int x, int y, Color color, int size) {
            this.x = x;
            this.y = y;
            this.size = size;
			this.color = color;
        }

        override public void Draw(Graphics  g) {

            //Ritar en cirkel på den angivna positionen

            g.FillEllipse(new SolidBrush(color), x, y, size, size);
        }

    }
}
