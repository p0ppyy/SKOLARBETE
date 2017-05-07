
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Rita
{
    class Button
    {

        Rectangle bounds;
       
        Color color;

        Image image;

        bool isSelected = false;

        public Rectangle Bounds {
            get {
                return bounds;
            }

            set {
                bounds = value;
            }

        }

        public bool IsSelected {
            get {
                return isSelected;
            }
            set {
                isSelected = value;
            }

        }

        public Button(int x, int y, int width, int height, Color color) {

            bounds = new Rectangle(x, y, width, height);
            
            this.color = color;

        }

        public Button(int x, int y, int width, int height, Image img) {
            bounds = new Rectangle(x, y, width, height);
            this.image = img;

        
        }

        public void Draw(Graphics g) {

            if (image != null)
                g.DrawImage(image, bounds);
            else if(color != null) {
                g.FillRectangle(new SolidBrush(color), bounds);
            }

            if (isSelected) {
                DrawSelection(g);
            }

        }

        public void DrawSelection(Graphics g)
        {
            g.DrawRectangle(new System.Drawing.Pen(Color.Red, 3), bounds.X - 3, bounds.Y - 3, bounds.Height + 3, bounds.Width + 3);
        }

        public void changeColor(Color color) {
            this.color = color;
        }




    }
}
