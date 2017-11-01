using System.Drawing;

namespace MethodMonteCarlo
{
    public class DrawFigure
    {
        private readonly Pen _penForEllipse = new Pen(Color.Red);
        private readonly Pen _penForRhombus = new Pen(Color.Blue);
        private readonly Pen _penForTriangle = new Pen(Color.Green);
 
        public Figure Draw(TypesFigures typesFigure, Parameters parameters, Graphics graph)
        {
            switch (typesFigure)
            {
                case TypesFigures.Ellipse:
                    ellipse(graph, parameters);
                    break;
                case TypesFigures.Rhombus:
                    rhombus(graph, parameters);
                    break;
                case TypesFigures.Triangle:
                    triangle(graph, parameters);
                    break;
                default:
                    return null;
            }

            return new Figure(parameters, typesFigure);
        }

        private void rhombus(Graphics graph, Parameters parameters)
        {
            graph.DrawPolygon(_penForRhombus, new[]
            {
                new Point(parameters.StartingPoint.X + parameters.Width / 2,
                    parameters.StartingPoint.Y),
                new Point(parameters.EndPoint.X, parameters.EndPoint.Y - parameters.Height / 2),
                new Point(parameters.StartingPoint.X + parameters.Width / 2,
                    parameters.EndPoint.Y),
                new Point(parameters.StartingPoint.X,
                    parameters.StartingPoint.Y + parameters.Height / 2)
            });
        }

        private void triangle(Graphics graph, Parameters parameters)
        {
            graph.DrawPolygon(_penForTriangle,
                new[]
                {
                    new Point(parameters.StartingPoint.X, parameters.EndPoint.Y),
                    new Point(parameters.StartingPoint.X +
                              (parameters.EndPoint.X - parameters.StartingPoint.X) / 2,
                        parameters.StartingPoint.Y),
                    new Point(parameters.EndPoint.X, parameters.EndPoint.Y)

                });
        }

        private void ellipse(Graphics graph, Parameters parameters)
        {
            graph.DrawEllipse(_penForEllipse, parameters.StartingPoint.X, parameters.StartingPoint.Y,
                parameters.Width, parameters.Height);
        }
    }
}