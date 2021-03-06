using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.Models
{
    class SKLine
    {
        public SKPoint FirstPoint { get; set; }
        public SKPoint SecondPoint { get; set; }

        public SKLine(SKPoint firstPoint, SKPoint secondPoint)
        {
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
        }

    }
}
