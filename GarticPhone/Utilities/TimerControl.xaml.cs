using GarticPhone.Models;
using GarticPhone.ModelsToDraw;
using GarticPhone.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace GarticPhone.Utilities
{
    /// <summary>
    /// Logique d'interaction pour TimerControl.xaml
    /// </summary>
    /// 

    

    public partial class TimerControl : UserControl
    {
        public MainViewModel ViewModel => DataContext as MainViewModel;

        private float _sweepAngle;

        public TimerControl()
        {
            InitializeComponent();
        }

        private void ViewModel_TimerTick(float currentEvolution, bool isFinished)
        {
            _sweepAngle = 360f * currentEvolution;
            skElement.InvalidateVisual();
        }

        private void OnTimerSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var info = e.Info;
            var canvas = surface.Canvas;

            DrawTimer(canvas, info);
        }
        

        private void DrawTimer(SKCanvas canvas, SKImageInfo info)
        {
            canvas.Clear();
            canvas.DrawPaint(new SKPaint { Color = new SKColor(96,96,96) });
            

            SKPoint center = new SKPoint(info.Width / 2 - 15, info.Height / 2);
            float radius = Math.Min(info.Width / 2, info.Height / 2) - 10;
            SKRect rect = new SKRect(center.X - radius, center.Y - radius,
                                     center.X + radius, center.Y + radius);

            var surroundCircle = new YCircle(center, radius + 2, new SKColor(48, 48, 48), 4, SKPaintStyle.Stroke);

            surroundCircle.Draw(canvas);

            var backgroundCircle = new YCircle(center, radius , new SKColor(80,80,80), 1, SKPaintStyle.Fill);

            backgroundCircle.Draw(canvas);

            float startAngle = 270;

            

            var path = new YPath(new SKPath(), new SKColor(245, 222, 179), 5);

            path.DrawingPath.MoveTo(center);
            path.Paint.Style = SKPaintStyle.Fill;
            path.DrawingPath.ArcTo(rect, startAngle, _sweepAngle, false);
            path.DrawingPath.Close();

            path.Draw(canvas);
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.TimerTick += ViewModel_TimerTick;
        }
    }
}
