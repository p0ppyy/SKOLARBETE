using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Rita
{
    class DrawArea : Panel
    {

        bool fileCreated, fileSaved;
        string fileName = "Untitled";
        ImageFormat format = ImageFormat.Png;

        public int maxWidth = Screen.PrimaryScreen.Bounds.Width - 20;
        public int maxHeight = Screen.PrimaryScreen.Bounds.Height - 175;

        public DrawArea() {
            this.MaximumSize = new Size(maxWidth, maxHeight);
            Console.WriteLine(maxHeight);
        }

        public DrawArea(int x, int y, int width, int height) {
            this.DoubleBuffered = true;
            this.Width = width;
            this.Height = height;
            this.Location = new Point(x, y);
            this.BackColor = Color.White;
            this.MaximumSize = new Size(maxWidth, maxHeight);
        }
    
        public DrawArea(int x, int y, int width, int height, int scale)
        {
            this.DoubleBuffered = true;
            this.Width = width;
            this.Height = height;
            this.Location = new Point(x, y);
            this.BackColor = Color.White;
            this.Scale(new Size(scale, scale));

        }

        public bool FileCreated{
            set {
                fileCreated = value;
            }
            get {
                return fileCreated;
            }
        }

        public bool FileSaved {
            set {
                fileSaved = value;
            }
            get {
                return fileSaved;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        public void Reposition(int x, int y) {
            this.Location = new Point(x, y);
        }

        public void SaveAsImage() {
            if (fileCreated)
            {
                Bitmap bm = new Bitmap(Width, Height);
                DrawToBitmap(bm, new Rectangle(0, 0, Width, Height));
                bm.Save(fileName, format);
                fileSaved = true;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images | *.png; *.bmp; *.jpg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    fileName = sfd.FileName;
                    string ext = System.IO.Path.GetExtension(fileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;

                    }

                    Bitmap bm = new Bitmap(Width, Height);
                    DrawToBitmap(bm, new Rectangle(0, 0, Width,  Height));
                    bm.Save(fileName, format);
                    fileCreated = true;
                    fileSaved = true;
                }

            }
        }

    }
}
