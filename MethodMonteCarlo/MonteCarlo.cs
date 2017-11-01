using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace MethodMonteCarlo
{
    public partial class MonteCarlo : Form
    {
        private Graphics _graph;
        private Parameters _parameters;
        private TypesFigures _typeFigure;
        private bool _isMouseDown;
        private Bitmap _originalBitmap;
        private Bitmap _tempBitmap;
        private DrawFigure _figureDraw;
        private readonly List<Figure> _figuresList;
        private readonly Random _randPoint;
        private Rectangle _describedFigure;

        public int AmountOfPoints => string.IsNullOrEmpty(amountOfPoints.Text) ? 0 : int.Parse(amountOfPoints.Text);

        public MonteCarlo()
        {
            InitializeComponent();
            DrawingField.Paint += (sender, e) =>
            {
                if (_graph != null)
                    return;
                _originalBitmap = new Bitmap(((PictureBox)sender).Width, ((PictureBox)sender).Height, e.Graphics);
                _graph = Graphics.FromImage(_originalBitmap);
                ((PictureBox)sender).BackColor = Color.White;
            };
            _randPoint = new Random();
            _parameters = new Parameters();
            _figureDraw = new DrawFigure();
            _typeFigure = TypesFigures.None;
            _figuresList = new List<Figure>();
            Figures.ItemClicked += (sender, e) =>
            {
                var type = getTypesFigures(e.ClickedItem.Name);
                if (type == TypesFigures.Сlear)
                {
                    _figuresList.Clear();
                    resultTable.Rows.Clear();
                    _parameters = new Parameters();
                    _figureDraw = new DrawFigure();
                    _originalBitmap = new Bitmap(DrawingField.Width, DrawingField.Height, _graph);
                    _graph = Graphics.FromImage(_originalBitmap);
                    DrawingField.Image = _originalBitmap;
                    return;
                }
                _typeFigure = type;
            };
            Calculate.Click += (sender, e) =>
            {
                AreaСalculation();
                resultTable.Rows.Clear();
                foreach (var figure in _figuresList)
                {
                    var imgFigure = Properties.Resources.ResourceManager.GetObject(figure.TypeFigure.ToString());

                    if (figure.ExpectedArea == null || figure.CalculatedArea == null) continue;
                    var rowNumber = resultTable.Rows.Add(imgFigure, figure.ExpectedArea.Value.ToString("##.000"),
                        figure.CalculatedArea.Value.ToString("##.000"));
                    resultTable.Rows[rowNumber].HeaderCell.Value = (rowNumber + 1).ToString();
                }
            };
        }

        private static TypesFigures getTypesFigures(string clickedItemName)
        {
            TypesFigures typesFigures;
            Enum.TryParse(clickedItemName, out typesFigures);
            return typesFigures;
        }

        private void DrawingField_MouseDown(object sender, MouseEventArgs e)
        {
            _parameters.StartingPoint = new Point(e.X, e.Y);
            _isMouseDown = true;
        }


        private void DrawingField_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown)
                return;
            _tempBitmap = new Bitmap(_originalBitmap);
            _graph = Graphics.FromImage(_tempBitmap);
            _parameters.EndPoint = new Point(e.X, e.Y);
            _figureDraw.Draw(_typeFigure, _parameters, _graph);
            DrawingField.Image = _tempBitmap;
        }

        private void DrawingField_MouseUp(object sender, MouseEventArgs e)
        {
            _graph = Graphics.FromImage(_originalBitmap);
            _figureDraw.Draw(_typeFigure, _parameters, _graph);
            _figuresList.Add(new Figure(new Parameters(_parameters), _typeFigure));
            _parameters = new Parameters();
            DrawingField.Image = _originalBitmap;
            _isMouseDown = false;
        }

        public void AreaСalculation()
        {
            if (_figuresList == null || _figuresList.Count == 0)
                return;
            var minX = _figuresList.Min(fig => fig.Parameters.StartingPoint.X);
            var maxX = _figuresList.Max(fig => fig.Parameters.EndPoint.X);
            var minY = _figuresList.Min(fig => fig.Parameters.StartingPoint.Y);
            var maxY = _figuresList.Max(fig => fig.Parameters.EndPoint.Y);
            _describedFigure = new Rectangle(minX, minY, maxX - minX, maxY - minY);

            if (AmountOfPoints == 0)
            {
                return;
            }
            _figuresList.ForEach(f => f.Сlearing());
            for (var iPoint = 0; iPoint < AmountOfPoints; iPoint++)
            {
                var rX = _randPoint.Next(minX, maxX);
                var rY = _randPoint.Next(minY, maxY);
                _figuresList.ForEach(f => f.BelongsFigure(new Point(rX, rY)));
                _graph.FillRectangle(new SolidBrush(Color.Black), new Rectangle(rX, rY, 1, 1));
                DrawingField.Image = _originalBitmap;
            }

            var areaDescribedFigure = _describedFigure.Width * _describedFigure.Height;
            _figuresList.ForEach(f => f.SetCalculatedArea(areaDescribedFigure, AmountOfPoints));
        }
    }
}
