using System.Collections.Generic;
using System.Drawing;


namespace Rigit_deformation.Триангуляция_и_контур
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// The border
    /// </summary>
    public class Border
    {
        public List<Point> ExactPointsList;
        public List<Point> ShortPointsList;

        private Bitmap _bitmapFromImage;

        public Border(Bitmap bitmap)
        {
            this._bitmapFromImage = bitmap;
            this.ExactPointsList = this.GetBorder();
            this.ShortPointsList = this.ShortCutBorder();
        }


        private List<Point> GetBorder()
        {
            // флажок, указывающий движется обход контура вниз или вверх
            bool down = true;

            // флажок, указывающий движется обход контура влево или вправо
            bool left = true;

            List<Point> borderPoints = new List<Point>();
            Point firstPoint = this.findFirst();
            Point previousPoint = firstPoint;
            Point nextPoint = firstPoint;

            do
            {
                borderPoints.Add(nextPoint);
                this._bitmapFromImage.SetPixel(nextPoint.X, nextPoint.Y, Color.Red);
                previousPoint = nextPoint;

                nextPoint = this.findNeighbour(previousPoint, firstPoint, down);

                down = nextPoint.Y >= previousPoint.Y;
            }
            while (!firstPoint.Equals(nextPoint));

            return borderPoints;
        }

        private Point findFirst()
        {
            Point firstPoint = new Point();

            for (int y = 0; y < this._bitmapFromImage.Height-1; y++)
                for (int x = 0; x < this._bitmapFromImage.Width-1; x++)
                {
                    if (this._bitmapFromImage.GetPixel(x, y).A != 0)
                    {
                        firstPoint.X = x;
                        firstPoint.Y = y;
                        return firstPoint;
                    }

                }

            if (firstPoint.IsEmpty)
            {
                throw new FirstPointNotFoundException("Первая закрашенная точка не была найдена");
            }
            return firstPoint;
        }

        private Point findNeighbour(Point p, Point firstPoint, bool down)
        {
            List<Point> neighBorderPoints = new List<Point>();

            for (int y = p.Y - 1; y <= p.Y + 1; y++)
                for (int x = p.X - 1; x <= p.X + 1; x++)
                {
                    if (((x == p.X) && (y == p.Y))||((x<0)||(y<0))||(x>=this._bitmapFromImage.Width)||(y>=this._bitmapFromImage.Height)) continue;

                    if ((isBorder(x,y))&&(this._bitmapFromImage.GetPixel(x,y).R !=255)||((firstPoint.X==x) && (firstPoint.Y==y)))
                    {
                        neighBorderPoints.Add(new Point(x, y));
                    }
                }
            if (neighBorderPoints.Count == 0)
                throw new FirstPointNotFoundException(
                    "Не была найдена соседняя точка для точки Х = " + Convert.ToString(p.X) + " Y = "
                    + Convert.ToString(p.Y));
            if (neighBorderPoints.Count == 1)
            {
                return neighBorderPoints[0];
            }
            // если соседей много
            else
            {
                List<Point> sortNeighBorderPoints = new List<Point>();

                if (down)
                {
                    // по минимальному Х и максимальному Y
                    sortNeighBorderPoints =
                       neighBorderPoints.OrderBy(t => t.X).ThenByDescending(t => t.Y).ToList();
                }
                else
                {
                    // по максимальному Х и минимальному Y
                    sortNeighBorderPoints =
                       neighBorderPoints.OrderByDescending(t => t.X).ThenBy(t => t.Y).ToList();
                }
                return sortNeighBorderPoints[0];
            }
        }

        private bool isBorder(int x, int y)
        {
            if ((x == this._bitmapFromImage.Width-1) || (x == 0) || (y == this._bitmapFromImage.Height-1) || (y == 0))
            {
                return (this._bitmapFromImage.GetPixel(x, y).A > 0);
            }
            else
            {
                // если пиксель не на границе изображения в целом

                if ((this._bitmapFromImage.GetPixel(x, y - 1).A > 0) && (this._bitmapFromImage.GetPixel(x, y + 1).A > 0)
                    && (this._bitmapFromImage.GetPixel(x - 1, y).A > 0) && (this._bitmapFromImage.GetPixel(x + 1, y).A > 0))
                {
                    return false;
                }
            }
            return this._bitmapFromImage.GetPixel(x, y).A > 0;
        }

        public void DrawExactBorder(Graphics g)
        {
            foreach (Point elem in this.ExactPointsList)
            {
                g.DrawEllipse(Pens.Green, elem.X, elem.Y, 1, 1);
            }
        }

        private List<Point> ShortCutBorder()
        {
            List<Point> shortList = new List<Point>();
            for (int i = 0; i < this.ExactPointsList.Count; i += 30)
            {
                shortList.Add(this.ExactPointsList[i]);
            }
            return shortList;
        }
        public void DrawShortCutBorder(Graphics g)
        {
            for (int i = 0; i < this.ShortPointsList.Count - 1; i++)
            {
                g.DrawLine(Pens.Red, this.ShortPointsList[i].X, this.ShortPointsList[i].Y, this.ShortPointsList[i + 1].X, this.ShortPointsList[i + 1].Y);
            }
        }

    }
}
