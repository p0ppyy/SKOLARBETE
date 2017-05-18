using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Rita
{
    public partial class From1 : Form
    {

        #region Variables

		Stack<List<Tool>> strokes = new Stack<List<Tool>>();

		List<Tool> tempStrokes = new List<Tool>();

		bool isMouseDown = false;

		Rectangle mouseRect = new Rectangle();

		Color selectedColor = new Color();

        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        Button colorButton;

        Button eraserButton;

        Button undoButton;

        Button pencilButton;

        Button brushButton;

        Button sizeButton;

        int toolSize = 0;

        string fileName = "Untitled";

        ImageFormat format = ImageFormat.Png;

        FolderBrowserDialog folderBrowser;

        public enum Tools {
            Brush,
            Pen,
            NA
        };

        public Tools selectedTool;

        DrawArea drawingArea;

        #endregion

        public From1()
        {
            InitializeComponent();
            //Gör att fönstret börjar i fullskärm
            this.WindowState = FormWindowState.Maximized;
            Console.WriteLine(Screen.PrimaryScreen.Bounds.Height);
        }

		private void Form1_Load(object sender, EventArgs e)
		{
            //Gör en rektangel som kommer flyttas med muspekarens position.
            mouseRect = new Rectangle();
            mouseRect.Width = 2;
			mouseRect.Height = 2;           

            //Gör den valda färgen till vit.   
            selectedColor = Color.White;

            //Sätter ursprungsvärdet på rit storleken.
            toolSize = brushSizeBar.Value;

            //Sätter valt verktyg till NA.
            selectedTool = Tools.NA;

            //Initierar alla knappar och deras grafik.
            colorButton = new Button(10, 34, 30, 30, selectedColor);
            eraserButton = new Button(130, 34, 30, 30, Properties.Resources.eraser);
            undoButton = new Button(170, 34,30, 30, Properties.Resources.undo);
            pencilButton = new Button(50, 34, 30, 30, Properties.Resources.pencil);
            brushButton = new Button(90, 34, 30, 30, Properties.Resources.brush);
            sizeButton = new Button(210, 34, 30, 30, Color.White);

            //Ädrar texten på ritstorleks lablen till rätt.
            lblBrushSize.Text = "Size: " + toolSize;

            //Initierar en ny rityta för att kunna ha tillgång till maxstorlek på ritytan.
            drawingArea = new DrawArea();

            //Skapar en ny fil.
            createNewFile();

            Console.WriteLine(Tools.Pen.ToString());

		}

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //Kontrollerar om muspekaren är över en av knapparna när musen klickas

            if (mouseRect.IntersectsWith(colorButton.Bounds))
            {
                //Öppnar colordialog och gör selectedColor till den valda färgen i dialogen
                colorDialog1.ShowDialog();
                selectedColor = colorDialog1.Color;           
                colorButton.changeColor(selectedColor);
                //Gör suddknappen inte vald för att färgen har ändrats
                eraserButton.IsSelected = false;

            }
            else if (mouseRect.IntersectsWith(eraserButton.Bounds))
            {
                //Lätt lösning på suddi, gör bara färgen till vit och
                //avväljer alla andra verktyg
                selectedColor = Color.FromName("Control");
                eraserButton.IsSelected = true;
                pencilButton.IsSelected = false;
                brushButton.IsSelected = false;
            }
            else if (mouseRect.IntersectsWith(undoButton.Bounds))
            {   
                //Poppar det sista elementet i strokes stacken

                if (strokes.Count > 0)
                {
                    strokes.Pop();
                    Invalidate();
                }

            }
            else if (mouseRect.IntersectsWith(pencilButton.Bounds))
            {

                //Väljer pen knappen och avväljer alla andra.
                pencilButton.IsSelected = true;
                brushButton.IsSelected = false;
                eraserButton.IsSelected = false;
                selectedTool = Tools.Pen;
            }
            else if (mouseRect.IntersectsWith(brushButton.Bounds))
            {
                //Väljer pensel knappen och avväljer alla andra.
                brushButton.IsSelected = true;
                pencilButton.IsSelected = false;
                eraserButton.IsSelected = false;
                selectedTool = Tools.Brush;
            }

            //Annars ritar den med det valda vertyget

            else {
                drawWithTool(selectedTool, e.X, e.Y);
            }

            //Invaliderar UI för att rita om all grafik

            Invalidate();

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //Updaterar musrektanglens position när musen rör på sig.
            mouseRect.Location = new Point(e.X, e.Y);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Rendrerar alla knappar
            colorButton.Draw(e.Graphics);

            eraserButton.Draw(e.Graphics);

            undoButton.Draw(e.Graphics);

            pencilButton.Draw(e.Graphics);

            brushButton.Draw(e.Graphics);

            sizeButton.Draw(e.Graphics, selectedTool.ToString(), toolSize);

		}

        private void brushSizeBar_Scroll(object sender, EventArgs e)
        {
            //Updaterar ritstorleken när scrollens värde ändras

            toolSize = brushSizeBar.Value;
            lblBrushSize.Text = "Sizes: " + toolSize;

        }

        public void drawingArea_MouseMove(object sender, MouseEventArgs e) {

            //Körs varje gång musenpekaren rör på sig i ritytan

            //Checkar om musen är nere
            if (isMouseDown)
            {

                //Ritar med valt verktyg
                drawWithTool(selectedTool, e.X, e.Y);
                //Gör ritytan inte sparad
                drawingArea.FileSaved = false;

            }
            //Checkar att musen inte är nere och att den temporära listan med strokes har något innehåll
            else if (!isMouseDown && tempStrokes.Count > 0)
            {
                //Pushar temp listan till stacken och tömmer temp listan
                strokes.Push(new List<Tool>(tempStrokes));

                tempStrokes.Clear();
            }

        }

        private void drawingArea_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }

        private void drawingArea_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void drawingArea_MouseClick(object sender, MouseEventArgs e) {

            drawWithTool(selectedTool, e.X, e.Y);
        }

        public void renderDrawingArea(object sender, PaintEventArgs e) {

            //Funtion för att rendera all grafik på ritytan


            //Går igenom alla listor i stacken och renderar alla strokes i dem.
            for (int i = strokes.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < strokes.ElementAt(i).Count; j++)
                {
                    strokes.ElementAt(i)[j].Draw(e.Graphics);
                }
            }

             /**
              * Går igenom temp listan och renderar alla strokes i den.
              * Gör detta för att strokes ska renderas samtidigt som man
              * håller inne musen
             **/

            foreach (Tool tool in tempStrokes)
            {
                tool.Draw(e.Graphics);
            }

        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            //Invaliderar ritytan vilket gör att renderDrawingArea() körs.
            //Sker bara om ritytan finns
            if (drawingArea != null)
            {
                drawingArea.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Körs när man klickat på ny fil i toolbaren
            createNewFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Körs när man klickat på save i toolbaren.
            drawingArea.SaveAsImage();
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            //Körs när man klickat på save as i toolbaren.
            drawingArea.SaveAsImage();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Körs när man klickat på open i toolbaren.
            openFile();
               
        }

        private void drawWithTool(Tools tool, int x, int y)
        {
            //Switchar mellan valtverktyg och lägger till en stroke av valt verkyg i templistan.
            switch (tool)
            {
                case Tools.Pen:
                   
                    tempStrokes.Add(new Pen(x, y, selectedColor, toolSize));
                    break;
                case Tools.Brush:
                    tempStrokes.Add(new Brush(x, y, selectedColor, toolSize));
                    break;

                default:
                    break;
            }
        }
  
        private void initDrawingArea(){
            //Initierar alla funtioner till ritytan och lägger till den i forms
            this.Controls.Add(drawingArea);
            drawingArea.MouseMove += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseMove);
            drawingArea.MouseDown += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseDown);
            drawingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseUp);
            drawingArea.MouseClick += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseClick);
            drawingArea.Paint += new PaintEventHandler(renderDrawingArea);
        }

        private void disposeDrawingArea() {
            //Tarbort ritytan från forms.
            this.Controls.Remove(drawingArea);
        }

        private void openFile() {
            //Öppnar en OpenFileDialog som där man kan öppna en bild.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            ofd.Title = "Please select a image to open";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Tar bort nuvarande rityta och gör en BitMap av vald fil.
                disposeDrawingArea();
                Bitmap img = new Bitmap(ofd.FileName);

                if (img.Width > drawingArea.maxWidth && img.Height > drawingArea.maxHeight)
                {
                    //Körs om bildens bredd och höjd är större än skärmen 
                    //Har inte hittat ett sätt att skala upp eller ner 
                    //bilder därför klipper den bara av den i kanten så
                    //den får plats.
                    drawingArea = new DrawArea(10, 100, img.Width, img.Height);
                    drawingArea.BackgroundImage = Image.FromHbitmap(img.GetHbitmap());
                    Console.WriteLine("file 1");
                }
                else if (img.Width > drawingArea.maxWidth)
                {
                    //Klipper av bilden om bredden är bredare än skärmen.
                    //Och positonerar ritytan i mitten av fönstret.
                    drawingArea = new DrawArea(10, (Height / 2) - (img.Height / 2), img.Width, img.Height);
                    drawingArea.BackgroundImage = Image.FromHbitmap(img.GetHbitmap());
                    Console.WriteLine("file 1");
                }
                else if (img.Height > drawingArea.maxHeight)
                {
                    //Klipper av bilden om höjden är högre än skärmen.
                    //Och positonerar ritytan i mitten av fönstret.
                    drawingArea = new DrawArea((Width / 2) - (img.Width / 2), 100, img.Width, img.Height);
                    drawingArea.BackgroundImage = Image.FromHbitmap(img.GetHbitmap());
                    Console.WriteLine("file 2");
                }
                else
                {
                    //Gör ritytan lika stor som bilden
                    //Positonerar ritytan i mitten av fönstret.
                    //Renderar bilden på ritytan
                    drawingArea = new DrawArea((Width / 2) - (img.Width / 2) - 8, (Height / 2) - (img.Height / 2), img.Width, img.Height);
                    drawingArea.BackgroundImage = Image.FromHbitmap(img.GetHbitmap());
                    Console.WriteLine("file 3");
                }
                initDrawingArea();
            }
        }

        private void createNewFile() {
            //Tarbort nuvarande rityta och skapar en ny
            disposeDrawingArea();
            //Skapar en nyfil form som körs som en dialog.
            NewFile form = new NewFile();
            form.ShowDialog();
            int formWidth = form.width;
            int formHeight = form.height;
            //Kod för att postionera ritytan i mitten av skärmen,
            //hanterar även om man skulle ha ett värde som är större
            //än skärmens storlek.
            if (formWidth > 1900 && formHeight > 905)
            {
                drawingArea = new DrawArea(10, 100, form.width, form.height);
            }
            else if (formWidth > 1900)
            {
                drawingArea = new DrawArea(10, (Height / 2) - (formHeight / 2), form.width, form.height);
            }
            else if (formHeight > 905) {
                drawingArea = new DrawArea((Width / 2) - (formWidth / 2), 100, formWidth, formHeight);
            }
            else
            {
                drawingArea = new DrawArea((Width / 2) - (formWidth / 2), (Height / 2) - (formHeight / 2), formWidth, formHeight);
            }
            //Initierar ritytan
            initDrawingArea();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //Körs innan man stänger fönstret
            //Detta görs för att kolla om man har sparat filen.
            if (!drawingArea.FileSaved && drawingArea.FileCreated)
            {
                e.Cancel = false;
                DialogResult result = MessageBox.Show("Save changes to " + fileName + " before quiting", "Drawing 1.0", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    drawingArea.SaveAsImage();
                }
                else if (result == DialogResult.Cancel) {
                    e.Cancel = true;
                }
            }
        }      

        private void From1_ResizeEnd(object sender, EventArgs e)
        {

            //Ompositionerar ritytan till mitten när fönstret ändrat storlek

            if ((Height / 2) - (drawingArea.Height / 2) < 100)
            {
                drawingArea.Reposition((Width / 2) - (drawingArea.Width / 2), (Height / 2) - (drawingArea.Height / 2) + (100 - (Height / 2) + (drawingArea.Height / 2)));
            }
            else
            {
                drawingArea.Reposition((Width / 2) - (drawingArea.Width / 2), (Height / 2) - (drawingArea.Height / 2));
            }
        }

    }

}