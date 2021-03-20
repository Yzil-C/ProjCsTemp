using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YPoint : YFigure
    {
        public SKPoint DrawingPoint { get; set; }

        public YPoint(SKPoint point, SKColor color, float strokeWidth)
        {
            DrawingPoint = point;
            Paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = color,
                StrokeWidth = strokeWidth
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            canvas.DrawPoint(DrawingPoint, Paint);
        }

        public override bool Contains(SKPoint point)
        {
            if(DrawingPoint.X == point.X && DrawingPoint.Y == point.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
