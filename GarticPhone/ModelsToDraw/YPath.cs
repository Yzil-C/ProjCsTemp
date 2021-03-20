using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YPath : YFigure
    {
        public SKPath DrawingPath { get; set; }

        public YPath(SKPath drawingPath, SKColor color, float strokeWidth)
        {
            DrawingPath = drawingPath;
            Paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = color,
                StrokeWidth = strokeWidth
            };
        }

        public override void Draw(SKCanvas canvas)
        {
            canvas.DrawPath(DrawingPath, Paint);
        }

        public override bool Contains(SKPoint point)
        {
            return DrawingPath.Contains(point.X, point.Y);
        }
    }
}
