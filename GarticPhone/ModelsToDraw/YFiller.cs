using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    class YFiller : YFigure
    {
        public SKPoint BasePoint { get; set; }
        public List<SKPoint> Points { get; set; }
        //public SKPath Path { get; set; }

        public YFiller(SKPoint point, SKColor color)
        {
            BasePoint = point;
            Points = new List<SKPoint>();
            //Path = new SKPath();
            //Path.MoveTo(point.X, point.Y);
            Paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = color,
                StrokeWidth = 5
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            foreach(var point in Points)
            {
                canvas.DrawPoint(point, Paint);
            }

            //canvas.DrawPath(Path, Paint);
        }


        public bool FillElement(SKSurface surface, SKImageInfo dstinf, SKColor currentColor, int x, int y)
        {
            SKBitmap bitmap = new SKBitmap(dstinf.Width, dstinf.Height);

            IntPtr dstpixels = bitmap.GetPixels();

            surface.ReadPixels(dstinf, dstpixels, dstinf.RowBytes, 0, 0);

            var pointsToFound = new Stack<SKPoint>();

            if (bitmap.GetPixel(x, y) == currentColor)
                return false;

            pointsToFound.Push(new SKPoint(x, y));

            var baseColor = bitmap.GetPixel((int)Math.Round(BasePoint.X), (int)Math.Round(BasePoint.Y));
            var currentPoints = new Stack<SKPoint>();

            while (pointsToFound.Count > 0)
            {
                currentPoints.Push(pointsToFound.Pop());
                var currentPoint = currentPoints.Peek();
                var currentX = (int)currentPoint.X;
                var currentY = (int)currentPoint.Y;
                bitmap.SetPixel((int)currentPoint.X, (int)currentPoint.Y, currentColor);
                Points.Add(currentPoint);
                //Path.LineTo(currentX, currentY);
                if (currentX - 2 > 0 && currentY - 2 > 0 && currentY + 2 < bitmap.Height && currentX + 2 < bitmap.Width)
                {
                    if (bitmap.GetPixel(currentX, currentY + 2) == baseColor)
                        pointsToFound.Push(new SKPoint(currentX, currentY + 2));
                    if (bitmap.GetPixel(currentX, currentY - 2) == baseColor)
                        pointsToFound.Push(new SKPoint(currentX, currentY - 2));
                    if (bitmap.GetPixel(currentX + 2, currentY) == baseColor)
                        pointsToFound.Push(new SKPoint(currentX + 2, currentY));
                    if (bitmap.GetPixel(currentX - 2, currentY) == baseColor)
                        pointsToFound.Push(new SKPoint(currentX - 2, currentY));
                }
            }

            return true;
        }   

        /*public bool FillElement(SKSurface surface, SKImageInfo dstinf, SKColor currentColor, int x, int y)
        {
            SKBitmap bitmap = new SKBitmap(dstinf.Width, dstinf.Height);

            IntPtr dstpixels = bitmap.GetPixels();

            surface.ReadPixels(dstinf, dstpixels, dstinf.RowBytes, 0, 0);

            var pointsToFound = new Stack<SKPoint>();

            if (bitmap.GetPixel(x, y) == currentColor)
                return false;

            var baseColor = bitmap.GetPixel((int)Math.Round(BasePoint.X), (int)Math.Round(BasePoint.Y));
            var currentPoints = new Stack<SKPoint>();

            pointsToFound.Push(new SKPoint(x, y));
            while (pointsToFound.Count > 0)
            {

                currentPoints.Push(pointsToFound.Pop());
                var westPoint = currentPoints.Peek();
                var eastPoint = currentPoints.Peek();
                var currentY = (int)currentPoints.Peek().Y;
                if (currentY - 1 > 0 && currentY + 1 < bitmap.Height)
                {
                    if (!Path.Contains(currentPoints.Peek().X, currentY))
                    {
                        while (bitmap.GetPixel((int)eastPoint.X, currentY) == baseColor && eastPoint.X + 1 < bitmap.Width)
                            eastPoint.X++;

                        while (bitmap.GetPixel((int)westPoint.X, currentY) == baseColor && westPoint.X - 1 > 0)
                            westPoint.X--;

                        Path.LineTo(eastPoint.X, currentY);
                        Path.LineTo(westPoint.X, currentY);

                        for (int cpt = (int)westPoint.X; cpt < (int)eastPoint.X; cpt++)
                        {
                            bitmap.SetPixel(cpt, (int)eastPoint.Y, currentColor);
                            if (bitmap.GetPixel(cpt, (int)eastPoint.Y + 1) == baseColor)
                                pointsToFound.Push(new SKPoint(cpt, (int)eastPoint.Y + 1));
                            if (bitmap.GetPixel(cpt, (int)eastPoint.Y - 1) == baseColor)
                                pointsToFound.Push(new SKPoint(cpt, (int)eastPoint.Y - 1));
                        }
                    }   
                }     
            }


            return true;
        }*/
    }
}
