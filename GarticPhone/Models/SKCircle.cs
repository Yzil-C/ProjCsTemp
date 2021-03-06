using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarticPhone.Models
{
    class SKCircle
    {
        public SKPoint Point { get; set; }
        public float Radius { get; set; }

        public SKCircle(SKPoint point, float radius)
        {
            Point = point;
            Radius = radius;
        }
    }
}
