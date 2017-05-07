using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Text : Tool
    {
        string text;

        public Text(string text, int x, int y, Color color) {
            this.text = text;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public override void Draw(Graphics graphics)
        {
            graphics.DrawString(text, new Font("Arial", 16), new SolidBrush(color), x, y);
        }
    }
}
