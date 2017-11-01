using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
 
namespace MonteCarlo
{
    public partial class MethodMonteCarlo : Form
    {
        private Graphics _graph;
        private readonly Parameters _parametersFigures;
        private TypesFigures TypeFigure;
        private bool _isMouseDown;
        private Bitmap _originalBitmap;
        private Bitmap _tempBitmap;
        private DrawFigure figureDraw;
        private readonly List<Figure> _figuresList;

        public MethodMonteCarlo()
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
            _parametersFigures = new Parameters();
            figureDraw = new DrawFigure();
            _figuresList = new List<Figure>();
            Figures.ItemClicked += (sender, e) =>
            {
                var type = getTypesFigures(e.ClickedItem.Name);
                if (type == TypesFigures.Сlear)
                {
                    _figuresList.Clear();
                    return;
                }
                TypeFigure = type;
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
            _parametersFigures.StartingPoint = new Point(e.X, e.Y);
            _isMouseDown = true;
        }


        private void DrawingField_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown)
                return;
            _tempBitmap = new Bitmap(_originalBitmap);
            _graph = Graphics.FromImage(_tempBitmap);
            _parametersFigures.EndPoint = new Point(e.X, e.Y);
            figureDraw.Draw(TypeFigure, _parametersFigures, _graph);
            DrawingField.Image = _tempBitmap;
        }

        private void DrawingField_MouseUp(object sender, MouseEventArgs e)
        {
            _graph = Graphics.FromImage(_originalBitmap);
            var paintedFigure = figureDraw.Draw(TypeFigure, _parametersFigures, _graph);
            if (paintedFigure != null)
                _figuresList.Add(paintedFigure);
            DrawingField.Image = _originalBitmap;
            _isMouseDown = false;
        }
    }
}
