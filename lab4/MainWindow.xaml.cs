using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int x1, y1, x2, y2, r;
        private double zoomMax = 5;
        private double zoomMin = 0.5;
        private double zoomSpeed = 0.001;
        private double zoom = 1;
        private double pointSize = 5;
        private int gridSize = 50;
        private List<(int, int)> points;


        public MainWindow()
        {
            InitializeComponent();
            points = new List<(int, int)>();
            PaintGrid();
        }

        public bool GetValues()
        {
            return int.TryParse(x1TextBox.Text, out x1)
                && int.TryParse(y1TextBox.Text, out y1)
                && int.TryParse(x2TextBox.Text, out x2)
                && int.TryParse(y2TextBox.Text, out y2)
                && int.TryParse(rTextBox.Text, out r);
        }

        public void CheckNumber(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !int.TryParse(fullText, out int val) || Math.Abs(val) > gridSize;
        }

        private void StepClick(object sender, RoutedEventArgs e)
        {
            if (!GetValues())
            {
                _ = MessageBox.Show("Input values must be integer");
                return;
            }
            points.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool swap = false;
            if (x1 == x2 || Math.Abs(y2 - y1) > Math.Abs(x2 - x1))
            {
                swap = true;
                (x1, y1) = (y1, x1);
                (x2, y2) = (y2, x2);
            }
            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }

            double k = (double)(y2 - y1) / (x2 - x1);
            double b = y1 - k * x1;
            for (int x = x1; x <= x2; x++)
            {
                int y = (int)Math.Round(k * x + b);
                points.Add(swap ? (y, x) : (x, y));
            }
            stopwatch.Stop();
            PaintGrid();
            timeTextBox.Text = stopwatch.Elapsed.TotalMilliseconds + " milliseconds";
        }

        private void CDAClick(object sender, RoutedEventArgs e)
        {
            if (!GetValues())
            {
                _ = MessageBox.Show("Input values must be integer");
                return;
            }
            points.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool swap = false;
            if (Math.Abs(y2 - y1) > Math.Abs(x2 - x1))
            {
                swap = true;
                (x1, y1) = (y1, x1);
                (x2, y2) = (y2, x2);
            }
            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }
            double x = x1;
            double y = y1;
            double dydx = (double)(y2 - y1) / (x2 - x1);
            for (; x <= x2; x++)
            {
                points.Add(swap ? ((int)Math.Round(y), (int)x) : ((int)x, (int)Math.Round(y)));
                y += dydx;
            }
            stopwatch.Stop();
            PaintGrid();
            timeTextBox.Text = stopwatch.Elapsed.TotalMilliseconds + " milliseconds";
        }

        private void ABClick(object sender, RoutedEventArgs e)
        {
            if (!GetValues())
            {
                _ = MessageBox.Show("Input values must be integer");
                return;
            }
            points.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool swap = false;
            if (Math.Abs(y2 - y1) > Math.Abs(x2 - x1))
            {
                swap = true;
                (x1, y1) = (y1, x1);
                (x2, y2) = (y2, x2);
            }
            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }
            int step = y2 > y1 ? 1 : -1;
            int x = x1;
            int y = y1;
            int dx = x2 - x1;
            int dy = Math.Abs(y2 - y1);
            int er = 2 * dy - dx;
            for (; x <= x2; x++)
            {
                points.Add(swap ? (y, x) : (x, y));
                if (er >= 0)
                {
                    y += step;
                    er -= 2 * dx;
                }
                er += 2 * dy;
            }
            stopwatch.Stop();
            PaintGrid();
            timeTextBox.Text = stopwatch.Elapsed.TotalMilliseconds + " milliseconds";
        }

        private void ABOClick(object sender, RoutedEventArgs e)
        {
            if (!GetValues())
            {
                _ = MessageBox.Show("Input values must be integer");
                return;
            }
            if (r <= 0)
            {
                _ = MessageBox.Show("Radius value must be positive");
                return;
            }
            points.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();          
            int x = 0;
            int y = r;
            int er = 3 - 2 * r;
            for (; x <= y; x++)
            {
                points.Add((x + x1, y + y1));
                points.Add((-x + x1, y + y1));
                points.Add((x + x1, -y + y1));
                points.Add((-x + x1, -y + y1));
                points.Add((y + x1, x + y1));
                points.Add((-y + x1, x + y1));
                points.Add((y + x1, -x + y1));
                points.Add((-y + x1, -x + y1));
                if (er >= 0)
                {
                    y--;
                    er += 4 - 4 * y;
                }
                er += 4 * x + 6;
            }
            stopwatch.Stop();
            PaintGrid();
            timeTextBox.Text = stopwatch.Elapsed.TotalMilliseconds + " milliseconds";
        }

        private void ZoomCanvas(object sender, MouseWheelEventArgs e)
        {
            zoom += zoomSpeed * e.Delta;
            if (zoom < zoomMin) { zoom = zoomMin; }
            if (zoom > zoomMax) { zoom = zoomMax; }

            Point mousePos = e.GetPosition(canvas);

            if (zoom > 1)
            {
                canvas.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y); 
            }
            else
            {
                canvas.RenderTransform = new ScaleTransform(zoom, zoom); 
            }
        }

        public void PaintGrid()
        {
            canvas.Children.Clear();
            Line xLine = new Line();
            Line yLine = new Line();
            xLine.Stroke = Brushes.Red;
            yLine.Stroke = Brushes.Red;
            xLine.X1 = 0;
            xLine.X2 = canvas.Width;
            xLine.Y1 = canvas.Height / 2;
            xLine.Y2 = xLine.Y1;
            yLine.X1 = canvas.Width / 2;
            yLine.X2 = yLine.X1;
            yLine.Y1 = 0;
            yLine.Y2 = canvas.Height;
            yLine.StrokeThickness = 2;
            canvas.Children.Add(xLine);
            canvas.Children.Add(yLine);

            for (int i = -gridSize; i <= gridSize; i+= 10)
            {
                TextBlock xTextBlock = new TextBlock();
                TextBlock yTextBlock = new TextBlock();
                xTextBlock.FontSize = 10;
                yTextBlock.FontSize = 10;
                xTextBlock.Text = i.ToString();
                xTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                yTextBlock.Text = i.ToString();
                yTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                Canvas.SetLeft(xTextBlock, (i + gridSize) * pointSize);
                Canvas.SetTop(xTextBlock, 0);
                Canvas.SetLeft(yTextBlock, 0);
                Canvas.SetTop(yTextBlock, canvas.Height - (i + gridSize) * pointSize);
                canvas.Children.Add(xTextBlock);
                canvas.Children.Add(yTextBlock);
            }

            DrawPoints();
        }

        public void DrawPoints()
        {
            foreach ((int, int) point in points)
            {
                Rectangle rect = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Width = pointSize,
                    Height = pointSize,
                    Fill = Brushes.Black
                };
                double x = point.Item1 * pointSize + canvas.Width / 2;
                double y = point.Item2 * pointSize + canvas.Height / 2;
                Canvas.SetLeft(rect, x);
                Canvas.SetBottom(rect, y);
                canvas.Children.Add(rect);
            }
        }
    }
}
