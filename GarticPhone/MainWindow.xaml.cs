
using GarticPhone.ModelsToDraw;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        fill,
    }
    public partial class MainWindow : Window
    {
        private Stack<YFigure> _history = new Stack<YFigure>();
        private Stack<YFigure> _redoStack = new Stack<YFigure>();

        private YPath _yPath;
        private YFiller _yFiller;
		private SKPoint _skPoint;
        private Dictionary<int, System.Windows.Shapes.Ellipse> _strokeWidthEllipseInf = new Dictionary<int, System.Windows.Shapes.Ellipse>();
        private Dictionary<State, Button> _buttonToDrawInf = new Dictionary<State, Button>();

        private int _strokeWidth = 5;

		private bool _isMouseDown = false;
        private bool _canRedo = false;

        private State _state = State.pen;

		public Window Window => Application.Current.MainWindow;
		public DpiScale DpiScale => VisualTreeHelper.GetDpi(Window);


		public MainWindow()
		{
			InitializeComponent();

            //Initialize strokeWidth to fill Elipse correctly
            _strokeWidthEllipseInf.Add(3, StrokeWidthEllipse1);
            _strokeWidthEllipseInf.Add(5, StrokeWidthEllipse2);
            _strokeWidthEllipseInf.Add(7, StrokeWidthEllipse3);
            _strokeWidthEllipseInf.Add(10, StrokeWidthEllipse4);
            _strokeWidthEllipseInf.Add(14, StrokeWidthEllipse5);

            //Initialize buttonToDraw to fill background correctly;
            _buttonToDrawInf.Add(State.pen, penState);
            _buttonToDrawInf.Add(State.rubber, rubberState);
            _buttonToDrawInf.Add(State.square, squareState);
            _buttonToDrawInf.Add(State.circle, circleState);
            _buttonToDrawInf.Add(State.squareFilled, squareFilledState);
            _buttonToDrawInf.Add(State.circleFilled, circleFilledState);
            _buttonToDrawInf.Add(State.line, lineState);
            _buttonToDrawInf.Add(State.fill, fillState);
        }
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var surface = e.Surface;
            var info = e.Info;
			var canvas = surface.Canvas;

            if (_state == State.fill && !_canRedo)
            {
                if (_yFiller.FillElement(surface, info, GetCurrentColor(), (int)Math.Round(_yFiller.BasePoint.X), (int)Math.Round(_yFiller.BasePoint.Y)))
                    _history.Push(_yFiller);
            }
            
            canvas.Clear();
            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(255, 255, 255)
            });
            foreach (var yFigure in _history.Reverse())
            {   
                yFigure.Draw(canvas);
            }
		}

        private SKColor GetCurrentColor()
        {
            byte r = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).R;
            byte g = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).G;
            byte b = ((Color)CurrentColor.Fill.GetValue(SolidColorBrush.ColorProperty)).B;
            return new SKColor(r, g, b);
        }

        private void skElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _canRedo = false;
            _redoStack = new Stack<YFigure>();
            _isMouseDown = true;
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = (float)(pixelPosition.X * DpiScale.DpiScaleX - 350 * DpiScale.DpiScaleX);
            var y = (float)(pixelPosition.Y * DpiScale.DpiScaleY - 150 * DpiScale.DpiScaleY);

            _skPoint = new SKPoint(x, y);

            var color = GetCurrentColor();

            if(_state == State.fill)
            {
                _yFiller = new YFiller(_skPoint, color);
            }
            else
            {
                if (_state == State.rubber)
                    color = new SKColor(255, 255, 255);
                _history.Push(new YPoint(_skPoint, color, _strokeWidth));
                _yPath = new YPath(new SKPath(), color, _strokeWidth);
                _yPath.DrawingPath.MoveTo(_skPoint);
            }
            skElement.InvalidateVisual();    
        }

        private void skElement_MouseMove(object sender, MouseEventArgs e)
        {
            var pixelPosition = e.GetPosition(sender as Canvas);
            var x = (float)(pixelPosition.X * DpiScale.DpiScaleX - 350 * DpiScale.DpiScaleX);
            var y = (float)(pixelPosition.Y * DpiScale.DpiScaleY - 150 * DpiScale.DpiScaleY);

            /*if(!(x < _winInf.Width && x > 0 && y < _winInf.Height && y > 0))
            {
                _isMouseDown = false;
            }*/
            if (_isMouseDown)
			{
                var skPoint = new SKPoint(x,y);

                var color = GetCurrentColor();

                YFigure currentFigure = null;

                switch (_state)
                {
                    case State.pen:
                        _yPath.DrawingPath.LineTo(skPoint);
                        currentFigure = _yPath;
                        break;

                    case State.rubber:
                        _yPath.DrawingPath.LineTo(skPoint);
                        currentFigure = _yPath;
                        break;

                    case State.square:
                        currentFigure = new YRect(_skPoint.X, _skPoint.Y, x  - _skPoint.X, y - _skPoint.Y, color, _strokeWidth, SKPaintStyle.Stroke);
                        break;

                    case State.circle:
                        float radius = (float)Math.Sqrt(Math.Pow(x - _skPoint.X, 2) + Math.Pow(y - _skPoint.Y, 2));
                        currentFigure = new YCircle(_skPoint, radius, color, _strokeWidth, SKPaintStyle.Stroke);
                        break;

                    case State.squareFilled:
                        currentFigure = new YRect(_skPoint.X, _skPoint.Y, x - _skPoint.X, y - _skPoint.Y, color, _strokeWidth, SKPaintStyle.Fill);
                        break;

                    case State.circleFilled:
                        float radiusFilled = (float)Math.Sqrt(Math.Pow(x - _skPoint.X, 2) + Math.Pow(y - _skPoint.Y, 2));
                        currentFigure = new YCircle(_skPoint, radiusFilled, color, _strokeWidth, SKPaintStyle.Fill);
                        break;

                    case State.line:
                        currentFigure = new YLine(_skPoint, new SKPoint(x, y), color, _strokeWidth);
                        break;
                }

                if(currentFigure!=null)
                {
                    _history.Pop();
                    _history.Push(currentFigure);
                    skElement.InvalidateVisual();
                }
			}
		}

        

        private void skElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void StrokeWidth1_Button_Click(object sender, RoutedEventArgs e)
        {
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(245, 222, 179));
            _strokeWidth = 3;
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(0,0,0));
        }

        private void StrokeWidth2_Button_Click(object sender, RoutedEventArgs e)
        {
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(245, 222, 179));
            _strokeWidth = 5;
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void StrokeWidth3_Button_Click(object sender, RoutedEventArgs e)
        {
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(245, 222, 179));
            _strokeWidth = 7;
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void StrokeWidth4_Button_Click(object sender, RoutedEventArgs e)
        {
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(245, 222, 179));
            _strokeWidth = 10;
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void StrokeWidth5_Button_Click(object sender, RoutedEventArgs e)
        {
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(245, 222, 179));
            _strokeWidth = 14;
            _strokeWidthEllipseInf[_strokeWidth].Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void BlackColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void SlateGrayColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(112, 128, 144));
        }

        private void WhiteColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void MediumOrchidColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(186, 85, 211));
        }

        private void DarkOrchid(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(153, 50, 204));
        }

        private void DarkBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 139));
        }

        private void MediumSlateBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(123, 104, 238));
        }

        private void LightSkyBlueColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(135, 206, 250));
        }

        private void YellowGreenColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(154, 205, 50));
        }

        private void GreenColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(0, 128, 0));
        }

        private void YellowColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        }

        private void OrangeColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 165, 0));
        }

        private void PinkColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203));
        }

        private void HotPinkColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 105, 180));
        }

        private void RedColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        private void FireBrickColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(178, 34, 34));
        }

        private void AnthiqueWhiteColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(250, 235, 215));
        }

        private void SaddleBrownColor(object sender, RoutedEventArgs e)
        {
            CurrentColor.Fill = new SolidColorBrush(Color.FromRgb(139, 69, 19));
        }

        private void OnPen(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.pen;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnRubber(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.rubber;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnSquare(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.square;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnCircle(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.circle;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnSquareFilled(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.squareFilled;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnCircleFilled(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.circleFilled;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));

        }

        private void OnLine(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.line;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnFill(object sender, RoutedEventArgs e)
        {
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(80, 80, 80));
            _state = State.fill;
            _buttonToDrawInf[_state].Background = new SolidColorBrush(Color.FromRgb(48, 48, 48));
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            if(_history.Count > 0)
            {
                _canRedo = true;
                _redoStack.Push(_history.Pop());
                skElement.InvalidateVisual();
            }
        }

        private void OnRestore(object sender, RoutedEventArgs e)
        {
            if(_canRedo && _redoStack.Count > 0)
            {
                _history.Push(_redoStack.Pop());
                skElement.InvalidateVisual();
            } 
        }
    }
}
