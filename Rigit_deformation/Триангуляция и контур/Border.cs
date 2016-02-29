using System.Collections.Generic;
using System.Drawing;


namespace Rigit_deformation.Триангуляция_и_контур
{
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
            List<Point> leftBorder = new List<Point>();
            List<Point> rightBorder = new List<Point>();
            int startX = -1;
            bool find = false;

            for (int y = 0; y < this._bitmapFromImage.Height; y++)
                for (int x = 0; x < this._bitmapFromImage.Width; x++)
                {
                    if (this._bitmapFromImage.GetPixel(x, y).A == 0) continue;
                    // пиксель закрашен
                    else
                    {
                        // он на границе картинки
                        if ((x == 0) || (y == 0) || (x == this._bitmapFromImage.Width - 1) || (y == this._bitmapFromImage.Height - 1))
                        {
                            if (find == false)
                            {
                                startX = x;
                                find = true;
                            }

                            if (x < startX) leftBorder.Add(new Point(x, y));
                            else rightBorder.Add(new Point(x, y));
                            continue;
                        }
                        else
                        // закрашен, но не на границе картинки
                        {
                            if (find == false)
                            {
                                startX = x; find = true;
                            }

                            if (isBorder(x, y))
                            {
                                if (x < startX) leftBorder.Add(new Point(x, y));
                                else rightBorder.Add(new Point(x, y));
                            }
                        }
                    }
                }
            rightBorder.Reverse();
            leftBorder.AddRange(rightBorder);
            return leftBorder;
        }

        private bool isBorder(int x, int y)
        {
            if ((this._bitmapFromImage.GetPixel(x, y - 1).A > 0) && (this._bitmapFromImage.GetPixel(x, y + 1).A > 0)
                && (this._bitmapFromImage.GetPixel(x - 1, y).A > 0) && (this._bitmapFromImage.GetPixel(x + 1, y).A > 0))
            {
                return false;
            }

            return true;
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
            List <Point> shortList = new List<Point>();
            for (int i  = 0; i  < this.ExactPointsList.Count; i+=10)
            {
                shortList.Add(this.ExactPointsList[i]);
            }
            return shortList;
        }
        public void DrawShortCutBorder(Graphics g)
        {
            for (int i = 0; i < this.ShortPointsList.Count-1; i++)
            {
                g.DrawLine(Pens.Red, this.ShortPointsList[i].X, this.ShortPointsList[i].Y, this.ShortPointsList[i+1].X, this.ShortPointsList[i+1].Y);
            }
        }
    }
}
