using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YRect : YFigure
    {
        public SKRect Rect { get; set; }

        public YRect(float xPoint, float yPoint, float width, float height, SKColor color, float strokeWidth, SKPaintStyle paintStyle)
        {
            Rect = new SKRect(xPoint, yPoint, xPoint + width, yPoint + height);
            Paint = new SKPaint
            {
                Style = paintStyle,
                Color = color,
                StrokeWidth = strokeWidth
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            canvas.DrawRect(Rect, Paint);
        }

        public override bool Contains(SKPoint point)
        {
            return Rect.Contains(point);
        }
    }
}
