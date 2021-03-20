using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YLine : YFigure
    {
        public SKPoint FirstPoint { get; set; }
        public SKPoint SecondPoint { get; set; }

        public YLine(SKPoint firstPoint, SKPoint secondPoint, SKColor color, float strokeWidth)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
            Paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = strokeWidth
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            canvas.DrawLine(FirstPoint, SecondPoint, Paint);
        }

        public override bool Contains(SKPoint point)
        {
            var linePath = new SKPath();
            linePath.MoveTo(FirstPoint.X, FirstPoint.Y);
            linePath.LineTo(SecondPoint.X, SecondPoint.Y);

            return linePath.Contains(point.X, point.Y);
        }
    }
}
