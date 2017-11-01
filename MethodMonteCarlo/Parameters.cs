using System.Drawing;

namespace MethodMonteCarlo
{
    public class Parameters
    {
        private Point _startingPoint;
        private Point _endPoint;

        public Parameters() { }
        public Parameters(Parameters par)
        {
            StartingPoint = par.StartingPoint;
            EndPoint = par.EndPoint;
        }
        public int Width => EndPoint.X - StartingPoint.X;


        public int Height => EndPoint.Y - StartingPoint.Y;

        public Point StartingPoint
        {
            get => new Point(_startingPoint.X < _endPoint.X ? _startingPoint.X : _endPoint.X,
                _startingPoint.Y < _endPoint.Y ? _startingPoint.Y : _endPoint.Y);
            set => _startingPoint = value;
        }

        public Point EndPoint
        {
            get => new Point(_startingPoint.X > _endPoint.X ? _startingPoint.X : _endPoint.X,
                _startingPoint.Y > _endPoint.Y ? _startingPoint.Y : _endPoint.Y);
            set => _endPoint = value;
        }
 
    }
}