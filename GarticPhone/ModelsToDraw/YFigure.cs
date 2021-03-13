using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.ModelsToDraw
{
    public class YFigure
    {
        public SKPaint Paint { get; set; }

        virtual public void Draw(SKCanvas canvas)
        {

        }

        virtual public bool Contains(SKPoint point) => false;
    }
}
