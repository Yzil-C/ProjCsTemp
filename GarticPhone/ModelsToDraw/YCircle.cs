using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YCircle : YFigure
    {
        public SKPoint Center { get; set; }
        public float Radius { get; set; }

        public YCircle(SKPoint center, float radius, SKColor color, float strokeWidth, SKPaintStyle paintStyle)
        {
            Center = center;
            Radius = radius;
            Paint = new SKPaint
            {
                Style = paintStyle,
                Color = color,
                StrokeWidth = strokeWidth
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            canvas.DrawCircle(Center, Radius, Paint);
        }

        public override bool Contains(SKPoint point)
        {
            var dX = Math.Abs(point.X - Center.X);
            var dY = Math.Abs(point.Y - Center.Y);

            var sumOfSquares = dX * dX + dY * dY;

            var distance = Math.Sqrt(sumOfSquares);

            return Radius >= distance;
        }
    }
}
