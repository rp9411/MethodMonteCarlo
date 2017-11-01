using System;
using System.Drawing;

namespace MonteCarlo
{
    public class Figure
    {
        public Parameters Parameters;
        public TypesFigures TypeFigure;
        public int NumberOfPointsCaught;
    

        public double? ExpectedArea
        {
            get
            {
                switch (TypeFigure)
                {
                    case TypesFigures.Triangle:
                        return (double) (Parameters.Height * Parameters.Width) / 2;
                    case TypesFigures.Rhombus:
                        return (double) (Parameters.Height * Parameters.Width) / 2;
                    case TypesFigures.Ellipse:
                        return Math.PI * ((double) Parameters.Height / 2) * ((double) Parameters.Width / 2);
                }
                return null;
            }
        }
   
        public double? CalculatedArea { get; private set; }

        public void SetCalculatedArea(int areaDescribedFigure, int amountOfPoints)
        {
            CalculatedArea = areaDescribedFigure * ((double)NumberOfPointsCaught / amountOfPoints);
        }

        public void Сlearing()
        {
            NumberOfPointsCaught = 0;
            CalculatedArea = 0;
        }

        public Figure(Parameters parameters, TypesFigures typeFigure)
        {
            NumberOfPointsCaught = 0;
            Parameters = parameters;
            TypeFigure = typeFigure;
        }

        public void BelongsFigure( Point arbitraryPoint)
        {
            var belongs = false;
            switch (TypeFigure)
            {
                case TypesFigures.Rhombus:
                    belongs= belongsToRhombus(Parameters, arbitraryPoint);
                    break;
                case TypesFigures.Triangle:
                    belongs= belongsToTriangle(Parameters, arbitraryPoint);
                    break;
                case TypesFigures.Ellipse:
                    belongs = belongsToEllipse(Parameters, arbitraryPoint);
                    break;
            }

            if (belongs)
                NumberOfPointsCaught++;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>http://www.abakbot.ru/online-2/280-pointreug</returns>
        private static bool belongsToTriangle(Parameters parametersFigurem, Point arbitraryPoint)
        {
            var pointA = new Point(parametersFigurem.StartingPoint.X + parametersFigurem.Height,
                parametersFigurem.StartingPoint.Y);
            var pointB = new Point(parametersFigurem.StartingPoint.X + (parametersFigurem.Height / 2),
                parametersFigurem.StartingPoint.Y);
            var pointC = new Point(parametersFigurem.EndPoint.X, parametersFigurem.EndPoint.Y);

            var pABD = pOfVectors(pointA, pointB, arbitraryPoint);
            var pBCD = pOfVectors(pointB, pointC, arbitraryPoint);
            var pACD = pOfVectors(pointA, pointC, arbitraryPoint);

            return ((pABD <= 0) && (pBCD <= 0) && (pACD <= 0))
                   || ((pABD >= 0) && (pBCD >= 0) && (pACD >= 0));
        }

        private static bool belongsToRhombus(Parameters parametersFigurem, Point arbitraryPoint)
        {
            var pointA = new Point(parametersFigurem.StartingPoint.X + parametersFigurem.Width / 2, parametersFigurem.StartingPoint.Y);
            var pointB = new Point(parametersFigurem.EndPoint.X, parametersFigurem.EndPoint.Y - parametersFigurem.Height / 2);
            var pointC = new Point(parametersFigurem.StartingPoint.X + parametersFigurem.Width / 2, parametersFigurem.EndPoint.Y);
            var pointD = new Point(parametersFigurem.StartingPoint.X,
                parametersFigurem.StartingPoint.Y - parametersFigurem.Height / 2);

            var pABM = pOfVectors(pointA, pointB, arbitraryPoint);
            var pBCM = pOfVectors(pointB, pointC, arbitraryPoint);
            var pCDM = pOfVectors(pointC, pointD, arbitraryPoint);
            var pDAM = pOfVectors(pointD, pointA, arbitraryPoint);

            return ((pABM <= 0) && (pBCM <= 0) && (pCDM <= 0) && (pDAM <= 0))
                   || ((pABM >= 0) && (pBCM >= 0) && (pCDM >= 0) && (pDAM >= 0));
        }

        private static bool belongsToEllipse(Parameters parametersFigurem, Point arbitraryPoint)
        {
            var a = parametersFigurem.Width / 2;
            var b = parametersFigurem.Height / 2;
            var centerOfEllipse = new Point(parametersFigurem.StartingPoint.X + a, parametersFigurem.StartingPoint.Y + b);

            return Math.Pow(centerOfEllipse.X - arbitraryPoint.X, 2) / Math.Pow(a, 2) +
                   Math.Pow(centerOfEllipse.Y - arbitraryPoint.Y, 2) / Math.Pow(b, 2) <= 1;
        }

        private static int pOfVectors(Point pointA, Point pointB, Point pointD)
        {
            return (pointA.X - pointD.X) * (pointB.Y - pointA.Y) - (pointB.X - pointA.X) * (pointA.Y - pointD.Y);
        }
    }
}