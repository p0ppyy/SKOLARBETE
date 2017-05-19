using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Färg
    {
        int x, y, size;

        Rectangle rect;

        Color color;

		SolidBrush sbrush;

		public bool selected = false;

        public Färg(int x, int y, Color color ,int size) {
            this.x = x;
            this.y = y;
			this.size = size;

            rect = new Rectangle(x,y,size,size);
			this.color = color;

			sbrush = new SolidBrush(color);

        }

        public Color Select() {

			selected = true;

            return color;
           
        }

        public void DeSelect() {
            selected = false;
        }

        public void Draw(Graphics g){
			
            g.FillRectangle(sbrush, rect.X, rect.Y, rect.Width, rect.Height);
            if (selected)
                DrawSelection(g);

		}

        public void DrawSelection(Graphics g) {
            g.DrawRectangle(new Pen(Color.Black, 5), rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4);
        }

		public Rectangle getRect() {
			return rect;
		}

		public int getSize() {
			return size;	
		}

    }
}
