using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GarticPhone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum State
    {
        pen,
        rubber,
        square,
        circle,
        squareFilled,
        circleFilled,
        line,
        fill
    }
    public partial class MainWindow : Window
    {
		private SKPath _skPath;
		private SKColor _skColor = new SKColor(0,0,0);
		private SKPoint? _skPoint;
        private SKPaintStyle _isFilled = SKPaintStyle.Stroke;
        private int _strokeWidth = 5;

        private float _rectangleWidth;
        private float _rectangleHeight;
        private float _radius;

		private bool _isMouseDown = false;
		
        private State _state = State.pen;

		public Window Window => Application.Current.MainWindow;
		public DpiScale DpiScale => VisualTreeHelper.GetDpi(Window);


		public MainWindow()
		{
			InitializeComponent();
		}

		private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var info = e.Info;
			var surface = e.Surface;
			var canvas = surface.Canvas;

            // Si tu dois redessiner la surface
            //canvas.Clear();
            if (_state == State.squareFilled || _state == State.circleFilled)
                _isFilled = SKPaintStyle.Fill;
            else
                _isFilled = SKPaintStyle.Stroke;

            if(_state == State.rubber)
            {
                _skColor = new SKColor(255, 255, 255);
            }
            else
            {
                byte r = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).R;
                byte g = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).G;
                byte b = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).B;
                _skColor = new SKColor(r, g, b);
            }

			var paint = new SKPaint()
			{
				Style = _isFilled,
				Color = _skColor,
				StrokeWidth = _strokeWidth
			};

            switch (_state)
            {
                default:
                    if (_skPath != null)
                    {
                        canvas.DrawPath(_skPath, paint);
                    }
                    if (_skPoint != null)
                    {
                        canvas.DrawPoint(_skPoint.Value, paint);
                    }
                    break;
                case State.square:
                    canvas.DrawRect((float)_skPoint.Value.X, (float)_skPoint.Value.Y, _rectangleWidth, _rectangleHeight, paint);
                    break;

                case State.circle:
                    canvas.DrawCircle(_skPoint.Value, _radius, paint);
                    break;

                case State.squareFilled:
                    canvas.DrawRect((float)_skPoint.Value.X, (float)_skPoint.Value.Y, _rectangleWidth, _rectangleHeight, paint);
                    break;

                case State.circleFilled:
                    canvas.DrawCircle(_skPoint.Value, _radius, paint);
                    break;

                case State.line:
                    break;

                case State.fill:
                    break;
            }
            
		}

        private void skElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = pixelPosition.X * DpiScale.DpiScaleX;
            var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    skElement.InvalidateVisual();
                    break;
                case State.fill: 
                    break;
                case State.square:
                    break;
                case State.squareFilled:
                    break;
                case State.circle:
                    break;
                case State.circleFilled:
                    break;
            }
		}

        private void skElement_MouseMove(object sender, MouseEventArgs e)
        {
			if (_isMouseDown)
			{
                var pixelPosition = e.GetPosition(sender as Canvas);
                var x = pixelPosition.X * DpiScale.DpiScaleX;
                var y = pixelPosition.Y * DpiScale.DpiScaleY;
                switch (_state)
                {
                    default:
                        var skPoint = new SKPoint((float)x - 250, (float)y - 150);
                        _skPath.LineTo(skPoint);
                        _isFilled = SKPaintStyle.Stroke;
                        skElement.InvalidateVisual();
                        break;
                    case State.square: 
                        break;
                    case State.circle:
                        break;
                    case State.squareFilled:
                        break;
                    case State.circleFilled:
                        break;
                    case State.line:
                        break;
                    case State.fill:
                        break;
                }
				
			}
		}

        private void skElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
			_isMouseDown = true;
			var pixelPosition = e.GetPosition(sender as Canvas);
			var x = pixelPosition.X * DpiScale.DpiScaleX;
			var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    var skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    _skPath = new SKPath();
                    _skPath.MoveTo(skPoint);
                    break;
                case State.square:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.circle:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.squareFilled:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.circleFilled:
                    _skPoint = new SKPoint((float)x - 250, (float)y - 150);
                    break;

                case State.line:
                    break;

                case State.fill:
                    break;
            }
        }

        private void skElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
			_isMouseDown = false;
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = pixelPosition.X * DpiScale.DpiScaleX;
            var y = pixelPosition.Y * DpiScale.DpiScaleY;
            switch (_state)
            {
                default:
                    _skPath = null;
                    break;
                case State.square:
                    _rectangleWidth = (float)((x - 250) - _skPoint.Value.X);
                    _rectangleHeight = (float)((y - 150) - _skPoint.Value.Y);
                    skElement.InvalidateVisual();
                    break;

                case State.circle:
                    _radius = (float)Math.Sqrt(Math.Pow((x - 250) - _skPoint.Value.X, 2) + Math.Pow((y - 150) - _skPoint.Value.Y, 2));
                    skElement.InvalidateVisual();
                    break;

                case State.squareFilled:
                    _rectangleWidth = (float)((x - 250) - _skPoint.Value.X);
                    _rectangleHeight = (float)((y - 150) - _skPoint.Value.Y);
                    skElement.InvalidateVisual();
                    break;

                case State.circleFilled:
                    _radius = (float)Math.Sqrt(Math.Pow((x - 250) - _skPoint.Value.X, 2) + Math.Pow((y - 150) - _skPoint.Value.Y, 2));
                    skElement.InvalidateVisual();
                    break;

                case State.line:
                    break;

                case State.fill:
                    break;
            }

        }

        private void StrokeWidth1_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 3;
        }

        private void StrokeWidth2_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 5;
        }

        private void StrokeWidth3_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 7;
        }

        private void StrokeWidth4_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 10;
        }

        private void StrokeWidth5_Button_Click(object sender, RoutedEventArgs e)
        {
			_strokeWidth = 14;
        }

        private void BlackColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(0, 0, 0);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void SlateGrayColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(112, 128, 144);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(112, 128, 0));
        }

        private void WhiteColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(255, 255, 255);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void MediumOrchidColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(186, 85, 211);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(186, 85, 211));
        }

        private void DarkOrchid(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(153, 50, 204);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(153, 50, 204));
        }

        private void DarkBlueColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(0, 0, 139);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 139));
        }

        private void MediumSlateBlueColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(123, 104, 238);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(123, 104, 238));
        }

        private void LightSkyBlueColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(135, 206, 250);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(135, 206, 250));
        }

        private void YellowGreenColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(154, 205, 50);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(154, 205, 50));
        }

        private void GreenColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(0, 128, 0);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
        }

        private void YellowColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(255, 255, 0);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        }

        private void OrangeColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(255, 165, 0);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
        }

        private void PinkColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(255, 192, 203);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203));
        }

        private void HotPinkColor(object sender, RoutedEventArgs e)
        {
			_skColor = new SKColor(255, 105, 180);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 105, 180));
        }

        private void RedColor(object sender, RoutedEventArgs e)
        {
            _skColor = new SKColor(255, 0, 0);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        private void FireBrickColor(object sender, RoutedEventArgs e)
        {
            _skColor = new SKColor(178, 34, 34);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(178, 34, 34));
        }

        private void AnthiqueWhiteColor(object sender, RoutedEventArgs e)
        {
            _skColor = new SKColor(250, 235, 215);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(250, 235, 215));
        }

        private void SaddleBrownColor(object sender, RoutedEventArgs e)
        {
            _skColor = new SKColor(139, 69, 19);
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(139, 69, 19));
        }

        private void OnPen(object sender, RoutedEventArgs e)
        {
            _state = State.pen;
        }

        private void OnRubber(object sender, RoutedEventArgs e)
        {
            _state = State.rubber;
            _skColor = new SKColor(255, 255, 255);
        }

        private void OnSquare(object sender, RoutedEventArgs e)
        {
            _state = State.square;
        }

        private void OnCircle(object sender, RoutedEventArgs e)
        {
            _state = State.circle;
        }

        private void OnSquareFilled(object sender, RoutedEventArgs e)
        {
            _state = State.squareFilled;
        }

        private void OnCircleFilled(object sender, RoutedEventArgs e)
        {
            _state = State.circleFilled;
            
        }

        private void OnLine(object sender, RoutedEventArgs e)
        {

        }

        private void OnFill(object sender, RoutedEventArgs e)
        {

        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {

        }

        private void OnRestore(object sender, RoutedEventArgs e)
        {

        }
    }
}
