using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Tool
    {
        //Generell class för ett verktyg

        public int x, y, size;
        public Color color;

        public virtual void Draw(Graphics graphics)
        {
        
        }


    }
}
