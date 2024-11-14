using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MultiMedia2
{
    public partial class Form1 : Form
    {
        private Point startPoint;
        private Point endPoint;
        private bool isDrawing = false;
        private Bitmap canvasBitmap; // Bitmap to store the drawings
        private Pen pen;
        private float thickness = 2; // Default thickness
        private string selectedShape = "Line"; // Default shape
        private Stack<Bitmap> undoStack = new Stack<Bitmap>();
        private Stack<Bitmap> redoStack = new Stack<Bitmap>();
        public Form1()
        {
            InitializeComponent();

            // Initialize pen and canvas bitmap
            pen = new Pen(Color.Blue, thickness);
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            // Set up the ComboBox for shape selection
            comboBox1.Items.AddRange(new object[] { "Line", "Square", "Rectangle", "Circle", "Ellipse", "Arc", "Pie", "Polygon", "Bezier Curve" });
            comboBox1.SelectedIndex = 0; // Default to "Line"

            // Set pictureBox1 image to the canvas bitmap
            pictureBox1.Image = canvasBitmap;

            // Wire up pictureBox events
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
            pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Start drawing
            SaveState();
            isDrawing = true;
            startPoint = e.Location;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // Track mouse movement while drawing
            if (isDrawing)
            {
                endPoint = e.Location;
                pictureBox1.Invalidate(); // Trigger paint event to show temporary shape
            }

            // Update status bar information
            toolStripStatusLabel1.Text = $"Tool: {selectedShape}, Thickness: {thickness}, Color: {pen.Color.Name}, Coordinates: ({e.X}, {e.Y})";
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // Complete the drawing and save it to the bitmap
            if (isDrawing)
            {
                isDrawing = false;

                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    switch (selectedShape)
                    {
                        case "Line":
                            g.DrawLine(pen, startPoint, endPoint);
                            break;
                        case "Square":
                            DrawSquare(g);
                            break;
                        case "Rectangle":
                            DrawRectangle(g);
                            break;
                        case "Circle":
                            DrawCircle(g);
                            break;
                        case "Ellipse":
                            DrawEllipse(g);
                            break;
                        case "Arc":
                            DrawArc(g);
                            break;
                        case "Pie":
                            DrawPie(g);
                            break;
                        case "Polygon":
                            DrawPolygon(g);
                            break;
                        case "Bezier Curve":
                            DrawBezierCurve(g);
                            break;
                    }
                }

                pictureBox1.Invalidate(); // Refresh the pictureBox to show the updated drawing
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Only show the temporary drawing during mouse movement
            if (isDrawing)
            {
                switch (selectedShape)
                {
                    case "Line":
                        e.Graphics.DrawLine(pen, startPoint, endPoint);
                        break;
                    case "Square":
                        DrawSquare(e.Graphics);
                        break;
                    case "Rectangle":
                        DrawRectangle(e.Graphics);
                        break;
                    case "Circle":
                        DrawCircle(e.Graphics);
                        break;
                    case "Ellipse":
                        DrawEllipse(e.Graphics);
                        break;
                    case "Arc":
                        DrawArc(e.Graphics);
                        break;
                    case "Pie":
                        DrawPie(e.Graphics);
                        break;
                    case "Polygon":
                        DrawPolygon(e.Graphics);
                        break;
                    case "Bezier Curve":
                        DrawBezierCurve(e.Graphics);
                        break;
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Update pen thickness
            thickness = (float)numericUpDown1.Value;
            pen.Width = thickness;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the selected shape based on ComboBox selection
            selectedShape = comboBox1.SelectedItem.ToString();
        }


        private void DrawSquare(Graphics g)
        {
            int sideLength = Math.Min(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y));
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            g.DrawRectangle(pen, x, y, sideLength, sideLength);
        }

        private void DrawRectangle(Graphics g)
        {
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(endPoint.X - startPoint.X);
            int height = Math.Abs(endPoint.Y - startPoint.Y);
            g.DrawRectangle(pen, x, y, width, height);
        }

        private void DrawCircle(Graphics g)
        {
            int diameter = Math.Min(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y));
            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            g.DrawEllipse(pen, x, y, diameter, diameter);
        }

        private void DrawEllipse(Graphics g)
        {
            int width = Math.Abs(endPoint.X - startPoint.X);
            int height = Math.Abs(endPoint.Y - startPoint.Y);
            g.DrawEllipse(pen, startPoint.X, startPoint.Y, width, height);
        }

        private void DrawArc(Graphics g)
        {
            int width = Math.Abs(endPoint.X - startPoint.X);
            int height = Math.Abs(endPoint.Y - startPoint.Y);
            g.DrawArc(pen, startPoint.X, startPoint.Y, width, height, 0, 180); // example arc
        }

        private void DrawPie(Graphics g)
        {
            int width = Math.Abs(endPoint.X - startPoint.X);
            int height = Math.Abs(endPoint.Y - startPoint.Y);
            g.DrawPie(pen, startPoint.X, startPoint.Y, width, height, 0, 90); // example pie
        }

        private void DrawPolygon(Graphics g)
        {
            Point[] points = { startPoint, new Point(endPoint.X, startPoint.Y), endPoint, new Point(startPoint.X, endPoint.Y) };
            g.DrawPolygon(pen, points);
        }

        private void DrawBezierCurve(Graphics g)
        {
            // Example Bezier curve with control points
            Point controlPoint1 = new Point(startPoint.X, endPoint.Y);
            Point controlPoint2 = new Point(endPoint.X, startPoint.Y);
            g.DrawBezier(pen, startPoint, controlPoint1, controlPoint2, endPoint);
        }

        private void buttonColor_Click_1(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog.Color;
            }

        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvasBitmap;
            pictureBox1.Invalidate();

        }

        private void Load_Click(object sender, EventArgs e)
        {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Image backgroundImage = Image.FromFile(openFileDialog.FileName);
                Graphics g = Graphics.FromImage(canvasBitmap);
                g.DrawImage(backgroundImage, 0, 0, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Invalidate(); // Refresh the pictureBox
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                canvasBitmap.Save(saveFileDialog.FileName);
            }
        }

        private void SaveState()
        {
            undoStack.Push((Bitmap)canvasBitmap.Clone());
            redoStack.Clear();
        }

        private void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push((Bitmap)canvasBitmap.Clone());
                canvasBitmap = undoStack.Pop();
                pictureBox1.Image = canvasBitmap;
                pictureBox1.Invalidate();
            }
        }

        private void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push((Bitmap)canvasBitmap.Clone());
                canvasBitmap = redoStack.Pop();
                pictureBox1.Image = canvasBitmap;
                pictureBox1.Invalidate();
            }
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
          Undo();
        }

        private void RedoBtn_Click(object sender, EventArgs e)
        {
            Redo();
        }
    }
}
