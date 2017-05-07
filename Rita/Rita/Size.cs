using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Size
    {
        Rectangle rect;
        int brushSize, x, y, size;
        bool selected;

        public Size(int brushSize, int x, int y, int size) {
            this.brushSize = brushSize;
            this.x = size;
            this.y = y;
            this.size = size;

            rect = new Rectangle(x, y, size, size);
        }

        public void Draw(Graphics g) {

            g.FillRectangle(new SolidBrush(Color.White), rect);
            g.FillEllipse(new SolidBrush(Color.Black), rect.X, rect.Y, brushSize, brushSize);
            if (selected) {
                DrawSelection(g);
            }
        }

        public void DrawSelection(Graphics g) {
            g.DrawRectangle(new Pen(Color.Black, 5), rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4);
        }

        public int Select() {
            selected = true;
            return brushSize;
        }

        public void DeSelect() {
            selected = false;
        }

        public Rectangle getRect() {
            return rect;
        }

    }
}
