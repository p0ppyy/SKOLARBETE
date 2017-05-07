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

        Queue<Tool> strokes = new Queue<Tool>();

		Stack<List<Tool>> strokesA = new Stack<List<Tool>>();

		List<Tool> temp = new List<Tool>();

		bool isMouseDown = false;

		Rectangle mouseRect = new Rectangle();

		Color selectedColor = new Color();

		Random rand = new Random();

        bool mouseClicked;

        Button colorButton;

        Button eraserButton;

        Button undoButton;

        Button pencilButton;

        Button brushButton;

        Button textButton;

        int toolSize = 0;

        string fileName = "Untitled";

        ImageFormat format = ImageFormat.Png;

        FolderBrowserDialog folderBrowser;

        public enum Tools {
            Brush,
            Pen,
            Text,
            NA
        };

        public Tools selectedTool;

        DrawArea drawingArea;

        #endregion

        public From1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			mouseRect.Width = 2;
			mouseRect.Height = 2;           

            selectedColor = Color.White;
            toolSize = brushSizeBar.Value;

            selectedTool = Tools.NA;

            colorButton = new Button(10, 34, 30, 30, selectedColor);
            eraserButton = new Button(130, 34, 30, 30, Properties.Resources.eraser);
            undoButton = new Button(170, 34,30, 30, Properties.Resources.undo);
            pencilButton = new Button(50, 34, 30, 30, Properties.Resources.pencil);
            brushButton = new Button(90, 34, 30, 30, Properties.Resources.brush);
            textButton = new Button(210, 34, 30, 30, Properties.Resources.text);

            mouseClicked = false;

            lblBrushSize.Text = "Size: " + toolSize;

            folderBrowser = new FolderBrowserDialog();

            drawingArea = new DrawArea();

            CreateNewFile();

		}

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseRect.IntersectsWith(colorButton.Bounds))
            {
                colorDialog1.ShowDialog();
                selectedColor = colorDialog1.Color;
                colorButton.changeColor(selectedColor);
                eraserButton.IsSelected = false;
            }
            else if (mouseRect.IntersectsWith(eraserButton.Bounds))
            {
                selectedColor = Color.FromName("Control");
                eraserButton.IsSelected = true;
                pencilButton.IsSelected = false;
                textButton.IsSelected = false;
                brushButton.IsSelected = false;
            }
            else if (mouseRect.IntersectsWith(undoButton.Bounds))
            {
                if (strokesA.Count > 0)
                {
                    strokesA.Pop();
                    Invalidate();
                }

            }
            else if (mouseRect.IntersectsWith(pencilButton.Bounds))
            {
                pencilButton.IsSelected = true;
                brushButton.IsSelected = false;
                eraserButton.IsSelected = false;
                textButton.IsSelected = false;
                selectedTool = Tools.Pen;
            }
            else if (mouseRect.IntersectsWith(brushButton.Bounds))
            {
                brushButton.IsSelected = true;
                pencilButton.IsSelected = false;
                eraserButton.IsSelected = false;
                textButton.IsSelected = false;
                selectedTool = Tools.Brush;
            } else if (mouseRect.IntersectsWith(textButton.Bounds)) {
                textButton.IsSelected = true;
                pencilButton.IsSelected = false;
                eraserButton.IsSelected = false;
                brushButton.IsSelected = false;

                selectedTool = Tools.Text;
            }

            else {
                switch (selectedTool)
                {
                    case Tools.Pen:
                        temp.Add(new Pen(e.X, e.Y, selectedColor, toolSize));
                        break;
                    case Tools.Brush:
                        temp.Add(new Brush(e.X, e.Y, selectedColor, toolSize));
                        break;

                    default:
                        break;
                }
            }

            Invalidate();

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseRect.Location = new Point(e.X, e.Y);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            colorButton.Draw(e.Graphics);

            eraserButton.Draw(e.Graphics);

            undoButton.Draw(e.Graphics);

            pencilButton.Draw(e.Graphics);

            brushButton.Draw(e.Graphics);

            textButton.Draw(e.Graphics);

		}

        private void btnClear_Click(object sender, EventArgs e)
        {
            strokesA.Clear();
			Invalidate();
		}

        private void brushSizeBar_Scroll(object sender, EventArgs e)
        {
            
            toolSize = brushSizeBar.Value;
            lblBrushSize.Text = "Sizes: " + toolSize;

        }

        public void drawingArea_MouseMove(object sender, MouseEventArgs e) {
            if (isMouseDown)
            {

                switch (selectedTool)
                {
                    case Tools.Pen:
                        temp.Add(new Pen(e.X, e.Y, selectedColor, toolSize));
                        break;
                    case Tools.Brush:
                        temp.Add(new Brush(e.X, e.Y, selectedColor, toolSize));
                        break;
                    default:
                        break;
                }
                drawingArea.FileSaved = false;

            }
            else if (!isMouseDown && temp.Count > 0)
            {
                strokesA.Push(new List<Tool>(temp));
                temp.Clear();
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

            switch (selectedTool)
            {
                case Tools.Pen:
                    temp.Add(new Pen(e.X, e.Y, selectedColor, toolSize));
                    break;
                case Tools.Brush:
                    temp.Add(new Brush(e.X, e.Y, selectedColor, toolSize));
                    break;
                case Tools.Text:
                    mouseClicked = true;
                    temp.Add(new Text("Hej", e.X, e.Y, selectedColor));

                    break;
                default:
                    break;
            }
        }

        public void RenderDrawingArea(object sender, PaintEventArgs e) {

            for (int i = strokesA.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < strokesA.ElementAt(i).Count; j++)
                {
                    strokesA.ElementAt(i)[j].Draw(e.Graphics);
                }
            }

            foreach (Tool tool in temp)
            {
                tool.Draw(e.Graphics);
            }

        }

        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (drawingArea != null)
            {
                drawingArea.Invalidate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawingArea.SaveAsImage();
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawingArea.SaveAsImage();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void InitDrawingArea(){
            this.Controls.Add(drawingArea);
            drawingArea.MouseMove += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseMove);
            drawingArea.MouseDown += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseDown);
            drawingArea.MouseUp += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseUp);
            drawingArea.MouseClick += new System.Windows.Forms.MouseEventHandler(drawingArea_MouseClick);
            drawingArea.Paint += new PaintEventHandler(RenderDrawingArea);
        }

        private void DisposeDrawingArea() {
            this.Controls.Remove(drawingArea);
        }

        private void CreateNewFile() {
            DisposeDrawingArea();
            NewFile form = new NewFile();
            form.ShowDialog();
            int formWidth = form.width;
            int formHeight = form.height;
            if (formWidth > 1900)
            {
                drawingArea = new DrawArea(10, (Height / 2) - (formHeight / 2), form.width, form.height);
            } else if (formHeight > 905) {
                drawingArea = new DrawArea((Width / 2) - (formWidth / 2), 100, formWidth, formHeight);
            }
            else
            {
                drawingArea = new DrawArea((Width / 2) - (formWidth / 2), (Height / 2) - (formHeight / 2), formWidth, formHeight);
            }
            InitDrawingArea();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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

    }

}