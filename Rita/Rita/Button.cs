
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
        //Class för knappar

        //Har en rektangel som finns för att checka om man klickat på knappen

        Rectangle bounds;
       
        Color color;

        Image image;

        int toolSize;

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

            //Om knappen ska ha en solid färg.

            bounds = new Rectangle(x, y, width, height);
            
            this.color = color;

        }

        public Button(int x, int y, int width, int height, Image img) {

            //Om knappen ska ha en bild .
            
            bounds = new Rectangle(x, y, width, height);
            this.image = img;

        
        }

        public void Draw(Graphics g) {

            //Ritar knappen beroende på om man har en bild eller solid färg,

            if (image != null)
                g.DrawImage(image, bounds);
            else if(color != null) {
                g.FillRectangle(new SolidBrush(color), bounds);
            }

            //Ritar selection om knappen är vald,

            if (isSelected) {
                DrawSelection(g);
            }

        }

        public void Draw(Graphics g, string tool, int toolSize)
        {

            g.FillRectangle(new SolidBrush(color), bounds);

            switch (tool)
            {
                case "Pen":
                    g.FillRectangle(new SolidBrush(Color.Black), (bounds.Width / 2) - (toolSize / 2), (bounds.Height / 2) - (toolSize / 2), toolSize, toolSize);
                    break;

                case "Brush":
                    g.FillEllipse(new SolidBrush(Color.Black), bounds.X, bounds.Y, toolSize, toolSize);
                    break;
                        
            }

        }       

        public void DrawSelection(Graphics g)
        {
            //Ritar en röd kant runt om knappen.

            g.DrawRectangle(new System.Drawing.Pen(Color.Red, 3), bounds.X - 3, bounds.Y - 3, bounds.Height + 3, bounds.Width + 3);
        }

        public void changeColor(Color color) {

            //Ändrar färg på knappen.

            this.color = color;
        }




    }
}
